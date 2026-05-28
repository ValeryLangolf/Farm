using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
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

        _musicModifier = new(_mixer, _musicSlider, Constants.MusicGroup);
        _sfxModifier = new(_mixer, _sfxSlider, Constants.SfxGroup);
    }
}