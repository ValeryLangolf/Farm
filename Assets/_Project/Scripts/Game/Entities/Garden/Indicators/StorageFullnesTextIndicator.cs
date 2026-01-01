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

        OnStorageProgressChanged(_data.ProfitLevel > 0);
        _data.StorageFilledChanged += OnStorageProgressChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _data.StorageFilledChanged -= OnStorageProgressChanged;
    }

    private void ProceccChanges()
    {
        float fullness = _data.StorageFullness;
        bool isActive = _data.IsPurchased && fullness > 0;
        _text.SetActive(isActive);

        if (isActive)
        {
            if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastValue, fullness))
            {
                _lastValue = fullness;
                _text.text = formattedText + Constants.DollarChar;
            }
        }
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        ProceccChanges();

    private void OnStorageProgressChanged(bool _) =>
        ProceccChanges();
}