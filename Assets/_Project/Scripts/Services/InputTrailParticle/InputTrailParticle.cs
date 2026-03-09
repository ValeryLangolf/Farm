using System.Collections.Generic;
using UnityEngine;

public class InputTrailParticle : MonoBehaviour, IService
{
    private const string InputTrailParticleServicePath = "InputTrailParticleService";

    private static InputTrailParticle s_instance;

    [SerializeField] private TrailParticle _trailPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private IInteractionDetector _interactionDetector;
    private Pool<TrailParticle> _pool;
    private readonly Dictionary<int, TrailParticle> _activeParticles = new();
    private bool _isSubscribed;

    public static InputTrailParticle Instance
    {
        get
        {
            if (s_instance == null)
            {
                InputTrailParticle prefab = Resources.Load<InputTrailParticle>(InputTrailParticleServicePath);

                if (prefab == null)
                    throw new System.Exception($"InputTrailParticle prefab not found at path: {InputTrailParticleServicePath}");

                s_instance = Instantiate(prefab);
                DontDestroyOnLoad(s_instance.gameObject);
                s_instance.gameObject.SetActive(true);
            }

            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        _interactionDetector = ServiceLocator.Get<IInteractionDetector>();
        InitializePool();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() =>
        Subscribe();

    private void OnDisable() =>
        Unsubscribe();

    public void SetEnabled(bool isOn)
    {
        if(isOn)
            Subscribe();
        else
            Unsubscribe();
    }

    private void InitializePool()
    {
        IFactory<TrailParticle> factory = new TrailParticleFactory(_trailPrefab);
        _pool = new Pool<TrailParticle>(factory, transform, _initialPoolSize);
    }

    private void Subscribe()
    {
        if(_isSubscribed) 
            return;

        _isSubscribed = true;

        _interactionDetector.InteractionsStarted += OnInteractionsStarted;
        _interactionDetector.InteractionsUpdated += OnInteractionsUpdated;
        _interactionDetector.InteractionsEnded += OnInteractionsEnded;
    }

    private void Unsubscribe()
    {
        if (_isSubscribed == false)
            return;

        _isSubscribed = false;

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