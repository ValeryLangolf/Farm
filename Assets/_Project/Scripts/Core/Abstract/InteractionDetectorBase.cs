using System;
using UnityEngine;

public abstract class InteractionDetectorBase : IInteractionDetector, IDisposable
{
    private const int MaxHits = 10;

    private readonly RaycastHit2D[] _hits = new RaycastHit2D[MaxHits];
    private int _hitCount;

    protected InteractionDetectorBase()
    {
        UpdateService.Instance.Updated += OnUpdate;
    }

    public event Action<RaycastHit2D[], int> HitsDetected;
    public event Action InputStarted;
    public event Action InputEnded;

    public void Dispose() =>
        UpdateService.Instance.Updated -= OnUpdate;

    protected abstract void HandleUpdate();

    protected void InvokeInputStarted() =>
        InputStarted?.Invoke();

    protected void InvokeInputEnded() =>
        InputEnded?.Invoke();

    protected void ProcessRaycast(Vector2 screenPosition)
    {
        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        _hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);

        if (_hitCount > 0)
            HitsDetected?.Invoke(_hits, _hitCount);
    }

    private void OnUpdate(float deltaTime) =>
        HandleUpdate();
}