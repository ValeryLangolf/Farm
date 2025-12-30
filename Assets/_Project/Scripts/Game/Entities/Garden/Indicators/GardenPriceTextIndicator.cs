using TMPro;
using UnityEngine;

public class GardenPriceTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

    private IReadOnlyGardenData _data;
    private float _lastValue;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable() =>
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        _text.SetActive(_data.IsPurchased == false);

        if (isPurchased)
            return;

        if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastValue, _data.GardenPurchasePrice) == false)
            return;

        _lastValue = _data.GardenPurchasePrice;
        _text.text = formattedText + Constants.DollarChar;
    }
}
