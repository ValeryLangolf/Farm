using UnityEngine;

public class GardenCanSpendImageIndicator : SwitchableImage
{
    private IWallet _wallet;
    [SerializeField] private Garden _garden;

    private void OnEnable()
    {
        _wallet ??= ServiceLocator.Get<IWallet>();

        OnWalletChanged(_wallet.Amount);
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