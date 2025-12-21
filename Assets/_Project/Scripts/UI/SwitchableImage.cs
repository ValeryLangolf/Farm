using UnityEngine;
using UnityEngine.UI;

public class SwitchableImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Sprite _activeSprite;

    public void UpdateState(bool isActive) =>
        _image.sprite = isActive ? _activeSprite : _inactiveSprite;

    public void SetActiveIcon(bool isActive) =>
        _image.SetActive(isActive);
}