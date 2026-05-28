using UnityEngine.Audio;

public interface IAudioService 
{
    Music Music { get; }

    Sfx Sfx { get; }

    AudioMixer Mixer { get; }
}
