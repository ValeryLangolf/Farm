using System;
using UnityEngine;

public class Storage
{
    private float _coinCapacity;
    private float _currentCoins;

    public bool IsFilled => _currentCoins >= _coinCapacity - Mathf.Epsilon;

    public void SetCapacity(float value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException($"{value}, значение должно быть больше нуля");

        _coinCapacity = value;
        _currentCoins = Mathf.Min(_currentCoins, _coinCapacity);
    }

    public float GiveCoins()
    {
        float tempCapacity = _currentCoins;
        _currentCoins = 0;

        return tempCapacity;
    }

    public void Increase(GameObject sender, float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException($"Отправитель: {sender.name}, {nameof(Storage)}: значение должно быть положительным");

        _currentCoins += value;
        _currentCoins = Mathf.Min(_currentCoins, _coinCapacity);
    }
}