using System;

public class PageNavigation
{
    private readonly int _itemsPerPage;
    private readonly Action<int, int> _pageChanged;

    private Func<int> _getTotalItemsCount;
    private int _totalPages = 0;
    private int _currentPage = 0;

    public PageNavigation(int itemsPerPage, Action<int, int> pageChanged)
    {
        _itemsPerPage = itemsPerPage;
        _pageChanged = pageChanged;
    }

    public int CurrentPage => _currentPage;

    public void Initialize(Func<int> getTotalItemsCount) =>
        _getTotalItemsCount = getTotalItemsCount;

    public void IncreasePage()
    {
        bool canIncrease = _currentPage + 1 < CalculateTotalPages();

        if (canIncrease == false)
            return;

        _currentPage++;
        NotifyPageChanged();
    }

    public void DecreasePage()
    {
        bool canDecrease = _currentPage - 1 >= 0;

        if (canDecrease == false)
            return;

        _currentPage--;
        NotifyPageChanged();
    }

    public void ResetToFirstPage()
    {
        _currentPage = 0;
        NotifyPageChanged();
    }

    public int CalculateTotalPages()
    {
        int itemsCount = _getTotalItemsCount?.Invoke() ?? 0;

        if (itemsCount == 0)
            return 1;

        int totalPages = (int)Math.Ceiling((double)itemsCount / _itemsPerPage);
        
        if (totalPages != _totalPages)
        {
            _totalPages = totalPages;
            NotifyPageChanged();
        }

        return _totalPages;
    }

    public int GetCurrentStartIndex()
    {
        int itemsCount = _getTotalItemsCount?.Invoke() ?? 0;

        if (itemsCount == 0)
            return 0;

        int startIndex = _currentPage * _itemsPerPage;

        if (startIndex >= itemsCount)
        {
            _currentPage = Math.Max(0, CalculateTotalPages() - 1);
            startIndex = _currentPage * _itemsPerPage;
        }

        return startIndex;
    }

    public int GetCurrentTakeCount()
    {
        int itemsCount = _getTotalItemsCount?.Invoke() ?? 0;

        if (itemsCount == 0)
            return 0;

        int startIndex = GetCurrentStartIndex();
        int takeCount = Math.Min(_itemsPerPage, itemsCount - startIndex);

        return takeCount;
    }

    private void NotifyPageChanged() =>
        _pageChanged?.Invoke(_currentPage, CalculateTotalPages());
}