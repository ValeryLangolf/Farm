using System;
using UnityEngine;

public class CoinCollector : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;
    private readonly IWallet _wallet;

    public CoinCollector(IInteractionDetector interactionDetector, IWallet wallet)
    {
        _interactionDetector = interactionDetector ?? throw new ArgumentNullException(nameof(interactionDetector));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        _interactionDetector.Swiped += OnSwiped;
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