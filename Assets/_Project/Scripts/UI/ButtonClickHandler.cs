using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickHandler : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action<ButtonClickHandler> Clicked;

    private void Awake() =>
        _button = GetComponent<Button>();

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.AddListener(OnClick);

    public virtual void SetInteractable(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }

    protected virtual void SetColor(Color color)
    {
        _button.image.color = color;
    }

    protected virtual void OnClick() =>
        Clicked?.Invoke(this);
}