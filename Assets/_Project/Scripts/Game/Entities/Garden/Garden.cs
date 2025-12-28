using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private GardenData _data;
    [SerializeField] private Sprite _icon;

    private IWallet _wallet;
    private Sfx _sfx;

    private Grover _grover;
    private Storage _storage;
    private Upgrader _upgrader;

    public event Action<float> GroverProgressChanged;
    public event Action<float> StorageProgressChanged;
    public event Action<bool> PurchaseStatusChanged;
    public event Action<int, float> UpgradeInfoChanged;
    public event Action RecalculateUpgradeInfo;

    public float GroverProgress => _grover != null ? _grover.Progress : 0;

    public float StorageProgress => _storage != null ? _storage.Progress : 0;

    public bool IsPurchased => _data.IsPurchased;

    public float Price => _data.PurchasePrice;

    public float Fullness => _data.StorageData.CurrentFullness;

    public GardenData Data => _data;

    public Sprite Icon => _icon;

    public int UpgradePlantsCount => _upgrader != null ? _upgrader.Count : 1;
    public float UpgradesCountPrice => _upgrader != null ? _upgrader.Price : 0;

    public int PlantsCount => _data.PlantCount;

    private void Awake()
    {
        _wallet = ServiceLocator.Get<IWallet>();
        _sfx = ServiceLocator.Get<IAudioService>().Sfx;
    }

    private void OnDestroy() 
    { 
        _grover.Dispose();
        _upgrader.Dispose();
    }

    public void SetData(GardenData data)
    {
        _data = data;

        _grover?.Dispose();

        _storage = new(_data.StorageData, OnStorageProgressChanged);
        _grover = new(_data.GroverData, OnGrowCompleted, OnGroverProgressChanged);
        _grover.Grow(_data.GroverData.ElapsedTime);
        _upgrader = new(_data, OnUpgradedPlantsCount, OnRecalculatedUpgradeInfo);

        if (_data.IsPurchased && _storage.IsFilled == false)
            _grover.StartRun();

        StorageProgressChanged?.Invoke(_storage.Progress);
        GroverProgressChanged?.Invoke(_grover.Progress);
        PurchaseStatusChanged?.Invoke(_data.IsPurchased);
    }


    public void HandleClick()
    {
        if (_data.IsPurchased == false)
            Purchase();
    }

    public bool TryCollect(out float value)
    {
        value = 0;

        if (_data.StorageData.CurrentFullness > 0)
        {
            value = _storage.GiveCoins();
            _grover.StartRun();
            _sfx.PlayCollectedCoin();
        }

        return _data.IsPurchased;
    }

    public void MakeUpgradePlantsCount()
    {
        _upgrader.UpgradePlantsCount();

    }

    private void Purchase()
    {
        if (_wallet.TrySpend(_data.PurchasePrice) == false)
            return;

        _data.IsPurchased = true;
        _grover.StartRun();
        PurchaseStatusChanged?.Invoke(_data.IsPurchased);
    }

    private void OnGrowCompleted()
    {
        _storage.Increase(_data.GrowingCycleRevenuePerSinglePlant * _data.PlantCount);

        if (_storage.IsFilled)
            _grover.StopRun();
    }

    private void OnGroverProgressChanged(float progress) =>
        GroverProgressChanged?.Invoke(progress);

    private void OnStorageProgressChanged(float value)
    {
        StorageProgressChanged?.Invoke(value);

        if (_storage.IsFilled)
        {
            _grover.StopRun();
            _grover.ResetElapsedTime();

            return;
        }

        _grover.StartRun();
    }

    private void OnUpgradedPlantsCount(int plantCount, float plantsPrice)
    {
        UpgradeInfoChanged?.Invoke(plantCount, plantsPrice);
    }


    private void OnRecalculatedUpgradeInfo()
    {
        RecalculateUpgradeInfo?.Invoke();
    }
}
