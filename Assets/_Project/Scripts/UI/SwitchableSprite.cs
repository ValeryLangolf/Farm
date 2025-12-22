using UnityEngine;

public class SwitchableSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private Sprite _activeSprite;

    public void UpdateState(bool isActive) =>
        _image.sprite = isActive ? _activeSprite : _inactiveSprite;

    public void SetColor(Color color) =>
        _image.color = color;
}