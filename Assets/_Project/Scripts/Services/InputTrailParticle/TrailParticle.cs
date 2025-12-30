using System;
using System.Collections;
using UnityEngine;

public class TrailParticle : MonoBehaviour, IPoolable<TrailParticle>
{
    [SerializeField] private ParticleSystem _particleSystem;

    private Camera _mainCamera;

    public event Action<TrailParticle> Deactivated;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnDisable() =>
        Deactivated?.Invoke(this);

    public void Initialize()
    {
        gameObject.SetActive(true);
        _particleSystem.Play();
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        if (_mainCamera == null)
            return;

        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y,
            Mathf.Abs(_mainCamera.transform.position.z)));

        worldPosition.z = 0;
        transform.position = worldPosition;
    }

    public void Stop()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        if (_particleSystem.main.stopAction == ParticleSystemStopAction.Disable)
            StartCoroutine(WaitForParticlesToFinish());
        else
            Deactivate();
    }

    private IEnumerator WaitForParticlesToFinish()
    {
        while (_particleSystem.IsAlive(true))
            yield return null;

        Deactivate();
    }

    public void Deactivate() =>
        Deactivated?.Invoke(this);
}