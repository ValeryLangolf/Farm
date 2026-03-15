using System;

public class Wallet : IWallet
{
    private float _amount;

    public event Action<float> Changed;

    public float Amount => _amount;

    public void SetAmount(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Значение должно быть положительным");

        _amount = amount;

        Changed?.Invoke(_amount);
    }

    public void Increase(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Значение должно быть положительным");

        _amount += value;

        Changed?.Invoke(_amount);
    }

    public bool TrySpend(float price)
    {
        if (CanSpend(price))
        {
            _amount -= price;
            Changed?.Invoke(_amount);

            return true;
        }

        return false;
    }

    public bool CanSpend(float price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Значение должно быть положительным");

        return price <= _amount;
    }
}