using System.Collections.Generic;
using UnityEngine;

public class TestUpgrdeShopStarter : MonoBehaviour
{
    public List<UpgradeInfo> UpgradeInfos;

    public UpgradeShopUI _shop;
    public UpgradeModeUI _mode;

    private void Awake()
    {
        _shop.Init();

        UpgradeInfos = new()
        {
            new("Speed", 100),
            new("Income", 200),
            new("Speed", 70),
            new("Speed", 100),
            new("Speed", 100),
            new("Speed", 100),
            new("Income", 100500)
        };

        foreach(UpgradeInfo upgradeInfo in UpgradeInfos)
        {
           _shop.AddShopItem(upgradeInfo);
        }

    }
}
