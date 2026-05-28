using System;
using UnityEngine;
using VContainer;

public class BootstrapScene : MonoBehaviour, IInjactable
{
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    private void Start() =>
        _sceneLoader.LoadScene(Constants.FirstLocationSceneName);
}