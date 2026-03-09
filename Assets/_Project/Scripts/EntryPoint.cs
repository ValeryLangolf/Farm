using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private void Awake() =>
        RegisterServices();

    private void Start()
    {
        StartRunServices();
        ServiceLocator.Get<SceneLoader>().LoadScene(Constants.FirstLocationSceneName);
    }

    private void RegisterServices()
    {
        if (ServiceLocator.TryGet(out IUpdateService updateService) == false)
        {
            updateService = UpdateService.Instance;
            ServiceLocator.Register(updateService);
        }

        if (ServiceLocator.TryGet(out IAudioService _) == false)
        {
            IAudioService audioService = AudioService.Instance;
            ServiceLocator.Register(audioService);
        }

        ServiceLocator.Register(SceneLoader.Instance);

        IInteractionDetector interactionDetector;
        IPointerPositionProvider pointerPositionProvider;

        if (Application.isMobilePlatform)
        {
            interactionDetector = new TouchInteractionDetector();
            pointerPositionProvider = new TouchPositionProvider(updateService);
        }
        else
        {
            interactionDetector = new MouseInteractionDetector();
            pointerPositionProvider = new MousePositionProvider(updateService);
        }

        ServiceLocator.Register(interactionDetector);
        ServiceLocator.Register(pointerPositionProvider);
        ServiceLocator.Register(new EntityClickHandler(interactionDetector));

        if (ServiceLocator.TryGet(out InputTrailParticle _) == false)
        {
            InputTrailParticle inputTrailParticle = InputTrailParticle.Instance;
            ServiceLocator.Register(inputTrailParticle);
        }
    }

    private void StartRunServices()
    {
        IReadOnlyList<IRunnable> runnables = ServiceLocator.GetServices<IRunnable>();

        foreach (IRunnable runnable in runnables)
            runnable?.StartRun();

        ServiceLocator.Get<IAudioService>().Music.Play();
    }
}