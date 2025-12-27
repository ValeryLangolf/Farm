using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private SliderInformer _musicSlider;
    [SerializeField] private SliderInformer _sfxSlider;

    private SavesData _savesData;
    private VolumeModifier _musicModifier;
    private VolumeModifier _sfxModifier;

    private void OnDestroy()
    {
        _musicModifier?.Dispose();
        _sfxModifier?.Dispose();

        Unsubscribe();
    }

    public void SetData(SavesData savesData)
    {
        _savesData = savesData;
        SetDataVolume();
    }

    public void Initialize()
    {
        Subscribe();
        _musicSlider.Initialize();
        _sfxSlider.Initialize();

        AudioMixer mixer = ServiceLocator.Get<IAudioService>().Mixer;
        _musicModifier = new(mixer, _musicSlider, Constants.MusicGroup);
        _sfxModifier = new(mixer, _sfxSlider, Constants.SfxGroup);
    }

    private void SetDataVolume()
    {
        _musicSlider.SetValue(_savesData.MusicVolume);
        _sfxSlider.SetValue(_savesData.SfxVolume);
    }

    private void Subscribe()
    {
        _musicSlider.Changed += OnChangedMusicSlider;
        _sfxSlider.Changed += OnChangedSfxSlider;
    }

    private void Unsubscribe()
    {
        if (_musicSlider != null)
            _musicSlider.Changed -= OnChangedMusicSlider;

        if (_sfxSlider != null)
            _sfxSlider.Changed -= OnChangedSfxSlider;
    }

    private void OnChangedMusicSlider(float volume) =>
        _savesData.MusicVolume = volume;

    private void OnChangedSfxSlider(float volume) =>
        _savesData.SfxVolume = volume;
}