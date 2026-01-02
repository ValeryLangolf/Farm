using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsCollection
{
    private readonly Transform _itemsContainer;
    private readonly List<IPagedItem> _items = new();
    private IReadOnlyList<object> _currentData;

    public ItemsCollection(Transform itemsContainer)
    {
        _itemsContainer = itemsContainer != null
            ? itemsContainer
            : throw new ArgumentNullException(nameof(itemsContainer));
    }

    public IReadOnlyList<IPagedItem> ItemsWithData =>
        _items.Where(item => item.HasData).ToList();

    public int ItemsWithDataCount =>
        _items.Count(item => item.HasData);

    public void CreateItems<T>(T itemPrefab, int count) where T : MonoBehaviour, IPagedItem
    {
        ClearItems();

        for (int index = 0; index < count; index++)
        {
            T item = UnityEngine.Object.Instantiate(itemPrefab, _itemsContainer);
            item.gameObject.SetActive(false);
            _items.Add(item);
        }
    }

    public void SetData(IEnumerable<object> data)
    {
        _currentData = data.ToList();
        UpdateItemsData();
    }

    public void UpdateItemsData()
    {
        for (int index = 0; index < _items.Count; index++)
        {
            IPagedItem item = _items[index];

            if (index < _currentData.Count)
            {
                object itemData = _currentData[index];
                item.SetData(itemData);

                continue;
            }

            item.ClearData();
        }
    }

    public void EnableItemsInRange(int startIndex, int count)
    {
        DisableAllItems();
        IReadOnlyList<IPagedItem> itemsWithData = ItemsWithData;

        if (itemsWithData.Count == 0)
            return;

        for (int index = startIndex; index < startIndex + count && index < itemsWithData.Count; index++)
        {
            IPagedItem item = itemsWithData[index];
            item.GameObject.SetActive(true);
            int pageIndex = index - startIndex;
            item.GameObject.transform.SetSiblingIndex(pageIndex);
        }
    }

    public IReadOnlyList<IPagedItem> GetItemsInPageRange(int startIndex, int count)
    {
        IReadOnlyList<IPagedItem> itemsWithData = ItemsWithData;

        if (itemsWithData.Count == 0)
            return new List<IPagedItem>();

        List<IPagedItem> result = new();

        for (int index = startIndex; index < startIndex + count && index < itemsWithData.Count; index++)
            result.Add(itemsWithData[index]);

        return result;
    }

    private void DisableAllItems()
    {
        foreach (IPagedItem item in _items)
            item.GameObject.SetActive(false);
    }

    private void ClearItems()
    {
        foreach (Transform child in _itemsContainer)
            UnityEngine.Object.Destroy(child.gameObject);

        _items.Clear();
    }
}