using System;
using UnityEngine;

public enum InteractionType
{
    Mouse,
    Touch
}

public readonly struct InteractionInfo : IEquatable<InteractionInfo>
{
    public readonly int Id;
    public readonly Vector2 Position;
    public readonly Vector2 StartPosition;
    public readonly float StartTime;
    public readonly InteractionType Type;
    public readonly RaycastHit2D[] RaycastHits;
    public readonly int HitCount;

    public InteractionInfo(
        int id,
        Vector2 position,
        Vector2 startPosition,
        float startTime,
        InteractionType type,
        RaycastHit2D[] raycastHits = null,
        int hitCount = 0)
    {
        Id = id;
        Position = position;
        StartPosition = startPosition;
        StartTime = startTime;
        Type = type;
        HitCount = Mathf.Min(hitCount, raycastHits?.Length ?? 0);

        if (raycastHits != null && HitCount > 0)
        {
            RaycastHits = new RaycastHit2D[HitCount];
            Array.Copy(raycastHits, RaycastHits, HitCount);
        }
        else
        {
            RaycastHits = null;
            HitCount = 0;
        }
    }

    public float DurationFromStart(float currentTime) =>
        currentTime - StartTime;

    public float DistanceFromStart =>
        Vector2.Distance(Position, StartPosition);

    public bool HasHits =>
        HitCount > 0;

    public bool Equals(InteractionInfo other) =>
        Id == other.Id && Type == other.Type;

    public override bool Equals(object obj) =>
        obj is InteractionInfo other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(Id, (int)Type);
}