public interface ISavingUtility
{
    void Save<T>(T data) where T : class;

    bool Load<T>(out T data) where T : class;

    void DeleteSaveFile();
}