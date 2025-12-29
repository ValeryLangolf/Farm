using System;
public class Storage
{
    private readonly ExtendedGardenData _data;

    public Storage(ExtendedGardenData data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public bool IsFilled => _data.StorageFullness >= _data.StorageCapacity;

    public void SetCapacity(float value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"значение должно быть больше нуля");

        _data.SetStorageCapacity(value);
    }

    public float GiveCoins()
    {
        float tempCapacity = _data.StorageFullness;
        _data.SetStorageFullnes(0);

        return tempCapacity;
    }

    public void Increase(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Значение должно быть положительным");

        _data.SetStorageFullnes(_data.StorageFullness + value);
    }
}