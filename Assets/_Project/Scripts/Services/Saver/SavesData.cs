using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavesData
{
    public float MusicVolume = 0.5f;
    public float SfxVolume = 0.75f;
    public List<LocationData> Locations = new();

    public SavesData() { }

    public SavesData(float musicVolume, float sfxVolume, List<LocationData> locations)
    {
        MusicVolume = musicVolume;
        SfxVolume = sfxVolume;
        Locations = locations;
    }

    public SavesData(SavesData other)
        : this(other.MusicVolume, other.SfxVolume, other.Locations)
    { }
}

[Serializable]
public class SavedGardenData
{
    [Min(0)] public int PlantsCount = 0;
    [Min(0)] public float StorageFullness = 0;
    [Min(0)] public float GrowthElapsedTime = 0;
    [Min(0)] public int StoreLevelUpgrade = 0;
}

[Serializable]
public class LocationData
{
    public int TutorialCounter = 0;
    public float WalletAmount = 0;
    public long LastServerTime = -1;
    public List<SavedGardenData> GardenDatas = new();
}