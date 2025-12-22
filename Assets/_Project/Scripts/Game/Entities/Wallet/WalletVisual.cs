using TMPro;
using UnityEngine;

public class WalletVisual : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        OnChanged(_wallet.Amount);
        _wallet.Changed += OnChanged;
    }

    private void OnDisable()
    {
        _wallet.Changed -= OnChanged;
    }

    private void OnChanged(float value)
    {
        string formattedText = NumberFormatter.FormatNumber(value) + Constants.DollarChar;
        _text.text = formattedText;
    }
}