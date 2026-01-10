using Spine.Unity;
using UnityEngine;

public class TutorialCursor : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SkeletonGraphic _skeletonAnimation;

    private RectTransform _rect;

    public TutorialCursor Show()
    {
        gameObject.SetActive(true);
        return this;
    }

    public TutorialCursor Hide()
    {
        gameObject.SetActive(false);
        return this;
    }

    public TutorialCursor SetWorldPosition(Vector3 worldPositon)
    {
        if (_rect == null)
            _rect = transform as RectTransform;

        RectTransform canvasRect = _canvas.transform as RectTransform;

        Camera cam;

        if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cam = null;
        }
        else
        {
            cam = _canvas.worldCamera != null
                ? _canvas.worldCamera
                : Camera.main;
        }

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPositon);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPoint,
            null,
            out Vector2 localPoint
        );

        _rect.anchoredPosition = localPoint;

        return this;
    }

    public TutorialCursor SetScreenPosition(Vector3 screenPositon)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = screenPositon;

        return this;
    }

    public TutorialCursor SetTouchAnimation()
    {
        SetAnimation("PurchaseTutorial");
        return this;
    }

    public TutorialCursor SetSwipeAnimation()
    {
        SetAnimation("SwipeTutorial");
        return this;
    }

    private void SetAnimation(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
    }
}
