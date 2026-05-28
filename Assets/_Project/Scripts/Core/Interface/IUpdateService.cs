using System;

public interface IUpdateService
{
    void Subscribe(Action<float> action);

    void Unsubscribe(Action<float> action);
}