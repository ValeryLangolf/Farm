using System;
using UnityEngine;

public class CoinCollector : IService, IDisposable
{
    private readonly IInteractionDetector _interactionDetector;
    private readonly IWallet _wallet;

    private bool _isOn;

    public CoinCollector(IInteractionDetector interactionDetector, IWallet wallet)
    {
        _interactionDetector = interactionDetector ?? throw new ArgumentNullException(nameof(interactionDetector));
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));

        SetEnable(true);
    }

    public void Dispose() =>
        SetEnable(false);

    public void SetEnable(bool isOn)
    {
        if(_isOn == isOn) 
            return;

        _isOn = isOn;

        if (isOn)
            _interactionDetector.Swiped += OnSwiped;
        else
            _interactionDetector.Swiped -= OnSwiped;
    }

    private void OnSwiped(RaycastHit2D[] hits, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
            if (hits[i].collider.TryGetComponent(out ICollectable collectable))
                if (collectable.TryCollect(out float value))
                    _wallet.Increase(value);
    }
}