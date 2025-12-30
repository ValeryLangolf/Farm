using System;
using UnityEngine;

public class Upgrader : IDisposable
{
    private const float PriceMultiplier = 1.2f;
    private const float CountTresholdMultiplier = 2f;
    private const int InitialCountTreshold = 25;

    private readonly IWallet _wallet;
    private readonly UIDirector _uiDirector;
    private readonly ExtendedGardenData _data;


    public Upgrader(ExtendedGardenData data)
    {
        _data = data;
        _wallet = ServiceLocator.Get<IWallet>();
        _uiDirector = ServiceLocator.Get<UIDirector>();

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

    public void UpgradePlantsCount()
    {
        if (_wallet.TrySpend(_data.PlantsPriceToUpgrade))
            _data.SetPlantsCount(_data.PlantsCount + _data.PlantsCountToUpgrade);
    }

    private void ProcessChanges()
    {
        UpgradeModeCountButtonType type = _uiDirector.UpgradeModeCountButtonType;

        int count = type switch
        {
            UpgradeModeCountButtonType.x1 => 1,
            UpgradeModeCountButtonType.x10 => 10,
            UpgradeModeCountButtonType.treshold => CalculateCountTreshold(),
            UpgradeModeCountButtonType.max => CalculateMaxCount(),
            _ => throw new InvalidCastException(nameof(type)),
        };

        float price = CalculatePrice(count + _data.PlantsCount) - CalculatePrice(_data.PlantsCount);
        _data.SetPlantsCountToUpgrade(count);
        _data.SetPlantsPriceToUpgrade(price);
    }

    private float CalculatePrice(int count) =>
        _data.InitialPlantPrice * ((Mathf.Pow(PriceMultiplier, count) - 1f) / (PriceMultiplier - 1f));

    private int CalculateCountTreshold()
    {
        int currentCount = _data.PlantsCount;
        int treshold = InitialCountTreshold;

        while (treshold <= currentCount)
        {
            int nextTreshold = Mathf.RoundToInt(treshold * CountTresholdMultiplier);

            if (nextTreshold <= treshold || nextTreshold > int.MaxValue * 0.5f)
                return 0;

            treshold = nextTreshold;
        }

        int remainingCount = Math.Max(0, treshold - currentCount);

        return remainingCount;
    }

    private int CalculateMaxCount()
    {
        int currentCount = _data.PlantsCount;
        float availableMoney = _wallet.Amount;
        float currentPrice = CalculatePrice(currentCount);
        float moneyWithCurrentPrice = availableMoney + currentPrice;

        if (moneyWithCurrentPrice <= currentPrice)
            return 1;

        float maxCountFloat = Mathf.Log(1f + moneyWithCurrentPrice * (PriceMultiplier - 1f) / _data.InitialPlantPrice) / Mathf.Log(PriceMultiplier);
        int maxPossibleCount = Mathf.FloorToInt(maxCountFloat);
        int affordableCount = Math.Max(1, maxPossibleCount - currentCount);

        return affordableCount;
    }

    private void OnUpgradeModeCountChanged(UpgradeModeCountButtonType _) =>
        ProcessChanges();

    private void OnWalletChanged(float _) =>
        ProcessChanges();

    private void OnPlantsCountChanged(int _) =>
        ProcessChanges();
}