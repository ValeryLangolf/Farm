using System;
using UnityEngine;

[Serializable]
public class ExtendedGardenData : IReadOnlyGardenData
{
    [SerializeField] private string _gardenName;
    [SerializeField] private Sprite _icon;
    [SerializeField, Min(1)] private float _purchasePrice = 1;
    [SerializeField, Min(1)] private float _initialGrowingCycleRevenue = 1;
    [SerializeField, Min(0.001f)] private float _initialCultivationDurationInSeconds = float.MaxValue;
    [SerializeField, Min(1)] private float _initialPlantPrice = 1;
    [SerializeField] private bool _isInitialPurchased;

    private SavedGardenData _savedData = new();
    private float _groverProgress;
    private float _storageProgress;
    private int _plantsCountToUpgrade;
    private float _plantsPriceToUpgrade;
    private float _cultivationDurationInSeconds = float.MaxValue;

    public event Action<int> PlantsCountChanged;
    public event Action<float> GroverProgressChanged;
    public event Action<float> StorageProgressChanged;
    public event Action<bool> PurchaseStatusChanged;
    public event Action<int> PlantsCountToUpgradeChanged;
    public event Action<float> PlantsPriceToUpgradeChanged;

    public SavedGardenData SavedData => _savedData;

    public string GardenName => _gardenName;

    public Sprite Icon => _icon;

    public float GardenPurchasePrice => _purchasePrice;

    public float CurrentGrowingCycleRevenue => _initialGrowingCycleRevenue * _savedData.PlantsCount;

    public float InitialCultivationDurationInSeconds => _initialCultivationDurationInSeconds;

    public bool IsPurchased => _savedData.IsPurchased;

    public int PlantsCount => _savedData.PlantsCount;

    public float StorageCapacity => _savedData.StorageCapacity;

    public float StorageFullness => _savedData.StorageFullness;

    public float GroverElapsedTime => _savedData.GroverElapsedTime;

    public float GroverProgress => _groverProgress;

    public float StorageProgress => _storageProgress;

    public int PlantsCountToUpgrade => _plantsCountToUpgrade;

    public float PlantsPriceToUpgrade => _plantsPriceToUpgrade;

    public float InitialPlantPrice => _initialPlantPrice;

    public float CultivationDurationInSeconds => _cultivationDurationInSeconds;

    public void SetSavedData(SavedGardenData savedData)
    {
        _savedData = savedData;

        if (_isInitialPurchased)
            _savedData.IsPurchased = true;

        UpdateGroverProgress();
        UpdadeStorageProgress();

        PurchaseStatusChanged?.Invoke(_savedData.IsPurchased);
        PlantsCountToUpgradeChanged?.Invoke(_plantsCountToUpgrade);
        PlantsPriceToUpgradeChanged?.Invoke(_plantsPriceToUpgrade);
        GroverProgressChanged?.Invoke(_groverProgress);
        StorageProgressChanged?.Invoke(_storageProgress);
    }

    public void SetPurchasedStatus(bool isPurchased)
    {
        _savedData.IsPurchased = isPurchased;
        PurchaseStatusChanged?.Invoke(isPurchased);
    }

    public void SetGroverElapsedTime(float value)
    {
        _savedData.GroverElapsedTime = value;
        UpdateGroverProgress();
    }

    public void SetStorageCapacity(float value)
    {
        _savedData.StorageCapacity = value;
        UpdadeStorageProgress();
    }

    public void SetStorageFullnes(float value)
    {
        _savedData.StorageFullness = value;
        UpdadeStorageProgress();
    }

    public void SetPlantsCount(int value)
    {
        _savedData.PlantsCount = value;
        PlantsCountChanged?.Invoke(value);
    }

    public void SetPlantsCountToUpgrade(int value)
    {
        _plantsCountToUpgrade = value;
        PlantsCountToUpgradeChanged?.Invoke(_plantsCountToUpgrade);
    }

    public void SetPlantsPriceToUpgrade(float value)
    {
        _plantsPriceToUpgrade = value;
        PlantsPriceToUpgradeChanged?.Invoke(_plantsPriceToUpgrade);
    }

    public void SetCultivationDurationInSeconds(float value)
    {
        _cultivationDurationInSeconds = value;
    }


    private void UpdateGroverProgress()
    {
        _groverProgress = _savedData.GroverElapsedTime / _cultivationDurationInSeconds;
        GroverProgressChanged?.Invoke(_groverProgress);
    }

    private void UpdadeStorageProgress()
    {
        _storageProgress = _savedData.StorageFullness / _savedData.StorageCapacity;
        StorageProgressChanged?.Invoke(_storageProgress);
    }
}