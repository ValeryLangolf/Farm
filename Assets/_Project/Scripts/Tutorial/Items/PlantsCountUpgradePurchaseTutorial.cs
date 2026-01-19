using UnityEngine;

public class PlantsCountUpgradePurchaseTutorial : TutorialItem
{
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cursorOffset;
    [SerializeField] private TutorialItem _nextItem;

    private UIDirector _uiDirector;
    private PlantPurchaseButton _purchaseButton;

    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _purchaseButton = _uiDirector.FirstGardenPlantPurchaseButton;
    }

    public override void Activate()
    {
        _purchaseButton.Clicked += OnPurchaseButtonClicked;

        _uiDirector.HideUpgradeShopButton();
        _uiDirector.HideUpgradesModeButton();
        _cursor.SetParent(_purchaseButton.transform)
            .SetWorldPosition(_purchaseButton.Center.position + _cursorOffset)
            .Show()
            .SetTouchAnimation();
            
    }

    public override void Deactivate()
    {
        _uiDirector.ShowUpgradeShopButton();
        _uiDirector.ShowUpgradesModeButton();
        _nextItem.Activate();
        Destroy(gameObject);
    }

    private void OnPurchaseButtonClicked(ButtonClickHandler handler)
    {
        _purchaseButton.Clicked -= OnPurchaseButtonClicked;
        _cursor.ResetParent().Hide();
        Deactivate();
    }

}
