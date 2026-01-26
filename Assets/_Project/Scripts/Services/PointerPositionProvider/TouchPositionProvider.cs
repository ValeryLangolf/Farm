using UnityEngine;

public class TouchPositionProvider : PointerPositionProviderBase
{
    public TouchPositionProvider(IUpdateService updateService) : base(updateService) { }

    protected override PositionInfo GetCurrentPosition()
    {
        if (Input.touchCount > 0)
        {
            Vector2 screenPoint = Input.GetTouch(0).position;
            Vector3 worldPoint = ScreenToWorldPoint(screenPoint);
            return new PositionInfo(screenPoint, worldPoint, true);
        }

        return new PositionInfo(Vector2.zero, Vector3.zero, false);
    }
}