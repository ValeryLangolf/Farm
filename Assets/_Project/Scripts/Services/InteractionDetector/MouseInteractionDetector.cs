using UnityEngine;

public class MouseInteractionDetector : InteractionDetectorBase
{
    private bool _wasMousePressed = false;

    protected override void HandleUpdate()
    {
        bool isMousePressed = Input.GetMouseButton(0);
        Vector2 mousePosition = Input.mousePosition;

        if (isMousePressed && _wasMousePressed == false)
            InvokeInputStarted(mousePosition);
        else if (isMousePressed == false && _wasMousePressed)
            InvokeInputEnded(mousePosition);

        if (isMousePressed)
            ProcessRaycast(mousePosition);

        _wasMousePressed = isMousePressed;
    }
}