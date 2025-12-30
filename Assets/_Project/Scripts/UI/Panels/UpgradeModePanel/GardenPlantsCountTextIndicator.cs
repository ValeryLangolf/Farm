using TMPro;
using UnityEngine;

public class GardenPlantsCountTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPlantsCountChanged(_data.PlantsCount);
        _data.PlantsCountChanged += OnPlantsCountChanged;
    }

    private void OnDisable() =>
        _data.PlantsCountChanged -= OnPlantsCountChanged;

    private void OnPlantsCountChanged(int value) =>
        _text.text = value.ToString();
}