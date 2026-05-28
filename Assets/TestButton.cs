using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TestButton : MonoBehaviour
{
    [SerializeField] private SavingMediator _savingMediator;

    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    private void OnEnable() =>
        GetComponent<Button>().onClick.AddListener(OnClick);

    private void OnDisable() =>
        GetComponent<Button>().onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        if (_sceneLoader.CurrentSceneName == Constants.FirstLocationSceneName)
        {
            _savingMediator.Save();
            _sceneLoader.LoadScene(Constants.SecondLocationSceneName);
        }
        else if (_sceneLoader.CurrentSceneName == Constants.SecondLocationSceneName)
        {
            _savingMediator.Save();
            _sceneLoader.LoadScene(Constants.FirstLocationSceneName);
        }
    }
}