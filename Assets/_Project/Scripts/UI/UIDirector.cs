using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDirector : MonoBehaviour, IService
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private ProgressResetterButton _progressResetterButton;
    [SerializeField] private SettingsOpenerButton _settingsOpenerButton;
    [SerializeField] private SettingsCloserButton _settingsCloserButton;

    [SerializeField] private UpgradeModeUI _upgradeModePanel;
    [SerializeField] private Button _upgradeModeButton;
    [SerializeField] private Button _upgradeModeCloseButton;
    [SerializeField] private List<UpgradeModeCountButton> _upgradeModeCountButtons;

    [SerializeField] private Button _upgradeShopButton;

    private SavingMediator _savingMediator;
    private UpgradeModeCountButtonType _currentCountButtonType = UpgradeModeCountButtonType.x1;

    public event Action<UpgradeModeCountButtonType> UpgradeModeCountChanged;
    public event Action<bool> UpgradeModeEnabledChanged;

    public UpgradeModeCountButtonType UpgradeModeCountButtonType => _currentCountButtonType;
    public bool IsUpgradeModeActive => _upgradeModePanel.gameObject.activeSelf;

    private void Awake()
    {
        _savingMediator = ServiceLocator.Get<SavingMediator>();
        _settingsPanel.Hide();
        Toggle(_upgradeModeCountButtons[0]);
    }

    private void OnEnable()
    {
        _progressResetterButton.Clicked += OnClickResetProgressButton;
        _settingsOpenerButton.Clicked += OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked += OnClickCloseSettingsButton;
        _upgradeModeButton.onClick.AddListener(OnClickUpgradeModeButton);
        _upgradeModeCloseButton.onClick.AddListener(OnClickUpgradeModeButton);

        foreach(var button in _upgradeModeCountButtons)
        {
            button.Clicked += OnClickUpgradeModeCountButton;
        }
    }

    private void OnDisable()
    {
        _progressResetterButton.Clicked -= OnClickResetProgressButton;
        _settingsOpenerButton.Clicked -= OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked -= OnClickCloseSettingsButton;
        _upgradeModeButton.onClick.RemoveListener(OnClickUpgradeModeButton);
        _upgradeModeCloseButton.onClick.RemoveListener(OnClickUpgradeModeButton);

        foreach (var button in _upgradeModeCountButtons)
        {
            button.Clicked -= OnClickUpgradeModeCountButton;
        }
    }

    private void OnClickResetProgressButton(ButtonClickHandler _) =>
        _savingMediator.ResetProgress();

    private void OnClickOpenSettingsButton(ButtonClickHandler _) =>
        _settingsPanel.Show();

    private void OnClickCloseSettingsButton(ButtonClickHandler _) =>
        _settingsPanel.Hide();

    private void OnClickUpgradeModeButton()
    {
        bool isOn = _upgradeModePanel.gameObject.activeSelf;

        _upgradeModePanel.SetActive(isOn == false);
        _upgradeShopButton.SetActive(isOn);

        UpgradeModeEnabledChanged?.Invoke(isOn == false);
    }

    private void OnClickUpgradeModeCountButton(ButtonClickHandler button)
    {
        if (button is UpgradeModeCountButton countButton == false)
        {
            return;
        }
        _currentCountButtonType = countButton.Type;

        Toggle(countButton);
    }

    private void Toggle(UpgradeModeCountButton button)
    {
        foreach (var item in _upgradeModeCountButtons)
        {
            item.SetUntoggled();
        }

        button.SetToggled();

        UpgradeModeCountChanged?.Invoke(button.Type);
    }
}