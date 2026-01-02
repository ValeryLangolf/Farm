using UnityEngine;
using UnityEngine.UI;

public abstract class PageScrollButtonBase : MonoBehaviour
{
    [SerializeField] private PagedContainer _pagedContainer;
    [SerializeField] private Button _button;

    public PagedContainer PagedContainer => _pagedContainer;

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(OnClick);

        OnPageChanged(_pagedContainer.CurrentPage, _pagedContainer.TotalPages);
        _pagedContainer.PageChanged += OnPageChanged;
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        _pagedContainer.PageChanged -= OnPageChanged;
    }

    protected abstract void OnClick();

    protected abstract void OnPageChanged(int currentPage, int totalPages);

    protected void SetActiveButton(bool isActive) =>
        _button.gameObject.SetActive(isActive);
}