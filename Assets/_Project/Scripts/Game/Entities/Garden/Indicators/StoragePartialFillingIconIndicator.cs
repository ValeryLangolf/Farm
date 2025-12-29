using UnityEngine;

public class StoragePartialFillingIconIndicator : MonoBehaviour 
{
    [SerializeField] private Garden _garden;
    [SerializeField] private GameObject _indicator;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnStorageChanged(_data.StorageProgress);
        _data.StorageProgressChanged += OnStorageChanged;
    }

    private void OnDisable() =>
        _data.StorageProgressChanged += OnStorageChanged;

    private void OnStorageChanged(float progress) =>
        _indicator.SetActive(progress > 0);
}