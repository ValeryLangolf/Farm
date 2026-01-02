using UnityEngine;
using UnityEngine.UI;

public class ProductionTimeItem : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _icon;

    public void SetInfo(Sprite sprite) =>
        _icon.sprite = sprite;

    private void OnAnimationFinished() =>
        Destroy(gameObject);
}