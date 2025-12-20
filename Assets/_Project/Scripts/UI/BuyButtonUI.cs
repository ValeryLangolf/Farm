using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Color _blockedColor;

    private Color _unblockedColor = Color.white;    

    private BuyButtonState _state;

    public event Action Clicked;

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void Init(BuyButtonState state, float price)
    {
        SetState(state);
        SetPriceText(price);
        _button.onClick.AddListener(OnClick);
    }

    public void SetState(BuyButtonState state)
    {
        _state = state;
        ApplyStateActions();
    }

    public void SetPriceText(float price)
    {
        _priceText.text = price.ToString();
    }

    private void OnClick()
    {
        Clicked?.Invoke();
    }

    private void ApplyStateActions()
    {
        switch (_state)
        {
            case BuyButtonState.Blocked:
                ApplyBlockedStateActions();
                break;

                case BuyButtonState.Unblocked:
                ApplyUnblockedStateActions();
                break;
        }
    }

    private void ApplyBlockedStateActions()
    {
        _button.interactable = false;
        _button.image.color = _blockedColor; //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }

    private void ApplyUnblockedStateActions()
    {
        _button.interactable = true;
        _button.image.color = _unblockedColor; //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }

}

public enum BuyButtonState
{
    Blocked,
    Unblocked
}