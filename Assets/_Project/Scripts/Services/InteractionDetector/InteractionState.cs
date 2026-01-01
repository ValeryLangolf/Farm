using UnityEngine;

public partial class InteractionStateHandler
{
    private struct InteractionState
    {
        public Vector2 StartPosition;
        public float StartTime;
        public Vector2 LastPosition;
        public bool IsActive;
        public float StationaryTime;
        public bool WasMoved;
    }
}