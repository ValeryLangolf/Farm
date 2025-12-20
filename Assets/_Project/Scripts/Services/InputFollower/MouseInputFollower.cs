using UnityEngine;

public class MouseInputFollower : InputFollowerBase
{
    public MouseInputFollower(Transform transform) : base(transform) { }

    protected override bool TryGetInputPosition(out Vector3 position)
    {
        position = Input.mousePosition;

        return true;
    }
}