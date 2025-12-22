using System;
public class Storage
{
    private readonly StorageData _data;
    private readonly Action<float> _changed;

    public Storage(StorageData data, Action<float> changed)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _changed = changed;
    }

    public bool IsFilled => _data.CurrentFullness >= _data.Capacity;

    public float Progress => _data.CurrentFullness / _data.Capacity;

    public void SetCapacity(float value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"значение должно быть больше нуля");

        _data.Capacity = value;
    }

    public float GiveCoins()
    {
        float tempCapacity = _data.CurrentFullness;
        _data.CurrentFullness = 0;
        _changed?.Invoke(Progress);

        return tempCapacity;
    }

    public void Increase(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Значение должно быть положительным");

        _data.CurrentFullness += value;

        _changed?.Invoke(Progress);
    }
}