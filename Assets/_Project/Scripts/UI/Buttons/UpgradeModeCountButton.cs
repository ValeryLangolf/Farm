using UnityEngine;

public class UpgradeModeCountButton : ButtonClickHandler 
{
    [SerializeField] private UpgradeModeCountButtonType _type;

    public UpgradeModeCountButtonType Type => _type;

    public void SetToggled()
    {
        SetColor(Color.yellow);
    }

    public void SetUntoggled()
    {
        SetColor(Color.white);
    }
}
