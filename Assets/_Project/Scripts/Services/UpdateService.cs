using System;
using UnityEngine;

public class UpdateService : MonoBehaviour
{
    private static UpdateService s_instance;

    private static bool s_destroyed;

    public static bool IsDestroyed => s_destroyed;

    public static UpdateService Instance
    {
        get
        {
            if (s_instance == null)
                CreateInstance();

            return s_instance;
        }
    }

    public event Action<float> Updated;

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() =>
        Updated?.Invoke(Time.deltaTime);

    private void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
            s_destroyed = true;
        }
    }

    private static void CreateInstance()
    {
        GameObject gameObject = new GameObject(nameof(UpdateService));
        s_instance = gameObject.AddComponent<UpdateService>();
    }
}