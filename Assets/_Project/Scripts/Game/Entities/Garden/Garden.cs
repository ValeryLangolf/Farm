using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private ExtendedGardenData _data;    

    private IWallet _wallet;
    private Sfx _sfx;

    private Grover _grover;
    private Storage _storage;
    private Upgrader _upgrader;

    public event Action<int, float> UpgradeInfoChanged;
    public event Action RecalculatedUpgradeInfo;

    public IReadOnlyGardenData ReadOnlyData => _data;

    public int UpgradePlantsCount => _upgrader != null ? _upgrader.Count : 1;

    public float UpgradesCountPrice => _upgrader != null ? _upgrader.Price : 0;

    private void Awake()
    {
        _wallet = ServiceLocator.Get<IWallet>();
        _sfx = ServiceLocator.Get<IAudioService>().Sfx;

        _data.StorageProgressChanged += OnStorageProgressChanged;
    }

    private void OnDestroy()
    {
        _grover.Dispose();
        _upgrader.Dispose();

        _data.StorageProgressChanged -= OnStorageProgressChanged;
    }

    public void SetData(SavedGardenData data)
    {
        if(data == null)
            throw new ArgumentNullException(nameof(data));

        _data.SetSavedData(data); 
        _grover?.Dispose();
        _upgrader?.Dispose();

        _storage = new(_data);
        _grover = new(_data, OnGrowCompleted);
        _grover.Grow(_data.GroverElapsedTime);
        _upgrader = new(_data, OnUpgradedPlantsCount, OnRecalculatedUpgradeInfo);

        if (_data.IsPurchased && _storage.IsFilled == false)
            _grover.StartRun();
    }

    public void HandleClick()
    {
        if (_data.IsPurchased == false)
            Purchase();
    }

    public bool TryCollect(out float value)
    {
        if (_data.StorageFullness > 0)
        {
            value = _storage.GiveCoins();
            _grover.StartRun();
            _sfx.PlayCollectedCoin();

            return true;
        }

        value = 0;

        return false;
    }

    public void MakeUpgradePlantsCount() =>
        _upgrader.UpgradePlantsCount();

    private void Purchase()
    {
        if (_data.IsPurchased)
            return;

        if (_wallet.TrySpend(_data.GardenPurchasePrice))
        {
            _data.SetPurchasedStatus(true);
            _grover.StartRun();
        }
    }

    private void OnGrowCompleted()
    {
        _storage.Increase(_data.InitialGrowingCycleRevenue * _data.PlantsCount);

        if (_storage.IsFilled)
            _grover.StopRun();
    }

    private void OnStorageProgressChanged(float value)
    {
        if (_storage.IsFilled)
        {
            _grover.StopRun();
            _grover.ResetElapsedTime();

            return;
        }

        _grover.StartRun();
    }

    private void OnUpgradedPlantsCount(int plantCount, float plantsPrice) =>
        UpgradeInfoChanged?.Invoke(plantCount, plantsPrice);

    private void OnRecalculatedUpgradeInfo() =>
        RecalculatedUpgradeInfo?.Invoke();
}