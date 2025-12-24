using System;
using UnityEngine;

public abstract class InputFollowerBase : IInputFollower, IRunnable, IDisposable
{
    private readonly Camera _mainCamera;
    private readonly Transform _transform;
    private readonly float _distanceFromCamera;

    private bool _isSubscribed;

    public InputFollowerBase(Transform transform)
    {
        _transform = transform;
        _mainCamera = Camera.main;
        _distanceFromCamera = Mathf.Abs(_transform.position.z - _mainCamera.transform.position.z);
    }

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

    protected abstract bool TryGetInputPosition(out Vector3 position);

    private void Subscribe()
    {
        if(_isSubscribed)
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

    private void OnUpdate(float _) =>
        Follow();

    private void Follow()
    {
        if (_mainCamera == null)
            return;

        if (TryGetInputPosition(out Vector3 inputPosition))
        {
            inputPosition.z = _distanceFromCamera;

            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(inputPosition);
            worldPosition.z = _transform.position.z;

            _transform.position = worldPosition;
        }
    }
}