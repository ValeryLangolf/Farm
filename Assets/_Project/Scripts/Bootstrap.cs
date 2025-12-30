using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Garden> _gardens;
    [SerializeField] private InputTrailParticle _trailParticle;
    [SerializeField] private Audio _audio;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private UIDirector _uiDirector;

    private bool _isApplicationQuitting = false;

    private void Awake()
    {
        RegisterServices();
        StartRunServices();

        ServiceLocator.Get<IAudioService>().Music.Play();
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

    private void OnDisable()
    {
        if (_isApplicationQuitting == false)
            SaveProgress();
    }

    private void OnDestroy()
    {
        StopRunServices();
        DisposeServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Register(UpdateService.Instance);

        _audio.Initialize();
        ServiceLocator.Register(_audio as IAudioService);

        IWallet wallet = new Wallet();
        ServiceLocator.Register(wallet);

        ServiceLocator.Register(_uiDirector);
        ServiceLocator.Register(new SavingMediator(_gardens, _settingsPanel));

        IInteractionDetector interactionDetector;

        if (Application.isMobilePlatform)
            interactionDetector = new TouchInteractionDetector();
        else
            interactionDetector = new MouseInteractionDetector();

        ServiceLocator.Register(interactionDetector);
        ServiceLocator.Register(new CoinCollector(interactionDetector, wallet));
        ServiceLocator.Register(new EntityClickHandler(interactionDetector));
        ServiceLocator.Register(_trailParticle);
    }

    private void StartRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StartRun();
    }

    private void StopRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StopRun();
    }

    private void DisposeServices()
    {
        IReadOnlyList<IDisposable> disposables = ServiceLocator.GetServices<IDisposable>();

        foreach (IDisposable disposable in disposables)
            disposable?.Dispose();
    }

    private void SaveProgress()
    {
        if (ServiceLocator.TryGet(out SavingMediator savingMediator))
            savingMediator.Save();
    }
}