using System;
using UnityEngine;

public class Storage
{
    private readonly Action<float> _changed;

    private long _capacity;
    private long _currentValue;

    public Storage(long capacity, long currentValue, Action<float> changed)
    {
        _capacity = capacity > 0? capacity : throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "«начение должно быть больше нул€");
        
        if(currentValue < 0) 
            throw new ArgumentOutOfRangeException(nameof(currentValue), currentValue, "«начение должно быть больше или равно нулю");

        _changed = changed;
        Increase(currentValue);
    }

    public bool IsFilled => _currentValue >= _capacity;

    public float Progress => Mathf.Min(_currentValue / _capacity, _capacity);

    public void SetCapacity(long value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"значение должно быть больше нул€");

        _capacity = value;
    }

    public long GiveCoins()
    {
        long tempCapacity = _currentValue;
        _currentValue = 0;

        return tempCapacity;
    }

    public void Increase(long value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"«начение должно быть положительным");

        _currentValue += value;
        _changed?.Invoke(value);
    }
}