using System;
using UnityEngine;

public class UIDirector : MonoBehaviour, IService
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private ProgressResetterButton _progressResetterButton;
    [SerializeField] private SettingsOpenerButton _settingsOpenerButton;
    [SerializeField] private SettingsCloserButton _settingsCloserButton;

    private SavingMediator _savingMediator;

    private void Awake()
    {
        _savingMediator = ServiceLocator.Get<SavingMediator>();
        _settingsPanel.Hide();
    }

    private void OnEnable()
    {
        _progressResetterButton.Clicked += OnClickResetProgressButton;
        _settingsOpenerButton.Clicked += OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked += OnClickCloseSettingsButton;
    }

    private void OnDisable()
    {
        _progressResetterButton.Clicked -= OnClickResetProgressButton;
        _settingsOpenerButton.Clicked -= OnClickOpenSettingsButton;
        _settingsCloserButton.Clicked -= OnClickCloseSettingsButton;
    }

    private void OnClickResetProgressButton() =>
        _savingMediator.ResetProgress();

    private void OnClickOpenSettingsButton() =>
        _settingsPanel.Show();

    private void OnClickCloseSettingsButton() =>
        _settingsPanel.Hide();
}