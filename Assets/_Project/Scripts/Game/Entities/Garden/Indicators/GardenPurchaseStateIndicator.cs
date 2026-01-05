using UnityEngine;

public class GardenPurchaseStateIndicator : SwitchableSprite
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Color _enoughMoneyColor;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;

    private void Awake()
    {
        _data = _garden.ReadOnlyData;
        _wallet = ServiceLocator.Get<IWallet>();
    }

    private void OnEnable()
    {
        OnPurchaseStatusChanged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChanged();

        OnWalletChanged();
        _wallet.Changed += (_) => OnWalletChanged();
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChanged();
        _wallet.Changed -= (_) => OnWalletChanged();
    }

    private void ProcessChanged()
    {
        if (_data.IsPurchased)
            SetColor(Color.white);

        UpdateState(_data.IsPurchased);

        Color color = _data.IsPurchased || _wallet.CanSpend(_data.GardenPurchasePrice) == false ? Color.white : _enoughMoneyColor;
        SetColor(color);
    }

    private void OnPurchaseStatusChanged() =>
        ProcessChanged();

    private void OnWalletChanged() =>
        ProcessChanged();
}