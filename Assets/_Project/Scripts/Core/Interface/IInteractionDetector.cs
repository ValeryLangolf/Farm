using System;
using System.Collections.Generic;

public interface IInteractionDetector : IService, IRunnable
{
    event Action<IReadOnlyList<InteractionInfo>> Swiped;
    event Action<IReadOnlyList<InteractionInfo>> Clicked;

    event Action<IReadOnlyList<InteractionInfo>> InteractionsUpdated;
    event Action<IReadOnlyList<InteractionInfo>> InteractionsStarted;
    event Action<IReadOnlyList<InteractionInfo>> InteractionsEnded;

    IReadOnlyList<InteractionInfo> CurrentInteractions { get; }
}