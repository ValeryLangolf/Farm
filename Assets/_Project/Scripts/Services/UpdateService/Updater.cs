using System;

public class Updater
{
    public event Action<float> Updated;

    private bool _isSubscribed;

    public void Subscribe()
    {
        if (_isSubscribed)
            return;

        if (UpdateService.IsDestroyed == false)
        {
            UpdateService.Instance.Updated += OnUpdate;
            _isSubscribed = true;
        }
    }

    public void Unsubscribe()
    {
        if (_isSubscribed == false)
            return;

        if (UpdateService.IsDestroyed == false)
            UpdateService.Instance.Updated -= OnUpdate;

        _isSubscribed = false;
    }

    public void Dispose() =>
        Unsubscribe();

    private void OnUpdate(float deltaTime) =>
        Updated?.Invoke(deltaTime);
}