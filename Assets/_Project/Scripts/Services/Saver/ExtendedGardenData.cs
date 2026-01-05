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
    private float _groverProgress = 0;
    private int _plantsCountToUpgrade = 1;
    private float _plantsPriceToUpgrade = float.MaxValue;
    private float _costStoreLevelUpgrade = float.MaxValue;

    public event Action<int> PlantsCountChanged;
    public event Action<float> GroverProgressChanged;
    public event Action<float> GroverElapsedTimeChanged;
    public event Action<bool> StorageFilledChanged;
    public event Action<bool> PurchaseStatusChanged;
    public event Action<int> PlantsCountToUpgradeChanged;
    public event Action<float> PlantsPriceToUpgradeChanged;
    public event Action<float> CostStoreLevelUpgradeChanged;
    public event Action<Sprite> PlantCountTresholdChanged;

    public int GardenIndex { get; set; }

    public float GardenPurchasePrice { get; set; }

    public float InitialCultivationDurationInSeconds { get; set; }

    public float InitialPlantPrice { get; set; }

    public float InitialPlantRevenue { get; set; }

    public float CultivationDurationInSeconds { get; set; }

    public bool IsStorageInfinity => SavedData.StoreLevelUpgrade > 0;

    public bool IsStorageFilled => SavedData.StorageFullness > 0
        && IsStorageInfinity == false;

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

    public float GroverProgress 
    { 
        get
        {
            return _groverProgress;
        }
        set
        {
            if (Mathf.Approximately(_groverProgress, value))
                return;

            _groverProgress = value;
            GroverProgressChanged?.Invoke(_groverProgress);
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
            if(Mathf.Approximately(_plantsPriceToUpgrade, value)) 
                return;

            _plantsPriceToUpgrade = value;
            PlantsPriceToUpgradeChanged?.Invoke(_plantsCountToUpgrade);
        }
    }

    public bool IsPurchased
    {
        get
        {
            return SavedData.IsPurchased;
        }
        set
        {
            if (SavedData.IsPurchased == value)
                return;

            SavedData.IsPurchased = value;
            PurchaseStatusChanged?.Invoke(value);
        }
    }

    public int PlantsCount
    {
        get
        {
            return SavedData.PlantsCount;
        }
        set
        {
            if(SavedData.PlantsCount == value) 
                return;

            SavedData.PlantsCount = value;
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
            if(Mathf.Approximately(SavedData.StorageFullness, value))
                return;

            SavedData.StorageFullness = value;
            StorageFilledChanged?.Invoke(IsStorageFilled);
        }
    }

    public float GroverElapsedTime
    {
        get
        {
            return SavedData.GroverElapsedTime;
        }
        set
        {
            if(Mathf.Approximately(SavedData.GroverElapsedTime, value)) 
                return;

            SavedData.GroverElapsedTime = value;
            GroverElapsedTimeChanged?.Invoke(value);
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
            if(Mathf.Approximately(_costStoreLevelUpgrade, value)) 
                return;

            _costStoreLevelUpgrade = value;
            CostStoreLevelUpgradeChanged?.Invoke(value);
        }
    }

    public void InvokePlantCountTresholdChanged() =>
        PlantCountTresholdChanged?.Invoke(Icon);

    public void InvokeAllDataChanged()
    {
        PlantsCountChanged?.Invoke(PlantsCount);
        GroverProgressChanged?.Invoke(GroverProgress);
        GroverElapsedTimeChanged?.Invoke(GroverElapsedTime);
        StorageFilledChanged?.Invoke(IsStorageFilled);
        PurchaseStatusChanged?.Invoke(IsPurchased);
        PlantsCountToUpgradeChanged?.Invoke(PlantsCountToUpgrade);
        PlantsPriceToUpgradeChanged?.Invoke(PlantsPriceToUpgrade);
        CostStoreLevelUpgradeChanged?.Invoke(CostStoreLevelUpgrade);
    }
}