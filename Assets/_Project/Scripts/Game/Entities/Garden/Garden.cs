using System;
using UnityEngine;

public class Garden : MonoBehaviour, ICollectable, IClickable
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _purchasePrice = 10;
    [SerializeField] private float _cultivationDurationInSeconds;
    [SerializeField] private float _plantCount;
    [SerializeField] private float _storageCapacity = 1;
    [SerializeField] private float _coinRevenue = 1;
    [SerializeField] private bool _isPurchased;

    private Storage _storage;
    private Grover _grover;

    public event Action<float> GroverProgressChanged;
    public event Action<float> StorageProgressChanged;
    public event Action<bool> PurchaseStatusChanged;

    public float GroverProgress => _grover != null ? _grover.Progress : 0;

    public float StorageProgress => _storage != null ? _storage.Progress : 0;

    public bool IsPurchased => _isPurchased;

    private void Awake()
    {
        _grover = new(_cultivationDurationInSeconds, 0, OnGrowCompleted, OnGroverProgressChanged);
        _storage = new(_storageCapacity, 0, OnStorageProgressChanged);

        if(_isPurchased)
            _grover.StartRun();
    }

    private void OnDestroy() =>
        _grover.Dispose();

    public void HandleClick()
    {
        if (_isPurchased == false)
            Purchase();
    }

    public bool TryCollect(out float value)
    {
        value = 0;

        if (_isPurchased)
        {
            value = _storage.GiveCoins();
            _grover.StartRun();
        }        

        return _isPurchased;
    }

    private void Purchase()
    {
        _isPurchased = true;
        _grover.StartRun();
        PurchaseStatusChanged?.Invoke(_isPurchased);
    }

    private void OnGrowCompleted()
    {
        _storage.Increase(_coinRevenue);

        if (_storage.IsFilled)
            _grover.StopRun();
    }

    private void OnGroverProgressChanged(float progress) =>
        GroverProgressChanged?.Invoke(progress);

    private void OnStorageProgressChanged(float value)
    {
        StorageProgressChanged?.Invoke(value);

        if (_isPurchased)
            _grover.StartRun();
        else
            _grover.StopRun();
    }
}