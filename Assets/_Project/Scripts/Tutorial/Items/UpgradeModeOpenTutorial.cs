using System;
using TMPro;
using UnityEngine;

public class UpgradeModeOpenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;
    [SerializeField] private TextMeshProUGUI _text;

    private IReadOnlyGardenData _garden;
    private OpenerUpgradePanelButton _button;
    private UIDirector _uiDirector;
    private IWallet _wallet;
    private bool _isActivated = false;

    private void Awake()
    {
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0].ReadOnlyData;
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _wallet = ServiceLocator.Get<IWallet>();
        _button = _uiDirector.OpenerUpgradePanelButton;
        _text.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        _wallet.Changed += OnWalletChanged;
    }

    public override void Deactivate()
    {

        _text.gameObject.SetActive(false);
        _cursor.Hide();
        Destroy(gameObject);
        _nextItem.Activate();
    }

    private void OnUpgradePanelOpened(ButtonClickHandler _)
    {
        _button.Clicked -= OnUpgradePanelOpened;
        Deactivate();
    }

    private void OnWalletChanged(float obj)
    {
        if (_wallet.CanSpend(_garden.PlantsPriceToUpgrade))
        {
            _wallet.Changed -= OnWalletChanged;

            if (_button.TryGetComponent<RectTransform>(out var rectTransform) == false)
            {
                return;
            }

            _text.gameObject.SetActive(true);
            _uiDirector.ShowUpgradesModeButton();

            Vector3 newPosition = _button.Center.position;

            _cursor.SetScreenPosition(newPosition + _cusrsorOffset)
                .SetTouchAnimation()
                .Show();

            _button.Clicked += OnUpgradePanelOpened;
        }
    }
}
