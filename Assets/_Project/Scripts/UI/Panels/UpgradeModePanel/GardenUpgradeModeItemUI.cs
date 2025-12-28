using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GardenUpgradeModeItemUI : MonoBehaviour 
{ 
    [SerializeField] private Garden _garden;
    [SerializeField] private Image _icon; //’з имеет ли смысл добавл€ть спрайт через сериалайз или лучше через инит.
    [SerializeField] private TextMeshProUGUI _buyCountText;
    [SerializeField] private TextMeshProUGUI _currentCountText;
    [SerializeField] private BuyButtonUI _buyButton;
    [SerializeField] private GameObject _childObject;

    private UIDirector _uiDirector;
    private IWallet _wallet;

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
        _wallet = ServiceLocator.Get<IWallet>();
        _buyButton.Init();
        OnUpgradeModeEnabledChanged(false);
    }

    private void OnEnable()
    {
        OnUpgradeModeCountChanged();
        OnPurchaseStatusChaged(_garden.IsPurchased);
        OnUpgradeModeEnabledChanged(_uiDirector.IsUpgradeModeActive);
        _garden.RecalculateUpgradeInfo += OnUpgradeModeCountChanged;
        _garden.PurchaseStatusChanged += OnPurchaseStatusChaged;
        _uiDirector.UpgradeModeEnabledChanged += OnUpgradeModeEnabledChanged;
        _buyButton.Clicked += OnBuyClicked;

    }

    private void OnDisable()
    {
        _garden.RecalculateUpgradeInfo -= OnUpgradeModeCountChanged;
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChaged;
        _uiDirector.UpgradeModeEnabledChanged -= OnUpgradeModeEnabledChanged;
        _buyButton.Clicked -= OnBuyClicked;
    }

    private void OnBuyClicked(ButtonClickHandler _)
    {
        _garden.MakeUpgradePlantsCount();
        OnUpgradeModeCountChanged();
    }

    private void OnUpgradeModeCountChanged()
    {
        _buyCountText.text = "+" + _garden.UpgradePlantsCount;
        _currentCountText.text = _garden.PlantsCount.ToString();
        _buyButton.SetPriceText(_garden.UpgradesCountPrice); // ќткудат-то надо получить этот текст
        _buyButton.Clicked += ApplyUpgrade;

        BuyButtonState state = _wallet.CanSpend(_garden.UpgradesCountPrice) ? BuyButtonState.Unblocked : BuyButtonState.Blocked;
        _buyButton.SetState(state); 
    }

    public void ApplyUpgrade(ButtonClickHandler _)
    {
        Upgraded?.Invoke();
    }

    private void OnPurchaseStatusChaged(bool isPurchased)
    {
        _childObject.SetActive(_uiDirector.IsUpgradeModeActive && isPurchased);
    }

    private void OnUpgradeModeEnabledChanged(bool enabled)
    {
        _childObject.SetActive(enabled && _garden.IsPurchased);
    }
}
