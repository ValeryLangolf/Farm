using System;
using UnityEngine;

public class GardenUpgrader : IDisposable
{
    private const int CostStoreUpgradeMultiplier = 1000;

    private readonly IWallet _wallet;
    private readonly UIDirector _uiDirector;
    private readonly ExtendedGardenData _data;

    public GardenUpgrader(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _wallet = ServiceLocator.Get<IWallet>();
        _uiDirector = ServiceLocator.Get<UIDirector>();

        UpdateCostStoreLevelInfo();
        ProcessChanges();
        _uiDirector.UpgradeModeCountChanged += OnUpgradeModeCountChanged;
        _wallet.Changed += OnWalletChanged;
        _data.PlantsCountChanged += OnPlantsCountChanged;
    }

    public void Dispose()
    {
        _uiDirector.UpgradeModeCountChanged -= OnUpgradeModeCountChanged;
        _wallet.Changed -= OnWalletChanged;
        _data.PlantsCountChanged -= OnPlantsCountChanged;
    }

    public void UpgradeStoreLevel()
    {
        _data.StoreLevelUpgrade++;
        UpdateCostStoreLevelInfo();
    }

    private void UpdateCostStoreLevelInfo()
    {
        _data.CostStoreLevelUpgrade = FormulaCalculator.CalculateCostStoreLevelUpgrade(
            _data.InitialCostStoreLevelUpgrade,
            _data.StoreLevelUpgrade,
            CostStoreUpgradeMultiplier);
    }

    private void ProcessChanges()
    {
        UpgradeModeCountButtonType type = _uiDirector.UpgradeModeCountButtonType;

        int count = type switch
        {
            UpgradeModeCountButtonType.x1 => 1,
            UpgradeModeCountButtonType.x10 => 10,
            UpgradeModeCountButtonType.treshold => CalculateMaxCountTreshold(),
            UpgradeModeCountButtonType.max => Mathf.Max(1, CalculateMaxCountPlantsWithThisMoney(_wallet.Amount)),
            _ => throw new InvalidCastException(nameof(type)),
        };

        _data.PlantsCountToUpgrade = count;
        _data.PlantsPriceToUpgrade = CalculatePlantsPriceToUpgrade(_data.PlantsCountToUpgrade);
    }

    private int CalculateMaxCountPlantsWithThisMoney(float availableMoney)
    {
        return FormulaCalculator.CalculateAffordableAdditionalPlants(
            _data.InitialPlantPrice,
            _data.PlantsCount,
            _data.PlantCostMultiplier,
            availableMoney);
    }

    private float CalculatePlantsPriceToUpgrade(int plantsCountToUpgrade)
    {
        return FormulaCalculator.CalculateIntervalPrice(
            _data.InitialPlantPrice,
            _data.PlantCostMultiplier,
            _data.PlantsCount,
            _data.PlantsCount + plantsCountToUpgrade);
    }

    private int CalculateMaxCountTreshold()
    {
        return FormulaCalculator.CalculatePlantsCountTreshold(
            _data.PlantsCount,
            Constants.TresholdPlants,
            Constants.TresholdPlantsMultiplier);
    }

    private void OnUpgradeModeCountChanged(UpgradeModeCountButtonType _) =>
        ProcessChanges();

    private void OnWalletChanged(float _)
    {
        if (_data.IsPurchased == false)
            return;

        if (_uiDirector.UpgradeModeCountButtonType == UpgradeModeCountButtonType.max)
        {
            _data.PlantsCountToUpgrade = Mathf.Max(1, CalculateMaxCountPlantsWithThisMoney(_wallet.Amount));
            _data.PlantsPriceToUpgrade = CalculatePlantsPriceToUpgrade(_data.PlantsCountToUpgrade);
        }
    }

    private void OnPlantsCountChanged(int _) =>
        ProcessChanges();
}