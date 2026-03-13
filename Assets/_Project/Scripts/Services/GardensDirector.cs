using System;
using UnityEngine;
using System.Collections.Generic;

public class GardensDirector : MonoBehaviour, IService
{
    [SerializeField] private List<Garden> _gardens;

    private void Awake()
    {
        if (_gardens == null || _gardens.Count == 0)
            throw new ArgumentException(nameof(_gardens));
    }

    public IReadOnlyList<Garden> Gardens => _gardens;

    public List<SavedGardenData> GetGardensData()
    {
        List<SavedGardenData> gardenData = new();

        for (int i = 0; i < _gardens.Count; i++)
            if (_gardens[i] != null)
                gardenData.Add(_gardens[i].GetData());

        return gardenData;
    }

    public void SetData(List<SavedGardenData> datas)
    {
        if (_gardens.Count != datas.Count)
            throw new Exception($"Несоответствие количества '{nameof(_gardens)}' ({_gardens.Count}) и '{nameof(datas)}' ({datas.Count})");

        for (int i = 0; i < _gardens.Count; i++)
            if (_gardens[i] != null)
                _gardens[i].SetData(datas[i], i);
    }
}