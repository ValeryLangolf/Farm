using System.Collections.Generic;
using UnityEngine;

public class ProductionTimeDisplay : MonoBehaviour
{
    [SerializeField] private ProductionTimeItem _itemPrefab;

    private IReadOnlyList<Garden> _gardens;

    private void Awake()
    {
        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    private void OnEnable()
    {
        foreach(Garden garden in _gardens)
            garden.ReadOnlyData.PlantCountThresholdChanged += OnTresholdChanged;
    }

    private void OnDisable()
    {
        foreach (Garden garden in _gardens)
            garden.ReadOnlyData.PlantCountThresholdChanged -= OnTresholdChanged;
    }

    private void CreateItem(Sprite sprite)
    {
        ProductionTimeItem item = Instantiate(_itemPrefab, transform);
        item.SetInfo(sprite);
    }

    private void OnTresholdChanged(Sprite icon) =>
        CreateItem(icon);
}