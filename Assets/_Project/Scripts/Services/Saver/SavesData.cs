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

    public List<SavedGardenData> GardenDatas = new();
}

[Serializable]
public class SavedGardenData
{
    [Min(1)] public int PlantsCount = 0;
    [Min(0)] public float StorageFullness = 0;
    [Min(0)] public float GrowthElapsedTime = 0;
    [Min(0)] public int StoreLevelUpgrade = 0;
}