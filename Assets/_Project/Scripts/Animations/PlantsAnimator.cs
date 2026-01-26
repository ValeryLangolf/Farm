using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class PlantsAnimator : MonoBehaviour
{
    private const string AppearAnimationName = "Appear";
    private const string IdleAnimationName = "Idle";
    private const string CollectAnimationName = "Collect";

    [SerializeField] private SkeletonAnimation _skeletonAnimation;

    public event Action AppearAnimationEnded;

    private void Awake()
    {
        _skeletonAnimation.Initialize(false);
        _skeletonAnimation.AnimationState.Data.DefaultMix = 0f;
    }

    private void OnEnable() =>
        _skeletonAnimation.AnimationState.Complete += OnEndAnimation;

    private void OnDisable() =>
        _skeletonAnimation.AnimationState.Complete -= OnEndAnimation;

    public void SetAppearAnimation() =>
        _skeletonAnimation.AnimationState.SetAnimation(0, AppearAnimationName, false);

    public void SetIdleAnimation() =>
        _skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimationName, true).MixDuration = 0.3f;

    public void SetCollectAnimation() =>
        _skeletonAnimation.AnimationState.SetAnimation(0, CollectAnimationName, false);

    private void OnEndAnimation(TrackEntry trackEntry)
    {
        if(trackEntry.Animation.Name == AppearAnimationName)
            AppearAnimationEnded?.Invoke();
        else if(trackEntry.Animation.Name == CollectAnimationName)
            SetIdleAnimation();
    }
}