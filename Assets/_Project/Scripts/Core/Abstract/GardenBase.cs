using UnityEngine;

public abstract class GardenBase : MonoBehaviour, ICollectable
{
    [SerializeField] private GardenFillingMarker _fillingMarker;
    [SerializeField] private float _purchasePrice;
    [SerializeField] private float _coinRevenue;
    [SerializeField] private float _cultivationDurationInSeconds;

    private readonly Storage _storage = new();
    private Grover _grover;

    public float Progress => _grover.Progress;

    private void Awake()
    {
        _grover = new(_cultivationDurationInSeconds, OnGrowCompleted);
        _fillingMarker.Hide();
    }

    private void OnDestroy() =>
        _grover.Dispose();

    public void Collect()
    {
        Debug.Log("Collect");
    }

    private void OnGrowCompleted()
    {
        _fillingMarker.Show();
        _storage.Increase(gameObject, _coinRevenue);
    }
}