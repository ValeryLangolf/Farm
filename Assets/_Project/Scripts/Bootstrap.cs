using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Garden> _gardens;
    [SerializeField] private ParticleSystem _trailParticle;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ProgressResetterButton _progressResetterButton;
    [SerializeField] private List<UIPanel> _panelsBlockedTrail;

    private readonly List<IService> _services = new();
    private SavingMediator _savingMediator;

    private void Awake()
    {
        _savingMediator = new(
            _gardens,
            _wallet,
            _progressResetterButton);

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
        _services.Add(new CoinCollector(interactionDetector, _wallet));
        _services.Add(new InteractionHandler(interactionDetector));
        InputTrailParticle inputTrailParticle = new InputTrailParticle(_trailParticle, interactionDetector);
        _services.Add(inputTrailParticle);

        foreach (UIPanel panel in _panelsBlockedTrail)
        {
            panel.SetTrailParticle(inputTrailParticle);
        }
    }

    private void OnDisable()
    {
        _savingMediator.Dispose();
    }

    private void OnDestroy()
    {
        foreach (IService service in _services)
            if(service is IDisposable disposable)
                disposable.Dispose();
    }
}