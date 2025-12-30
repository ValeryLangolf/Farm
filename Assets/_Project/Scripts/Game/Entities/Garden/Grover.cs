using System;

public class Grover : IDisposable
{
    private readonly ExtendedGardenData _data;

    private bool _isRunning;

    public Grover(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
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

    private void CompleteGrowing()
    {
        _data.SetGroverElapsedTime(_data.GroverElapsedTime - _data.InitialCultivationDurationInSeconds);
        _data.SetStorageFullnes(_data.StorageFullness + _data.InitialGrowingCycleRevenue * _data.PlantsCount);

        if (_data.StorageFullness >= _data.StorageCapacity)
        {
            StopRun();
            _data.SetGroverElapsedTime(0);
        }
    }

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);
}