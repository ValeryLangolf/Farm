using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _music;

    private bool _isMute = false;
    private bool _isPause = false;

    public void SetMute()
    {
        _isMute = true;
        _source.Pause();
    }

    public void ResetMute()
    {
        _isMute = false;

        if (_isPause)
            return;

        if (_source.clip != null)
        {
            UnPause();
            return;
        }

        Play();
    }

    public void Play()
    {
        Play(_music);
    }

    public void Pause()
    {
        _isPause = true;
        _source.Pause();
    }

    public void UnPause()
    {
        _isPause = false;
        _source.UnPause();

        if (_source.isPlaying == false)
            _source.Play();
    }

    private void Play(AudioClip clip)
    {
        _isPause = false;
        _source.clip = clip;
        _source.time = 0f;

        if (_isMute)
            return;

        _source.Play();
    }
}