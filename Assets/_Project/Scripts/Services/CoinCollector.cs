using System;
using UnityEngine;

public class CoinCollector : IService, IDisposable
{
    private readonly IInteractionDetector _swipeHandler;

    public CoinCollector(IInteractionDetector swipeHandler)
    {
        _swipeHandler = swipeHandler;
        _swipeHandler.HitsDetected += OnHitsDetected;
    }

    public void Dispose() =>
        _swipeHandler.HitsDetected -= OnHitsDetected;        

    private void OnHitsDetected(RaycastHit2D[] hits, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out ICollectable collectable))
                collectable.Collect();
    }
}