using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private string _upgradeDescription;
    [SerializeField] private string _gardenName;
    [SerializeField] private BuyButtonUI _buyButton;

    private UpgradeInfo _upgradeInfo;

    public event Action<UpgradeInfo> Upgraded;

    private void OnDestroy()
    {
        _buyButton.Clicked -= ApplyUpgrade;
    }

    public void Init(UpgradeInfo upgradeInfo, float price)
    {
        SetUpgradeInfo(upgradeInfo);
        _image.sprite = _upgradeInfo.Icon;
        _buyButton.SetPriceText(price);
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

    public void SetUpgradeInfo(UpgradeInfo upgradeInfo)
    {
        _upgradeInfo = upgradeInfo;
    }

    private void ApplyUpgrade()
    {
        Debug.Log("Apply");
        Upgraded?.Invoke(_upgradeInfo);
    }
}

public class UpgradeInfo //“естовый класс чтобы проверить работоспособность. ѕотом передалать или удалить
{
    public Sprite Icon;
    public string Description;
}
