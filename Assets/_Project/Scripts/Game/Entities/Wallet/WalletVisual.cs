using TMPro;
using UnityEngine;

public class WalletVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private IWallet _wallet;
    private float _lastValue;

    private void OnEnable()
    {
        _wallet ??= ServiceLocator.Get<IWallet>();

        OnChanged(_wallet.Amount);
        UpdateTextIfNeeded(_wallet.Amount);
        _wallet.Changed += OnChanged;
    }

    private void OnChanged(float value) =>
        UpdateTextIfNeeded(value);

    private void UpdateTextIfNeeded(float value)
    {
        if (NumberFormatter.NeedUpdateText(out string formattedText, _lastValue, value) == false)
            return;

        _lastValue = value;
        _text.text = formattedText + Constants.DollarChar;
    }
}