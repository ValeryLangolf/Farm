public interface ISaver<T>
{
    T Data { get; }

    void Save(T data);

    T Load();

    void ResetProgress();
}
