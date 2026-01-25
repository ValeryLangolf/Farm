using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlantsAnimator _animator;

    private void OnEnable()
    {
        _animator.AppearAnimationEnded += OnEndAppearAnimation;
    }

    private void OnDisable()
    {
        _animator.AppearAnimationEnded -= OnEndAppearAnimation;
    }

    public void HandleCollect()
    {
        _animator.SetCollectAnimation();
    }

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

    private void OnEndAppearAnimation()
    {
        _animator.SetIdleAnimation();
    }
}
