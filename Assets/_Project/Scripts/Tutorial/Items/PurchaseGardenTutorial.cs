using System;
using TMPro;
using UnityEngine;
using VContainer;

public class PurchaseGardenTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private Garden _garden;

    [Inject]
    public void Construct(UIDirector uiDirector, IGardensDirector gardensDirector)
    {
        _uiDirector = uiDirector != null ? uiDirector : throw new ArgumentNullException(nameof(uiDirector));

        if (gardensDirector == null)
            throw new ArgumentNullException(nameof(gardensDirector));

        _garden = gardensDirector.Gardens[0];
    }

    protected override void OnActivated()
    {
        _text.Show();
        _uiDirector.ProhibitShowingShopButton();
        _uiDirector.ProhibitShowingUpgradesModeButton();

        _finger.SetWorldPosition(_garden.transform.position + _fingerOffset)
            .SetParent(_garden.transform)
            .SetTouchAnimation()
            .Show();

        _garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
        OnPurchaseStatusChanged(_garden.ReadOnlyData.IsPurchased);
    }

    protected override void OnDeactivated()
    {
        _garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;

        _text.Hide();
        _uiDirector.AllowShowingUpgradesModeButton();
        _uiDirector.AllowShowingShopButton();
        _finger.ResetAll();
    }

    private void OnPurchaseStatusChanged(bool isPurchased)
    {
        if (isPurchased)
            Deactivate();
    }
}