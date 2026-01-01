using System;
using UnityEngine;

public readonly struct InteractionInfo : IEquatable<InteractionInfo>
{
    public readonly int Id;
    public readonly Vector2 Position;
    public readonly Vector2 StartPosition;
    public readonly float StartTime;
    public readonly RaycastHit2D[] RaycastHits;
    public readonly int HitCount;

    public InteractionInfo(
        int id,
        Vector2 position,
        Vector2 startPosition,
        float startTime,
        RaycastHit2D[] raycastHits = null,
        int hitCount = 0)
    {
        Id = id;
        Position = position;
        StartPosition = startPosition;
        StartTime = startTime;
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

    public bool HasHits =>
        HitCount > 0;

    public bool Equals(InteractionInfo other) =>
        Id == other.Id;

    public override bool Equals(object obj) =>
        obj is InteractionInfo other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(Id);
}