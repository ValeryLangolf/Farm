using System;
using UnityEngine;

public class Upgrader : IDisposable
{
    private const float MULTIPLYER = 1.2f;
    private readonly Action<int, float> _upgradedCount;
    private readonly Action _recalculated;
    private readonly UIDirector _uidirector;

    private GardenData _data;
    private IWallet _wallet;
    private float _currentUpgradePrice = 1; //Добавить инфу в дату?
    private int _count;
    private float _price;
    private int _countTreshold = 25;

    public Upgrader(GardenData data, Action<int, float> onUpgradedCount, Action recalculated)
    {
        _data = data;
        _wallet = ServiceLocator.Get<IWallet>();
        _upgradedCount = onUpgradedCount;
        _uidirector = ServiceLocator.Get<UIDirector>();
        _recalculated = recalculated;
        _uidirector.UpgradeModeCountChanged += OnUpgradeModeCountChanged;
    }

    public Action<float> UpgradeApproved;

    public float CurrentUpgradePrice => _currentUpgradePrice;
    public float Price => _price;
    public int Count => _count;

    public void UpgradePlantsCount()
    {
        if (_wallet.TrySpend(_price))
        {
            _upgradedCount?.Invoke(_count, _price);
        }
    }

    public bool CanUpgrade()  //Если может, то кнопка активна
    {
        return _wallet.CanSpend(_price);
    }

    private float CalculatePrice(int count)
    {
        return _currentUpgradePrice * ((Mathf.Pow(MULTIPLYER, count) - 1f) / (MULTIPLYER - 1f));
    }

    private int CalculateMaxCount()
    {
        return Mathf.FloorToInt(Mathf.Log(1f + _wallet.Amount * (MULTIPLYER - 1f) / _currentUpgradePrice) / Mathf.Log(MULTIPLYER));
    }

    private void OnUpgradeModeCountChanged(UpgradeModeCountButtonType type)
    {
        _count = type switch
        {
            UpgradeModeCountButtonType.x1 => 1,
            UpgradeModeCountButtonType.x10 => 10,
            UpgradeModeCountButtonType.treshold => Math.Max(0, _countTreshold - _data.PlantCount),
            UpgradeModeCountButtonType.max => CalculateMaxCount(),
            _ => throw new InvalidCastException(nameof(_count)),
        };

        _price = CalculatePrice(_count);

        _recalculated?.Invoke();
    }

    public void Dispose()
    {
        _uidirector.UpgradeModeCountChanged -= OnUpgradeModeCountChanged;
    }
}