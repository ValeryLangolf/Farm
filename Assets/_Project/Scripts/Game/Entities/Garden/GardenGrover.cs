using System;

public class GardenGrover : IDisposable
{
    // private const float InitialCultivationMultiplier = 2.4f;
    private readonly ExtendedGardenData _data;
    private readonly IUpdateService _updater = ServiceLocator.Get<IUpdateService>();

    private float _currentCountTreshold;
    private float _profitMultiplier;

    public GardenGrover(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));

        _data.PlantsCountChanged += OnPlantsCountChanged;
        _data.CostStoreLevelUpgradeChanged += OnCostStoreLevelUpgradeChanged;

        _currentCountTreshold = CalculateTresholdMultiplier(_data.PlantsCount);
        UpdateCultivationDuration();
        _profitMultiplier = CalculateProfitMultiplier();
        UpdateProgress();
    }

    public void Dispose()
    {
        _data.PlantsCountChanged -= OnPlantsCountChanged;
        _data.CostStoreLevelUpgradeChanged -= OnCostStoreLevelUpgradeChanged;
        _updater?.Unsubscribe(OnUpdated);
    }

    public void StartRun() =>
        _updater?.Subscribe(OnUpdated);

    public void StopRun() =>
        _updater?.Unsubscribe(OnUpdated);

    public void ProcessRunnableStatus()
    {
        if (_data.IsPurchased && (_data.StorageFullness == 0 || _data.IsStorageInfinity))
            StartRun();
        else
            StopRun();
    }

    public void Grow(float deltaTime)
    {
        if (deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Значение должно быть положительным");

        _data.GrowthElapsedTime += deltaTime;
        UpdateProgress();

        while (_data.GrowthElapsedTime >= _data.CultivationDurationInSeconds)
            CompleteGrowing();
    }

    private void CompleteGrowing()
    {
        _data.GrowthElapsedTime -= _data.CultivationDurationInSeconds;
        UpdateProgress();
        _data.StorageFullness += _data.InitialPlantRevenue * _data.PlantsCount * _profitMultiplier;

        if (_data.IsStorageInfinity == false && _data.StorageFullness > 0)
        {
            StopRun();
            _data.GrowthElapsedTime = 0;
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
        float cultivationDuration = _data.InitialCultivationDurationInSeconds / _currentCountTreshold;
        _data.CultivationDurationInSeconds = cultivationDuration;
    }

    private float CalculateProfitMultiplier()
    {
        int profitMultiplier = 1;

        for (int i = 1; i < _data.StoreLevelUpgrade; i++)
            profitMultiplier *= Constants.ProfitUpgradeMultiplier;

        return profitMultiplier;
    }

    private void UpdateProgress() =>
        _data.GrowthProgress = _data.GrowthElapsedTime / _data.CultivationDurationInSeconds;

    private void OnUpdated(float deltaTime) =>
        Grow(deltaTime);

    private void OnPlantsCountChanged(int plantsCount)
    {
        float newTresholdMultiplier = CalculateTresholdMultiplier(plantsCount);

        if (Math.Abs(newTresholdMultiplier - _currentCountTreshold) > 0.001f)
        {
            _currentCountTreshold = newTresholdMultiplier;
            UpdateCultivationDuration();
            _data.NotifyPlantCountThresholdChanged();
        }
    }

    private void OnCostStoreLevelUpgradeChanged(float _)
    {
        if (_data.StoreLevelUpgrade > 0)
            StartRun();

        _profitMultiplier = CalculateProfitMultiplier();
    }
}