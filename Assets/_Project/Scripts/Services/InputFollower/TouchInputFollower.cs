using UnityEngine;

public class TouchInputFollower : InputFollowerBase
{
    public TouchInputFollower(Transform transform) : base(transform) { }

    protected override bool TryGetInputPosition(out Vector3 position)
    {
        if (Input.touchCount > 0)
        {
            position = Input.GetTouch(0).position;

            return true;
        }

        position = Vector3.zero;

        return false;
    }
}