using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private UpgradeShopItemUI _upgradeShopItem;
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _prevPageButton;
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField][TextArea] private string _stackUpgradeDescription = "Stack Harvest";
    [SerializeField][TextArea] private string _profitUpgradeDescription = "Profit x3";

    private int _currentPage = 0;
    private int _maxPageCount = 3;
    private int _itemsPerPage = 3;

    private List<UpgradeShopItemUI> _shopItems;
    private IReadOnlyList<Garden> _gardens;

    //Œ¡ÕŒ¬Àﬂ“‹ »Õ‘Œ ¬ Ã¿√¿«»Õ≈

    private void Awake()
    {
        ClearChilds();

        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;
        _shopItems = new();
        _nextPageButton.onClick.AddListener(IncreasePage);
        _prevPageButton.onClick.AddListener(DecreasePage);

       foreach (Garden garden in _gardens)
        {
            if (garden.ReadOnlyData.IsPurchased)
            {
                AddShopItem(garden);
            }
        }
    }

    private void OnEnable()
    {
        _currentPage = 0;
    }

    private void OnDestroy()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _prevPageButton.onClick.RemoveAllListeners();
    }

    public void AddShopItem(Garden garden)
    {
        UpgradeShopItemUI item = Instantiate(_upgradeShopItem, _parentObject);

        string description = garden.ReadOnlyData.ProfitLevel == 0 ? _stackUpgradeDescription : _profitUpgradeDescription; 
        item.Init(garden, description);
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
       // _shopItems = _shopItems.OrderBy(item => item.UpgradeInfo.Price).ToList();
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

    private void ClearChilds()
    {
        foreach(Transform child in _parentObject)
        {
            Destroy(child.gameObject);
        }
    }
}
