using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModeUI : UIPanel
{
    [SerializeField] private List<UpgradeModeItemUI> _upgradeModeItems;
    [SerializeField] private List<GameObject> _objectsToChangeVisibility;
    [SerializeField] private ToggledButtonUI _x1Button, _x10Button, _tresholdButton, _maxButton;

    private ToggledButtonSwitcherUI _toggledButtonSwitcher;
    private List<ToggledButtonUI> _countButtons;

    public event Action X1ButtonClicked;
    public event Action X10ButtonClicked;
    public event Action TresholdButtonClicked;
    public event Action MaxButtonClicked;


    private void OnEnable()
    {
        for (int i = 0; i < _countButtons.Count; i++)
        {
            _countButtons[i].SetState(ToggledButtonState.Released);
        }
        _toggledButtonSwitcher.SetPressedByDefaultButton(_countButtons[0]);

        SetObjectsVisibility(false);
    }

    private void OnDisable()
    {
        SetObjectsVisibility(true);
    }

    public override void Init()
    {
        base.Init();

        _toggledButtonSwitcher = new();

        _countButtons.Add(_x1Button);
        _countButtons.Add(_x10Button);
        _countButtons.Add(_tresholdButton);
        _countButtons.Add(_maxButton);

        for (int i = 0; i < _countButtons.Count; i++)
        {
            _countButtons[i].Init();
            _toggledButtonSwitcher.AddButton(_countButtons[i]);
        }

        _toggledButtonSwitcher.SetPressedByDefaultButton(_countButtons[0]);
    }

    private void SetObjectsVisibility(bool isOn)
    {
        foreach (GameObject obj in _objectsToChangeVisibility)
        {
            obj.SetActive(isOn);
        }

        foreach (UpgradeModeItemUI item in _upgradeModeItems)
        {
            if (item.Garden.IsPurchased)
            {
                item.gameObject.SetActive(isOn == false);
            }
        }
    }


}
