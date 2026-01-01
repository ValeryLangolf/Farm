using System;

public interface IUpdateService : IService
{
    void Subscribe(Action<float> action);

    void Unsubscribe(Action<float> action);

    bool TryUnsubscribe(Action<float> action);
}