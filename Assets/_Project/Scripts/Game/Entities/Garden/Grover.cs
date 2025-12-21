using System;

public class Grover : IDisposable
{
    private readonly float _cultivationDurationInSeconds;
    private readonly Action _completed;
    private readonly Action<float> _progressChanged;

    private float _elapsedTime;
    private bool _isRunning;

    public Grover(float cultivationDurationInSeconds, float elapsedTime, Action completed, Action<float> progressChanged)
    {
        if (cultivationDurationInSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(cultivationDurationInSeconds), cultivationDurationInSeconds, "ƒлительность должна быть положительным числом");

        if (elapsedTime < 0)
            throw new ArgumentOutOfRangeException(nameof(elapsedTime), elapsedTime, "«атраченное врем€ должно быть положительным числом");

        _cultivationDurationInSeconds = cultivationDurationInSeconds;
        _elapsedTime = elapsedTime;
        _completed = completed;
        _progressChanged = progressChanged;

        Grow(_elapsedTime);
    }

    public float Progress => _elapsedTime / _cultivationDurationInSeconds;

    public void Dispose()
    {
        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdated;
    }

    public void StartRun()
    {
        if(_isRunning)
            return;

        _isRunning = true;

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated += OnUpdated;
    }

    public void StopRun()
    {
        if (_isRunning == false)
            return;

        _isRunning = false;
        _elapsedTime = 0;

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdated;
    }

    private void Grow(float deltaTime)
    {
        _elapsedTime += deltaTime;
        _progressChanged?.Invoke(Progress);

        while (_elapsedTime >= _cultivationDurationInSeconds)
            CompleteGrowing();
    }

    private void CompleteGrowing()
    {
        _elapsedTime -= _cultivationDurationInSeconds;
        _completed?.Invoke();
    }

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);
}