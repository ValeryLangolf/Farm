using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private PagedContainer _pagedContainer;
    [SerializeField] private ShopItem _shopItemPrefab;

    [SerializeField][TextArea] private string _stackUpgradeDescription = "Stack Harvest";
    [SerializeField][TextArea] private string _profitUpgradeDescription = "Profit x3";

    private IReadOnlyList<Garden> _gardens;
    private IWallet _wallet;
    private float _cheapestUpgrade = float.MaxValue;

    public float CheapestUpgrade => _cheapestUpgrade;

    private void Awake()
    {
        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;
        _wallet = ServiceLocator.Get<IWallet>();

        InitializePagedContainer();
    }

    private void OnEnable()
    {
        SubscribeToGardenEvents();
        _wallet.Changed += OnWalletChanged;

        ResetPagination();
        UpdateShopItems();
    }

    private void OnDisable()
    {
        UnsubscribeFromGardenEvents();
        _wallet.Changed -= OnWalletChanged;
    }

    private void InitializePagedContainer() =>
        _pagedContainer.Initialize(_shopItemPrefab, _gardens.Count);

    private void SubscribeToGardenEvents()
    {
        foreach (Garden garden in _gardens)
        {
            garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
            garden.ReadOnlyData.CostStoreLevelUpgradeChanged += CostStoreLevelUpgradeChanged;
        }
    }

    private void UnsubscribeFromGardenEvents()
    {
        foreach (Garden garden in _gardens)
        {
            garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
            garden.ReadOnlyData.CostStoreLevelUpgradeChanged -= CostStoreLevelUpgradeChanged;
        }
    }

    private void ResetPagination() =>
        _pagedContainer.ResetToFirstPage();

    private void UpdateShopItems()
    {
        List<object> upgradeDataList = new();

        List<Garden> purchasedGardens = _gardens
            .Where(garden => garden.ReadOnlyData.IsPurchased)
            .OrderBy(garden => garden.ReadOnlyData.CostStoreLevelUpgrade)
            .ToList();

        _cheapestUpgrade = purchasedGardens.Count > 0 ? purchasedGardens[0].ReadOnlyData.CostStoreLevelUpgrade : float.MaxValue;

        foreach (Garden garden in purchasedGardens)
        {
            string description = garden.ReadOnlyData.StoreLevelUpgrade == 0
                ? _stackUpgradeDescription
                : _profitUpgradeDescription;

            bool canPurchase = _wallet.CanSpend(garden.ReadOnlyData.CostStoreLevelUpgrade);

            ShopItemData data = new(
                gardenName: garden.ReadOnlyData.GardenName,
                icon: garden.ReadOnlyData.Icon,
                price: garden.ReadOnlyData.CostStoreLevelUpgrade,
                profitLevel: garden.ReadOnlyData.StoreLevelUpgrade,
                upgradeRequested: () => garden.UpgradeStoreLevel(),
                description: description,
                canPurchase: canPurchase,
                onButtonClick: () => UpdateShopItems()
            );

            upgradeDataList.Add(data);
        }

        _pagedContainer.SetData(upgradeDataList);
    }

    private void OnPurchaseStatusChanged(bool _) =>
        UpdateShopItems();

    private void CostStoreLevelUpgradeChanged(float _) =>
        UpdateShopItems();

    private void OnWalletChanged(float _) =>
        UpdateShopItems();
}