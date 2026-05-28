using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private Fader _fader;

    private bool _isLoading;

    public string CurrentSceneName => SceneManager.GetActiveScene().name;

    public void ReloadCurrentScene()
    {
        if (_isLoading)
            return;

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        string name = GetSceneNameByIndex(currentScene);
        LoadScene(name);
    }

    public void LoadScene(string name)
    {
        if (_isLoading)
            return;

        StartCoroutine(LoadSceneRoutine(name));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _isLoading = true;
        bool waitFading = true;
        _fader.PlayFadeIn(() => waitFading = false);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (waitFading || async.progress < 0.9f)
            yield return null;

        async.allowSceneActivation = true;
        waitFading = true;
        _fader.PlayFadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;

        _isLoading = false;
    }

    private string GetSceneNameByIndex(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            throw new System.ArgumentOutOfRangeException(nameof(buildIndex));

        string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

        return System.IO.Path.GetFileNameWithoutExtension(scenePath);
    }
}