using System;
using UnityEngine;

public interface IReadOnlyGardenData
{
    public event Action<float> GrowthProgressChanged;
    public event Action<float> StorageFullnessChanged;
    public event Action<bool> PurchaseStatusChanged;
    public event Action<int> PlantsCountToUpgradeChanged;
    public event Action<float> PlantsPriceToUpgradeChanged;
    public event Action<int> PlantsCountChanged;
    public event Action<float> CostStoreLevelUpgradeChanged;
    public event Action<Sprite> PlantCountThresholdChanged;

    public string GardenName { get; }

    public Sprite Icon { get; }

    public float GardenPurchasePrice { get; }

    public bool IsStorageInfinity { get; }

    public bool IsPurchased { get; }

    public int PlantsCount { get; }

    public float StorageFullness { get; }

    public float GrowthElapsedTime { get; }

    public float GrowthProgress { get; }

    public int PlantsCountToUpgrade { get; }

    public float PlantsPriceToUpgrade { get; }

    public int StoreLevelUpgrade { get; }

    public float CostStoreLevelUpgrade { get; }
}