using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeEffectUI : UIPanel
{
    [SerializeField] private Button _activateButton;
    [SerializeField] private TextMeshProUGUI _infoTextDynamic;

    public override void  Init()
    {
        base.Init();
    }

    public void Activate()
    {

    }
}
