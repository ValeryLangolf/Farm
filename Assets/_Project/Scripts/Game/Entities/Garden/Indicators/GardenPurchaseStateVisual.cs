using UnityEngine;

public class GardenPurchaseStateVisual : SwitchableSprite
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Color _enoughMoneyColor;

    private IWallet _wallet;

    private void OnEnable()
    {
        _wallet ??= ServiceLocator.Get<IWallet>();

        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnWalletChanged(_wallet.Amount);
        _wallet.Changed += OnWalletChanged;
    }

    private void OnDisable() =>
        _wallet.Changed -= OnWalletChanged;

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