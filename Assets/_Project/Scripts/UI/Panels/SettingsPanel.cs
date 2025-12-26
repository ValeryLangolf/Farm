using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private SliderInformer _musicSlider;
    [SerializeField] private SliderInformer _sfxSlider;

    private SavesData _savesData;
    private VolumeModifier _musicModifier;
    private VolumeModifier _sfxModifier;

    private void Awake()
    {
        InitializeVolumeSliders();
        InitializeVolumeModifiers();
    }

    private void OnEnable()
    {
        _musicSlider.Changed += OnChangedMusicSlider;
        _sfxSlider.Changed += OnChangedSfxSlider;
    }

    private void OnDisable()
    {
        _musicSlider.Changed -= OnChangedMusicSlider;
        _sfxSlider.Changed -= OnChangedSfxSlider;
    }

    private void OnDestroy()
    {
        _musicModifier?.Dispose();
        _sfxModifier?.Dispose();
    }

    public void SetData(SavesData savesData) =>
        _savesData = savesData;

    private void InitializeVolumeSliders() =>
        ResetSlidersToSavedValues();

    private void InitializeVolumeModifiers()
    {
        AudioMixer mixer = ServiceLocator.Get<IAudioService>().Mixer;
        _musicModifier = new(mixer, _musicSlider, Constants.MusicGroup);
        _sfxModifier = new(mixer, _sfxSlider, Constants.SfxGroup);
    }

    private void ResetSlidersToSavedValues()
    {
        _musicSlider.SetValue(_savesData.MusicVolume);
        _sfxSlider.SetValue(_savesData.SfxVolume);
    }

    private void OnChangedMusicSlider(float obj)
    {
        _savesData.MusicVolume = _musicSlider.Value;
    }

    private void OnChangedSfxSlider(float obj)
    {
        _savesData.SfxVolume = _sfxSlider.Value;
    }
}