using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateService : MonoBehaviour, IUpdateService, IRunnable, IDisposable
{
    private const string UpdateServicePath = "UpdateService";

    private static UpdateService s_instance;

    [SerializeField] private bool _isLogRepeatSubscriptions = false;

    private readonly HashSet<Action<float>> _actionSet = new();
    private Action<float> _updated;
    private bool _isRunning;

    public static UpdateService Instance
    {
        get
        {
            if (s_instance == null)
            {
                UpdateService prefab = Resources.Load<UpdateService>(UpdateServicePath);

                if (prefab == null)
                    throw new System.Exception($"SceneLoader prefab not found at path: {UpdateServicePath}");

                s_instance = Instantiate(prefab);
                DontDestroyOnLoad(s_instance.gameObject);
                s_instance.gameObject.SetActive(true);
            }

            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (_isRunning == false)
            return;

        float deltaTime = Time.deltaTime;

        _updated?.Invoke(deltaTime);
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
            if (_isLogRepeatSubscriptions)
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
            if (_isLogRepeatSubscriptions)
                Debug.LogWarning($"{nameof(UpdateService)}: Попытка отписать неподписанное событие {action.Method.Name}.");

            return;
        }

        _actionSet.Remove(action);
        _updated -= action;
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