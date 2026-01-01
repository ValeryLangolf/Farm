using System;
using System.Collections.Generic;
using UnityEngine;

public partial class InteractionStateHandler
{
    private const int MaxHits = 10;
    private const float ClickMaxDistance = 10f;
    private const float StationaryTimeThreshold = 0.05f;
    private const float StationaryDistanceThreshold = 2f;

    private readonly RaycastHit2D[] _hits = new RaycastHit2D[MaxHits];
    private readonly List<InteractionInfo> _currentInteractions = new();
    private readonly Dictionary<int, InteractionState> _interactionStates = new();

    public event Action<IReadOnlyList<InteractionInfo>> InteractionsUpdated;
    public event Action<IReadOnlyList<InteractionInfo>> InteractionsStarted;
    public event Action<IReadOnlyList<InteractionInfo>> InteractionsEnded;

    public IReadOnlyList<InteractionInfo> CurrentInteractions => _currentInteractions;

    public void StartInteraction(int id, Vector2 position)
    {
        if (_interactionStates.ContainsKey(id))
            return;

        InteractionState state = new()
        {
            StartPosition = position,
            StartTime = Time.time,
            LastPosition = position,
            IsActive = true,
            StationaryTime = 0f,
            WasMoved = false
        };

        _interactionStates[id] = state;
        UpdateCurrentInteractionsList();

        List<InteractionInfo> startedInteractions = GetInteractionsByIds(new[] { id });
        InteractionsStarted?.Invoke(startedInteractions);
        InteractionsUpdated?.Invoke(_currentInteractions);
    }

    public void UpdateInteractionPosition(int id, Vector2 position, bool isStationaryPhase)
    {
        if (_interactionStates.TryGetValue(id, out InteractionState state) == false)
            return;

        float distanceFromStart = Vector2.Distance(position, state.StartPosition);

        if (distanceFromStart > StationaryDistanceThreshold)
        {
            state.WasMoved = true;
            state.StationaryTime = 0f;
        }
        else if (isStationaryPhase && state.WasMoved == false)
        {
            state.StationaryTime += Time.deltaTime;
        }

        state.LastPosition = position;
        _interactionStates[id] = state;
        UpdateCurrentInteractionsList();

        InteractionsUpdated?.Invoke(_currentInteractions);
    }

    public InteractionInfo EndInteraction(int id, Vector2 position)
    {
        if (_interactionStates.TryGetValue(id, out InteractionState state) == false)
            return default;

        state.IsActive = false;
        state.LastPosition = position;
        _interactionStates[id] = state;

        UpdateCurrentInteractionsList();

        List<InteractionInfo> endedInteractions = GetInteractionsByIds(new[] { id });
        InteractionsEnded?.Invoke(endedInteractions);

        InteractionInfo interactionInfo = CreateInteractionInfoForEnd(id, position, state);

        _interactionStates.Remove(id);
        UpdateCurrentInteractionsList();

        InteractionsUpdated?.Invoke(_currentInteractions);

        return interactionInfo;
    }

    public InteractionInfo ProcessRaycastForInteraction(int id, Vector2 position)
    {
        if (_interactionStates.TryGetValue(id, out InteractionState state) == false)
            return default;

        if (Camera.main == null)
            return default;

        Ray ray = Camera.main.ScreenPointToRay(position);
        int hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);

        if (hitCount == 0)
            return default;

        InteractionInfo interactionInfo = new(
            id,
            position,
            state.StartPosition,
            state.StartTime,
            _hits,
            hitCount);

        return interactionInfo;
    }

    private InteractionInfo CreateInteractionInfoForEnd(int id, Vector2 position, InteractionState state)
    {
        float inputDistance = Vector2.Distance(state.StartPosition, position);

        bool isClick = (inputDistance <= ClickMaxDistance)
            || (state.WasMoved == false && state.StationaryTime >= StationaryTimeThreshold);

        if (isClick == false)
            return default;

        if (Camera.main == null)
            return default;

        Ray ray = Camera.main.ScreenPointToRay(position);
        int hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _hits);

        if (hitCount == 0)
            return default;

        InteractionInfo interactionInfo = new(
            id,
            position,
            state.StartPosition,
            state.StartTime,
            _hits,
            hitCount);

        return interactionInfo;
    }

    private void UpdateCurrentInteractionsList()
    {
        _currentInteractions.Clear();

        foreach (KeyValuePair<int, InteractionState> kvp in _interactionStates)
        {
            InteractionInfo interaction = new(
                kvp.Key,
                kvp.Value.LastPosition,
                kvp.Value.StartPosition,
                kvp.Value.StartTime);

            _currentInteractions.Add(interaction);
        }
    }

    private List<InteractionInfo> GetInteractionsByIds(IEnumerable<int> ids)
    {
        List<InteractionInfo> result = new();

        foreach (int id in ids)
        {
            if (_interactionStates.TryGetValue(id, out InteractionState state))
            {
                result.Add(new InteractionInfo(
                    id,
                    state.LastPosition,
                    state.StartPosition,
                    state.StartTime));
            }
        }

        return result;
    }
}