using System;
using UnityEngine;

public class PurchaseGardenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private Garden _garden;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;

    private UIDirector _uiDirector;

    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
    }

    private void OnEnable()
    {
        _garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    public override void Activate()
    {
        _uiDirector.HideUpgradesButtons();
        _cursor.SetPosition(_garden.transform.position + _cusrsorOffset)
            .SetTouchAnimation()
            .Show();
    }

    public override void Deactivate()
    {
        //_tutorial.SetCurrentItem(_nextItem);
        _cursor.Hide();
        Destroy(gameObject);
    }

    private void OnPurchaseStatusChanged(bool isChanged)
    {
        if (isChanged)
        {
            Deactivate();
        }
    }
}
