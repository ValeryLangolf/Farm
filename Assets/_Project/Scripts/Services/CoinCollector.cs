using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;
    private readonly IWallet _wallet;
    private bool _isEnabled;

    public CoinCollector(IInteractionDetector interactionDetector, IWallet wallet)
    {
        _interactionDetector = interactionDetector ??
            throw new ArgumentNullException(nameof(interactionDetector));
        _wallet = wallet ??
            throw new ArgumentNullException(nameof(wallet));

        SetEnabled(true);
    }

    public event Action Collected;

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

    private void Subscribe()
    {
        _interactionDetector.Swiped += OnSwiped;
        _interactionDetector.Clicked += OnSwiped;
    }

    private void Unsubscribe()
    {
        _interactionDetector.Swiped -= OnSwiped;
        _interactionDetector.Clicked -= OnSwiped;
    }

    private void OnSwiped(IReadOnlyList<InteractionInfo> interactions)
    {
        foreach (InteractionInfo interaction in interactions)
            ProcessInteractionHits(interaction.RaycastHits, interaction.HitCount);
    }

    private void ProcessInteractionHits(RaycastHit2D[] hits, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out ICollectable collectable))
                if (collectable.TryCollect(out float value))
                    IncreaseWallet(value);
    }

    private void IncreaseWallet(float value)
    {
        _wallet.Increase(value);
        Collected?.Invoke();
    }
}