using System;
using UnityEngine;

public abstract class PointerPositionProviderBase : IPointerPositionProvider, IDisposable
{
    private const float Threshold = 0.01f;

    private readonly IUpdateService _updateService;
    private PositionInfo _positionInfo;
    private Camera _mainCamera;

    public PointerPositionProviderBase(IUpdateService updateService)
    {
        _updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
        _mainCamera = Camera.main;

        _updateService.Subscribe(OnUpdate);
    }

    public event Action<PositionInfo> Changed;

    public PositionInfo Position => _positionInfo;

    public void Dispose() =>
        _updateService?.Unsubscribe(OnUpdate);

    private void OnUpdate(float deltaTime)
    {
        PositionInfo position = GetCurrentPosition();

        if (ShouldNotifyChange(ref position))
        {
            _positionInfo = position;
            Changed?.Invoke(_positionInfo);
        }
    }

    protected abstract PositionInfo GetCurrentPosition();

    protected Vector3 ScreenToWorldPoint(Vector2 screenPoint)
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        if (_mainCamera == null)
            return Vector3.zero;

        return _mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 0));
    }

    private bool ShouldNotifyChange(ref PositionInfo newPosition)
    {
        if (_positionInfo.HasPosition != newPosition.HasPosition)
            return true;

        if (_positionInfo.HasPosition == false && newPosition.HasPosition == false)
            return false;

        return Vector2.Distance(_positionInfo.ScreenPoint, newPosition.ScreenPoint) > Threshold;
    }
}