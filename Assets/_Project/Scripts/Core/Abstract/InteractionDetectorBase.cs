using System;
using UnityEngine;

public abstract class InteractionDetectorBase : IInteractionDetector, IRunnable, IDisposable
{
    private const int MaxHits = 10;
    private const float ClickMaxDuration = 0.3f;
    private const float ClickMaxDistance = 10f;

    private readonly RaycastHit2D[] _hits = new RaycastHit2D[MaxHits];
    private Vector2 _inputStartPosition;
    private float _inputStartTime;
    private bool _isSubscribed;

    public event Action<RaycastHit2D[], int> Swiped;
    public event Action<RaycastHit2D[], int> Clicked;
    public event Action InputStarted;
    public event Action InputEnded;

    public void Dispose() =>
        StopRun();

    public void PauseRun() =>
        StopRun();

    public void ResumeRun() =>
        StartRun();

    public void StartRun() =>
        Subscribe();

    public void StopRun() =>
        Unsubscribe();

    protected abstract void HandleUpdate();

    protected void InvokeInputStarted(Vector2 inputPosition)
    {
        _inputStartPosition = inputPosition;
        _inputStartTime = Time.time;
        InputStarted?.Invoke();
    }

    protected void InvokeInputEnded(Vector2 screenPosition)
    {
        InputEnded?.Invoke();

        float inputDuration = Time.time - _inputStartTime;
        float inputDistance = Vector2.Distance(_inputStartPosition, screenPosition);

        if (inputDuration <= ClickMaxDuration && inputDistance <= ClickMaxDistance)
            ProcessRaycastForClick(screenPosition);
    }

    protected void ProcessRaycast(Vector2 screenPosition)
    {
        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        int hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);

        if (hitCount > 0)
            Swiped?.Invoke(_hits, hitCount);
    }

    private void Subscribe()
    {
        if (_isSubscribed)
            return;

        _isSubscribed = true;

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated += OnUpdate;
    }

    private void Unsubscribe()
    {
        if (_isSubscribed == false)
            return;

        _isSubscribed = false;

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdate;
    }

    private void ProcessRaycastForClick(Vector2 screenPosition)
    {
        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        int hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);
        Clicked?.Invoke(_hits, hitCount);
    }

    private void OnUpdate(float deltaTime) =>
        HandleUpdate();
}