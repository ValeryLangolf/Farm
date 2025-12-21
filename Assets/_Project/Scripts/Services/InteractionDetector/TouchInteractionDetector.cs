using UnityEngine;

public class TouchInteractionDetector : InteractionDetectorBase
{
    private bool _wasTouching = false;
    private bool _hasActiveTouch = false;
    private Vector2 _lastTouchPosition;

    protected override void HandleUpdate()
    {
        bool hasTouch = Input.touchCount > 0;

        if (hasTouch && _wasTouching == false)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _hasActiveTouch = true;
                InvokeInputStarted(_lastTouchPosition);
            }
        }
        else if (hasTouch == false && _wasTouching && _hasActiveTouch)
        {
            InvokeInputEnded(_lastTouchPosition);
            _hasActiveTouch = false;
        }

        if (_hasActiveTouch)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                ProcessRaycast(touch.position);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                InvokeInputEnded(touch.position);
                _hasActiveTouch = false;
            }
        }

        _wasTouching = hasTouch;
    }
}