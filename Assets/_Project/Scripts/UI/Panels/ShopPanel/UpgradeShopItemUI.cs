using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ProfitLevelUpPurchaseButton _purchaseButton;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _gardenNameText;

    private IWallet _wallet;

    private Garden _garden;

    public float Price => _garden.ReadOnlyData.LevelUpPrice;

    public bool IsPurchased => _garden.ReadOnlyData.IsPurchased;

    private void Awake()
    {
        _wallet = ServiceLocator.Get<IWallet>();
    }

    private void OnEnable()
    {
        _purchaseButton.Clicked += OnClickUpgrade;
        _wallet.Changed += OnWalletChanged;
        OnWalletChanged(_wallet.Amount);
        if(_garden != null)
        _garden.ReadOnlyData.ProfitLevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _purchaseButton.Clicked -= OnClickUpgrade;
        _wallet.Changed -= OnWalletChanged;
        if (_garden != null)
            _garden.ReadOnlyData.ProfitLevelChanged -= OnLevelChanged;
    }


    public void UpdateInfo(Garden garden, string description)
    {
        _garden = garden;
        IReadOnlyGardenData data = garden.ReadOnlyData;

        _upgradeDescriptionText.text = description;
        _gardenNameText.text = data.GardenName;
        _image.sprite = data.Icon;
        ProcessChanged();
    }

    private void ProcessChanged()
    {
        if (_garden != null)
        {
            _purchaseButton.SetInteractable(_wallet.CanSpend(_garden.ReadOnlyData.LevelUpPrice));
            _purchaseButton.SetText(MoneyFormatter.FormatNumber(_garden.ReadOnlyData.LevelUpPrice));
        }
    }

    private void OnWalletChanged(float value)
    {
        ProcessChanged();
    }

    private void OnClickUpgrade(ButtonClickHandler _) =>
        _garden.UpgradeProfit();

    private void OnLevelChanged(float obj)
    {
        ProcessChanged();
    }
}