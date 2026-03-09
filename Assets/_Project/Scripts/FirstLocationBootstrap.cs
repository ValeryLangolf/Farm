using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstLocationBootstrap : MonoBehaviour
{
    [SerializeField] private GardensDirector _gardensDirector;    
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private UIDirector _uiDirector;
    [SerializeField] private Tutorial _tutorial;

    private bool _isApplicationQuitting = false;

    private void Awake() =>
        RegisterServices();

    private void Start() =>
        StartRunServices();

    private void OnDisable()
    {
        if (_isApplicationQuitting == false)
            SaveProgress();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveProgress();
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
        SaveProgress();
    }

    private void OnDestroy()
    {
        StopRunServices();
        DisposeServices();

        if(ServiceLocator.TryRemoveService(out IWallet wallet))
        {
            if(wallet is IDisposable disposable)
                disposable.Dispose();
        }
    }

    private void RegisterServices()
    {
        IWallet wallet = new Wallet();
        ServiceLocator.Register(wallet);

        ServiceLocator.Register(_gardensDirector);
        ServiceLocator.Register(_uiDirector);
        ServiceLocator.Register(new SavingMediator(wallet, _gardensDirector, _settingsPanel, _tutorial));

        IInteractionDetector interactionDetector = ServiceLocator.Get<IInteractionDetector>();

        ServiceLocator.Register(new CoinCollector(interactionDetector, wallet));        
    }

    private void StartRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StartRun();

        ServiceLocator.Get<IAudioService>().Music.Play();

        _tutorial.Run();
    }

    private void StopRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StopRun();
    }

    private void SaveProgress()
    {
        if (ServiceLocator.TryGet(out SavingMediator savingMediator))
            savingMediator.Save();
    }

    private void DisposeServices()
    {
        IReadOnlyList<IDisposable> disposables = ServiceLocator.GetServices<IDisposable>();

        foreach (IDisposable disposable in disposables)
            disposable?.Dispose();
    }
}