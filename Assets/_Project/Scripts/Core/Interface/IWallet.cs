using System;

public interface IWallet
{
    event Action<float> Changed;

    float Amount { get; }

    void SetAmount(float amount);

    void Increase(float value);

    bool TrySpend(float price);

    bool CanSpend(float price);
}