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
    [SerializeField] private List<UpgradeModeCountButton> _upgradeModeCountButtons;

    private SavingMediator _savingMediator;
    private IInteractionDetector _interactionDetector;
    private CoinCollector _coinCollector;
    private EntityClickHandler _entityClickHandler;
    private InputTrailParticle _inputTrailParticle;
    private UpgradeModeCountButtonType _currentCountButtonType = UpgradeModeCountButtonType.x1;

    public event Action<UpgradeModeCountButtonType> UpgradeModeCountChanged;
    public event Action<bool> UpgradeModeEnabledChanged;

    public UpgradeModeCountButtonType UpgradeModeCountButtonType => _currentCountButtonType;
    public bool IsUpgradeModeActive { get; private set; }

    private void Awake()
    {
        _savingMediator = ServiceLocator.Get<SavingMediator>();
        _interactionDetector = ServiceLocator.Get<IInteractionDetector>();
        _inputTrailParticle = ServiceLocator.Get<InputTrailParticle>();
        _coinCollector = ServiceLocator.Get<CoinCollector>();
        _entityClickHandler = ServiceLocator.Get<EntityClickHandler>();

        _settingsPanel.Hide();
        _upgradeModePanel.Hide();
        _shopPanel.Hide();
        _darkPanel.Hide();
        Toggle(_upgradeModeCountButtons[0]);
    }

    private void Start()
    {
        _settingsPanel.Initialize();
    }

    private void OnEnable()
    {
        _progressResetterButton.Clicked += OnClickResetProgressButton;
        _settingsOpenerButton.Clicked += OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked += OnClickCloseSettingsButton;
        _openerUpgradePanelButton.Clicked += OnClickOpenUpgradePanelButton;
        _closerUpgradePanelButton.Clicked += OnClickOpenUpgradePanelButton;
        _openerShopPanelButton.Clicked += OnClickOpenShopPanelButton;
        _closerShopPanelButton.Clicked += OnClickOpenShopPanelButton;

        foreach (var button in _upgradeModeCountButtons)
            button.Clicked += OnClickUpgradeModeCountButton;
    }

    private void OnDisable()
    {
        _progressResetterButton.Clicked -= OnClickResetProgressButton;
        _settingsOpenerButton.Clicked -= OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked -= OnClickCloseSettingsButton;
        _openerUpgradePanelButton.Clicked -= OnClickOpenUpgradePanelButton;
        _closerUpgradePanelButton.Clicked -= OnClickOpenUpgradePanelButton;
        _openerShopPanelButton.Clicked -= OnClickOpenShopPanelButton;
        _closerShopPanelButton.Clicked -= OnClickOpenShopPanelButton;

        foreach (var button in _upgradeModeCountButtons)
            button.Clicked -= OnClickUpgradeModeCountButton;
    }

    private void SwitchInteractionDetector(bool enabled)
    {
        if (enabled)
            _interactionDetector.ResumeRun();
        else
            _interactionDetector.PauseRun();
    }

    private void OnClickResetProgressButton(ButtonClickHandler _) =>
        _savingMediator.ResetProgress();

    private void OnClickOpenSettingsButton(ButtonClickHandler _) =>
        _settingsPanel.Show();

    private void OnClickCloseSettingsButton(ButtonClickHandler _) =>
        _settingsPanel.Hide();

    private void OnClickOpenUpgradePanelButton(ButtonClickHandler _)
    {
        bool isOn = _upgradeModePanel.IsActiveSelf();
        _upgradeModePanel.SetActive(isOn == false);
        _openerShopPanelButton.SetActive(isOn);
        _coinCollector.SetEnabled(isOn);
        _entityClickHandler.SetEnabled(isOn);
        IsUpgradeModeActive = isOn == false;

        UpgradeModeEnabledChanged?.Invoke(isOn == false);
    }

    private void OnClickOpenShopPanelButton(ButtonClickHandler _)
    {
        bool isOn = _shopPanel.IsActiveSelf();

        _shopPanel.SetActive(isOn == false);
        _openerUpgradePanelButton.SetActive(isOn);
        _darkPanel.SetActive(isOn == false);
        SwitchInteractionDetector(isOn);
        _inputTrailParticle.SetActive(isOn);
    }

    private void OnClickUpgradeModeCountButton(ButtonClickHandler button)
    {
        if (button is UpgradeModeCountButton countButton == false)
            return;

        _currentCountButtonType = countButton.Type;
        Toggle(countButton);
    }

    private void Toggle(UpgradeModeCountButton button)
    {
        foreach (var item in _upgradeModeCountButtons)
            item.SetUntoggled();

        button.SetToggled();
        UpgradeModeCountChanged?.Invoke(button.Type);
    }
}