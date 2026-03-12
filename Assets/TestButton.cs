using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private void OnEnable() =>
        GetComponent<Button>().onClick.AddListener(OnClick);

    private void OnDisable() =>
        GetComponent<Button>().onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        SceneLoader sceneLoader = ServiceLocator.Get<SceneLoader>();

        if (sceneLoader.CurrentScene == Constants.FirstLocationSceneName)
            sceneLoader.LoadScene(Constants.SecondLocationSceneName);
        else if (sceneLoader.CurrentScene == Constants.SecondLocationSceneName)
            sceneLoader.LoadScene(Constants.FirstLocationSceneName);
    }
}