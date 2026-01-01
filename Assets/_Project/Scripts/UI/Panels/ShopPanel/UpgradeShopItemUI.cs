using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ProfitLevelUpPurchaseButton _purchaseButton;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _gardenNameText;

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
        IReadOnlyGardenData data = garden.ReadOnlyData;
        IWallet wallet = ServiceLocator.Get<IWallet>();

        _upgradeDescriptionText.text = description;
        _gardenNameText.text = data.GardenName;
        _image.sprite = data.Icon;
        _purchaseButton.SetText(MoneyFormatter.FormatNumber(data.LevelUpPrice));
    }

    private void OnClickUpgrade(ButtonClickHandler _) =>
        _garden.UpgradeProfit();
}