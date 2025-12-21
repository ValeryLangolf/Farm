using UnityEngine;

public class GardenVisual : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private SpriteRenderer _gardenImage;
    [SerializeField] private Sprite _saleSprite;
    [SerializeField] private Sprite _purchasedSprite;
    [SerializeField] private ProgressText _groverProgressText;
    [SerializeField] private ProgressText _storageProgressText;
    [SerializeField] private ProgressBar _groverProgressBar;
    [SerializeField] private ProgressBar _storageProgressBar;
    [SerializeField] private GameObject _saleIcon;

    private void OnEnable()
    {
        OnPurchaseStatusChanged(_garden.IsPurchased);
        _garden.PurchaseStatusChanged += OnPurchaseStatusChanged;

        OnGroverProgressChanged(_garden.GroverProgress);
        _garden.GroverProgressChanged += OnGroverProgressChanged;

        OnStorageProgressChanged(_garden.StorageProgress);
        _garden.StorageProgressChanged += OnStorageProgressChanged;

        
    }

    private void OnDisable()
    {
        _garden.PurchaseStatusChanged -= OnPurchaseStatusChanged;
        _garden.GroverProgressChanged -= OnGroverProgressChanged;
        _garden.StorageProgressChanged -= OnStorageProgressChanged;
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        _gardenImage.sprite = isPurchased ? _purchasedSprite : _saleSprite;

        _groverProgressText.SetActive(isPurchased);
        _storageProgressText.SetActive(isPurchased);
        _groverProgressBar.SetActive(isPurchased);
        _storageProgressBar.SetActive(isPurchased);
        _saleIcon.SetActive(isPurchased == false);
    }

    private void OnGroverProgressChanged(float progress)
    {
        _groverProgressBar.SetProgress(progress);
        _groverProgressText.SetProgress(progress);
    }

    private void OnStorageProgressChanged(float progress)
    {
        _storageProgressBar.SetProgress(progress);
        _storageProgressText.SetProgress(progress);
    }
}