using System;
using TMPro;
using UnityEngine;

public class PriceTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

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
        _text.SetActive(_garden.Fullness != 0 || _garden.IsPurchased == false);

        if (isPurchased)
            return;

        _text.text = NumberFormatter.FormatNumber(_garden.Price) + Constants.DollarChar;
        _text.color = Color.white;
    }

    private void OnStorageProgressChanged(float _)
    {
        _text.SetActive(_garden.Fullness != 0 || _garden.IsPurchased == false);

        if(_garden.Fullness > 0 && _garden.IsPurchased)
        {
            _text.text = NumberFormatter.FormatNumber(_garden.Fullness) + Constants.DollarChar;
            _text.color = Color.yellow;
        }
    }
}