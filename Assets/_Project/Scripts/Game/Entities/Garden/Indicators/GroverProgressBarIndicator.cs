using System;
using UnityEngine;

public class GroverProgressBarIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressBar _bar;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPurchaseStatusChanged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChanged();

        OnGroverProgressChanged();
        _data.GrowthProgressChanged += (_) => OnGroverProgressChanged();

        OnStorageFilledChanged();
        _data.StorageFullnessChanged += (_) => OnStorageFilledChanged();
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChanged();
        _data.GrowthProgressChanged -= (_) => OnGroverProgressChanged();
        _data.StorageFullnessChanged -= (_) => OnStorageFilledChanged();
    }

    private void ProcessChanges()
    {
        bool isActive = _data.IsPurchased && (_data.IsStorageInfinity || _data.StorageFullness == 0);

        _bar.SetActive(isActive);

        if (isActive)
            _bar.SetProgress(_data.GrowthProgress);
    }

    private void OnPurchaseStatusChanged() =>
        ProcessChanges();

    private void OnGroverProgressChanged() =>
        ProcessChanges();

    private void OnStorageFilledChanged() =>
        ProcessChanges();
}