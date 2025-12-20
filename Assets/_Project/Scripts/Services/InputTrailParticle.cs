using System;
using UnityEngine;

public class InputTrailParticle : IService, IDisposable
{
    private readonly ParticleSystem _particle;
    private readonly IInteractionDetector _swipeHandler;

    public InputTrailParticle(ParticleSystem particle, IInteractionDetector swipeHandler)
    {
        _particle = particle != null ? particle : throw new ArgumentNullException(nameof(particle));
        _swipeHandler = swipeHandler ?? throw new ArgumentNullException(nameof(swipeHandler));

        _swipeHandler.InputStarted += PlayParticle;
        _swipeHandler.InputEnded += StopParticle;
    }

    public void Dispose()
    {
        StopParticle();

        _swipeHandler.InputStarted -= PlayParticle;
        _swipeHandler.InputEnded -= StopParticle;
    }

    private void PlayParticle()
    {
        if (_particle != null && _particle.isPlaying == false)
            _particle.Play();
    }

    private void StopParticle()
    {
        if (_particle != null)
            _particle.Stop();
    }
}