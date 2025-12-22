using System;
using UnityEngine;

public class CoinCollector : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;
    private readonly Wallet _wallet;

    public CoinCollector(IInteractionDetector interactionDetector, Wallet wallet)
    {
        _interactionDetector = interactionDetector ?? throw new ArgumentNullException(nameof(interactionDetector));
        _interactionDetector.Swiped += OnSwiped;
        _wallet = wallet;
    }

    public void Dispose() =>
        _interactionDetector.Swiped -= OnSwiped;

    private void OnSwiped(RaycastHit2D[] hits, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out ICollectable collectable))
                if (collectable.TryCollect(out float value))
                    _wallet.Increase(value);        
    }
}