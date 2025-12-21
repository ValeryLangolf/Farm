using UnityEngine;

public class StorageProgressBarIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressBar _bar;

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
        _bar.SetActive(isPurchased);

    private void OnStorageProgressChanged(float progress) =>
        _bar.SetProgress(progress);
}