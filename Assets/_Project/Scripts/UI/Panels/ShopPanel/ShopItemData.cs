using System;
using UnityEngine;

public readonly struct ShopItemData
{
    public ShopItemData(
        string gardenName,
        Sprite icon,
        float price,
        int profitLevel,
        Action upgradeRequested,
        string description,
        bool canPurchase,
        Action onButtonClick = null)
    {
        GardenName = gardenName;
        Icon = icon;
        Price = price;
        ProfitLevel = profitLevel;
        UpgradeRequested = upgradeRequested;
        Description = description;
        CanPurchase = canPurchase;
        OnButtonClick = onButtonClick;
    }

    public string GardenName { get; }

    public Sprite Icon { get; }

    public float Price { get; }

    public int ProfitLevel { get; }

    public Action UpgradeRequested { get; }

    public string Description { get; }

    public bool CanPurchase { get; }

    public Action OnButtonClick { get; }
}