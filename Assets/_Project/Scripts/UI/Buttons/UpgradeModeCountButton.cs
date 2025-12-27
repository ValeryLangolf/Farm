using UnityEngine;

public enum UpgradeModeCountButtonType
{
    x1,
    x10,
    treshold,
    max
}

public class UpgradeModeCountButton : ButtonClickHandler 
{
    [SerializeField] private UpgradeModeCountButtonType _type;

    public UpgradeModeCountButtonType Type => _type;

    public void SetToggled() =>
        SetColor(Color.yellow);

    public void SetUntoggled() =>
        SetColor(Color.white);
}