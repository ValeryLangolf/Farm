using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _gardenNameText;
    [SerializeField] private ProfitLevelUpPurchaseButton _purchaseButton;

    private Garden _garden;

    private void OnEnable()
    {
        _purchaseButton.Clicked += OnClickUpgrade;
    }

    private void OnDisable()
    {
        _purchaseButton.Clicked -= OnClickUpgrade;
    }
    public void Init(Garden garden, string description)
    {
        _garden = garden;
        _upgradeDescriptionText.text = description;
        _gardenNameText.text = _garden.ReadOnlyData.GardenName;
        _image.sprite = _garden.ReadOnlyData.Icon;
    }

    private void OnClickUpgrade(ButtonClickHandler _)
    {
        _garden.UpgradeProfit();
    }

}
