using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavesData
{
    public float MusicVolume;
    public float SfxVolume;
    public List<LocationData> Locations;

    public SavesData(SavesData other)
    {
        MusicVolume = other.MusicVolume;
        SfxVolume = other.SfxVolume;
        Locations = new(other.Locations);

        Locations = new();

        foreach(LocationData location in other.Locations)
        {
            Locations.Add(new(location));
        }
    }
}

[Serializable]
public class LocationData
{
    public int TutorialCounter = 0;
    public float WalletAmount = 0;
    public long LastServerTime = -1;
    public List<SavedGardenData> GardenDatas = new();

    public LocationData() { }

    public LocationData(LocationData other)
    {
        TutorialCounter = other.TutorialCounter;
        WalletAmount = other.WalletAmount;
        LastServerTime = other.LastServerTime;

        GardenDatas = new();

        foreach (SavedGardenData gardenData in other.GardenDatas)
        {
            GardenDatas.Add(new(gardenData));
        }
    }
}

[Serializable]
public class SavedGardenData
{
    [Min(0)] public int PlantsCount = 0;
    [Min(0)] public float StorageFullness = 0;
    [Min(0)] public float GrowthElapsedTime = 0;
    [Min(0)] public int StoreLevelUpgrade = 0;

    public SavedGardenData(SavedGardenData other)
    {
        PlantsCount = other.PlantsCount;
        StorageFullness = other.StorageFullness;
        GrowthElapsedTime = other.GrowthElapsedTime;
        StoreLevelUpgrade = other.StoreLevelUpgrade;
    }
}