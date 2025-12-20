using System;
using UnityEngine;

public class Grover : IDisposable
{
    private readonly float _cultivationDurationInSeconds;
    private readonly Action _completed;

    private float _remainingTime;
    private float _progress = 0;

    public Grover(float cultivationDurationInSeconds, Action completed)
    {
        _cultivationDurationInSeconds = cultivationDurationInSeconds;
        _completed = completed;

        UpdateService.Instance.Updated += OnUpdated;
    }

    public float Progress => _progress;

    public void Dispose() =>
        UpdateService.Instance.Updated -= OnUpdated;

    private void OnUpdated(float _) =>
        Grow();

    public void Grow()
    {
        if (_remainingTime <= 0)
            return;

        _remainingTime = Mathf.Max(0, _remainingTime - Time.deltaTime);
        _progress = 1 - (_remainingTime / _cultivationDurationInSeconds);

        if (Mathf.Approximately(_progress, 1f))
        {
            _progress = 1f;
            _completed?.Invoke();
        }
    }
}