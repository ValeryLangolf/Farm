using TMPro;
using UnityEngine;

public class StoreLevelUpgradeButton : ButtonClickHandler 
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(string text) =>
        _text.text = text;
}