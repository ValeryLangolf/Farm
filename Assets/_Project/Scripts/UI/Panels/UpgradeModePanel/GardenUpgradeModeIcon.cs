using UnityEngine;
using UnityEngine.UI;

public class GardenUpgradeModeIcon : MonoBehaviour
{
    [SerializeField] private Garden _garden;
    [SerializeField] private Image _icon;

    private void Awake() =>
        _icon.sprite = _garden.ReadOnlyData.Icon;
}