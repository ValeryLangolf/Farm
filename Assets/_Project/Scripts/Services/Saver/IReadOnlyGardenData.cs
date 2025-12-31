using System;
using UnityEngine;

public interface IReadOnlyGardenData
{
    event Action<float> GroverProgressChanged;
    event Action<float> StorageProgressChanged;
    event Action<bool> PurchaseStatusChanged;
    event Action<int> PlantsCountToUpgradeChanged;
    event Action<float> PlantsPriceToUpgradeChanged;
    event Action<int> PlantsCountChanged;

    Sprite Icon { get; }

    float GardenPurchasePrice { get; }

    bool IsPurchased { get; }

    int PlantsCount { get; }

    bool IsStorageInfinity { get; }

    float StorageFullness { get; }

    float GroverElapsedTime { get; }

    float GroverProgress { get; }

    float StorageProgress { get; }

    int PlantsCountToUpgrade { get; }

    float PlantsPriceToUpgrade { get; }

    int ProfitLevel { get; }
   string GardenName {  get; }  
}