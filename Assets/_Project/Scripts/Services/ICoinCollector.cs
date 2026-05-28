using System;

public interface ICoinCollector
{
    public event Action Collected;

    public void SetEnabled(bool isEnabled);
}