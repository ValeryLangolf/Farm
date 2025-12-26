using TMPro;
using UnityEngine;

public class StorageFullnesTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

    private float _lastValue;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnStorageProgressChanged(_garden.StorageProgress);
        _garden.StorageProgressChanged += OnStorageProgressChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _garden.StorageProgressChanged -= OnStorageProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        if (isPurchased == false)
        {
            _text.SetActive(false);
            return;
        }

        OnStorageProgressChanged(_garden.StorageProgress);
    }

    private void OnStorageProgressChanged(float _)
    {
        float fullness = _garden.Fullness;

        _text.SetActive(fullness > 0);

        if (fullness > 0)
        {
            if (NumberFormatter.NeedUpdateText(out string formattedText, _lastValue, fullness) == false)
                return;

            _lastValue = fullness;
            _text.text = formattedText + Constants.DollarChar;
        }
    }
}