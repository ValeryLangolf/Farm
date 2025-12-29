using System;
using UnityEngine;

[Serializable]
public class ExtendedGardenData : IReadOnlyGardenData
{
    [SerializeField] private Sprite _icon;
    [SerializeField, Min(1)] private float _purchasePrice = 1;
    [SerializeField, Min(1)] private float _initialGrowingCycleRevenue = 1;
    [SerializeField, Min(0.001f)] private float _initialCultivationDurationInSeconds = 1;
    [SerializeField] private bool _isInitialPurchased;

    private SavedGardenData _savedData = new();
    private float _groverProgress;
    private float _storageProgress;

    public event Action<float> GroverProgressChanged;
    public event Action<float> StorageProgressChanged;
    public event Action<bool> PurchaseStatusChanged;

    public SavedGardenData SavedData => _savedData;

    public Sprite Icon => _icon;

    public float GardenPurchasePrice => _purchasePrice;

    public float InitialGrowingCycleRevenue => _initialGrowingCycleRevenue;

    public float CurrentGrowingCycleRevenue => _initialGrowingCycleRevenue * _savedData.PlantCount;

    public float InitialCultivationDurationInSeconds => _initialCultivationDurationInSeconds;

    public bool IsPurchased => _savedData.IsPurchased;

    public int PlantsCount => _savedData.PlantCount;

    public float StorageCapacity => _savedData.StorageCapacity;

    public float StorageFullness => _savedData.StorageFullness;

    public float GroverElapsedTime => _savedData.GroverElapsedTime;

    public float GroverProgress => _groverProgress;

    public float StorageProgress => _storageProgress;

    public void SetSavedData(SavedGardenData savedData)
    {
        _savedData = savedData;

        if (_isInitialPurchased)
            _savedData.IsPurchased = true;

        UpdateGroverProgress();
        UpdadeStorageProgress();

        PurchaseStatusChanged?.Invoke(_savedData.IsPurchased);
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
        _savedData.PlantCount = value;
    }

    private void UpdateGroverProgress()
    {
        _groverProgress = _savedData.GroverElapsedTime / InitialCultivationDurationInSeconds;
        GroverProgressChanged?.Invoke(_groverProgress);
    }

    private void UpdadeStorageProgress()
    {
        _storageProgress = _savedData.StorageFullness / _savedData.StorageCapacity;
        StorageProgressChanged?.Invoke(_storageProgress);
    }
}