using System.Collections.Generic;
using UnityEngine;

public class UpgradeModeUI : UIPanel
{
    [SerializeField] private List<ToggledButtonUI> _countButtons;
    [SerializeField] private List<GameObject> _objectsToChangeVisibility;
    private ToggledButtonSwitcherUI _toggledButtonSwitcher;


    // Подумать над классом. Какбудто стоит СетСтейт сделать в свичере.
    private void OnEnable()
    {
        for (int i = 0; i < _countButtons.Count; i++)
        {
            _countButtons[i].SetState(ToggledButtonState.Released);
        }
        _toggledButtonSwitcher.SetPressedByDefaultButton(_countButtons[0]);

        DisableObjects();
    }

    private void OnDisable()
    {
        EnableObjects();
    }

    public override void Init()
    {
        base.Init();

        _toggledButtonSwitcher = new();

        for (int i = 0; i < _countButtons.Count; i++)
        {
            _countButtons[i].Init();
            _toggledButtonSwitcher.AddButton(_countButtons[i]);
        }

        _toggledButtonSwitcher.SetPressedByDefaultButton(_countButtons[0]);
    }

    private void DisableObjects()
    {
        foreach(GameObject obj in _objectsToChangeVisibility)
        {
            obj.SetActive(false);
        }
    }

    private void EnableObjects()
    {
        foreach (GameObject obj in _objectsToChangeVisibility)
        {
            obj.SetActive(true);
        }
    }
}
