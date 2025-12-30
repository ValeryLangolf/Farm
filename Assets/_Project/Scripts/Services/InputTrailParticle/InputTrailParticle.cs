using System.Collections.Generic;
using UnityEngine;

public class InputTrailParticle : MonoBehaviour, IService
{
    [SerializeField] private TrailParticle _trailPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private IInteractionDetector _interactionDetector;
    private Pool<TrailParticle> _pool;
    private readonly Dictionary<int, TrailParticle> _activeParticles = new();

    private void Awake()
    {
        _interactionDetector = ServiceLocator.Get<IInteractionDetector>();
        InitializePool();
    }

    private void OnEnable() =>
        Subscribe();

    private void OnDisable() =>
        Unsubscribe();

    private void InitializePool()
    {
        IFactory<TrailParticle> factory = new TrailParticleFactory(_trailPrefab);
        _pool = new Pool<TrailParticle>(factory, transform, _initialPoolSize);
    }

    private void Subscribe()
    {
        _interactionDetector.InteractionsStarted += OnInteractionsStarted;
        _interactionDetector.InteractionsUpdated += OnInteractionsUpdated;
        _interactionDetector.InteractionsEnded += OnInteractionsEnded;
    }

    private void Unsubscribe()
    {
        _interactionDetector.InteractionsStarted -= OnInteractionsStarted;
        _interactionDetector.InteractionsUpdated -= OnInteractionsUpdated;
        _interactionDetector.InteractionsEnded -= OnInteractionsEnded;

        StopAllParticles();
    }

    private void AddActiveParticle(int id, TrailParticle particle, Vector2 position)
    {
        particle.Initialize();
        particle.UpdatePosition(position);
        _activeParticles[id] = particle;
    }

    private void RemoveActiveParticle(int id, TrailParticle particle)
    {
        particle.Stop();
        _activeParticles.Remove(id);
    }

    private void StopAllParticles()
    {
        foreach (TrailParticle particle in _activeParticles.Values)
            particle.Stop();

        _activeParticles.Clear();
    }

    private void OnInteractionsStarted(IReadOnlyList<InteractionInfo> interactions)
    {
        foreach (InteractionInfo interaction in interactions)
            if (_activeParticles.ContainsKey(interaction.Id) == false)
                if (_pool.TryGive(out TrailParticle particle))
                    AddActiveParticle(interaction.Id, particle, interaction.Position);
    }

    private void OnInteractionsUpdated(IReadOnlyList<InteractionInfo> interactions)
    {
        foreach (InteractionInfo interaction in interactions)
            if (_activeParticles.TryGetValue(interaction.Id, out TrailParticle particle))
                particle.UpdatePosition(interaction.Position);
    }

    private void OnInteractionsEnded(IReadOnlyList<InteractionInfo> interactions)
    {
        foreach (InteractionInfo interaction in interactions)
            if (_activeParticles.TryGetValue(interaction.Id, out TrailParticle particle))
                RemoveActiveParticle(interaction.Id, particle);
    }
}