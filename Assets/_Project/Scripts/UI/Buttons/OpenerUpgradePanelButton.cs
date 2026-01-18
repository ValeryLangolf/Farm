using UnityEngine;

public class OpenerUpgradePanelButton : ButtonClickHandler
{
    [SerializeField] private Transform _center;

    public Transform Center => _center;
}