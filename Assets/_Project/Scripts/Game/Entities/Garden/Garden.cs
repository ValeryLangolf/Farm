using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private GardenData _data;

    private Grover _grover;
    private Storage _storage;

    public event Action<float> GroverProgressChanged;
    public event Action<float> StorageProgressChanged;
    public event Action<bool> PurchaseStatusChanged;

    public float GroverProgress => _grover != null ? _grover.Progress : 0;

    public float StorageProgress => _storage != null ? _storage.Progress : 0;

    public bool IsPurchased => _data.IsPurchased;

    public float Price => _data.PurchasePrice;

    public GardenData Data => _data;

    private void OnDestroy() =>
        _grover.Dispose();

    public void Initialize()
    {
        _grover?.Dispose();

        _storage = new(_data.StorageData, OnStorageProgressChanged);
        _grover = new(_data.GroverData, OnGrowCompleted, OnGroverProgressChanged);
        _grover.Grow(_data.GroverData.ElapsedTime);

        if (_data.IsPurchased && _storage.IsFilled == false)
            _grover.StartRun();

        StorageProgressChanged?.Invoke(_storage.Progress);
        GroverProgressChanged?.Invoke(_grover.Progress);
        PurchaseStatusChanged?.Invoke(_data.IsPurchased);
    }

    public void SetData(GardenData data)
    {
        _data = data;

        Initialize();
    }

    public void HandleClick()
    {
        if (_data.IsPurchased == false)
            Purchase();
    }

    public bool TryCollect(out float value)
    {
        value = 0;

        if (_data.IsPurchased)
        {
            value = _storage.GiveCoins();
            _grover.StartRun();
        }

        return _data.IsPurchased;
    }

    private void Purchase()
    {
        if (_wallet.TrySpend(_data.PurchasePrice) == false)
            return;

        _data.IsPurchased = true;
        _grover.StartRun();
        PurchaseStatusChanged?.Invoke(_data.IsPurchased);
    }

    private void OnGrowCompleted()
    {
        _storage.Increase(_data.GrowingCycleRevenuePerSinglePlant * _data.PlantCount);

        if (_storage.IsFilled)
            _grover.StopRun();
    }

    private void OnGroverProgressChanged(float progress) =>
        GroverProgressChanged?.Invoke(progress);

    private void OnStorageProgressChanged(float value)
    {
        StorageProgressChanged?.Invoke(value);

        if (_storage.IsFilled)
        {
            _grover.StopRun();
            _grover.ResetElapsedTime();

            return;
        }

        _grover.StartRun();
    }
}