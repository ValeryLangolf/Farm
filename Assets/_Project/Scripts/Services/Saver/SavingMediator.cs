using System;
using System.Collections.Generic;

public class SavingMediator : IService
{
    private const string FileName = "Saves";

    private readonly IWallet _wallet;
    private readonly GardensDirector _gardensDirector;
    private readonly SettingsPanel _settingsPanel;
    private readonly ISaver<SavesData> _saver;
    private readonly Tutorial _tutorial;
    private readonly int _locationIndex;

    public SavingMediator(
        IWallet wallet, 
        GardensDirector gardensDirector, 
        SettingsPanel settingsPanel, 
        Tutorial tutorial, 
        int locationIndex,
        ISaver<SavesData> saver)
    {
        _gardensDirector = gardensDirector != null ? gardensDirector : throw new ArgumentNullException(nameof(gardensDirector));
        _settingsPanel = settingsPanel != null ? settingsPanel : throw new ArgumentException(nameof(settingsPanel));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _tutorial = tutorial;
        _locationIndex = locationIndex;
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
        RestoreGameState();
    }

    public void Save()
    {
        SavesData data = _saver.Data;
        data.MusicVolume = _settingsPanel.MusicVolume;
        data.SfxVolume = _settingsPanel.SfxVolume;

        LocationData currentLocationData = new()
        {
            TutorialCounter = _tutorial != null ? _tutorial.Counter : _saver.Data.Locations[_locationIndex].TutorialCounter,
            WalletAmount = _wallet.Amount,
            LastServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            GardenDatas = _gardensDirector.GetGardensData(),
        };

        if (_locationIndex < 0 || _locationIndex > data.Locations.Count - 1)
            throw new ArgumentOutOfRangeException(nameof(_locationIndex), _locationIndex, "Локация с таким индексом не зарегистрирована");

        data.Locations[_locationIndex] = currentLocationData;
        _saver.Save(data);
    }

    public void ResetProgress()
    {
        _saver.ResetProgress();
        RestoreGameState();
    }

    private void BuildData()
    {

    }

    private void RestoreGameState()
    {
        SavesData data = _saver.Data;
        LocationData locationData = data.Locations[_locationIndex];

        _wallet?.SetAmount(locationData.WalletAmount);

        if (_gardensDirector != null)
        {
            List<SavedGardenData> gardenDatas = locationData.GardenDatas;
            _gardensDirector.SetData(gardenDatas);
        }

        if (_settingsPanel != null)
            _settingsPanel.SetData(data.MusicVolume, data.SfxVolume);

        if (_tutorial != null)
            _tutorial.SetCounter(locationData.TutorialCounter);
    }
}