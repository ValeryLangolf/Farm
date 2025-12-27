using UnityEngine;
using UnityEngine.UI;

public class AdsButtonUI : ButtonClickHandler
{
    [SerializeField] private Button _button;
}

public enum AdsButtonState
{
    AdsReady,
    ActivateReady
}
