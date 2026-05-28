using System;
using TMPro;
using UnityEngine;
using VContainer;

public class UpgradeModeOpenTutorial : TutorialItem, IInjactable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private UIDirector _uiDirector;
    private IReadOnlyGardenData _garden;
    private OpenerUpgradePanelButton _button;
    private IWallet _wallet;

    [Inject]
    public void Construct(IWallet wallet, IGardensDirector gardensDirector, UIDirector uiDirector)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _uiDirector = uiDirector != null ? uiDirector : throw new ArgumentNullException(nameof(uiDirector));

        if (gardensDirector == null)
            throw new ArgumentNullException(nameof(gardensDirector));

        _garden = gardensDirector.Gardens[0].ReadOnlyData;
    }

    protected override void OnActivated()
    {
        _button = _uiDirector.OpenerUpgradePanelButton;

        _text.Hide();
        _uiDirector.ProhibitShowingShopButton();
        _uiDirector.ProhibitShowingUpgradesModeButton();

        _wallet.Changed += OnWalletChanged;
    }

    protected override void OnDeactivated()
    {
        _wallet.Changed -= OnWalletChanged;
        _button.Clicked -= OnUpgradePanelOpened;

        _uiDirector.AllowShowingUpgradesModeButton();
        _uiDirector.AllowShowingShopButton();

        _text.Hide();
        _finger.ResetAll();
    }

    private void OnWalletChanged(float obj)
    {
        if (_wallet.CanSpend(_garden.PlantsPriceToUpgrade))
        {
            _wallet.Changed -= OnWalletChanged;

            _uiDirector.AllowShowingUpgradesModeButton();
            _text.Show();

            _finger.SetParent(_button.transform)
                .SetScreenSpaceOverlayPosition(_button.Center.position + _fingerOffset)
                .SetTouchAnimation()
                .Show();

            _button.Clicked += OnUpgradePanelOpened;
        }
    }

    private void OnUpgradePanelOpened(ButtonClickHandler _) =>
        Deactivate();
}