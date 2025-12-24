using System;

public interface IWallet : IService
{
    event Action<float> Changed;

    float Amount { get; }

    void SetData(SavesData data);

    void Increase(float value);

    bool TrySpend(float price);

    bool CanSpend(float price);
}