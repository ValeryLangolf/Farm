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
        SavingMediator savingMediator = ServiceLocator.Get<SavingMediator>();

        if (sceneLoader.CurrentSceneName == Constants.FirstLocationSceneName)
        {
            savingMediator.Save();
            sceneLoader.LoadScene(Constants.SecondLocationSceneName);
        }
        else if (sceneLoader.CurrentSceneName == Constants.SecondLocationSceneName)
        {
            savingMediator.Save();
            sceneLoader.LoadScene(Constants.FirstLocationSceneName);
        }
    }
}