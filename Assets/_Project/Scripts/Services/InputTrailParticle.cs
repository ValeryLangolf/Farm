using UnityEngine;

public class InputTrailParticle : MonoBehaviour, IService
{
    [SerializeField] private ParticleSystem _particle;

    private IInteractionDetector _swipeHandler;

    private void Awake() =>
        _swipeHandler = ServiceLocator.Get<IInteractionDetector>();

    private void OnEnable() =>
        Subscribe();

    private void OnDisable() =>
        Unsubscribe();

    private void PlayParticle()
    {
        if (_particle.isPlaying == false)
            _particle.Play();
    }

    private void StopParticle() =>
        _particle.Stop();

    private void Subscribe()
    {
        _swipeHandler.InputStarted += PlayParticle;
        _swipeHandler.InputEnded += StopParticle;
    }

    private void Unsubscribe()
    {
        StopParticle();
        _swipeHandler.InputStarted -= PlayParticle;
        _swipeHandler.InputEnded -= StopParticle;
    }
}