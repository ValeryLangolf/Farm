using System;
using UnityEngine;
using VContainer;

public class SwipeGardenTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private Garden _garden;
    private ICoinCollector _coinCollector;

    [Inject]
    public void Construct(
        UIDirector uiDirector,
        IGardensDirector gardensDirector,
        ICoinCollector coinCollector)
    {
        _uiDirector = uiDirector != null ? uiDirector : throw new ArgumentNullException(nameof(uiDirector));
        _coinCollector = coinCollector ?? throw new ArgumentNullException(nameof(coinCollector));

        if (gardensDirector == null)
            throw new ArgumentNullException(nameof(gardensDirector));

        _garden = gardensDirector.Gardens[0];
    }

    protected override void OnActivated()
    {
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