using System.Collections.Generic;
using UnityEngine;

public class ToggledButtonSwitcherUI 
{
    private ToggledButtonUI _pressedByDefaultButton;
    private List<ToggledButtonUI> _buttons;

    public ToggledButtonSwitcherUI()
    {
        _buttons = new();
    }

    public void DeInit()
    {
        foreach (var button in _buttons)
        {
            button.Clicked -= Toggle;
            MonoBehaviour.Destroy(button);
        }

        _pressedByDefaultButton = null;
        _buttons.Clear();
    }

    public void SetPressedByDefaultButton(ToggledButtonUI button)
    {
        _pressedByDefaultButton = button;
        _pressedByDefaultButton.SetState(ToggledButtonState.Pressed);
    }

    public void AddButton(ToggledButtonUI button)
    {
        _buttons.Add(button);   
        button.Clicked += Toggle;
    }

    private void Toggle(ToggledButtonUI button)
    {
        foreach (var item in _buttons)
        {
            if(item == button)
            {
                item.SetState(ToggledButtonState.Pressed);
            }
            else
            {
                item.SetState(ToggledButtonState.Released);
            }
        }
    }
}
