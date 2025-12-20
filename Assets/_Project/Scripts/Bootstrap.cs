using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ParticleSystem _trailParticle;

    private readonly List<IService> _services = new();

    private void Awake()
    {
        IInteractionDetector interactionDetector;

        if (Application.isMobilePlatform)
        {
            interactionDetector = new TouchInteractionDetector();
            _services.Add(new TouchInputFollower(_trailParticle.transform));
        }
        else
        {
            interactionDetector = new MouseInteractionDetector();
            _services.Add(new MouseInputFollower(_trailParticle.transform));
        }

        _services.Add(interactionDetector);
        _services.Add(new CoinCollector(interactionDetector));
        _services.Add(new InputTrailParticle(_trailParticle, interactionDetector));
    }

    private void OnDestroy()
    {
        foreach (IService service in _services)
            if(service is IDisposable disposable)
                disposable.Dispose();
    }
}