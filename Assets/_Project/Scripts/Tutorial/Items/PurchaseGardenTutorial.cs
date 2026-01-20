using TMPro;
using UnityEngine;

public class PurchaseGardenTutorial : TutorialItem
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private Garden _garden;
    private UIDirector _uiDirector;

    protected override void OnActivated()
    {
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0];
        _uiDirector = ServiceLocator.Get<UIDirector>();

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