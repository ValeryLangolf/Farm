using UnityEngine;
using UnityEngine.Audio;

public class AudioService : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Music _music;
    [SerializeField] private Sfx _sfx;

    public Music Music => _music;

    public Sfx Sfx => _sfx;

    public AudioMixer Mixer => _mixer;
}