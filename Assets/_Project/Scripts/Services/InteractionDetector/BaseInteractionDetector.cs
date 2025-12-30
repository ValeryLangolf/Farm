using System;
using System.Collections.Generic;

public abstract class BaseInteractionDetector : IInteractionDetector, IRunnable, IDisposable
{
    protected readonly InteractionStateHandler StateHandler = new();
    protected readonly Updater Updater = new();

    protected BaseInteractionDetector()
    {
        Updater.Updated += HandleUpdate;
    }

    public event Action<IReadOnlyList<InteractionInfo>> Swiped;
    public event Action<IReadOnlyList<InteractionInfo>> Clicked;

    public IReadOnlyList<InteractionInfo> CurrentInteractions => StateHandler.CurrentInteractions;

    public event Action<IReadOnlyList<InteractionInfo>> InteractionsUpdated
    {
        add => StateHandler.InteractionsUpdated += value;
        remove => StateHandler.InteractionsUpdated -= value;
    }

    public event Action<IReadOnlyList<InteractionInfo>> InteractionsStarted
    {
        add => StateHandler.InteractionsStarted += value;
        remove => StateHandler.InteractionsStarted -= value;
    }

    public event Action<IReadOnlyList<InteractionInfo>> InteractionsEnded
    {
        add => StateHandler.InteractionsEnded += value;
        remove => StateHandler.InteractionsEnded -= value;
    }

    public virtual void Dispose()
    {
        StopRun();
        Updater.Dispose();
        Swiped = null;
        Clicked = null;
    }

    public void PauseRun() =>
        StopRun();

    public void ResumeRun() =>
        StartRun();

    public void StartRun() =>
        Updater.Subscribe();

    public void StopRun() =>
        Updater.Unsubscribe();

    protected abstract void HandleUpdate(float deltaTime);

    protected void InvokeSwiped(IReadOnlyList<InteractionInfo> interactions) =>
        Swiped?.Invoke(interactions);

    protected void InvokeClicked(IReadOnlyList<InteractionInfo> interactions) =>
        Clicked?.Invoke(interactions);
}