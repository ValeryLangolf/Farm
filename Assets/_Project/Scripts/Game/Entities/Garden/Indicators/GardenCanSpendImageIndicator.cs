using System;
using UnityEngine;
using VContainer;

public class GardenCanSpendImageIndicator : SwitchableImage, IInjactable
{
    [SerializeField] private Garden _garden;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;

    [Inject]
    public void Construct(IWallet wallet)
    {
        _data = _garden.ReadOnlyData;
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
    }

    private void OnEnable()
    {
        OnWalletChanged();
        _wallet.Changed += (_) => OnWalletChanged();

        OnPurchaseStatusChanged();
        _data.PurchaseStatusChanged += (_) => OnPurchaseStatusChanged();
    }

    private void OnDisable()
    {
        _wallet.Changed -= (_) => OnWalletChanged();
        _data.PurchaseStatusChanged -= (_) => OnPurchaseStatusChanged();
    }

    private void ProcessChanges()
    {
        bool isActive = _data.IsPurchased == false;
        SetActiveIcon(isActive);

        if(isActive)
            UpdateState(_wallet.Amount >= _data.GardenPurchasePrice);
    }

    private void OnWalletChanged() =>
        ProcessChanges();

    private void OnPurchaseStatusChanged() =>
        ProcessChanges();
}