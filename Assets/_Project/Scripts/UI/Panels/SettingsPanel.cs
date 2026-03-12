using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private SliderInformer _musicSlider;
    [SerializeField] private SliderInformer _sfxSlider;

    private VolumeModifier _musicModifier;
    private VolumeModifier _sfxModifier;

    public float MusicVolume => _musicSlider.Value;

    public float SfxVolume => _sfxSlider.Value;

    private void OnDestroy()
    {
        _musicModifier?.Dispose();
        _sfxModifier?.Dispose();
    }

    public void SetData(float musicVolume, float sfxVolume)
    {
        _musicSlider.SetValue(musicVolume);
        _sfxSlider.SetValue(sfxVolume);
    }

    public void Initialize()
    {
        _musicSlider.Initialize();
        _sfxSlider.Initialize();

        AudioMixer mixer = ServiceLocator.Get<IAudioService>().Mixer;
        _musicModifier = new(mixer, _musicSlider, Constants.MusicGroup);
        _sfxModifier = new(mixer, _sfxSlider, Constants.SfxGroup);
    }
}