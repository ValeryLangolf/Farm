using System;
using TMPro;
using UnityEngine;
using VContainer;

public class UpgradeShopOpenTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private OpenerShopPanelButton _button;
    private UIDirector _uiDIrector;
    private IWallet _wallet;

    [Inject]
    public void Construct(IWallet wallet, UIDirector uiDIrector)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _uiDIrector = uiDIrector != null ? uiDIrector : throw new ArgumentNullException(nameof(uiDIrector));
    }

    protected override void OnActivated()
    {
        _button = _uiDIrector.OpenerShopPanelButton;

        _text.SetActive(false);
        _uiDIrector.ProhibitShowingShopButton();

        _wallet.Changed += OnWalletChanged;
        OnWalletChanged(_wallet.Amount);

        _button.Clicked += OnButtonCliked;
    }

    protected override void OnDeactivated()
    {
        _wallet.Changed -= OnWalletChanged;
        _button.Clicked -= OnButtonCliked;

        _uiDIrector.AllowShowingShopButton();
        _text.SetActive(false);
        _finger.ResetAll();
    }

    private void OnWalletChanged(float value)
    {
        if (value >= _uiDIrector.CheapestUpgradePrice)
        {
            _text.gameObject.SetActive(true);
            _uiDIrector.AllowShowingShopButton();

            _finger.SetTouchAnimation()
                .SetParent(_button.transform)
                .SetScreenSpaceOverlayPosition(_button.Center.position + _fingerOffset)
                .Show();
        }
        else
        {
            _finger.ResetAll();
        }
    }

    private void OnButtonCliked(ButtonClickHandler handler) =>
        Deactivate();
}