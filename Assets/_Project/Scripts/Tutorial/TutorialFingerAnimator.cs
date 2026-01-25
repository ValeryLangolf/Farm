using Spine.Unity;
using UnityEngine;

public class TutorialFingerAnimator : MonoBehaviour
{
    private const string ClickAnimationName = "PurchaseTutorial";
    private const string SwipeAnimationName = "SwipeTutorial";

    [SerializeField] private SkeletonGraphic _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation.AnimationState.Data.DefaultMix = 0f;
    }

    public void SetTouchAnimation() =>
        SetAnimation(ClickAnimationName);

    public void SetSwipeAnimation() =>
        SetAnimation(SwipeAnimationName);

    private void SetAnimation(string animationName) =>
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
}