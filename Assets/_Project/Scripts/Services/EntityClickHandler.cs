using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityClickHandler : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;

    private bool _isEnabled;

    public EntityClickHandler(IInteractionDetector swipeHandler)
    {
        _interactionDetector = swipeHandler;
        SetEnabled(true);
    }

    public void Dispose() =>
        SetEnabled(false);

    public void SetEnabled(bool isEnabled)
    {
        if (_isEnabled == isEnabled)
            return;

        _isEnabled = isEnabled;

        if (isEnabled)
            Subscribe();
        else
            Unsubscribe();
    }

    private void Subscribe() =>
        _interactionDetector.Clicked += OnHitDetected;

    private void Unsubscribe() =>
        _interactionDetector.Clicked -= OnHitDetected;

    private void OnHitDetected(IReadOnlyList<InteractionInfo> interactions)
    {
        foreach (InteractionInfo interaction in interactions)
            ProcessInteractionHits(interaction.RaycastHits, interaction.HitCount);
    }

    private void ProcessInteractionHits(RaycastHit2D[] hits, int hitCount)
    {
        if (hitCount == 0)
            return;

        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out IClickable collectable))
                collectable.ProcessClick();
    }
}