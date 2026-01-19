using TMPro;
using UnityEngine;

public class UpgradeShopOpenTutorial : TutorialItem
{
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cursorOffset;
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private TextMeshProUGUI _text;

    private OpenerShopPanelButton _button;

    private UIDirector _uiDIrector;
    private IWallet _wallet;

    private void Awake()
    {
        _uiDIrector = ServiceLocator.Get<UIDirector>();
        _wallet = ServiceLocator.Get<IWallet>();
        _button = _uiDIrector.OpenerShopPanelButton;
        _text.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        _uiDIrector.HideUpgradeShopButton();
        _wallet.Changed += OnWalletChanged;
    }
    public override void Deactivate()
    {
        _cursor.Hide()
            .ResetParent();
        _wallet.Changed -= OnWalletChanged;
       _nextItem.Activate();
        Destroy(gameObject);
    }

    private void OnWalletChanged(float value)
    {
        Debug.Log("Стоимость " + value + "Самая дешевая " + _uiDIrector.CheapestUpgradePrice);

       if(value >= _uiDIrector.CheapestUpgradePrice)
        {
            
            _text.gameObject.SetActive(true);
            _uiDIrector.ShowUpgradeShopButton();
            _button.Clicked += OnButtonCliked;
            _cursor.Show()
                .SetTouchAnimation()
                .SetScreenPosition(_button.Center.position + _cursorOffset)
                .SetParent(_button.transform);
        }
    }

    private void OnButtonCliked(ButtonClickHandler handler)
    {
        _button.Clicked -= OnButtonCliked;
        Deactivate();
    }
}
