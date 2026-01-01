using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    private const int ITEMS_PER_PAGE = 3;

    [SerializeField] private Transform _parentObject;
    [SerializeField] private UpgradeShopItemUI _upgradeShopItem;
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _prevPageButton;
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField][TextArea] private string _stackUpgradeDescription = "Stack Harvest";
    [SerializeField][TextArea] private string _profitUpgradeDescription = "Profit x3";

    private int _currentPage = 0;
    private readonly int _itemsPerPage = 3;

    private readonly List<UpgradeShopItemUI> _shopItems = new();
    private IReadOnlyList<Garden> _gardens;

    private void Awake()
    {
        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;

        ClearChilds();
        CreateAllItems();
        UpdateShopItems();
        UpdateInfo();
    }

    private void OnEnable()
    {
        _currentPage = 0;

        _nextPageButton.onClick.AddListener(IncreasePage);
        _prevPageButton.onClick.AddListener(DecreasePage);

        OnPurchaseStatusChanged(true);

        foreach (Garden garden in _gardens)
            garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _prevPageButton.onClick.RemoveAllListeners();

        foreach (Garden garden in _gardens)
            garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    private void ClearChilds()
    {
        foreach (Transform child in _parentObject)
            Destroy(child.gameObject);
    }

    private void CreateAllItems()
    {
        foreach (Garden _ in _gardens)
        {
            UpgradeShopItemUI item = Instantiate(_upgradeShopItem, _parentObject);
            item.gameObject.SetActive(false);
            _shopItems.Add(item);
        }
    }

    private void UpdateShopItems()
    {
        IReadOnlyList<Garden> sortedGardens = SortGardens();
        FillItems(sortedGardens);
    }

    private IReadOnlyList<Garden> SortGardens()
    {
        return _gardens
            .Where(garden => garden.ReadOnlyData.IsPurchased)
            .OrderBy(garden => garden.ReadOnlyData.LevelUpPrice)
            .ToList();
    }

    private void FillItems(IReadOnlyList<Garden> sortedGardens)
    {
        for (int index = 0; index < _shopItems.Count; index++)
        {
            bool hasCorrespondingGarden = index < sortedGardens.Count;

            if (hasCorrespondingGarden == false)
            {
                _shopItems[index].gameObject.SetActive(false);

                continue;
            }

            Garden garden = sortedGardens[index];
            string description = garden.ReadOnlyData.ProfitLevel == 0
                ? _stackUpgradeDescription
                : _profitUpgradeDescription;

            _shopItems[index].UpdateInfo(garden, description);
        }

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        EnableItemsInRange();
        ChangeButtonsVisibility();
        SetPageText();
    }

    public void IncreasePage()
    {
        bool canIncrease = _currentPage + 1 < CalculateAvailablePagesCount();

        if (canIncrease == false)
            return;

        _currentPage++;
        UpdateInfo();
    }

    private int CalculateAvailablePagesCount()
    {
        int activeItemsCount = _shopItems.Count(item => item.gameObject.activeSelf);
        
        return Mathf.CeilToInt((float)activeItemsCount / _itemsPerPage);
    }

    public void DecreasePage()
    {
        bool canDecrease = _currentPage - 1 >= 0;

        if (canDecrease == false)
            return;

        _currentPage--;
        UpdateInfo();
    }

    private void SetPageText()
    {
        string text = (_currentPage + 1) + "/" + CalculateAvailablePagesCount();
        _pageText.text = text;
    }

    private void ChangeButtonsVisibility()
    {
        int availablePagesCount = CalculateAvailablePagesCount();

        if (_currentPage == 0)
        {
            _prevPageButton.gameObject.SetActive(false);
            _nextPageButton.gameObject.SetActive(true);
        }
        else if (_currentPage == availablePagesCount - 1)
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
        //DisableAllItems();

        //List<UpgradeShopItemUI> activeItems = _shopItems
        //    .Where(item => item.Price)
        //    .ToList();

        //if (activeItems.Count == 0)
        //    return;

        //int startIndex = _currentPage * ITEMS_PER_PAGE;
        //int takeCount = Math.Min(ITEMS_PER_PAGE, activeItems.Count - startIndex);

        //for (int index = startIndex; index < startIndex + takeCount; index++)
        //{
        //    activeItems[index].gameObject.SetActive(true);
        //    activeItems[index].transform.SetSiblingIndex(index - startIndex);
        //}
    }

    private void DisableAllItems()
    {
        foreach (UpgradeShopItemUI item in _shopItems)
            item.gameObject.SetActive(false);
    }

    private void OnPurchaseStatusChanged(bool purchased)
    {
        Debug.Log(_shopItems.Count);

        UpdateShopItems();
    }
}