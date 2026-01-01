using UnityEngine.Audio;

public interface IAudioService : IService 
{
    Music Music { get; }

    Sfx Sfx { get; }

    AudioMixer Mixer { get; }
}