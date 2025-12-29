using System;
using System.Collections.Generic;

public class SavingMediator : IService
{
    private const string FileName = "Saves";

    private readonly IWallet _wallet;
    private readonly List<Garden> _gardens;
    private readonly SettingsPanel _settingsPanel;
    private readonly Saver _saver;

    public SavingMediator(List<Garden> gardens, SettingsPanel settingsPanel)
    {
        if (gardens == null || gardens.Count == 0)
            throw new ArgumentException(nameof(gardens));

        _gardens = gardens;
        _settingsPanel = settingsPanel != null ? settingsPanel : throw new ArgumentException(nameof(settingsPanel));
        _wallet = ServiceLocator.Get<IWallet>();

        IDataEncryptor dataEncryptor = new DataEncryptor();
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

        foreach (Garden _ in _gardens)
            data.GardenDatas.Add(new());

        return data;
    }

    private void RestoreGameState()
    {
        _wallet?.SetData(_saver.Data);

        List<SavedGardenData> datas = _saver.Data.GardenDatas;

        for (int i = 0; i < _gardens.Count; i++)
            if (_gardens[i] != null)
                _gardens[i].SetData(datas[i]);

        if (_settingsPanel != null)
            _settingsPanel.SetData(_saver.Data);
    }
}