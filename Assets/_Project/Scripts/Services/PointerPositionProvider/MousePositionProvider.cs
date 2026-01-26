using UnityEngine;

public class MousePositionProvider : PointerPositionProviderBase
{
    public MousePositionProvider(IUpdateService updateService) : base(updateService) { }

    protected override PositionInfo GetCurrentPosition()
    {
        if (Input.mousePresent)
        {
            Vector2 screenPoint = Input.mousePosition;
            Vector3 worldPoint = ScreenToWorldPoint(screenPoint);

            return new PositionInfo(screenPoint, worldPoint, true);
        }

        return new PositionInfo(Vector2.zero, Vector3.zero, false);
    }
}