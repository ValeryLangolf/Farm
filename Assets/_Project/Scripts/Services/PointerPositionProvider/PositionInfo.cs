using UnityEngine;

public readonly struct PositionInfo
{
    private readonly Vector2 _screenPoint;
    private readonly Vector3 _worldPoint;
    private readonly bool _hasPosition;

    public PositionInfo(Vector2 screenPoint, Vector3 worldPoint, bool hasPosition)
    {
        _screenPoint = screenPoint;
        _worldPoint = worldPoint;
        _hasPosition = hasPosition;
    }

    public readonly Vector2 ScreenPoint => _screenPoint;

    public readonly Vector3 WorldPoint => _worldPoint;

    public readonly bool HasPosition => _hasPosition;
}