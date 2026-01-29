using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimator : MonoBehaviour
{
    private const string AppearAnimationName = "Appear";
    private const string IdleAnimationName = "Idle";
    private const string CollectAnimationBaseName = "Collect";

    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private int _collectAnimationsCount;
    [SerializeField] private float _timeToResetAnimationIndex = 1f;

    private int _currentCollectAnimationsIndex = 0;

    public event Action AppearAnimationEnded;

    private void Awake()
    {
        _skeletonAnimation.Initialize(false);
        _skeletonAnimation.AnimationState.Data.DefaultMix = 0f;

        _currentCollectAnimationsIndex =
            UnityEngine.Random.Range(0, _collectAnimationsCount);
    }

    private void OnEnable() =>
        _skeletonAnimation.AnimationState.Complete += OnEndAnimation;

    private void OnDisable() =>
        _skeletonAnimation.AnimationState.Complete -= OnEndAnimation;

    public void SetAppearAnimation() =>
        _skeletonAnimation.AnimationState.SetAnimation(0, AppearAnimationName, false);

    public void SetIdleAnimation() =>
        _skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimationName, true).MixDuration = 0.3f;

    public void SetCollectAnimation()
    {
        string animationName = $"{CollectAnimationBaseName}{_currentCollectAnimationsIndex + 1}";
        int trackIndex = _currentCollectAnimationsIndex + 1;

        var state = _skeletonAnimation.AnimationState;

        var entry = state.SetAnimation(trackIndex, animationName, false);
        entry.MixDuration = 0f;

        _currentCollectAnimationsIndex =
            (_currentCollectAnimationsIndex + 1) % _collectAnimationsCount;
    }

    private void OnEndAnimation(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name.StartsWith(CollectAnimationBaseName))
        {
            _skeletonAnimation.AnimationState.ClearTrack(trackEntry.TrackIndex);
            return;
        }

        if (trackEntry.Animation.Name == AppearAnimationName)
        {
            AppearAnimationEnded?.Invoke();
        }
    }
}