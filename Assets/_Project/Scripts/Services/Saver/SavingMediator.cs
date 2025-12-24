using System;
using System.Collections.Generic;

public class SavingMediator : IService, IDisposable
{
    private const string FileName = "Saves";

    private readonly IWallet _wallet;
    private readonly List<Garden> _gardens;
    private readonly ProgressResetterButton _progressResetterButton;
    private readonly Saver _saver;

    public SavingMediator(List<Garden> gardens, IWallet wallet, ProgressResetterButton progressResetterButton)
    {
        if (gardens == null || gardens.Count == 0)
            throw new ArgumentException(nameof(gardens));

        _gardens = gardens;
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _progressResetterButton = progressResetterButton != null ? progressResetterButton : throw new ArgumentNullException(nameof(progressResetterButton));

        IDataEncryptor dataEncryptor = new DataEncryptor();
        ISavingUtility savingUtility = new JsonSavingUtility(FileName, dataEncryptor);
        SavesData initialData = BuildData();
        _saver = new(savingUtility, initialData);
        RestoreGameState();

        _progressResetterButton.Clicked += OnClick;
    }

    public void Dispose() =>
        _progressResetterButton.Clicked -= OnClick;

    public void Save() =>
        _saver.Save();

    private SavesData BuildData()
    {
        SavesData data = new()
        {
            WalletAmount = _wallet.Amount
        };

        foreach (Garden garden in _gardens)
            data.GardenDatas.Add(garden.Data);

        return data;
    }

    private void OnClick()
    {
        _saver.ResetProgress();
        RestoreGameState();
    }

    private void RestoreGameState()
    {
        if (_wallet != null)
            _wallet.SetData(_saver.Data);

        List<GardenData> datas = _saver.Data.GardenDatas;

        for (int i = 0; i < _gardens.Count; i++)
            if (_gardens[i] != null)
                _gardens[i].SetData(datas[i]);
    }
}