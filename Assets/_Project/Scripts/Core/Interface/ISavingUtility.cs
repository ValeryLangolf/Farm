public interface ISavingUtility
{
    void Save<T>(T data) where T : class;

    bool TryLoad<T>(out T data) where T : class;

    void DeleteSaveFile();
}