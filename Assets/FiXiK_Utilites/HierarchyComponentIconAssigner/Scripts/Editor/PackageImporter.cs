#if UNITY_EDITOR
using UnityEditor;

namespace FiXiK.HierarchyComponentIconAssigner
{
    public class PackageImporter
    {
        private const string EditorPrefsKey = "FiXiK.HierarchyComponentIconAssigner_Imported";

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            bool hasBeenImported = EditorPrefs.GetBool(EditorPrefsKey, false);

            if (hasBeenImported == false)
            {
                EditorApplication.delayCall += () =>
                {
                    ShowWelcomeWindow();
                    EditorPrefs.SetBool(EditorPrefsKey, true);
                };
            }
        }

        private static void ShowWelcomeWindow()
        {
            HierarchyIconsWindow.ShowWindow();

            string tittle = "Hierarchy Component Icon Assigner";
            string message = "Пакет успешно установлен!\n\n" +
                "Окно настройки иконок открыто автоматически.\n" +
                $"Вы всегда можете открыть его через меню: \"{HierarchyIconsWindow.MenuPath} -> {HierarchyIconsWindow.WindowTitle}\"";

            string author = "С уважением, ваш FiXiK";
            string buttonCancel = "От души, бро!";

            EditorUtility.DisplayDialog(tittle, message + "\n\n" + author, buttonCancel);
        }

        //[MenuItem("Tools/Reset Import Flag")]
        //private static void ResetImportFlag() =>
        //    EditorPrefs.DeleteKey(EditorPrefsKey);
    }
}
#endif