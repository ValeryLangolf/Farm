using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModeItemUI : MonoBehaviour 
{ 
    [SerializeField] private Garden _garden;
    [SerializeField] private Image _icon; //’з имеет ли смысл добавл€ть спрайт через сериалайз или лучше через инит.
    [SerializeField] private TextMeshProUGUI _buyCountText;
    [SerializeField] private TextMeshProUGUI _currentCountText;
    [SerializeField] private BuyButtonUI _buyButton;
    [SerializeField] private GameObject _childObject;

    private UIDirector _uiDirector;

    public event Action Upgraded;

    public Garden Garden => _garden;

    private void OnDestroy()
    {
        _buyButton.Clicked -= ApplyUpgrade;
    }

    private void Awake()
    {
        _icon.sprite = _garden.Icon;
        _uiDirector = ServiceLocator.Get<UIDirector>();
    }

    private void OnEnable()
    {
        OnUpgradeModeCountChanged();
        OnPurchaseStatusChaged(_garden.IsPurchased);
        OnUpgradeModeEnabledChanged(_uiDirector.IsUpgradeModeActive);
        _garden.RecalculateUpgradeInfo += OnUpgradeModeCountChanged;
        _garden.PurchaseStatusChanged += OnPurchaseStatusChaged;
        _uiDirector.UpgradeModeEnabledChanged += OnUpgradeModeEnabledChanged;

    }

    private void OnDisable()
    {
        _garden.RecalculateUpgradeInfo -= OnUpgradeModeCountChanged;
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChaged;
        _uiDirector.UpgradeModeEnabledChanged -= OnUpgradeModeEnabledChanged;
    }

    private void OnUpgradeModeCountChanged()
    {
        _buyCountText.text = "+" + _garden.UpgradePlantsCount;
        _currentCountText.text = _garden.PlantsCount.ToString();
        _buyButton.SetPriceText(_garden.UpgradesCountPrice); // ќткудат-то надо получить этот текст
        _buyButton.Clicked += ApplyUpgrade;
    }

    public void SetBuyCountText(float count)
    {
        _buyCountText.text = count.ToString();
    }

    public void SetCurrentCountText(float count)
    {
        _currentCountText.text = count.ToString();
    }

    public void ApplyUpgrade(ButtonClickHandler _)
    {
        Upgraded?.Invoke();
    }

    private void OnPurchaseStatusChaged(bool isPurchased)
    {
        _childObject.SetActive(isPurchased);
    }

    private void OnUpgradeModeEnabledChanged(bool enabled)
    {
        _childObject.SetActive(enabled && _garden.IsPurchased);
    }

}
