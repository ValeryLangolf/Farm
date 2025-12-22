using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModeItemUI : UIPanel
{
    [SerializeField] private Image _icon; //’з имеет ли смысл добавл€ть спрайт через сериалайз или лучше через инит.
    [SerializeField] private TextMeshProUGUI _buyCountText;
    [SerializeField] private TextMeshProUGUI _currentCountText;
    [SerializeField] private BuyButtonUI _buyButton;

    public event Action Upgraded;

    private void OnDestroy()
    {
        _buyButton.Clicked -= ApplyUpgrade;
    }

    public void Init(Sprite icon, float buyCount, float currentCount)
    {
        _icon.sprite = icon;
        _buyCountText.text = "+" + buyCount;
        _currentCountText.text += currentCount;
        _buyButton.SetPriceText(100); // ќткудат-то надо получить этот текст
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

    public void ApplyUpgrade()
    {
        Upgraded?.Invoke();
    }
}
