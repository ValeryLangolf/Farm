using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class PlantAimToMouse : MonoBehaviour
{
    [SerializeField] private string _targetBoneName = "FOLLOWER";

    [SerializeField] private float _maxDistance = 2.5f;
    [SerializeField] private float _followSpeed = 12f;
    [SerializeField] private float _returnSpeed = 8f;

    private SkeletonAnimation _skeletonAnimation;
    private Bone _targetBone;
    private Camera _camera;

    private Vector2 _currentLocalPos;
    private Vector2 _setupLocalPos;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _camera = Camera.main;

        _skeletonAnimation.Initialize(false);
        _targetBone = _skeletonAnimation.Skeleton.FindBone(_targetBoneName);

        _setupLocalPos = new Vector2(
            _targetBone.Data.X,
            _targetBone.Data.Y
        );

        _currentLocalPos = _setupLocalPos;
    }

    private void OnEnable()
    {
        _skeletonAnimation.UpdateLocal += OnUpdateLocal;
    }

    private void OnDisable()
    {
        _skeletonAnimation.UpdateLocal -= OnUpdateLocal;
    }

    private void OnUpdateLocal(ISkeletonAnimation skeletonAnimation)
    {
        if (_targetBone == null)
            return;

        Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 skeletonWorldPos = transform.position;
        Vector2 toMouse = mouseWorld - skeletonWorldPos;
        float distance = toMouse.magnitude;

        Vector2 targetLocal;

        if (distance <= _maxDistance)
        {
            targetLocal = toMouse / transform.lossyScale.x;
        }
        else
        {
            targetLocal = _setupLocalPos;
        }

        float speed = distance <= _maxDistance ? _followSpeed : _returnSpeed;

        _currentLocalPos = Vector2.Lerp(
            _currentLocalPos,
            targetLocal,
            Time.deltaTime * speed
        );

        _targetBone.X = _currentLocalPos.x;
        _targetBone.Y = _currentLocalPos.y;
    }
}
