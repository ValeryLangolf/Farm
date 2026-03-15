#if UNITY_EDITOR
using System;
using System.IO;
using PlasticGui.PreferencesWindow;
using UnityEditor;
using UnityEngine;

public class SaveManagerWindow : EditorWindow
{
    private const string WindowName = "Диспетчер сохранений";
    private const string ToolsName = "Tools/" + WindowName;
    private const string MessageFileNotFound = "File not found";
    private const string MessageSaveFileDoesNotExist = "Файл сохранений отсутствует";
    private const string MessageFolderNotFound = "Путь к файлу не найден";
    private const string MessageDataPathNotExist = "Не удалось получить путь к файлу";
    private const string ButtonOk = "Ясненько";
    private const string ButtonYes = "Да";
    private const string ButtonNo = "Нет";

    private string _filePath;
    private bool _fileExists;
    private long _fileSize;
    private DateTime _lastWriteTime;

    [MenuItem(ToolsName)]
    private static void ShowWindow()
    {
        SaveManagerWindow window = GetWindow<SaveManagerWindow>();
        window.titleContent = new GUIContent(WindowName);
        window.Show();
    }

    private void OnEnable()
    {
        UpdateFileInfo();
    }

    private void UpdateFileInfo()
    {
        _filePath = Path.Combine(Application.persistentDataPath, Constants.SavesFileName);
        _fileExists = File.Exists(_filePath);

        if (_fileExists)
        {
            FileInfo info = new(_filePath);
            _fileSize = info.Length;
            _lastWriteTime = info.LastWriteTime;
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Информация о сохранённом файле", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Путь:", _filePath);
        EditorGUILayout.LabelField("Существует:", _fileExists ? "Да" : "Нет");

        if (_fileExists)
        {
            EditorGUILayout.LabelField("Размер:", FormatFileSize(_fileSize));
            EditorGUILayout.LabelField("Последнее изменение:", _lastWriteTime.ToString("dd.MM.yyyy в HHч mmм ssс"));
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Обновить инфу", GUILayout.Height(30)))
            UpdateInfo();

        GUI.enabled = _fileExists;

        if (GUILayout.Button("Открыть файл", GUILayout.Height(30)))
            OpenFile();

        if (GUILayout.Button("Открыть путь к файлу", GUILayout.Height(30)))
            OpenFolder();

        if (GUILayout.Button("Удалить файл сохранений", GUILayout.Height(30)))
            DeleteFile();

        GUI.enabled = true;

        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox("Используй эти инструменты для управления файлом сохранения игры. Удаление приведёт к обнулению прогресса.", MessageType.Info);
    }

    private void UpdateInfo() =>
        UpdateFileInfo();

    private void OpenFile()
    {
        if (_fileExists == false)
        {
            EditorUtility.DisplayDialog(MessageFileNotFound, MessageSaveFileDoesNotExist, ButtonOk);

            return;
        }

        try
        {
            EditorUtility.OpenWithDefaultApp(_filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open file: {e.Message}");
            EditorUtility.DisplayDialog("Error", "Could not open the file. See console for details.", ButtonOk);
        }
    }

    private void OpenFolder()
    {
        if (_fileExists == false)
        {
            string directory = Application.persistentDataPath;

            if (Directory.Exists(directory))
                EditorUtility.RevealInFinder(directory);
            else
                EditorUtility.DisplayDialog(MessageFolderNotFound, MessageDataPathNotExist, ButtonOk);

            return;
        }

        try
        {
            EditorUtility.RevealInFinder(_filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open folder: {e.Message}");
            EditorUtility.DisplayDialog("Error", "Could not open the folder. See console for details.", ButtonOk);
        }
    }

    private void DeleteFile()
    {
        if (_fileExists == false)
        {
            EditorUtility.DisplayDialog(MessageFileNotFound, MessageSaveFileDoesNotExist, ButtonOk);

            return;
        }

        bool confirm = EditorUtility.DisplayDialog(
            "Подтвердите удаление",
            "Вы уверены, что хотите удалить сохраненный файл? Это действие невозможно отменить",
            ButtonYes,
            ButtonNo);

        if (confirm)
        {
            try
            {
                File.Delete(_filePath);
                UpdateFileInfo();
                Debug.Log("Сохранённый файл удалён");
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось удалить файл: {e.Message}");
                EditorUtility.DisplayDialog("Ошибка", "Не удалось удалить файл. Подробности смотрите в разделе консоль", ButtonOk);
            }
        }
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
#endif