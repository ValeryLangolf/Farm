public interface ISaver<T>
{
    public T Data { get; }

    public void Save(T data);

    public T Load();

    public void ResetProgress();
}