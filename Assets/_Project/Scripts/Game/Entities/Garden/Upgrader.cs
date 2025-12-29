using System;
using UnityEngine;

public class Upgrader : IDisposable
{
    private const float MULTIPLYER = 1.2f;
    private readonly Action<int, float> _upgradedCount;
    private readonly Action _recalculated;
    private readonly UIDirector _uidirector;

    private readonly ExtendedGardenData _data;
    private readonly IWallet _wallet;
    private float _priceToUpgrade = 1; //Добавить инфу в дату?
    private int _plantsCountToUpgrade;
    private float _initialPrice;
    private int _countTreshold = 25;

    public Upgrader(ExtendedGardenData data, Action<int, float> onUpgradedCount, Action recalculated)
    {
        _data = data;
        _wallet = ServiceLocator.Get<IWallet>();
        _upgradedCount = onUpgradedCount;
        _uidirector = ServiceLocator.Get<UIDirector>();
        _recalculated = recalculated;
        _uidirector.UpgradeModeCountChanged += OnUpgradeModeCountChanged;
    }

    public Action<float> UpgradeApproved;

    public float CurrentUpgradePrice => _priceToUpgrade;
    public float Price => _priceToUpgrade;
    public int Count => _plantsCountToUpgrade;

    public void UpgradePlantsCount()
    {
        if (_wallet.TrySpend(_priceToUpgrade))
        {
            _priceToUpgrade = CalculatePrice(_countTreshold);
            _upgradedCount?.Invoke(_plantsCountToUpgrade, _priceToUpgrade);
            _data.SetPlantsCount(_data.PlantsCount + _plantsCountToUpgrade);
        }
    }

    private float CalculatePrice(int count)
    {
        _priceToUpgrade = _initialPrice * ((Mathf.Pow(MULTIPLYER, count) - 1f) / (MULTIPLYER - 1f));
        return _priceToUpgrade;
    }

    private int CalculateMaxCount()
    {
        _plantsCountToUpgrade = Mathf.FloorToInt(Mathf.Log(1f + _wallet.Amount * (MULTIPLYER - 1f) / _initialPrice) / Mathf.Log(MULTIPLYER));
        return _plantsCountToUpgrade;
    }

    private void OnUpgradeModeCountChanged(UpgradeModeCountButtonType type)
    {
        _plantsCountToUpgrade = type switch
        {
            UpgradeModeCountButtonType.x1 => 1,
            UpgradeModeCountButtonType.x10 => 10,
            UpgradeModeCountButtonType.treshold => Math.Max(0, _countTreshold - _data.PlantsCount),
            UpgradeModeCountButtonType.max => CalculateMaxCount(),
            _ => throw new InvalidCastException(nameof(_plantsCountToUpgrade)),
        };

        _priceToUpgrade = CalculatePrice(_plantsCountToUpgrade);

        _recalculated?.Invoke();
    }

    public void Dispose()
    {
        _uidirector.UpgradeModeCountChanged -= OnUpgradeModeCountChanged;
    }
}