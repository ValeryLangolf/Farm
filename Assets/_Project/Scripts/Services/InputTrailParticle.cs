using System;
using System.Diagnostics.Contracts;
using UnityEngine;

public class InputTrailParticle : IService, IDisposable
{
    private readonly ParticleSystem _particle;
    private readonly IInteractionDetector _swipeHandler;
    private bool _subscribed;

    public InputTrailParticle(ParticleSystem particle, IInteractionDetector swipeHandler)
    {
        _particle = particle != null ? particle : throw new ArgumentNullException(nameof(particle));
        _swipeHandler = swipeHandler ?? throw new ArgumentNullException(nameof(swipeHandler));

        Subscribe();
    }

    public void Dispose()
    {
        UnSubscribe();
    }

    public void SetActive(bool isOn)
    {
       if(isOn)
        {
            Subscribe();
        }
        else
        {
            UnSubscribe();
        }
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

    private void Subscribe()
    {
        if(_subscribed)
            return; 

        _subscribed = true;
        _swipeHandler.InputStarted += PlayParticle;
        _swipeHandler.InputEnded += StopParticle;
    }

    private void UnSubscribe()
    {
        if (_subscribed == false)
            return;

        _subscribed = false;
        _swipeHandler.InputStarted -= PlayParticle;
        _swipeHandler.InputEnded -= StopParticle;
        StopParticle();
    }
}