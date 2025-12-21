using UnityEngine;

public class GroverProgressBarIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressBar _bar;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnGroverProgressChanged(_garden.GroverProgress);
        _garden.GroverProgressChanged += OnGroverProgressChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _garden.GroverProgressChanged -= OnGroverProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        _bar.SetActive(isPurchased);

    private void OnGroverProgressChanged(float progress) =>
        _bar.SetProgress(progress);
}