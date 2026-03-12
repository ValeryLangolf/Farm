using System;
using UnityEngine;

public interface ISaver<T>
{
    T Data { get; }

    void Save(T data);

    T Load();

    void ResetProgress();
}

public class Saver<T> : ISaver<T> where T : class
{
    private readonly ISavingUtility _savingUtility;
    private readonly T _initialData;

    private T _currentData;

    public Saver(ISavingUtility savingUtility, T initialData)
    {
        _savingUtility = savingUtility ?? throw new ArgumentNullException(nameof(savingUtility));
        _initialData = initialData ?? throw new ArgumentNullException(nameof(initialData));

        _currentData = Load();
    }

    public T Data => _currentData;

    public T Load()
    {
        if (_savingUtility.TryLoad(out _currentData) == false)
        {
            T currentData = JsonUtility.FromJson<T>(JsonUtility.ToJson(_initialData));

            return currentData;
        }

        return _initialData;
    }

    public void Save(T data)
    {
        _currentData = data;
        _savingUtility.Save(_currentData);
    }

    public void ResetProgress()
    {
        _savingUtility.DeleteSaveFile();
        _currentData = _initialData;
        _currentData = JsonUtility.FromJson<T>(JsonUtility.ToJson(_initialData));
        Save(_currentData);
    }
}