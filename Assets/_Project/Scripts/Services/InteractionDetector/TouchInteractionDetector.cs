using UnityEngine;

public class TouchInteractionDetector : InteractionDetectorBase
{
    private bool _hasActiveTouch = false;
    private Vector2 _lastTouchPosition;
    private int _lastTouchId = -1;

    protected override void HandleUpdate()
    {
        if (Input.touchCount == 0)
        {
            if (_hasActiveTouch)
            {
                InvokeInputEnded(_lastTouchPosition);
                _hasActiveTouch = false;
                _lastTouchId = -1;
            }

            return;
        }

        Touch touch = GetRelevantTouch();

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (_hasActiveTouch == false)
                {
                    _hasActiveTouch = true;
                    _lastTouchId = touch.fingerId;
                    _lastTouchPosition = touch.position;
                    InvokeInputStarted(_lastTouchPosition);
                }
                break;

            case TouchPhase.Moved:
                if (_hasActiveTouch && touch.fingerId == _lastTouchId)
                {
                    _lastTouchPosition = touch.position;
                    ProcessRaycast(_lastTouchPosition);
                }
                break;

            case TouchPhase.Stationary:
                if (_hasActiveTouch && touch.fingerId == _lastTouchId)
                {
                    UpdateStationaryTime(Time.deltaTime);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (_hasActiveTouch && touch.fingerId == _lastTouchId)
                {
                    _lastTouchPosition = touch.position;
                    InvokeInputEnded(_lastTouchPosition);
                    _hasActiveTouch = false;
                    _lastTouchId = -1;
                }
                break;
        }
    }

    private Touch GetRelevantTouch()
    {
        if (_lastTouchId != -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.fingerId == _lastTouchId)
                    return touch;
            }
        }

        return Input.GetTouch(0);
    }
}