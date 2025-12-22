using UnityEngine;

public class GardenPurchaseStateVisual : SwitchableSprite
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Color _enoughMoneyColor;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnWalletChanged(_wallet.Amount);
        _wallet.Changed += OnWalletChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        if (isPurchased)
            SetColor(Color.white);

        UpdateState(isPurchased);
    }

    private void OnWalletChanged(float value)
    {
        Color color = _garden.IsPurchased || _wallet.CanSpend(_garden.Price) == false ? Color.white : _enoughMoneyColor;
        SetColor(color);
    }
}