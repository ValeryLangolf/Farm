using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlantAnimator _animator;
    [SerializeField] private PointerBoneFollower _pointerBoneFollower;

    private IPointerPositionProvider _positionProvider;

    private void Awake() =>
        _positionProvider = ServiceLocator.Get<IPointerPositionProvider>();

    private void OnEnable()
    {
        _animator.AppearAnimationEnded += OnEndAppearAnimation;
        _positionProvider.Changed += OnPointerPositionChanged;
    }

    private void OnDisable()
    {
        _animator.AppearAnimationEnded -= OnEndAppearAnimation;
        _positionProvider.Changed -= OnPointerPositionChanged;
    }

    public void HandleCollect() =>
        _animator.SetCollectAnimation();

    public void ShowIdle()
    {
        gameObject.SetActive(true);
        _animator.SetIdleAnimation();
    }

    public void ShowAppear()
    {
        gameObject.SetActive(true);
        _animator.SetAppearAnimation();
    }

    private void OnEndAppearAnimation() =>
        _animator.SetIdleAnimation();

    private void OnPointerPositionChanged(PositionInfo positionInfo) =>
        _pointerBoneFollower.SetPointerPositionInfo(positionInfo);
}