using System;
using System.Collections.Generic;
using UnityEngine;

public class SavingMediator : IService
{
    private const string FileName = "Saves";

    private readonly IWallet _wallet;
    private readonly GardensDirector _gardensDirector;
    private readonly SettingsPanel _settingsPanel;
    private readonly Saver _saver;
    private readonly Tutorial _tutorial;

    public SavingMediator(IWallet wallet, GardensDirector gardensDirector, SettingsPanel settingsPanel, Tutorial tutorial)
    {
        _gardensDirector = gardensDirector != null ? gardensDirector : throw new ArgumentNullException(nameof(gardensDirector));
        _settingsPanel = settingsPanel != null ? settingsPanel : throw new ArgumentException(nameof(settingsPanel));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _tutorial = tutorial != null ? tutorial : throw new ArgumentNullException(nameof(tutorial));

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
        SavesData data = new ()
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

        if(_tutorial != null)
            _tutorial.SetData(_saver.Data);
    }
}