using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class PlantsAnimator : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private string _targetBoneName = "FOLLOWER";

    private Bone _targetBone;

    public event Action AppearAnimationEnded;

    private void Awake()
    {
        _skeletonAnimation.Initialize(false);
        _targetBone = _skeletonAnimation.Skeleton.FindBone(_targetBoneName);
        _skeletonAnimation.AnimationState.Data.DefaultMix = 0f;
    }

    private void OnEnable()
    {
        _skeletonAnimation.UpdateLocal += OnUpdateLocal;
        _skeletonAnimation.AnimationState.Complete += OnEndAnimation;
    }

    private void OnDisable()
    {
        _skeletonAnimation.UpdateLocal -= OnUpdateLocal;
        _skeletonAnimation.AnimationState.Complete -= OnEndAnimation;
    }

    public void SetAppearAnimation()
    {
        string animationName = "Appear";
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false) ;
    }

    public void SetIdleAnimation()
    {
        string animationName = "Idle";
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true).MixDuration = 0.3f;
    }

    public void SetCollectAnimation()
    {
        string animationName = "Collect";
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
    }

    private void OnEndAnimation(TrackEntry trackEntry)
    {
        if(trackEntry.Animation.Name == "Appear")
        {
            AppearAnimationEnded?.Invoke();
        }
        else if(trackEntry.Animation.Name == "Collect")
        {
            SetIdleAnimation();
        }
    }

    private void OnUpdateLocal(ISkeletonAnimation skeletonAnimation)
    {
        //if (_targetBone == null)
        //    return;

        //Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        //mouseWorld.z = 0f;

        //Vector3 skeletonWorldPos = transform.position;
        //Vector2 toMouse = mouseWorld - skeletonWorldPos;
        //float distance = toMouse.magnitude;

        //Vector2 targetLocal;

        //if (distance <= _maxDistance)
        //{
        //    targetLocal = toMouse / transform.lossyScale.x;
        //}
        //else
        //{
        //    targetLocal = _setupLocalPos;
        //}

        //float speed = distance <= _maxDistance ? _followSpeed : _returnSpeed;

        //_currentLocalPos = Vector2.Lerp(
        //    _currentLocalPos,
        //    targetLocal,
        //    Time.deltaTime * speed
        //);

        //_targetBone.X = _currentLocalPos.x;
        //_targetBone.Y = _currentLocalPos.y;
    }
}