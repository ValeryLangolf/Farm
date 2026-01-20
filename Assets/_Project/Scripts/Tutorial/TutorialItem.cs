using System;
using UnityEngine;

public abstract class TutorialItem : MonoBehaviour
{
    public event Action<TutorialItem> Deactivated;

    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
        OnActivated();
    }

    public void Deactivate()
    {
        OnDeactivated();
        IsActive = false;

        Deactivated?.Invoke(this);
    }

    protected abstract void OnActivated();

    protected abstract void OnDeactivated();
}