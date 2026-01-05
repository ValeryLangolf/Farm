using System;
using UnityEditor;
using UnityEngine;

public class StoragePartialFillingIconIndicator : MonoBehaviour 
{
    [SerializeField] private Garden _garden;
    [SerializeField] private GameObject _indicator;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPurchaseStatusChanged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChanged();

        OnStorageChanged();
        _data.StorageFullnessChanged += (_) => OnStorageChanged();
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChanged();
        _data.StorageFullnessChanged -= (_) => OnStorageChanged();
    }

    private void ProcessChanges()
    {
        bool isActive = _data.IsPurchased && _data.StorageFullness > 0;
        _indicator.SetActive(isActive);
    }

    private void OnPurchaseStatusChanged() =>
        ProcessChanges();

    private void OnStorageChanged() =>
        ProcessChanges();
}