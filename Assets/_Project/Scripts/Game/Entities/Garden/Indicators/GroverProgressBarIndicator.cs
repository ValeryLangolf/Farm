using UnityEngine;

public class GroverProgressBarIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private ProgressBar _bar;

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
        _bar.SetActive(isPurchased);

    private void OnGroverProgressChanged(float progress) =>
        _bar.SetProgress(progress);
}