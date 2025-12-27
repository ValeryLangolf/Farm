using UnityEngine;

public static class MonoExtansion
{
    public static void Show(this MonoBehaviour monoBehaviour) =>
        monoBehaviour.gameObject.SetActive(true);

    public static void Hide(this MonoBehaviour monoBehaviour) =>
        monoBehaviour.gameObject.SetActive(false);

    public static void SetActive(this MonoBehaviour monoBehaviour, bool isActive) =>
        monoBehaviour.gameObject.SetActive(isActive);

    public static bool IsActiveSelf(this MonoBehaviour monoBehaviour) =>
        monoBehaviour.gameObject.activeSelf;
}