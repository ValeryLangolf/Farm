using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private void Awake() =>
        RegisterServices();

    private void Start() =>
        StartRunServices();

    private void RegisterServices()
    {
        IUpdateService updateService = UpdateService.Instance;
        ServiceLocator.Register(updateService);
        ServiceLocator.Register(AudioService.Instance as IAudioService);
        ServiceLocator.Register(SceneLoader.Instance);

        IInteractionDetector interactionDetector =
            Application.isMobilePlatform ?
            new TouchInteractionDetector() :
            new MouseInteractionDetector();

        IPointerPositionProvider pointerPositionProvider =
            Application.isMobilePlatform ?
            new TouchPositionProvider(updateService) :
            new MousePositionProvider(updateService);

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

        ServiceLocator.Get<SceneLoader>().LoadScene(Constants.FirstLocationSceneName);
    }
}