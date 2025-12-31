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
        OnPurchaseStatusChanged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnStorageChanged(_data.StorageProgress);
        _data.StorageProgressChanged += OnStorageChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _data.StorageProgressChanged -= OnStorageChanged;
    }

    private void ProcessChanges()
    {
        bool isActive = _data.IsPurchased && _data.StorageProgress > Mathf.Epsilon;
        _indicator.SetActive(isActive);
    }

    private void OnPurchaseStatusChanged(bool _) =>
        ProcessChanges();

    private void OnStorageChanged(float _) =>
        ProcessChanges();
}