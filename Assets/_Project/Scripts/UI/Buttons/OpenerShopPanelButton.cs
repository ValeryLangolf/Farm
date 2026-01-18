
using UnityEngine;

public class OpenerShopPanelButton : ButtonClickHandler 
{
    [SerializeField] private Transform _center;

    public Transform Center => _center;
}