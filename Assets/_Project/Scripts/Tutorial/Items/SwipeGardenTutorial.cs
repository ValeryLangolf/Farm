using UnityEngine;

public class SwipeGardenTutorial : TutorialItem
{
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private Garden _garden;
    private UIDirector _uiDirector;
    private CoinCollector _coinCollector;

    protected override void OnActivated()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _coinCollector = ServiceLocator.Get<CoinCollector>();
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0];

        _uiDirector.ProhibitShowingShopButton();
        _uiDirector.ProhibitShowingUpgradesModeButton();

        _garden.ReadOnlyData.StorageFullnessChanged += OnStorageFullnessChanged;
        _coinCollector.Collected += OnCoinCollected;
        OnStorageFullnessChanged(_garden.ReadOnlyData.StorageFullness);
    }

    protected override void OnDeactivated()
    {
        _garden.ReadOnlyData.StorageFullnessChanged -= OnStorageFullnessChanged;
        _coinCollector.Collected -= OnCoinCollected;

        _uiDirector.AllowShowingUpgradesModeButton();
        _uiDirector.AllowShowingShopButton();
        _finger.ResetAll();
    }

    private void OnStorageFullnessChanged(float _)
    {
        _finger.SetWorldPosition(_garden.transform.position + _fingerOffset)
            .SetSwipeAnimation()
            .Show();
    }

    private void OnCoinCollected() =>
        Deactivate();
}