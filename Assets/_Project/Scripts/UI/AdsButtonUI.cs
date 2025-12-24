using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;
}

public enum AdsButtonState
{
    AdsReady,
    ActivateReady
}
