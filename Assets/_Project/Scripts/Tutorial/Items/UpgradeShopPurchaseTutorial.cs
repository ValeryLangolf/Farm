using TMPro;
using UnityEngine;

public class UpgradeShopPurchaseTutorial : TutorialItem
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDIrector;
    private StoreLevelUpgradeButton _button;

    protected override void OnActivated()
    {
        _uiDIrector = ServiceLocator.Get<UIDirector>();
        _button = (_uiDIrector.FirstPagedItem as ShopItem).PurchaseButton;

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