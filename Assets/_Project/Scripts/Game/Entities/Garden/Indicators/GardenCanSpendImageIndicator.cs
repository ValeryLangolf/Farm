using UnityEngine;

public class GardenCanSpendImageIndicator : SwitchableImage
{
    [SerializeField] private Garden _garden;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;

    private void Awake()
    {
        _data = _garden.ReadOnlyData;
        _wallet = ServiceLocator.Get<IWallet>();
    }

    private void OnEnable()
    {
        OnWalletChanged(_wallet.Amount);
        _wallet.Changed += OnWalletChanged;

        OnPurchaseStatusChanged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _wallet.Changed -= OnWalletChanged;
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void OnWalletChanged(float value) =>
        UpdateState(value >= _data.GardenPurchasePrice);

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        SetActiveIcon(isPurchased == false);
}