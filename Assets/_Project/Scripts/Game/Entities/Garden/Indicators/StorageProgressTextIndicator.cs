using UnityEngine;

public class StorageProgressTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressText _progressText;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_data.IsPurchased);
        _data.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnStorageProgressChanged(_data.StorageProgress);
        _data.StorageProgressChanged += OnStorageProgressChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _data.StorageProgressChanged -= OnStorageProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        _progressText.SetActive(isPurchased);

    private void OnStorageProgressChanged(float progress) =>
        _progressText.SetProgress(progress);
}