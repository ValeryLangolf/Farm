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
        OnPurchaseStatusChaged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChaged;

        OnUpgradeModeEnabledChanged(_uiDirector.IsUpgradeModeActive);
        _uiDirector.UpgradeModeEnabledChanged += OnUpgradeModeEnabledChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChaged;
        _uiDirector.UpgradeModeEnabledChanged -= OnUpgradeModeEnabledChanged;
    }

    private void ProcessVisibleStateChanges() =>
        _childObject.SetActive(_uiDirector.IsUpgradeModeActive && _data.IsPurchased);

    private void OnPurchaseStatusChaged(bool _) =>
        ProcessVisibleStateChanges();

    private void OnUpgradeModeEnabledChanged(bool enabled) =>
        _childObject.SetActive(enabled && _data.IsPurchased);
}