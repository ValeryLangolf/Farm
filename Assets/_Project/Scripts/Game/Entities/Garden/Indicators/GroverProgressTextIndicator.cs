using UnityEngine;

public class GroverProgressTextIndicator : MonoBehaviour
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

        OnGroverProgressChanged(_data.GroverProgress);
        _data.GroverProgressChanged += OnGroverProgressChanged;
    }

    private void OnDisable()
    {
        _data.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _data.GroverProgressChanged -= OnGroverProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased) =>
        _progressText.SetActive(isPurchased);

    private void OnGroverProgressChanged(float progress) =>
        _progressText.SetProgress(progress);
}