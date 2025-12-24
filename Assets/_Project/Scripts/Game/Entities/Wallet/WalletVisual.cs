using TMPro;
using UnityEngine;

public class WalletVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private IWallet _wallet;

    private void OnEnable()
    {
        _wallet ??= ServiceLocator.Get<IWallet>();

        OnChanged(_wallet.Amount);
        _wallet.Changed += OnChanged;
    }

    private void OnDisable() =>
        _wallet.Changed -= OnChanged;

    private void OnChanged(float value)
    {
        string formattedText = NumberFormatter.FormatNumber(value) + Constants.DollarChar;
        _text.text = formattedText;
    }
}