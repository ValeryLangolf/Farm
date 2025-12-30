using System;

public class Grover : IDisposable
{
    private const int InitialTresholdPlantsTime = 25;
    private const float TimeReductionMultiplier = 2f;

    private readonly ExtendedGardenData _data;

    private float _currentTresholdMultiplier = 1;
    private bool _isRunning;

    public Grover(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));

        _data.PlantsCountChanged += OnPlantsCountChanged;

        _currentTresholdMultiplier = CalculateTresholdMultiplier(_data.PlantsCount);
        UpdateCultivationDuration();
    }

    public void Dispose()
    {
        _data.PlantsCountChanged -= OnPlantsCountChanged;
        StopRun();
    }

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
        if (plantsCount < InitialTresholdPlantsTime)
            return 1f;

        int treshold = InitialTresholdPlantsTime;
        float multiplier = 1f;

        while (plantsCount >= treshold)
        {
            multiplier *= TimeReductionMultiplier;
            int nextTreshold = (int)(treshold * TimeReductionMultiplier);

            if (nextTreshold <= treshold || nextTreshold > float.MaxValue * 0.5f)
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