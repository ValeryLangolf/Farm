using System;
using TMPro;
using UnityEngine;
using VContainer;

public class PlantsCountUpgradePurchaseTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private PlantPurchaseButton _purchaseButton;

    [Inject]
    public void Construct(UIDirector uiDirector)
    {
        _uiDirector = uiDirector != null ? uiDirector : throw new ArgumentNullException(nameof(uiDirector));
    }

    protected override void OnActivated()
    {
        _purchaseButton = _uiDirector.FirstGardenPlantPurchaseButton;

        _uiDirector.ProhibitShowingShopButton();

        _finger.SetParent(_purchaseButton.transform)
            .SetWorldPosition(_purchaseButton.Center.position + _fingerOffset)
            .Show()
            .SetTouchAnimation();

        _text.Show();

        _purchaseButton.Clicked += OnPurchaseButtonClicked;
    }

    protected override void OnDeactivated()
    {
        _purchaseButton.Clicked -= OnPurchaseButtonClicked;

        _uiDirector.AllowShowingShopButton();
        _text.Hide();
        _finger.ResetAll();
    }

    private void OnPurchaseButtonClicked(ButtonClickHandler handler) =>
        Deactivate();
}