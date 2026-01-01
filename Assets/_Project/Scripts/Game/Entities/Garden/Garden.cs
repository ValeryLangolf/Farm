using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private ExtendedGardenData _data;

    private IWallet _wallet;
    private Sfx _sfx;
    private Grover _grover;
    private GardenUpgrader _upgrader;

    public IReadOnlyGardenData ReadOnlyData => _data;

    private void Awake()
    {
        _wallet = ServiceLocator.Get<IWallet>();
        _sfx = ServiceLocator.Get<IAudioService>().Sfx;
    }

    private void OnDestroy()
    {
        _grover?.Dispose();
        _upgrader?.Dispose();
    }

    public void SetData(SavedGardenData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        _grover?.Dispose();
        _upgrader?.Dispose();
        _data.SetSavedData(data);
        _grover = new(_data);
        _grover.Grow(_data.GroverElapsedTime);
        _upgrader = new(_data);
        ProcessRunnableStatusGrover();
    }

    public void ProcessClick()
    {
        if (_data.IsPurchased == false && _wallet.TrySpend(_data.GardenPurchasePrice))
        {
            _data.SetPurchasedStatus(true);
            ProcessRunnableStatusGrover();
        }
    }

    public bool TryCollect(out float value)
    {
        value = _data.StorageFullness;
        bool isSuccessful = value > 0;

        if (isSuccessful)
        {
            _data.SetStorageFullnes(0);
            ProcessRunnableStatusGrover();
            _sfx.PlayCollectedCoin();
        }

        return isSuccessful;
    }

    public void UpgradePlantsCount() =>
        _upgrader.UpgradePlantsCount();

    public void UpgradeProfit()
    {
        if (_wallet.TrySpend(_data.LevelUpPrice))
        {
            _data.SetProfitLevel(_data.ProfitLevel + 1);
        }
    }

    private void ProcessRunnableStatusGrover()
    {
        if (_data.IsPurchased && (_data.StorageFullness == 0 || _data.IsStorageInfinity))
            _grover.StartRun();
        else
            _grover.StopRun();
    }
}