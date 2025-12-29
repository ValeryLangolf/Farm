using System;

public class Grover : IDisposable
{
    private readonly ExtendedGardenData _data;
    private readonly Action _completed;

    private bool _isRunning;

    public Grover(ExtendedGardenData data, Action completed)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _completed = completed;
    }

    public void Dispose() =>
        StopRun();

    public void StartRun()
    {
        if (_isRunning)
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

        _data.SetGroverElapsedTime(_data.GroverElapsedTime + deltaTime);

        while (_data.GroverElapsedTime >= _data.InitialCultivationDurationInSeconds)
            CompleteGrowing();
    }

    public void ResetElapsedTime() =>
        _data.SetGroverElapsedTime(0);

    private void CompleteGrowing()
    {
        _data.SetGroverElapsedTime(_data.GroverElapsedTime - _data.InitialCultivationDurationInSeconds);
        _completed?.Invoke();
    }

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);
}