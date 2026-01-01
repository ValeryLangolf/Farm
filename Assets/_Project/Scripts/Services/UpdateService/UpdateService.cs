using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateService : MonoBehaviour, IUpdateService, IRunnable, IDisposable
{
    private readonly HashSet<Action<float>> _actionSet = new();
    private Action<float> _updated;
    private bool _isRunning;

    private void Update()
    {
        if (_isRunning == false)
            return;

        float deltaTime = Time.deltaTime;

        _updated?.Invoke(deltaTime);
    }

    public static UpdateService Create()
    {
        GameObject gameObject = new(nameof(UpdateService));
        UpdateService service = gameObject.AddComponent<UpdateService>();

        return service;
    }

    public void Dispose()
    {
        _isRunning = false;
        _actionSet.Clear();
        _updated = null;
    }

    public void Subscribe(Action<float> action)
    {
        if (_actionSet.Contains(action))
        {
            Debug.LogWarning($"{nameof(UpdateService)}: Повторная попытка подписать событие {action.Method.Name}.");

            return;
        }

        _actionSet.Add(action);
        _updated += action;
    }

    public void Unsubscribe(Action<float> action)
    {
        if (_actionSet.Contains(action) == false)
        {
            Debug.LogWarning($"{nameof(UpdateService)}: Попытка отписать неподписанное событие {action.Method.Name}.");
            
            return;
        }

        _actionSet.Remove(action);
        _updated -= action;
    }

    public bool TryUnsubscribe(Action<float> action)
    {
        bool subscribed = _actionSet.Contains(action);

        if(subscribed)
            Unsubscribe(action);
        
        return subscribed;
    }

    public void StartRun() =>
        _isRunning = true;

    public void PauseRun() =>
        StopRun();

    public void ResumeRun() =>
        StartRun();

    public void StopRun() =>
        _isRunning = false;
}