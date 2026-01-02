using TMPro;
using UnityEngine;

public class SelectedPageTextIndicator : MonoBehaviour
{
    [SerializeField] private PagedContainer _pagedContainer;
    [SerializeField] private TextMeshProUGUI _text;

    protected virtual void OnEnable()
    {
        OnPageChanged(_pagedContainer.CurrentPage, _pagedContainer.TotalPages);
        _pagedContainer.PageChanged += OnPageChanged;
    }

    protected virtual void OnDisable() =>
        _pagedContainer.PageChanged -= OnPageChanged;

    private void OnPageChanged(int currentPage, int totalPages) =>
        _text.text = $"{currentPage + 1}/{totalPages}";
}