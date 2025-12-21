using UnityEngine;

public class StorageProgressTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressText _progressText;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnStorageProgressChanged(_garden.StorageProgress);
        _garden.StorageProgressChanged += OnStorageProgressChanged;
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _garden.StorageProgressChanged -= OnStorageProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        _progressText.SetActive(isPurchased);

    private void OnStorageProgressChanged(float progress) =>
        _progressText.SetProgress(progress);
}