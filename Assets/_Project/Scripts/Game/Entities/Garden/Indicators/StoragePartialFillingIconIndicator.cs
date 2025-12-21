using UnityEngine;

public class StoragePartialFillingIconIndicator : MonoBehaviour 
{
    [SerializeField] private Garden _garden;
    [SerializeField] private GameObject _indicator;

    private void OnEnable()
    {
        OnStorageChanged(_garden.StorageProgress);
        _garden.StorageProgressChanged += OnStorageChanged;
    }

    private void OnDisable()
    {
        _garden.StorageProgressChanged += OnStorageChanged;
    }

    private void OnStorageChanged(float progress) =>
        _indicator.SetActive(progress > 0);
}