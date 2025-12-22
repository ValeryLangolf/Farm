using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavesData
{
    public float WalletAmount = 0;
    public float MusicVolume = 0.5f;
    public float SfxVolume = 0.75f;
    public long LastServerTime = -1;

    public List<GardenData> GardenDatas = new();
}

[Serializable]
public class GardenData
{
    public bool IsPurchased = false;
    [Min(1)] public float PurchasePrice = 1;
    [Min(1)] public float PlantCount = 1;
    [Min(1)] public float GrowingCycleRevenuePerSinglePlant = 1;
    public StorageData StorageData;
    public GroverData GroverData;
}

[Serializable]
public class StorageData
{
    [Min(1)] public float Capacity = 1;
    [Min(0)] public float CurrentFullness = 0;
}

[Serializable]
public class GroverData
{
    [Min(0.005f)] public float CultivationDurationInSeconds = 30;
    [Min(0)] public float ElapsedTime;
}