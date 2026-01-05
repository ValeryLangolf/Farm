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
        OnPurchaseStatusChanged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChanged();

        OnStorageFilledChanged();
        _data.StorageFullnessChanged += (_) => OnStorageFilledChanged();
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChanged();
        _data.StorageFullnessChanged -= (_) => OnStorageFilledChanged();
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

    private void OnPurchaseStatusChanged() =>
        ProceccChanges();

    private void OnStorageFilledChanged() =>
        ProceccChanges();
}