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
        ServiceLocator.Register(CreateSavingMediator());
        ServiceLocator.Register(new CoinCollector(ServiceLocator.Get<IInteractionDetector>(), wallet));
    }

    private SavingMediator CreateSavingMediator()
    {
        IEncryptor dataEncryptor = new NoEncrypt();
        ISavingUtility savingUtility = new JsonSavingUtility(Constants.SavesFileName, dataEncryptor);
        ISaver<SavesData> saver = new Saver<SavesData>(savingUtility, _savesDataConfig.GetSavesData());
        IWallet wallet = ServiceLocator.Get<IWallet>();

        return new(
            wallet,
            _gardensDirector,
            _settingsPanel,
            _tutorial,
            _locationIndex,
            saver);
    }

    private void StartRunServices()
    {
        ServiceLocator.Get<IAudioService>().Music.Play();

        if (_tutorial != null)
            _tutorial.Run();
    }

    private void SaveProgress()
    {
        if (ServiceLocator.TryGet(out SavingMediator savingMediator))
            savingMediator.Save();
    }
}