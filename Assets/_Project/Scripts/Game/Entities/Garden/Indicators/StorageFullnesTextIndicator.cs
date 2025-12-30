using TMPro;
using UnityEngine;

public class StorageFullnesTextIndicator : MonoBehaviour
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

        OnStorageProgressChanged(_data.StorageProgress);
        _data.StorageProgressChanged += OnStorageProgressChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _data.StorageProgressChanged -= OnStorageProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        if (isPurchased == false)
        {
            _text.SetActive(false);
            return;
        }

        OnStorageProgressChanged(_data.StorageProgress);
    }

    private void OnStorageProgressChanged(float _)
    {
        float fullness = _data.StorageFullness;

        _text.SetActive(fullness > 0);

        if (fullness > 0)
        {
            if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastValue, fullness) == false)
                return;

            _lastValue = fullness;
            _text.text = formattedText + Constants.DollarChar;
        }
    }
}