using System;
using UnityEngine;

public class Saver
{
    private readonly ISavingUtility _savingUtility;
    private readonly SavesData _initialData;

    private SavesData _currentData = new();

    public Saver(ISavingUtility savingUtility, SavesData initialData)
    {
        _savingUtility = savingUtility ?? throw new ArgumentNullException(nameof(savingUtility));
        _initialData = initialData ?? throw new ArgumentNullException(nameof(initialData));

        Load();
    }

    public SavesData Data => _currentData;

    public void ResetProgress()
    {
        _savingUtility.DeleteSaveFile();
        _currentData = JsonUtility.FromJson<SavesData>(JsonUtility.ToJson(_initialData));
        Save();
    }

    public void Save() =>
        _savingUtility.Save(_currentData);

    private void Load()
    {
        if (_savingUtility.Load(out _currentData) == false)
        {
            _currentData = JsonUtility.FromJson<SavesData>(JsonUtility.ToJson(_initialData));

            return;
        }

        if(_initialData.GardenDatas.Count == _currentData.GardenDatas.Count)
            return;

        SynchronizeGardenData();
    }

    private void SynchronizeGardenData()
    {
        bool isDataChanged = false;

        if (_initialData.GardenDatas.Count > _currentData.GardenDatas.Count)
        {
            AddMissingGardenData();
            isDataChanged = true;
        }
        else if (_initialData.GardenDatas.Count < _currentData.GardenDatas.Count)
        {
            RemoveExtraGardenData();
            isDataChanged = true;
        }

        if (isDataChanged)
            Save();
    }

    private void AddMissingGardenData()
    {
        for (int i = _currentData.GardenDatas.Count; i < _initialData.GardenDatas.Count; i++)
        {
            GardenData newGardenData = DeepCopy(_initialData.GardenDatas[i]);
            _currentData.GardenDatas.Add(newGardenData);
        }
    }

    private void RemoveExtraGardenData()
    {
        int itemsToRemove = _currentData.GardenDatas.Count - _initialData.GardenDatas.Count;
        _currentData.GardenDatas.RemoveRange(_initialData.GardenDatas.Count, itemsToRemove);
    }

    private T DeepCopy<T>(T source) where T : class
    {
        if (source == null)
            return null;

        string json = JsonUtility.ToJson(source);

        return JsonUtility.FromJson<T>(json);
    }
}