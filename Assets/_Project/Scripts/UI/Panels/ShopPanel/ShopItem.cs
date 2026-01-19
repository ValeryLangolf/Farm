using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPagedItem
{
    [SerializeField] private Image _image;
    [SerializeField] private StoreLevelUpgradeButton _purchaseButton;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _gardenNameText;

    private ShopItemData? _currentData;

    public GameObject GameObject => gameObject;

    public bool HasData => _currentData.HasValue;

    public StoreLevelUpgradeButton PurchaseButton => _purchaseButton;

    public void SetData(object data)
    {
        if (data is ShopItemData upgradeData)
            UpdateInfo(upgradeData);
    }

    public void ClearData()
    {
        _currentData = null;
        _upgradeDescriptionText.text = string.Empty;
        _gardenNameText.text = string.Empty;
        _image.sprite = null;
        _purchaseButton.SetText(string.Empty);
        _purchaseButton.SetInteractable(false);
    }

    private void UpdateInfo(ShopItemData data)
    {
        _currentData = data;
        UpdateDisplay();
        UpdatePurchaseButton();
    }

    private void UpdateDisplay()
    {
        if (_currentData.HasValue == false)
            return;

        ShopItemData data = _currentData.Value;
        _upgradeDescriptionText.text = data.Description;
        _gardenNameText.text = data.GardenName;
        _image.sprite = data.Icon;
        _purchaseButton.SetText(MoneyFormatter.FormatNumber(data.Price));
    }

    private void UpdatePurchaseButton()
    {
        if (_currentData.HasValue == false)
            return;

        ShopItemData data = _currentData.Value;
        _purchaseButton.SetInteractable(data.CanPurchase);
        _purchaseButton.Button.onClick.RemoveAllListeners();

        if (data.CanPurchase == false)
            return;

        _purchaseButton.Button.onClick.AddListener(() =>
        {
            data.UpgradeRequested?.Invoke();
            data.OnButtonClick?.Invoke();
        });
    }
}