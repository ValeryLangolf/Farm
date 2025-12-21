using System;
using UnityEngine;

public class InteractionHandler : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;

    public InteractionHandler(IInteractionDetector swipeHandler)
    {
        _interactionDetector = swipeHandler;
        _interactionDetector.Clicked += OnHitDetected;
    }

    public void Dispose() =>
        _interactionDetector.Clicked -= OnHitDetected;

    private void OnHitDetected(RaycastHit2D[] hits, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out IClickable collectable))
                collectable.HandleClick();
    }
}