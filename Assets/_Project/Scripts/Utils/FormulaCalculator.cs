using System;
using UnityEngine;

public static class FormulaCalculator
{
    public static float CalculatePurchasePrice(int gardenIndex, float basePrice, float multiplier)
    {
        if (gardenIndex <= 0)
            return 0f;

        float exactPrice = basePrice * Mathf.Pow(multiplier, gardenIndex);

        return exactPrice;
    }

    public static float CalculateInitialPlantRevenue(int gardenIndex, float gardenPrice)
    {
        if(gardenIndex <= 0)
            return 1f;

        if(gardenIndex == 1)
            return gardenPrice;

        float multiplier = 0.5f;

        if (gardenIndex == 2)
            multiplier = 0.75f;

        return gardenPrice * multiplier;
    }

    public static float CalculateIntervalPrice(float basePrice, float multiplier, int from, int to)
    {
        if (from >= to || Mathf.Approximately(multiplier, 1f))
            return basePrice * (to - from);

        float multiplierFrom = Mathf.Pow(multiplier, from);
        float multiplierDiff = Mathf.Pow(multiplier, to - from);

        return basePrice * multiplierFrom * (multiplierDiff - 1f) / (multiplier - 1f);
    }

    public static int CalculateAffordableAdditionalPlants(
        float initialPlantPrice, 
        int currentCount, 
        float multiplier, 
        float availableMoney)
    {
        if (availableMoney <= 0f || initialPlantPrice <= 0f || multiplier <= 1f)
            return 0;

        float nextPlantPrice = initialPlantPrice * Mathf.Pow(multiplier, currentCount);

        if (availableMoney < nextPlantPrice)
            return 0;

        if (Mathf.Approximately(multiplier, 1f))
        {
            int affordableCount = Mathf.FloorToInt(availableMoney / nextPlantPrice);

            return Mathf.Max(1, affordableCount);
        }

        float totalSpendable = availableMoney +
        initialPlantPrice * (Mathf.Pow(multiplier, currentCount) - 1f) / (multiplier - 1f);
        float logNumerator = 1f + totalSpendable * (multiplier - 1f) / initialPlantPrice;

        if (logNumerator <= 0f)
            return 1;

        float maxCountFloat = Mathf.Log(logNumerator, multiplier);
        int maxTotalPlants = Mathf.FloorToInt(maxCountFloat);
        int affordableAdditional = Mathf.Max(1, maxTotalPlants - currentCount);

        return affordableAdditional;
    }

    public static int CalculatePlantsCountTreshold(int currentCount, int treshold, float multiplier)
    {
        while (treshold <= currentCount)
        {
            int nextTreshold = Mathf.RoundToInt(treshold * multiplier);

            if (nextTreshold <= treshold || nextTreshold > int.MaxValue / multiplier)
                return 0;

            treshold = nextTreshold;
        }

        int remainingCount = Math.Max(0, treshold - currentCount);

        return remainingCount;
    }

    public static float CalculateInitialCultivationDurationInSeconds(int gardenIndex, float multiplier) =>
        Mathf.Pow(multiplier, gardenIndex);

    public static float CalculateInitialShopLevelPrice(int gardenIndex, float multiplier) =>
        Mathf.Pow(multiplier, gardenIndex);

    public static float CalculateCostStoreLevelUpgrade(float initialCost, int level, float multiplier) =>
        initialCost * Mathf.Pow(multiplier, level);
}