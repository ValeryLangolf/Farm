using UnityEngine;

public class GardenCanSpendImageIndicator : SwitchableImage
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Garden _garden;

    private void OnEnable()
    {
        OnWalletChanged(_wallet.Value);
        _wallet.Changed += OnWalletChanged;

        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _wallet.Changed -= OnWalletChanged;
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void OnWalletChanged(float value) =>
        UpdateState(value >= _garden.Price);

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        SetActiveIcon(isPurchased == false);
}