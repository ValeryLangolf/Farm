using TMPro;
using UnityEngine;

public class UpgradeModeOpenTutorial : TutorialItem
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TutorialFinger _finger;
    [SerializeField] private Vector3 _fingerOffset;

    private IReadOnlyGardenData _garden;
    private OpenerUpgradePanelButton _button;
    private UIDirector _uiDirector;
    private IWallet _wallet;

    protected override void OnActivated()
    {
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0].ReadOnlyData;
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _wallet = ServiceLocator.Get<IWallet>();
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