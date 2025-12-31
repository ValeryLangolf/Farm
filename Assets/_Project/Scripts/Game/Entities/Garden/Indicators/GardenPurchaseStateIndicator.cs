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
        OnPurchaseStatusChanged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnWalletChanged(_wallet.Amount);
        _wallet.Changed += OnWalletChanged;
    }

    private void OnDisable() =>
        _wallet.Changed -= OnWalletChanged;

    private void ProcessChanged()
    {
        if (_data.IsPurchased)
            SetColor(Color.white);

        UpdateState(_data.IsPurchased);

        Color color = _data.IsPurchased || _wallet.CanSpend(_data.GardenPurchasePrice) == false ? Color.white : _enoughMoneyColor;
        SetColor(color);
    }

    private void OnPurchaseStatusChanged(bool _) =>
        ProcessChanged();

    private void OnWalletChanged(float _) =>
        ProcessChanged();
}