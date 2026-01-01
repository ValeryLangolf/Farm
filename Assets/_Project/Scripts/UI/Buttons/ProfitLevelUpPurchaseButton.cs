using TMPro;
using UnityEngine;

public class ProfitLevelUpPurchaseButton : ButtonClickHandler 
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(string text) =>
        _text.text = text;
}