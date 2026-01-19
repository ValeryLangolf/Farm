using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PagedContainer : MonoBehaviour
{
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private int _itemsPerPage = 3;

    private ItemsCollection _itemsCollection;
    private PageNavigation _pageNavigation;
    private PagesRenderer _pagesRenderer;

    public event Action<int, int> PageChanged;

    public int TotalPages =>
        _pageNavigation?.CalculateTotalPages() ?? 1;

    public int CurrentPage =>
        _pageNavigation?.CurrentPage ?? 1;

    public ItemsCollection ItemsCollection => _itemsCollection;

    public IPagedItem FirstPagedItem => _itemsCollection.FirstPagedItem;

    public void Initialize<T>(T itemPrefab, int count) where T : MonoBehaviour, IPagedItem
    {
        InitializeComponents();
        _itemsCollection.CreateItems(itemPrefab, count);
        _pagesRenderer.UpdateView();
    }

    private void InitializeComponents()
    {
        _itemsCollection = new(_itemsContainer);
        _pageNavigation = new(_itemsPerPage, OnPageNavigationPageChanged);
        _pagesRenderer = new(_itemsCollection, _pageNavigation);

        _pageNavigation.Initialize(() => _itemsCollection.ItemsWithDataCount);
    }

    public void SetData(IEnumerable<object> data)
    {
        _itemsCollection.SetData(data.ToList());
        _pagesRenderer.UpdateView();
    }

    public void ResetToFirstPage() =>
        _pageNavigation.ResetToFirstPage();

    public void IncreasePage() =>
        _pageNavigation.IncreasePage();

    public void DecreasePage() =>
        _pageNavigation.DecreasePage();

    private void OnPageNavigationPageChanged(int currentPage, int totalPages)
    {
        _pagesRenderer.UpdateView();
        PageChanged?.Invoke(currentPage, totalPages);
    }
}