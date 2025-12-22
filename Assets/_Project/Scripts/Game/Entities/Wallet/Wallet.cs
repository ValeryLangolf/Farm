using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private SavesData _data = new();

    public event Action<float> Changed;

    public float Amount => _data.WalletAmount;

    public void SetData(SavesData data)
    {
        _data = data;

        if (_data.WalletAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(_data.WalletAmount), _data.WalletAmount, "Значение должно быть положительным");

        Changed?.Invoke(_data.WalletAmount);
    }

    public void Increase(float value)
    {
        if(value < 0) 
            throw new ArgumentOutOfRangeException(nameof(value), value, "Значение должно быть положительным");

        _data.WalletAmount += value;

        Changed?.Invoke(_data.WalletAmount);
    }

    public bool TrySpend(float price)
    {
        if(CanSpend(price))
        {
            _data.WalletAmount -= price;
            Changed?.Invoke(_data.WalletAmount);

            return true;
        }

        return false;
    }

    public bool CanSpend(float price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Значение должно быть положительным");

        return price <= _data.WalletAmount;
    }
}