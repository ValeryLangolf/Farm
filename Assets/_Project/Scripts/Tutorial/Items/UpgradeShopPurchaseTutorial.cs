using System;
using TMPro;
using UnityEngine;
using VContainer;

public class UpgradeShopPurchaseTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private StoreLevelUpgradeButton _button;

    [Inject]
    public void Construct(UIDirector uiDirector)
    {
        _uiDirector = uiDirector != null ? uiDirector : throw new ArgumentNullException(nameof(uiDirector));
    }

    protected override void OnActivated()
    {
        _button = (_uiDirector.FirstPagedItem as ShopItem).PurchaseButton;

        _text.SetActive(true);

        _finger.SetParent(_button.transform)
            .SetScreenSpaceOverlayPosition(_button.transform.position)
            .SetTouchAnimation()
            .Show();

        _button.Clicked += OnButtonCliked;
    }

    protected override void OnDeactivated()
    {
        _button.Clicked -= OnButtonCliked;

        _text.SetActive(false);
        _finger.ResetAll();
    }

    private void OnButtonCliked(ButtonClickHandler handler) =>
        Deactivate();
}