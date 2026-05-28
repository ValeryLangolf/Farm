using System;
using UnityEngine;
using VContainer;

public class GardenUpgradeModeDisplay : MonoBehaviour, IInjactable
{ 
    [SerializeField] private Garden _garden;
    [SerializeField] private GameObject _childObject;

    private UIDirector _uiDirector;
    private IReadOnlyGardenData _data;

    [Inject]
    public void Construct(UIDirector uIDirector)
    {
        _uiDirector = uIDirector != null ? uIDirector : throw new ArgumentNullException(nameof(uIDirector));
    }

    private void Awake()
    {
        _data = _garden.ReadOnlyData;
    }

    private void OnEnable()
    {
        OnPurchaseStatusChaged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChaged();

        OnUpgradeModeEnabledChanged();
        _uiDirector.UpgradeModeEnabledStatusChanged += (_) => OnUpgradeModeEnabledChanged();
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChaged();
        _uiDirector.UpgradeModeEnabledStatusChanged -= (_) => OnUpgradeModeEnabledChanged();
    }

    private void ProcessVisibleStateChanges() =>
        _childObject.SetActive(_uiDirector.IsUpgradeModeActive && _data.IsPurchased);

    private void OnPurchaseStatusChaged() =>
        ProcessVisibleStateChanges();

    private void OnUpgradeModeEnabledChanged() =>
        ProcessVisibleStateChanges();
}