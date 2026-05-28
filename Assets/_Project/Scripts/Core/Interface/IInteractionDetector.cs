using System;
using System.Collections.Generic;

public interface IInteractionDetector
{
    event Action<IReadOnlyList<InteractionInfo>> Swiped;
    event Action<IReadOnlyList<InteractionInfo>> Clicked;

    event Action<IReadOnlyList<InteractionInfo>> InteractionsUpdated;
    event Action<IReadOnlyList<InteractionInfo>> InteractionsStarted;
    event Action<IReadOnlyList<InteractionInfo>> InteractionsEnded;

    IReadOnlyList<InteractionInfo> CurrentInteractions { get; }

    public void PauseRun();

    public void ResumeRun();

    public void StartRun();

    public void StopRun();
}