using System.Collections.Generic;
using UnityEngine;

public class MouseInteractionDetector : BaseInteractionDetector
{
    private const int MouseInteractionId = 0;
    private const float MinimumMovementDistance = 0.1f;

    private bool _wasMousePressed;
    private Vector2 _previousMousePosition;
    private readonly List<InteractionInfo> _swipeInteractions = new();
    private readonly List<InteractionInfo> _clickInteractions = new();

    protected override void OnUpdate(float deltaTime)
    {
        _swipeInteractions.Clear();
        _clickInteractions.Clear();

        ProcessMouseInput();
        ProcessDelayedEvents();
    }

    private void ProcessMouseInput()
    {
        bool isMousePressed = Input.GetMouseButton(0);
        Vector2 mousePosition = Input.mousePosition;

        if (isMousePressed && _wasMousePressed == false)
        {
            _previousMousePosition = mousePosition;
            StateHandler.StartInteraction(MouseInteractionId, mousePosition);
        }
        else if (isMousePressed == false && _wasMousePressed)
        {
            InteractionInfo clickInfo = StateHandler.EndInteraction(MouseInteractionId, mousePosition);

            if (clickInfo.HasHits)
                _clickInteractions.Add(clickInfo);
        }

        if (isMousePressed)
        {
            StateHandler.UpdateInteractionPosition(MouseInteractionId, mousePosition, false);

            float movementDistance = Vector2.Distance(_previousMousePosition, mousePosition);

            if (movementDistance > MinimumMovementDistance)
            {
                InteractionInfo swipeInfo = StateHandler.ProcessRaycastForInteraction(
                    MouseInteractionId,
                    mousePosition);

                if (swipeInfo.HasHits)
                    _swipeInteractions.Add(swipeInfo);

                _previousMousePosition = mousePosition;
            }
        }

        _wasMousePressed = isMousePressed;
    }

    private void ProcessDelayedEvents()
    {
        if (_swipeInteractions.Count > 0)
            InvokeSwiped(_swipeInteractions);

        if (_clickInteractions.Count > 0)
            InvokeClicked(_clickInteractions);
    }
}