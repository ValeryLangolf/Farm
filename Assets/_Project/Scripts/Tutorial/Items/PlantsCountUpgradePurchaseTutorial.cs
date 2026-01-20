using UnityEngine;

public class PlantsCountUpgradePurchaseTutorial : TutorialItem
{
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private PlantPurchaseButton _purchaseButton;

    protected override void OnActivated()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _purchaseButton = _uiDirector.FirstGardenPlantPurchaseButton;

        _uiDirector.ProhibitShowingShopButton();

        _finger.SetParent(_purchaseButton.transform)
            .SetWorldPosition(_purchaseButton.Center.position + _fingerOffset)
            .Show()
            .SetTouchAnimation();

        _purchaseButton.Clicked += OnPurchaseButtonClicked;
    }

    protected override void OnDeactivated()
    {
        _purchaseButton.Clicked -= OnPurchaseButtonClicked;

        _uiDirector.AllowShowingShopButton();

        _finger.ResetAll();
    }

    private void OnPurchaseButtonClicked(ButtonClickHandler handler) =>
        Deactivate();
}