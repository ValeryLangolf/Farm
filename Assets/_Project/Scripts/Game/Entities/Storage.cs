using System;

public class Storage
{
    private readonly Action<float> _changed;

    private float _capacity;
    private float _currentValue;

    public Storage(float capacity, float currentValue, Action<float> changed)
    {
        _capacity = capacity > 0? capacity : throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "«начение должно быть больше нул€");
        
        if(currentValue < 0) 
            throw new ArgumentOutOfRangeException(nameof(currentValue), currentValue, "«начение должно быть больше или равно нулю");

        _changed = changed;
        Increase(currentValue);
    }

    public bool IsFilled => _currentValue >= _capacity;

    public float Progress => _currentValue / _capacity;

    public void SetCapacity(float value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"значение должно быть больше нул€");

        _capacity = value;
    }

    public float GiveCoins()
    {
        float tempCapacity = _currentValue;
        _currentValue = 0;
        _changed?.Invoke(Progress);

        return tempCapacity;
    }

    public void Increase(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"«начение должно быть положительным");

        _currentValue += value;

        _changed?.Invoke(Progress);
    }
}