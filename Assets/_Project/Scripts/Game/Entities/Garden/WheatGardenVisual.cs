using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheatGardenVisual : MonoBehaviour
{
    private const float Threshold = 0.01f;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _progressBar;
    [SerializeField] private WheatGarden _garden;

    private float _progressToText = 0;

    private void Update()
    {
        DisplayProgress();
    }

    private void DisplayProgress()
    {
        _progressBar.fillAmount = _garden.Progress;

        if (Mathf.Abs(_garden.Progress - _progressToText) > Threshold)
        {
            _progressToText = _progressBar.fillAmount;
            _text.text = $"{_progressToText * 100:F0}%";
        }
    }
}