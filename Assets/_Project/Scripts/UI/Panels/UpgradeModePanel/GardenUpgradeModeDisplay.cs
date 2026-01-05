using UnityEngine;

public class GardenUpgradeModeDisplay : MonoBehaviour 
{ 
    [SerializeField] private Garden _garden;
    [SerializeField] private GameObject _childObject;

    private UIDirector _uiDirector;
    private IReadOnlyGardenData _data;

    private void Awake()
    {
        _data = _garden.ReadOnlyData;
        _uiDirector = ServiceLocator.Get<UIDirector>();
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