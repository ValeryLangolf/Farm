using System;
using System.IO;
using UnityEngine;

public class JsonSavingUtility : ISavingUtility
{
    private readonly string _filePath;
    private readonly IEncryptor _encryptor;

    public JsonSavingUtility(string fileName, IEncryptor encryptor)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("Имя файла не может быть пустым", nameof(fileName));

        _encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
        _filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
    }

    public void Save<T>(T data) where T : class
    {
        if (data == null)
        {
            Debug.LogError("Попытка сохранить null data");

            return;
        }

        try
        {
            string json = JsonUtility.ToJson(data, true);
            string encrypted = _encryptor.Encrypt(json);
            File.WriteAllText(_filePath, encrypted);
        }
        catch (Exception exception)
        {
            Debug.LogError($"Ошибка сохранения: {exception.Message}");
        }
    }

    public bool Load<T>(out T data) where T : class
    {
        data = null;

        if (File.Exists(_filePath) == false)
            return false;

        try
        {
            string encrypted = File.ReadAllText(_filePath);
            string json = _encryptor.Decrypt(encrypted);
            data = JsonUtility.FromJson<T>(json);

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError($"Ошибка загрузки сохранений: {exception.Message}");
            
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }
}