using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour, IAudioService
{
    private static Audio s_instance;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Music _music;
    [SerializeField] private Sfx _sfx;

    public Music Music => _music;

    public Sfx Sfx => _sfx;

    public AudioMixer Mixer => _mixer;

    public void Initialize()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);

            return;
        }

        transform.SetParent(null);
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
}