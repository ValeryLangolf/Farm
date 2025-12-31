using System;

public class Grover : IDisposable
{
    private readonly ExtendedGardenData _data;
    private readonly Updater _updater = new();

    private float _currentTresholdMultiplier;

    public Grover(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));

        _data.PlantsCountChanged += OnPlantsCountChanged;
        _updater.Updated += OnUpdated;
        _currentTresholdMultiplier = CalculateTresholdMultiplier(_data.PlantsCount);
        UpdateCultivationDuration();
    }

    public void Dispose()
    {
        _updater.Dispose();
        _data.PlantsCountChanged -= OnPlantsCountChanged;
        StopRun();
    }

    public void StartRun() =>
        _updater.Subscribe();

    public void StopRun() =>
        _updater.Unsubscribe();

    public void Grow(float deltaTime)
    {
        if (deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "«начение должно быть положительным");

        _data.SetGroverElapsedTime(_data.GroverElapsedTime + deltaTime);

        while (_data.GroverElapsedTime >= _data.CultivationDurationInSeconds)
            CompleteGrowing();
    }

    private void CompleteGrowing()
    {
        _data.SetGroverElapsedTime(_data.GroverElapsedTime - _data.CultivationDurationInSeconds);
        _data.SetStorageFullnes(_data.StorageFullness + _data.CurrentGrowingCycleRevenue);

        if (_data.StorageFullness >= _data.StorageCapacity)
        {
            StopRun();
            _data.SetGroverElapsedTime(0);
        }
    }

    private float CalculateTresholdMultiplier(int plantsCount)
    {
        int treshold = Constants.TresholdPlants;

        if (plantsCount < treshold)
            return 1f;

        float multiplier = 1f;

        while (plantsCount >= treshold)
        {
            multiplier *= Constants.TresholdPlantsMultiplier;
            int nextTreshold = (int)(treshold * Constants.TresholdPlantsMultiplier);

            if (nextTreshold <= treshold || nextTreshold > int.MaxValue / Constants.TresholdPlantsMultiplier)
                break;

            treshold = nextTreshold;
        }

        return multiplier;
    }

    private void UpdateCultivationDuration()
    {
        float cultivationDuration = _data.InitialCultivationDurationInSeconds / _currentTresholdMultiplier;
        _data.SetCultivationDurationInSeconds(cultivationDuration);
    }

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);

    private void OnPlantsCountChanged(int plantsCount)
    {
        float newTresholdMultiplier = CalculateTresholdMultiplier(plantsCount);

        if (Math.Abs(newTresholdMultiplier - _currentTresholdMultiplier) > 0.001f)
        {
            _currentTresholdMultiplier = newTresholdMultiplier;
            UpdateCultivationDuration();

            UnityEngine.Debug.Log($"“еперь гр€дка \"{_data.GardenName}\" будет производить ресурсы на 50% быстрее");
        }
    }
}