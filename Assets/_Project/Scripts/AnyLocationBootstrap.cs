using System;
using System.Collections.Generic;
using UnityEngine;

public class AnyLocationBootstrap : MonoBehaviour
{
    [SerializeField] private GardensDirector _gardensDirector;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private UIDirector _uiDirector;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private SavesDataConfig _savesDataConfig;
    [SerializeField] private int _locationIndex;

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

        if (ServiceLocator.TryRemoveService(out IWallet wallet))
        {
            if (wallet is IDisposable disposable)
                disposable.Dispose();
        }

        ServiceLocator.TryRemoveService<IWallet>(out _);
        ServiceLocator.TryRemoveService<GardensDirector>(out _);
        ServiceLocator.TryRemoveService<UIDirector>(out _);
        ServiceLocator.TryRemoveService<SavingMediator>(out _);
        ServiceLocator.TryRemoveService<CoinCollector>(out _);
    }

    private void RegisterServices()
    {
        IWallet wallet = new Wallet();
        ServiceLocator.Register(wallet);
        ServiceLocator.Register(_gardensDirector);
        ServiceLocator.Register(_uiDirector);
        RegisterSavingMediator();
        ServiceLocator.Register(new CoinCollector(ServiceLocator.Get<IInteractionDetector>(), wallet));
    }

    private void RegisterSavingMediator()
    {
        IEncryptor dataEncryptor = new DataEncryptor();
        ISavingUtility savingUtility = new JsonSavingUtility("Saves", dataEncryptor);
        ISaver<SavesData> saver = new Saver<SavesData>(savingUtility, _savesDataConfig.SavesData);

        SavingMediator savingMediator = new(
            ServiceLocator.Get<IWallet>(),
            _gardensDirector,
            _settingsPanel,
            _tutorial,
            _locationIndex,
            saver);

        ServiceLocator.Register(savingMediator);
    }

    private void StartRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StartRun();

        ServiceLocator.Get<IAudioService>().Music.Play();

        if (_tutorial != null)
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