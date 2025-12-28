using TMPro;
using UnityEngine;

public class BuyButtonUI : ButtonClickHandler
{
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Color _blockedColor;

    private Color _unblockedColor = Color.white;
    private float _lastPrice;
    private BuyButtonState _state;

    public void Init()
    {
        SetState(_state);
        SetPriceText(0);
    }

    public void SetState(BuyButtonState state)
    {
        _state = state;
        ApplyStateActions();
    }

    public void SetPriceText(float price)
    {
        _priceText.text = price.ToString();

        if (NumberFormatter.NeedUpdateText(out string formattedText, _lastPrice, price))
            _priceText.text = formattedText;
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
        SetInteractable(false);
        SetColor(_blockedColor); //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }

    private void ApplyUnblockedStateActions()
    {
        SetInteractable(true);
        SetColor(_unblockedColor); //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }
}

public enum BuyButtonState
{
    Blocked,
    Unblocked
}