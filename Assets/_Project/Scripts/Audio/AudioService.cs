using UnityEngine;
using UnityEngine.Audio;

public class AudioService : MonoBehaviour, IAudioService
{
    private const string AudioServicePath = "AudioService";

    private static AudioService s_instance;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Music _music;
    [SerializeField] private Sfx _sfx;

    public static AudioService Instance
    {
        get
        {
            if (s_instance == null)
            {
                AudioService prefab = Resources.Load<AudioService>(AudioServicePath);

                if (prefab == null)
                    throw new System.Exception($"SceneLoader prefab not found at path: {AudioServicePath}");

                s_instance = Instantiate(prefab);
                DontDestroyOnLoad(s_instance.gameObject);
                s_instance.gameObject.SetActive(true);
            }

            return s_instance;
        }
    }

    public Music Music => _music;

    public Sfx Sfx => _sfx;

    public AudioMixer Mixer => _mixer;

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
}