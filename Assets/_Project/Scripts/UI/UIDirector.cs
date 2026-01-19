using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDirector : MonoBehaviour, IService
{
    [SerializeField] private DarkPanel _darkPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private UpgradePanel _upgradeModePanel;
    [SerializeField] private ShopPanel _shopPanel;

    [SerializeField] private ProgressResetterButton _progressResetterButton;
    [SerializeField] private SettingsOpenerButton _settingsOpenerButton;
    [SerializeField] private SettingsCloserButton _settingsCloserButton;
    [SerializeField] private OpenerUpgradePanelButton _openerUpgradePanelButton;
    [SerializeField] private OpenerUpgradePanelButton _closerUpgradePanelButton;
    [SerializeField] private OpenerShopPanelButton _openerShopPanelButton;
    [SerializeField] private OpenerShopPanelButton _closerShopPanelButton;
    [SerializeField] private PlantPurchaseButton _firstGardenPlantPurchaseButton;
    [SerializeField] private List<UpgradeModeCountButton> _upgradeModeCountButtons;

    private SavingMediator _savingMediator;
    private IInteractionDetector _interactionDetector;
    private CoinCollector _coinCollector;
    private EntityClickHandler _entityClickHandler;
    private InputTrailParticle _inputTrailParticle;
    private UpgradeModeCountButtonType _currentCountButtonType = UpgradeModeCountButtonType.x1;
    private bool _isUpgradeModeActive;
    private bool _canShowUpgradeModeButton = true;
    private bool _canShowUpgradeShopButton = true;

    public event Action<UpgradeModeCountButtonType> UpgradeModeCountChanged;
    public event Action<bool> UpgradeModeEnabledStatusChanged;
    public event Action Changed;

    public UpgradeModeCountButtonType UpgradeModeCountButtonType => _currentCountButtonType;

    public OpenerUpgradePanelButton OpenerUpgradePanelButton => _openerUpgradePanelButton;
    public OpenerShopPanelButton OpenerShopPanelButton => _openerShopPanelButton;

    public bool IsUpgradeModeActive => _isUpgradeModeActive;

    public PlantPurchaseButton FirstGardenPlantPurchaseButton => _firstGardenPlantPurchaseButton;

    public float CheapestUpgradePrice => _shopPanel.CheapestUpgrade;

    public IPagedItem FirstPagedItem => _shopPanel.FirstPagedItem;

    private void Awake()
    {
        _savingMediator = ServiceLocator.Get<SavingMediator>();
        _interactionDetector = ServiceLocator.Get<IInteractionDetector>();
        _inputTrailParticle = ServiceLocator.Get<InputTrailParticle>();
        _coinCollector = ServiceLocator.Get<CoinCollector>();
        _entityClickHandler = ServiceLocator.Get<EntityClickHandler>();

        ToggleUpgradeModeCountButton(_upgradeModeCountButtons[0]);
        ShowGameModeUI();

        _shopPanel.Init();
    }

    private void Start() =>
        _settingsPanel.Initialize();

    private void OnEnable()
    {
        _progressResetterButton.Clicked += OnClickResetProgressButton;
        _settingsOpenerButton.Clicked += OnClickOpenSettingsPanelButton;
        _settingsCloserButton.Clicked += OnClickOpenSettingsPanelButton;
        _openerUpgradePanelButton.Clicked += OnClickOpenUpgradePanelButton;
        _closerUpgradePanelButton.Clicked += OnClickOpenUpgradePanelButton;
        _openerShopPanelButton.Clicked += OnClickOpenShopPanelButton;
        _closerShopPanelButton.Clicked += OnClickOpenShopPanelButton;
        _darkPanel.Clicked += OnClickDarkPanelCloserButton;

        foreach (var button in _upgradeModeCountButtons)
            button.Clicked += OnClickUpgradeModeCountButton;
    }

    private void OnDisable()
    {
        _progressResetterButton.Clicked -= OnClickResetProgressButton;
        _settingsOpenerButton.Clicked -= OnClickOpenSettingsPanelButton;
        _settingsCloserButton.Clicked -= OnClickOpenSettingsPanelButton;
        _openerUpgradePanelButton.Clicked -= OnClickOpenUpgradePanelButton;
        _closerUpgradePanelButton.Clicked -= OnClickOpenUpgradePanelButton;
        _openerShopPanelButton.Clicked -= OnClickOpenShopPanelButton;
        _closerShopPanelButton.Clicked -= OnClickOpenShopPanelButton;
        _darkPanel.Clicked -= OnClickDarkPanelCloserButton;

        foreach (var button in _upgradeModeCountButtons)
            button.Clicked -= OnClickUpgradeModeCountButton;
    }

    public void HideUpgradesModeButton()
    {
        _canShowUpgradeModeButton = false;
        _openerUpgradePanelButton.Hide();
    }

    public void ShowUpgradesModeButton()
    {
        _canShowUpgradeModeButton = true;
        _openerUpgradePanelButton.Show();
    }

    public void ShowUpgradeShopButton()
    {
        _canShowUpgradeShopButton = true;
        _openerShopPanelButton.Show();
    }

    public void HideUpgradeShopButton()
    {
        _canShowUpgradeShopButton = false;
        _openerShopPanelButton.Hide();
    }

    private void SwitchInteractionDetector(bool enabled)
    {
        if (enabled)
            _interactionDetector.ResumeRun();
        else
            _interactionDetector.PauseRun();
    }

    private void ToggleUpgradeModeCountButton(UpgradeModeCountButton button)
    {
        foreach (var item in _upgradeModeCountButtons)
            item.SetUntoggled();

        button.SetToggled();
        UpgradeModeCountChanged?.Invoke(button.Type);
    }

    private void HideAllSwitchableUI()
    {
        _darkPanel.Hide();
        _settingsPanel.Hide();
        _upgradeModePanel.Hide();
        _shopPanel.Hide();

        _settingsOpenerButton.Hide();
        _openerUpgradePanelButton.Hide();
        _openerShopPanelButton.Hide();
    }

    private void ShowGameModeUI()
    {
        HideAllSwitchableUI();

        _settingsOpenerButton.Show();

        if (_canShowUpgradeModeButton)
        {
            _openerUpgradePanelButton.Show();
        }


        if (_canShowUpgradeShopButton)
        {
            _openerShopPanelButton.Show();
        }
    }

    private void OnClickResetProgressButton(ButtonClickHandler _) =>
        _savingMediator.ResetProgress();

    private void OnClickOpenSettingsPanelButton(ButtonClickHandler _)
    {
        bool isOn = _settingsPanel.IsActiveSelf() == false;

        if (isOn)
        {
            HideAllSwitchableUI();
            _darkPanel.Show();
            _settingsPanel.Show();
            _settingsOpenerButton.Show();
        }
        else
        {
            ShowGameModeUI();
        }

        SwitchInteractionDetector(isOn == false);
        _inputTrailParticle.SetEnabled(isOn == false);

        Changed?.Invoke();
    }

    private void OnClickOpenUpgradePanelButton(ButtonClickHandler _)
    {
        _isUpgradeModeActive = !_isUpgradeModeActive;

        if (_isUpgradeModeActive)
        {
            HideAllSwitchableUI();
            _upgradeModePanel.Show();
            if (_canShowUpgradeModeButton)
            {
                _openerUpgradePanelButton.Show();
            }
        }
        else
        {
            ShowGameModeUI();
        }

        _coinCollector.SetEnabled(_isUpgradeModeActive == false);
        _entityClickHandler.SetEnabled(_isUpgradeModeActive == false);

        UpgradeModeEnabledStatusChanged?.Invoke(_isUpgradeModeActive);
    }

    private void OnClickOpenShopPanelButton(ButtonClickHandler _)
    {
        bool isOn = _shopPanel.IsActiveSelf() == false;

        if (isOn)
        {
            HideAllSwitchableUI();
            _darkPanel.Show();
            _shopPanel.Show();

            if (_canShowUpgradeShopButton)
            {
                _openerShopPanelButton.Show();
            }
        }
        else
        {
            ShowGameModeUI();
        }

        SwitchInteractionDetector(isOn == false);
        _inputTrailParticle.SetEnabled(isOn == false);
    }

    private void OnClickDarkPanelCloserButton(ButtonClickHandler handler)
    {
        ShowGameModeUI();

        SwitchInteractionDetector(true);
        _inputTrailParticle.SetEnabled(true);
    }

    private void OnClickUpgradeModeCountButton(ButtonClickHandler button)
    {
        if (button is UpgradeModeCountButton countButton == false)
            return;

        _currentCountButtonType = countButton.Type;
        ToggleUpgradeModeCountButton(countButton);
    }
}