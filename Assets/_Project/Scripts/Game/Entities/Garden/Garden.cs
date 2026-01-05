using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private ExtendedGardenData _data;

    private IWallet _wallet;
    private Sfx _sfx;
    private GardenGrover _grover;
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

    public void SetData(SavedGardenData data, int index)
    {
        _data.GardenIndex = index >= 0 ? index : throw new ArgumentOutOfRangeException(nameof(index), index, "Значение должно быть положительным");
        _data.SavedData = data ?? throw new ArgumentNullException(nameof(data));

        _data.GardenPurchasePrice = FormulaCalculator.CalculatePurchasePrice(index, Constants.BaseGardenPrice, Constants.GardenPriceMultiplier);
        _data.InitialPlantPrice = index > 0 ? _data.GardenPurchasePrice : 3;
        _data.InitialPlantRevenue = FormulaCalculator.CalculateInitialPlantRevenue(index, _data.GardenPurchasePrice);

        _grover?.Dispose();
        _upgrader?.Dispose();
        _grover = new(_data);
        _upgrader = new(_data);
        _grover.Grow(_data.GrowthElapsedTime);
        _grover.ProcessRunnableStatus();

        _data.InvokeAllDataChanged();
    }

    public void ProcessClick()
    {
        if (_data.IsPurchased == false && _wallet.TrySpend(_data.GardenPurchasePrice))
        {
            _data.PlantsCount++;
            _grover.ProcessRunnableStatus();
        }
    }

    public bool TryCollect(out float value)
    {
        value = _data.StorageFullness;
        bool isSuccessful = value > 0;

        if (isSuccessful)
        {
            _data.StorageFullness = 0;
            _grover.ProcessRunnableStatus();
            _sfx.PlayCollectedCoin();
        }

        return isSuccessful;
    }

    public void UpgradePlantsCount()
    {
        int plantsCountToUpgrade = _data.PlantsCountToUpgrade;

        if (_wallet.TrySpend(_data.PlantsPriceToUpgrade))
            _data.PlantsCount += plantsCountToUpgrade;
    }

    public void UpgradeStoreLevel()
    {
        if (_wallet.TrySpend(_data.CostStoreLevelUpgrade))
            _upgrader.UpgradeStoreLevel();
    }
}