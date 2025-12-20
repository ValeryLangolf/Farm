using UnityEngine;

public class MouseInteractionDetector : InteractionDetectorBase
{
    private bool _wasMousePressed = false;

    protected override void HandleUpdate()
    {
        bool isMousePressed = Input.GetMouseButton(0);

        if (isMousePressed && _wasMousePressed == false)
            InvokeInputStarted();
        else if (isMousePressed == false && _wasMousePressed)
            InvokeInputEnded();

        if (isMousePressed)
            ProcessRaycast(Input.mousePosition);

        _wasMousePressed = isMousePressed;
    }
}