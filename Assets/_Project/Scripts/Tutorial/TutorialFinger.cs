using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] private TutorialFingerAnimator _animator;
    [SerializeField] private Vector3 _screenSpaceOverlayScale;

    private Transform _initialParent;
    private Transform _transform;
    private Vector3 _initialScale;

    public TutorialFinger Init()
    {
        _transform = transform;
        _initialScale = _transform.localScale;
        _initialParent = _transform.parent;

        Hide();

        return this;
    }

    public TutorialFinger ResetAll()
    {
        ResetParent().Hide();

        return this;
    }

    public TutorialFinger SetParent(Transform transform)
    {
        _transform.SetParent(transform);

        return this;
    }

    public TutorialFinger ResetParent()
    {
        if (_initialParent != null && _transform != null)
        {
            _transform.SetParent(_initialParent);
            _transform.localScale = _initialScale;
        }        

        return this;
    }

    public TutorialFinger Show()
    {
        gameObject.SetActive(true);

        return this;
    }

    public TutorialFinger Hide()
    {
        gameObject.SetActive(false);

        return this;
    }

    public TutorialFinger SetWorldPosition(Vector3 positon)
    {
        _transform.position = positon;

        return this;
    }

    public TutorialFinger SetScreenSpaceOverlayPosition(Vector3 screenPosition)
    {
        _transform.position = screenPosition;
        _transform.localScale = _screenSpaceOverlayScale;

        return this;
    }

    public TutorialFinger SetTouchAnimation()
    {
        _animator.SetTouchAnimation();

        return this;
    }

    public TutorialFinger SetSwipeAnimation()
    {
        _animator.SetSwipeAnimation();

        return this;
    }
}