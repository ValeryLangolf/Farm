using TMPro;
using UnityEngine;

public class GardenPriceTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

    private float _lastValue;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        _text.SetActive(_garden.IsPurchased == false);

        if (isPurchased)
            return;

        if (NumberFormatter.NeedUpdateText(out string formattedText, _lastValue, _garden.Price) == false)
            return;

        _lastValue = _garden.Price;
        _text.text = formattedText + Constants.DollarChar;
    }
}
