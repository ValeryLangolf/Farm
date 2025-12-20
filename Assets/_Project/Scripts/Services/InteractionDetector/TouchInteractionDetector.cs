using UnityEngine;

public class TouchInteractionDetector : InteractionDetectorBase
{
    private bool _wasTouching = false;
    private bool _hasActiveTouch = false;

    protected override void HandleUpdate()
    {
        bool hasTouch = Input.touchCount > 0;

        if (hasTouch && _wasTouching == false)
        {
            TouchPhase touchPhase = Input.GetTouch(0).phase;

            if (touchPhase == TouchPhase.Began)
            {
                _hasActiveTouch = true;
                InvokeInputStarted();
            }
        }
        else if (hasTouch == false && _wasTouching && _hasActiveTouch)
        {
            InvokeInputEnded();
            _hasActiveTouch = false;
        }

        if (_hasActiveTouch)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                ProcessRaycast(touch.position);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                InvokeInputEnded();
                _hasActiveTouch = false;
            }
        }

        _wasTouching = hasTouch;
    }
}