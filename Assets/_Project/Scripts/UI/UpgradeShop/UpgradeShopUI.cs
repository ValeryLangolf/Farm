using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopUI : UIPanel
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private UpgradeShopItemUI _upgradeShopItem;
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _prevPageButton;
    [SerializeField] private TextMeshProUGUI _pageText;

    private int _currentPage = 0;
    private int _maxPageCount = 3;
    private int _itemsPerPage = 3;

    private List<UpgradeShopItemUI> _shopItems;

    private void OnEnable()
    {
        _currentPage = 0;
    }

    private void OnDestroy()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _prevPageButton.onClick.RemoveAllListeners();
    }

    public override void Init()
    {
        base.Init();

        _shopItems = new();
        _nextPageButton.onClick.AddListener(IncreasePage);
        _prevPageButton.onClick.AddListener(DecreasePage);
    }

    public void AddShopItem(UpgradeInfo info)
    {
        UpgradeShopItemUI item = Instantiate(_upgradeShopItem, _parentObject);
        item.Init(info);
        _shopItems.Add(item);
        UpdateVisualization();
    }

    private void UpdateVisualization()
    {
        DisableAllItems();
        EnableItemsInRange();
        ChangeButtonsVisibiity();
        SetPageText();
    }

    public void IncreasePage()
    {
        if (_currentPage + 1 < CalculateAviablePagesCount())
        {
            _currentPage++;
            UpdateVisualization();
        }
    }

    private int CalculateAviablePagesCount()
    {
        return Mathf.CeilToInt((float)_shopItems.Count / _itemsPerPage);
    }

    public void DecreasePage()
    {
        if (_currentPage - 1 >= 0)
        {
            _currentPage--;
            UpdateVisualization();
        }
    }

    private void SetPageText()
    {
        string text = (_currentPage + 1) + "/" + CalculateAviablePagesCount();
        _pageText.text = text;
    }

    private void SortByPrice()
    {
        _shopItems = _shopItems.OrderBy(item => item.UpgradeInfo.Price).ToList();
    }

    private void DisableAllItems()
    {
        foreach (var item in _shopItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void ChangeButtonsVisibiity()
    {
        if(_currentPage == 0)
        {
            _prevPageButton.gameObject.SetActive(false);
            _nextPageButton.gameObject.SetActive(true);
        }
        else if (_currentPage == CalculateAviablePagesCount() - 1)
        {
            _nextPageButton.gameObject.SetActive(false);
            _prevPageButton.gameObject.SetActive(true);
        }
        else
        {
            _prevPageButton.gameObject.SetActive(true);
            _nextPageButton.gameObject.SetActive(true);
        }
    }

    private void EnableItemsInRange()
    {
        int startIndex = _currentPage * _maxPageCount;
        int takeCount = Math.Min(_itemsPerPage, _shopItems.Count - startIndex);
        int lastIndex = startIndex + takeCount;

        SortByPrice();

        for (int i = startIndex; i < lastIndex; i++)
        {
            _shopItems[i].gameObject.SetActive(true);
            _shopItems[i].transform.SetSiblingIndex(lastIndex - i);
        }
    }
}
