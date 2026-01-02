using System;
using System.Collections.Generic;

public class SavingMediator : IService
{
    private const string FileName = "Saves";

    private readonly IWallet _wallet;
    private readonly GardensDirector _gardensDirector;
    private readonly SettingsPanel _settingsPanel;
    private readonly Saver _saver;

    public SavingMediator(IWallet wallet, GardensDirector gardensDirector, SettingsPanel settingsPanel)
    {
        _gardensDirector = gardensDirector != null ? gardensDirector : throw new ArgumentNullException(nameof(gardensDirector));
        _settingsPanel = settingsPanel != null ? settingsPanel : throw new ArgumentException(nameof(settingsPanel));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        IEncryptor dataEncryptor = new DataEncryptor();
        ISavingUtility savingUtility = new JsonSavingUtility(FileName, dataEncryptor);
        SavesData initialData = BuildData();
        _saver = new(savingUtility, initialData);
        RestoreGameState();
    }

    public void Save() =>
        _saver.Save();

    public void ResetProgress()
    {
        _saver.ResetProgress();
        RestoreGameState();
    }

    private SavesData BuildData()
    {
        SavesData data = new()
        {
            WalletAmount = _wallet.Amount
        };

        foreach (Garden _ in _gardensDirector.Gardens)
            data.GardenDatas.Add(new());

        return data;
    }

    private void RestoreGameState()
    {
        _wallet?.SetData(_saver.Data);

        List<SavedGardenData> datas = _saver.Data.GardenDatas;
        _gardensDirector.SetData(datas);

        if (_settingsPanel != null)
            _settingsPanel.SetData(_saver.Data);
    }
}