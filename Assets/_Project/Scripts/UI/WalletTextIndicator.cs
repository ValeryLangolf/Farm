using System;
using TMPro;
using UnityEngine;
using VContainer;

public class WalletTextIndicator : MonoBehaviour, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;

    private IWallet _wallet;
    private float _lastValue;

    [Inject]
    public void Construct(IWallet wallet)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
    }

    private void OnEnable()
    {
        _wallet.Changed += OnChanged;
        OnChanged(_wallet.Amount);
    }

    private void OnDisable() =>
        _wallet.Changed -= OnChanged;

    private void OnChanged(float value) =>
        UpdateTextIfNeeded(value);

    private void UpdateTextIfNeeded(float value)
    {
        if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastValue, value) == false)
            return;

        _lastValue = value;
        _text.text = formattedText + Constants.DollarChar;
    }
}