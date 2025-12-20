using UnityEngine;

public static class MonoExtansion
{
    public static void Show(this MonoBehaviour monoBehaviour) =>
        monoBehaviour.gameObject.SetActive(true);

    public static void Hide(this MonoBehaviour monoBehaviour) =>
        monoBehaviour.gameObject.SetActive(false);
}