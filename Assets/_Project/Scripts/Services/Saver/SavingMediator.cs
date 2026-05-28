using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SavingMediator : MonoBehaviour, IInjactable
{
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private int _locationIndex;

    private IWallet _wallet;
    private IGardensDirector _gardensDirector;
    private ISaver<SavesData> _saver;
    private bool _isApplicationQuitting = false;

    private long _lastSaverTime;

    [Inject]
    public void Construct(
        IWallet wallet,
        IGardensDirector gardensDirector,
        ISaver<SavesData> saver)
    {
        _gardensDirector = gardensDirector ?? throw new ArgumentNullException(nameof(gardensDirector));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));

        RestoreGameState();
    }

    private void OnDisable()
    {
        if (_isApplicationQuitting == false)
            Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Save();
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
        Save();
    }

    public void Save()
    {
        if (IsRecentlySaved())
            return;

        SavesData data = CollectData();
        _saver.Save(data);
    }

    public void ResetProgress()
    {
        _saver.ResetProgress();
        RestoreGameState();
    }

    private SavesData CollectData()
    {
        SavesData data = _saver.Data;
        data.MusicVolume = _settingsPanel.MusicVolume;
        data.SfxVolume = _settingsPanel.SfxVolume;

        LocationData currentLocationData = new()
        {
            TutorialCounter = _tutorial != null ? _tutorial.Counter : _saver.Data.Locations[_locationIndex].TutorialCounter,
            WalletAmount = _wallet.Amount,
            LastServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            GardenDatas = new(_gardensDirector.GetGardensData()),
        };

        if (_locationIndex < 0 || _locationIndex > data.Locations.Count - 1)
            throw new ArgumentOutOfRangeException(nameof(_locationIndex), _locationIndex, "Локация с таким индексом не зарегистрирована");

        data.Locations[_locationIndex] = currentLocationData;

        return data;
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

    private bool IsRecentlySaved()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        bool isRecentlySaved = _lastSaverTime + 2000 > currentTime;

        if (isRecentlySaved == false)
            _lastSaverTime = currentTime;

        return isRecentlySaved;
    }
}