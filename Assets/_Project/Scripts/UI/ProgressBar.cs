using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void SetProgress(float progress) =>
        _fillImage.fillAmount = Mathf.Clamp01(progress);
}