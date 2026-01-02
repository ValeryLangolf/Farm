using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickHandler : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event Action<ButtonClickHandler> Clicked;

    public Button Button => _button;

    protected virtual void Awake() =>
        _button = GetComponent<Button>();

    protected virtual void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    protected virtual void OnDisable() =>
        _button.onClick.RemoveListener(OnClick);

    public virtual void SetInteractable(bool isInteractable) =>
        _button.interactable = isInteractable;

    protected virtual void SetColor(Color color) =>
        _button.image.color = color;

    protected virtual void OnClick() =>
        Clicked?.Invoke(this);
}