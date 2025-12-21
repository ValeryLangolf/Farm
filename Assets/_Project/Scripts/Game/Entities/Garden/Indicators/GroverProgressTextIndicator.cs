using UnityEngine;

public class GroverProgressTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressText _progressText;

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
        _progressText.SetActive(isPurchased);

    private void OnGroverProgressChanged(float progress) =>
        _progressText.SetProgress(progress);
}