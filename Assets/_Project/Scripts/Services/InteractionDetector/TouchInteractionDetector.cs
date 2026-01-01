using System.Collections.Generic;
using UnityEngine;

public class TouchInteractionDetector : BaseInteractionDetector
{
    private const float MinimumMovementDistance = 0.1f;

    private readonly List<InteractionInfo> _swipeInteractions = new();
    private readonly List<InteractionInfo> _clickInteractions = new();

    protected override void OnUpdate(float deltaTime)
    {
        _swipeInteractions.Clear();
        _clickInteractions.Clear();

        ProcessTouches();
        ProcessDelayedEvents();
    }

    private void ProcessTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StateHandler.StartInteraction(touch.fingerId, touch.position);
                    break;

                case TouchPhase.Moved:
                    StateHandler.UpdateInteractionPosition(touch.fingerId, touch.position, false);

                    if (touch.deltaPosition.magnitude > MinimumMovementDistance)
                    {
                        InteractionInfo swipeInfo = StateHandler.ProcessRaycastForInteraction(
                            touch.fingerId,
                            touch.position);

                        if (swipeInfo.HasHits)
                            _swipeInteractions.Add(swipeInfo);
                    }
                    break;

                case TouchPhase.Stationary:
                    StateHandler.UpdateInteractionPosition(touch.fingerId, touch.position, true);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    InteractionInfo clickInfo = StateHandler.EndInteraction(touch.fingerId, touch.position);

                    if (clickInfo.HasHits)
                        _clickInteractions.Add(clickInfo);
                    break;
            }
        }
    }

    private void ProcessDelayedEvents()
    {
        if (_swipeInteractions.Count > 0)
            InvokeSwiped(_swipeInteractions);

        if (_clickInteractions.Count > 0)
            InvokeClicked(_clickInteractions);
    }
}