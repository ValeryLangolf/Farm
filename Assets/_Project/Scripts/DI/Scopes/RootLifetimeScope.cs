using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private UpdateService _updateService;
    [SerializeField] private AudioService _audioService;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private InputTrailParticle _inputTrailParticle;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<IUpdateService>(_updateService);
        builder.RegisterComponent<IAudioService>(_audioService);
        builder.RegisterComponent<ISceneLoader>(_sceneLoader);
        builder.RegisterComponent<IInputTrailParticle>(_inputTrailParticle);
        builder.Register<IEntityClickHandler, EntityClickHandler>(Lifetime.Singleton);

        builder.Register<IEncryptor, NoEncrypt>(Lifetime.Singleton);
        builder.RegisterInstance(Constants.SavesFileName);
        builder.Register<ISavingUtility, JsonSavingUtility>(Lifetime.Singleton);

        if (Application.isMobilePlatform)
        {
            builder.Register<IInteractionDetector, TouchInteractionDetector>(Lifetime.Singleton);
            builder.Register<IPointerPositionProvider, TouchPositionProvider>(Lifetime.Singleton);
        }
        else
        {
            builder.Register<IInteractionDetector, MouseInteractionDetector>(Lifetime.Singleton);
            builder.Register<IPointerPositionProvider, MousePositionProvider>(Lifetime.Singleton);
        }
    }
}
