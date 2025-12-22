using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickHandler : MonoBehaviour
{
    private Button _button;

    public event Action Clicked;

    private void Awake() =>
        _button = GetComponent<Button>();

    private void OnEnable() =>
        _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
        _button.onClick.AddListener(OnClick);

    protected virtual void OnClick() =>
        Clicked?.Invoke();
}