using UnityEngine;

public class GardenPurchaseStateIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Color _enoughMoneyColor;
    [SerializeField] private GameObject _plant;

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
        _plant.SetActive(_data.IsPurchased);
    }

    private void OnPurchaseStatusChanged() =>
        ProcessChanged();

    private void OnWalletChanged() =>
        ProcessChanged();
}