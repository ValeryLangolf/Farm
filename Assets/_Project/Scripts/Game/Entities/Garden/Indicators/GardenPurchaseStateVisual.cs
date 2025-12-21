using UnityEngine;

public class GardenPurchaseStateVisual : SwitchableSprite
{
    [SerializeField] private Garden _garden;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        UpdateState(isPurchased);
}