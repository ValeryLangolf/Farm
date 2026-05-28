using System;
using UnityEngine;
using VContainer;

public class GardenPurchaseStateIndicator : MonoBehaviour, IInjactable
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Color _enoughMoneyColor;
    [SerializeField] private GameObject _plant;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;

    [Inject]
    public void Construct(IWallet wallet)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
    }

    private void Awake() =>
        _data = _garden.ReadOnlyData;

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
        _plant.SetActive(_data.IsPurchased);
    }

    private void OnPurchaseStatusChanged() =>
        ProcessChanged();

    private void OnWalletChanged() =>
        ProcessChanged();
}