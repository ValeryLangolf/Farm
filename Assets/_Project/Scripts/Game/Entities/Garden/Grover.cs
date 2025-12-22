using System;

public class Grover : IDisposable
{
    private readonly GroverData _data;
    private readonly Action _completed;
    private readonly Action<float> _progressChanged;

    private bool _isRunning;

    public Grover(GroverData data, Action completed, Action<float> progressChanged)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _completed = completed;
        _progressChanged = progressChanged;
    }

    public float Progress => _data.ElapsedTime / _data.CultivationDurationInSeconds;

    public void Dispose() =>
        StopRun();

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

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdated;
    }

    public void Grow(float deltaTime)
    {
        if (deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Значение должно быть положительным");

        _data.ElapsedTime += deltaTime;        

        while (_data.ElapsedTime >= _data.CultivationDurationInSeconds)
            CompleteGrowing();

        _progressChanged?.Invoke(Progress);
    }

    public void ResetElapsedTime()
    {
        _data.ElapsedTime = 0;
        _progressChanged?.Invoke(Progress);
    }

    private void CompleteGrowing()
    {
        _data.ElapsedTime -= _data.CultivationDurationInSeconds;
        _completed?.Invoke();
    }

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);
}