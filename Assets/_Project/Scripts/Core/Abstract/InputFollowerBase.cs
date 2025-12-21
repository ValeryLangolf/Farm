using System;
using UnityEngine;

public abstract class InputFollowerBase : IService, IDisposable
{
    private readonly Camera _mainCamera;
    private readonly Transform _transform;
    private readonly float _distanceFromCamera;

    public InputFollowerBase(Transform transform)
    {
        _transform = transform;
        _mainCamera = Camera.main;
        _distanceFromCamera = Mathf.Abs(_transform.position.z - _mainCamera.transform.position.z);

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated += OnUpdate;
    }

    public void Dispose()
    {
        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdate;
    }

    protected abstract bool TryGetInputPosition(out Vector3 position);

    private void OnUpdate(float _) =>
        UpdatePosition();

    private void UpdatePosition()
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