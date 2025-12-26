using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _gardenNameText;
    [SerializeField] private BuyButtonUI _buyButton;

    private UpgradeInfo _upgradeInfo;

    public event Action<UpgradeInfo> Upgraded;

    public UpgradeInfo UpgradeInfo => _upgradeInfo;

    private void OnDestroy()
    {
        _buyButton.Clicked -= ApplyUpgrade;
    }

    public void SetUpgradeInfo(UpgradeInfo upgradeInfo)
    {
        _upgradeInfo = upgradeInfo;
        _upgradeDescriptionText.text = upgradeInfo.Description;
    }

    public void Init(UpgradeInfo upgradeInfo)
    {
        SetUpgradeInfo(upgradeInfo);
        _image.sprite = _upgradeInfo.Icon;
        _buyButton.SetPriceText(upgradeInfo.Price);
        
        _buyButton.Clicked += ApplyUpgrade;
    }

    public void AllowBuy()
    {
        _buyButton.SetState(BuyButtonState.Unblocked);
    }

    public void DenyBuy()
    {
        _buyButton.SetState(BuyButtonState.Blocked);
    }

    private void ApplyUpgrade(ButtonClickHandler _)
    {
        Upgraded?.Invoke(_upgradeInfo);
    }
}

public class UpgradeInfo //“естовый класс чтобы проверить работоспособность. ѕотом передалать или удалить
{
    public UpgradeInfo(string description, float price)
    {
        Description = description;
        Price = price;
    }

    public Sprite Icon;
    public string Description;
    public float Price;
}
