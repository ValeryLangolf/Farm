using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggledButtonUI : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Color _pressedColor;

    private Color _releasedColor = Color.white;

    private ToggledButtonState _state;

    public event Action<ToggledButtonUI> Clicked;

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void Init()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void SetState(ToggledButtonState state)
    {
        _state = state;
        ApplyStateActions();
    }

    private void OnClick()
    {
        Clicked?.Invoke(this);
    }

    private void ApplyStateActions()
    {
        switch (_state)
        {
            case ToggledButtonState.Pressed:
                ApplyPressedStateActions();
                break;

            case ToggledButtonState.Released:
                ApplyReleasedStateActions();
                break;
        }
    }

    private void ApplyPressedStateActions()
    {
        _button.image.color = _pressedColor; //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }

    private void ApplyReleasedStateActions()
    {
        _button.image.color = _releasedColor; //Скорее всего, когда будет графика - будет подменяться спрайт, а не меняться цвет.
    }
}

public enum ToggledButtonState
{
    Pressed,
    Released
}
