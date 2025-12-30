using TMPro;
using UnityEngine;

public class GardenPlantsCountToUpgradeTextIndicator : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _text;

    private IReadOnlyGardenData _data;

    private void Awake() =>
        _data = _garden.ReadOnlyData;

    private void OnEnable()
    {
        OnPlantsCountToUpgradeChanged(_data.PlantsCountToUpgrade);
        _data.PlantsCountToUpgradeChanged += OnPlantsCountToUpgradeChanged;
    }

    private void OnDisable() =>
        _data.PlantsCountChanged -= OnPlantsCountToUpgradeChanged;

    private void OnPlantsCountToUpgradeChanged(int value) =>
        _text.text = $"+{value}";
}