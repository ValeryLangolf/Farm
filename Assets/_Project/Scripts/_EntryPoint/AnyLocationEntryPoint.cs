using System;
using UnityEngine;
using VContainer;

public class AnyLocationEntryPoint : MonoBehaviour, IInjactable
{
    [SerializeField] private Tutorial _tutorial;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
    }

    private void Start()
    {
        _audioService.Music.Play();

        if (_tutorial != null)
            _tutorial.Run();
    }
}