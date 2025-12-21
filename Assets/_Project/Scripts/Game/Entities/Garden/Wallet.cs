using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField, Min(0)] private float _value;

    public event Action<float> Changed;

    public float Value => _value;

    public void Increase(float value)
    {
        if(_value < 0) 
            throw new ArgumentOutOfRangeException(nameof(value), value, "Значение должно быть положительным");

        _value += value;

        Changed?.Invoke(_value);
    }

    public bool TrySpend(float price)
    {
        if(CanSpend(price))
        {
            _value -= price;
            Changed?.Invoke(_value);

            return true;
        }

        return false;
    }

    public bool CanSpend(float price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Значение должно быть положительным");

        return price < _value;
    }
}