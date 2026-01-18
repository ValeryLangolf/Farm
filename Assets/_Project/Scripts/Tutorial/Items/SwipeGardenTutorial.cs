using System;
using UnityEngine;

public class SwipeGardenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;

    private Garden _garden;
    private UIDirector _uiDirector;
    private CoinCollector _coinCollector;


    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _coinCollector = ServiceLocator.Get<CoinCollector>();
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0];
    }

    public override void Activate()
    {
        _garden.ReadOnlyData.StorageFullnessChanged += OnStorageFullnessChanged;
    }


    public override void Deactivate()
    {
        _garden.ReadOnlyData.StorageFullnessChanged -= OnStorageFullnessChanged;

        _tutorial.SetCurrentItem(_nextItem);
        _cursor.Hide();
        _nextItem.Activate();
        Destroy(gameObject);
    }

    private void OnStorageFullnessChanged(float _)
    {
        _coinCollector.Collected += OnCoinCollected;

        _uiDirector.HideUpgradeShopButton();
        _uiDirector.HideUpgradesModeButton();
        _cursor.SetWorldPosition(_garden.transform.position + _cusrsorOffset)
            .SetSwipeAnimation()
            .Show();
    }


    private void OnCoinCollected()
    {
        _coinCollector.Collected -= OnCoinCollected;
        Deactivate();
    }
}
