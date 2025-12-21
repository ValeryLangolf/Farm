using TMPro;
using UnityEngine;

public class ProgressText : MonoBehaviour
{
    private const float Threshold = 0.009f;

    [SerializeField] private TextMeshProUGUI _text;

    private float _savedProgress;

    public void SetProgress(float progress)
    {
        if (Mathf.Abs(progress - _savedProgress) < Threshold)
            return;

        _savedProgress = Mathf.Clamp01(progress);
        _text.text = $"{_savedProgress * 100:F0}%";
    }
}