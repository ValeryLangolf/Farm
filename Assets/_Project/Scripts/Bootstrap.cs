using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Garden> _gardens;
    [SerializeField] private ParticleSystem _trailParticle;
    [SerializeField] private ProgressResetterButton _progressResetterButton;

    private void Awake()
    {
        RegisterServices();
        StartRunServices();
    }

    private void OnDisable()
    {
        if (ServiceLocator.TryGet(out SavingMediator savingMediator))
            savingMediator.Save();
    }

    private void OnDestroy()
    {
        StopRunServices();
        DisposeServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Register(UpdateService.Instance);

        IWallet wallet = new Wallet();
        ServiceLocator.Register(wallet);

        ServiceLocator.Register(new SavingMediator(
             _gardens,
             wallet,
             _progressResetterButton));

        IInteractionDetector interactionDetector;
        IInputFollower inputFollower;

        if (Application.isMobilePlatform)
        {
            interactionDetector = new TouchInteractionDetector();
            inputFollower = new TouchInputFollower(_trailParticle.transform);
        }
        else
        {
            interactionDetector = new MouseInteractionDetector();
            inputFollower = new MouseInputFollower(_trailParticle.transform);
        }

        ServiceLocator.Register(inputFollower);
        ServiceLocator.Register(interactionDetector);
        ServiceLocator.Register(new CoinCollector(interactionDetector, wallet));
        ServiceLocator.Register(new InteractionHandler(interactionDetector));
        ServiceLocator.Register(new InputTrailParticle(_trailParticle, interactionDetector));
    }

    private void StartRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StartRun();
    }

    private void StopRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StopRun();
    }

    private void DisposeServices()
    {
        IReadOnlyList<IDisposable> disposables = ServiceLocator.GetServices<IDisposable>();

        foreach (IDisposable disposable in disposables)
            disposable?.Dispose();
    }
}
