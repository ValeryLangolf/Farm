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

    private readonly List<UpgradeShopItemUI> _shopItems = new();
    private IReadOnlyList<Garden> _gardens;

    private void Awake()
    {
        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;

        ClearChildren();
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
        {
            garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
            garden.ReadOnlyData.ProfitLevelChanged += OnProfitLevelChanged;
        }
    }

    private void OnDisable()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _prevPageButton.onClick.RemoveAllListeners();

        foreach (Garden garden in _gardens)
        {
            garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
            garden.ReadOnlyData.ProfitLevelChanged -= OnProfitLevelChanged;
        }
    }

    private void ClearChildren()
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
        foreach (UpgradeShopItemUI shopItem in _shopItems)
            shopItem.ClearData();

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
        int itemsWithDataCount = _shopItems.Count(item => item.HasData);

        if (itemsWithDataCount == 0)
            return 1;

        return Mathf.CeilToInt((float)itemsWithDataCount / ITEMS_PER_PAGE);
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

        if (availablePagesCount <= 1)
        {
            _prevPageButton.gameObject.SetActive(false);
            _nextPageButton.gameObject.SetActive(false);
        }
        else if (_currentPage == 0)
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
        DisableAllItems();

        List<UpgradeShopItemUI> itemsWithData = _shopItems
            .Where(item => item.HasData)
            .ToList();

        if (itemsWithData.Count == 0)
            return;

        int startIndex = _currentPage * ITEMS_PER_PAGE;
        int takeCount = Math.Min(ITEMS_PER_PAGE, itemsWithData.Count - startIndex);

        if (startIndex >= itemsWithData.Count)
        {
            _currentPage = Mathf.Max(0, CalculateAvailablePagesCount() - 1);
            startIndex = _currentPage * ITEMS_PER_PAGE;
            takeCount = Math.Min(ITEMS_PER_PAGE, itemsWithData.Count - startIndex);
        }

        for (int index = startIndex; index < startIndex + takeCount; index++)
        {
            itemsWithData[index].gameObject.SetActive(true);
            itemsWithData[index].transform.SetSiblingIndex(index - startIndex);
        }
    }

    private void DisableAllItems()
    {
        foreach (UpgradeShopItemUI item in _shopItems)
            item.gameObject.SetActive(false);
    }

    private void OnPurchaseStatusChanged(bool _) =>
        UpdateShopItems();

    private void OnProfitLevelChanged(float _) =>
        UpdateShopItems();
}