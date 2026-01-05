using System;
using UnityEngine;

[Serializable]
public class ExtendedGardenData : IReadOnlyGardenData
{
    [field: SerializeField] public string GardenName { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField, Min(1)] public float PlantCostMultiplier { get; private set; }

    [field: SerializeField, Min(1)] public float InitialCostStoreLevelUpgrade { get; private set; }

    private SavedGardenData _savedData = new();
    private float _growthProgress = 0;
    private int _plantsCountToUpgrade = 1;
    private float _plantsPriceToUpgrade = float.MaxValue;
    private float _costStoreLevelUpgrade = float.MaxValue;

    public event Action<int> PlantsCountChanged;
    public event Action<float> GrowthProgressChanged;
    public event Action<float> GrowthElapsedTimeChanged;
    public event Action<float> StorageFullnessChanged;
    public event Action<bool> PurchaseStatusChanged;
    public event Action<int> PlantsCountToUpgradeChanged;
    public event Action<float> PlantsPriceToUpgradeChanged;
    public event Action<float> CostStoreLevelUpgradeChanged;
    public event Action<Sprite> PlantCountThresholdChanged;

    public int GardenIndex { get; set; }

    public float GardenPurchasePrice { get; set; }

    public float InitialCultivationDurationInSeconds { get; set; }

    public float InitialPlantPrice { get; set; }

    public float InitialPlantRevenue { get; set; }

    public float CultivationDurationInSeconds { get; set; }

    public bool IsStorageInfinity => SavedData.StoreLevelUpgrade > 0;

    public SavedGardenData SavedData
    {
        get
        {
            return _savedData;
        }
        set
        {
            _savedData = value;
        }
    }

    public float GrowthProgress
    {
        get
        {
            return _growthProgress;
        }
        set
        {
            if (Mathf.Approximately(_growthProgress, value))
                return;

            _growthProgress = value;
            GrowthProgressChanged?.Invoke(_growthProgress);
        }
    }

    public int PlantsCountToUpgrade
    {
        get
        {
            return _plantsCountToUpgrade;
        }
        set
        {
            if (_plantsCountToUpgrade == value)
                return;

            _plantsCountToUpgrade = value;
            PlantsCountToUpgradeChanged?.Invoke(_plantsCountToUpgrade);
        }
    }

    public float PlantsPriceToUpgrade
    {
        get
        {
            return _plantsPriceToUpgrade;
        }
        set
        {
            if (Mathf.Approximately(_plantsPriceToUpgrade, value))
                return;

            _plantsPriceToUpgrade = value;
            PlantsPriceToUpgradeChanged?.Invoke(PlantsPriceToUpgrade);
        }
    }

    public bool IsPurchased => SavedData.PlantsCount > 0;

    public int PlantsCount
    {
        get
        {
            return SavedData.PlantsCount;
        }
        set
        {
            if (SavedData.PlantsCount == value)
                return;

            int lastCount = SavedData.PlantsCount;

            SavedData.PlantsCount = value;

            if ((lastCount == 0 && value != 0) || (lastCount != 0 && value == 0))
                PurchaseStatusChanged?.Invoke(value > 0);

            PlantsCountChanged?.Invoke(value);
        }
    }

    public float StorageFullness
    {
        get
        {
            return SavedData.StorageFullness;
        }
        set
        {
            if (Mathf.Approximately(SavedData.StorageFullness, value))
                return;

            SavedData.StorageFullness = value;
            StorageFullnessChanged?.Invoke(StorageFullness);
        }
    }

    public float GrowthElapsedTime
    {
        get
        {
            return SavedData.GrowthElapsedTime;
        }
        set
        {
            if (Mathf.Approximately(SavedData.GrowthElapsedTime, value))
                return;

            SavedData.GrowthElapsedTime = value;
            GrowthElapsedTimeChanged?.Invoke(value);
        }
    }

    public int StoreLevelUpgrade
    {
        get
        {
            return SavedData.StoreLevelUpgrade;
        }
        set
        {
            SavedData.StoreLevelUpgrade = value;
        }
    }

    public float CostStoreLevelUpgrade
    {
        get
        {
            return _costStoreLevelUpgrade;
        }
        set
        {
            if (Mathf.Approximately(_costStoreLevelUpgrade, value))
                return;

            _costStoreLevelUpgrade = value;
            CostStoreLevelUpgradeChanged?.Invoke(value);
        }
    }

    public void NotifyPlantCountThresholdChanged() =>
        PlantCountThresholdChanged?.Invoke(Icon);

    public void InvokeAllDataChanged()
    {
        PlantsCountChanged?.Invoke(PlantsCount);
        GrowthProgressChanged?.Invoke(GrowthProgress);
        GrowthElapsedTimeChanged?.Invoke(GrowthElapsedTime);
        StorageFullnessChanged?.Invoke(StorageFullness);
        PurchaseStatusChanged?.Invoke(IsPurchased);
        PlantsCountToUpgradeChanged?.Invoke(PlantsCountToUpgrade);
        PlantsPriceToUpgradeChanged?.Invoke(PlantsPriceToUpgrade);
        CostStoreLevelUpgradeChanged?.Invoke(CostStoreLevelUpgrade);
    }
}