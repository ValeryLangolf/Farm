using Spine;
using Spine.Unity;
using UnityEngine;

public class PointerBoneFollower : MonoBehaviour
{
    private const float Threshold = 0.001f;

    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private string _targetBoneName = "FOLLOWER";

    private Bone _targetBone;
    private Vector2 _initialBonePosition;
    private Vector2 _currentBonePosition;
    private PositionInfo _pointerPositionInfo = new();
    private float _maxDistanceSqr;
    private Transform _transform;
    private Vector2 _scaledDirection;

    private void OnValidate()
    {
        if (_maxDistance > 0)
            _maxDistanceSqr = _maxDistance * _maxDistance;
    }

    private void OnDrawGizmosSelected()
    {
        if (_transform == null)
            _transform = transform;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_transform.position, _maxDistance);
    }

    private void Start()
    {
        _targetBone = _skeletonAnimation.Skeleton.FindBone(_targetBoneName);
        _transform = transform;
        _maxDistanceSqr = _maxDistance * _maxDistance;
        _initialBonePosition = new Vector2(_targetBone.X, _targetBone.Y);
        _currentBonePosition = _initialBonePosition;
    }

    private void OnEnable() =>
        _skeletonAnimation.UpdateLocal += OnUpdateLocal;

    private void OnDisable() =>
        _skeletonAnimation.UpdateLocal -= OnUpdateLocal;

    public void SetPointerPositionInfo(PositionInfo position) =>
        _pointerPositionInfo = position;

    private void MoveTo(Vector2 localPosition, float speed)
    {
        Vector2 targetPosition = localPosition;
        float distanceSqr = (_currentBonePosition - targetPosition).sqrMagnitude;

        if (distanceSqr < Threshold)
        {
            _currentBonePosition = targetPosition;
        }
        else
        {
            _currentBonePosition = Vector2.Lerp(
                _currentBonePosition,
                targetPosition,
                speed * Time.deltaTime
            );
        }

        _targetBone.X = _currentBonePosition.x;
        _targetBone.Y = _currentBonePosition.y;
    }

    private bool CanFollow()
    {
        Vector3 pointerWorldPoint = _pointerPositionInfo.WorldPoint;
        Vector3 myPosition = _transform.position;

        float dx = pointerWorldPoint.x - myPosition.x;
        float dy = pointerWorldPoint.y - myPosition.y;
        float distanceSqr = dx * dx + dy * dy;

        if (distanceSqr < _maxDistanceSqr)
        {
            float scaleX = _transform.lossyScale.x;
            float invScaleX = 1f / scaleX;
            _scaledDirection.x = dx * invScaleX;
            _scaledDirection.y = dy * invScaleX;

            return true;
        }

        return false;
    }

    private void OnUpdateLocal(ISkeletonAnimation skeletonAnimation)
    {
        if (_pointerPositionInfo.HasPosition == false || CanFollow() == false)
            MoveTo(_initialBonePosition, _returnSpeed);
        else
            MoveTo(_scaledDirection, _followSpeed);
    }
}