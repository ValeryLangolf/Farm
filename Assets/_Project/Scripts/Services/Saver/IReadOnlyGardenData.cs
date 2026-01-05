using System;
using UnityEngine;

public interface IReadOnlyGardenData
{
    event Action<float> GrowthProgressChanged;
    event Action<float> StorageFullnessChanged;
    event Action<bool> PurchaseStatusChanged;
    event Action<int> PlantsCountToUpgradeChanged;
    event Action<float> PlantsPriceToUpgradeChanged;
    event Action<int> PlantsCountChanged;
    event Action<float> CostStoreLevelUpgradeChanged;
    event Action<Sprite> PlantCountThresholdChanged;

    string GardenName { get; }

    Sprite Icon { get; }

    float GardenPurchasePrice { get; }

    bool IsStorageInfinity { get; }

    bool IsPurchased { get; }

    int PlantsCount { get; }

    float StorageFullness { get; }

    float GrowthElapsedTime { get; }

    float GrowthProgress { get; }

    int PlantsCountToUpgrade { get; }

    float PlantsPriceToUpgrade { get; }

    int StoreLevelUpgrade { get; }

    float CostStoreLevelUpgrade { get; }
}