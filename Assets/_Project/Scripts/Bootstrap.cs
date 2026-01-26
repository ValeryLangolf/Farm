using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private UpdateService _updateService;
    [SerializeField] private GardensDirector _gardensDirector;
    [SerializeField] private InputTrailParticle _trailParticle;
    [SerializeField] private Audio _audio;
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
    }

    private void RegisterServices()
    {
        ServiceLocator.Register(_updateService as IUpdateService);

        IWallet wallet = new Wallet();
        ServiceLocator.Register(wallet);

        ServiceLocator.Register(_gardensDirector);
        ServiceLocator.Register(_audio as IAudioService);
        ServiceLocator.Register(_uiDirector);
        ServiceLocator.Register(new SavingMediator(wallet, _gardensDirector, _settingsPanel, _tutorial));

        IInteractionDetector interactionDetector;
        IPointerPositionProvider pointerPositionProvider;

        if (Application.isMobilePlatform)
        {
            interactionDetector = new TouchInteractionDetector();
            pointerPositionProvider = new TouchPositionProvider(_updateService);
        }
        else
        {
            interactionDetector = new MouseInteractionDetector();
            pointerPositionProvider = new MousePositionProvider(_updateService);
        }

        ServiceLocator.Register(interactionDetector);
        ServiceLocator.Register(pointerPositionProvider);
        ServiceLocator.Register(new CoinCollector(interactionDetector, wallet));
        ServiceLocator.Register(new EntityClickHandler(interactionDetector));
        ServiceLocator.Register(_trailParticle);
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