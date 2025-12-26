using UnityEngine;

public class Sfx : MonoBehaviour
{
    private const float SlightPitchDeviation = 0.1f;
    private const float AveragePitchDeviation = 0.2f;

    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _coinCollected;

    private bool _isMute;

    public bool IsMute => _isMute;

    public void SetMute() =>
        _isMute = true;

    public void ResetMute() =>
        _isMute = false;

    public void PlayClickButton() =>
        PlayOneShot(_buttonClick);

    public void PlayCollectedCoin() =>
        PlayOneShot(_coinCollected);

    private AudioSource PlayOneShot(AudioClip clip, float deviationPitch = 0f)
    {
        if (_isMute)
            return null;

        _source.pitch = Mathf.Approximately(deviationPitch, 0)
            ? 1
            : Random.Range(1 - deviationPitch, 1 + deviationPitch);

        _source.PlayOneShot(clip);

        return _source;
    }

    private AudioSource PlayOneShot(AudioClip[] clips, float deviationPitch = 0f)
    {
        if (_isMute)
            return null;

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[randomIndex];

        return PlayOneShot(clip, deviationPitch);
    }
}