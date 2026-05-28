public interface ISceneLoader
{
    public string CurrentSceneName { get; }

    public void ReloadCurrentScene();

    public void LoadScene(string name);
}