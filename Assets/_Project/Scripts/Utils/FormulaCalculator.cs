using System;
using UnityEngine;

public static class FormulaCalculator
{
    public static float CalculatePurchasePrice(int index, float basePrice, float multiplier)
    {
        if (index <= 0)
            return 0f;

        float exactPrice = basePrice * Mathf.Pow(multiplier, index - 1);

        return exactPrice;
    }

    internal static float CalculatePurchasePrice(int gardenIndex, int baseGardenPrice, object gardenPriceMultiplier)
    {
        throw new NotImplementedException();
    }
}