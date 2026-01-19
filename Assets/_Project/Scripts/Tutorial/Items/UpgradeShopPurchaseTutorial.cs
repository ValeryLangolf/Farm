using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeShopPurchaseTutorial : TutorialItem
{
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cursorOffset;
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private TextMeshProUGUI _text;

    private StoreLevelUpgradeButton _button;

    private UIDirector _uiDIrector;

    private void Awake()
    {
        _uiDIrector = ServiceLocator.Get<UIDirector>();
        ShopItem shopItem = _uiDIrector.FirstPagedItem as ShopItem;


        _button = shopItem.PurchaseButton;
        _text.gameObject.SetActive(false);

        _button.Clicked += OnButtonCliked;
    }

    public override void Activate()
    {
        _cursor.SetParent(_button.transform)
            .SetScreenPosition(_button.transform.position)
            .SetTouchAnimation()
            .Show();
    }

    public override void Deactivate()
    {
        _cursor.Hide()
            .ResetParent();
       
        //_nextItem.Activate();
        Destroy(gameObject);
    }

    private void OnButtonCliked(ButtonClickHandler handler)
    {
        _button.Clicked -= OnButtonCliked;
        Deactivate();
    }
}
