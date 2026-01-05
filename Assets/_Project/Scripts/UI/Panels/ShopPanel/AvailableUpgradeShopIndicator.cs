using System.Collections.Generic;
using UnityEngine;

public class AvailableUpgradeShopIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;

    private IWallet _wallet;
    private IReadOnlyList<Garden> _gardens;

    private void Awake()
    {
        _wallet = ServiceLocator.Get<IWallet>();
        _gardens = ServiceLocator.Get<GardensDirector>().Gardens;
    }

    private void OnEnable()
    {
        ProcessChanges();

        _wallet.Changed += OnWalletChanged;

        foreach (Garden garden in _gardens)
        {
            garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
            garden.ReadOnlyData.CostStoreLevelUpgradeChanged += OnCostStoreLevelUpgradeChanged;
        }
    }

    private void OnDisable()
    {
        _wallet.Changed -= OnWalletChanged;

        foreach (Garden garden in _gardens)
        {
            garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
            garden.ReadOnlyData.CostStoreLevelUpgradeChanged -= OnCostStoreLevelUpgradeChanged;
        }
    }

    private void ProcessChanges()
    {
        bool isActive = false;

        foreach (Garden garden in _gardens)
        {
            if (garden.ReadOnlyData.IsPurchased)
            {
                if (_wallet.CanSpend(garden.ReadOnlyData.CostStoreLevelUpgrade))
                {
                    isActive = true;

                    break;
                }
            }                
        }            

        _arrow.SetActive(isActive);
    }

    private void OnPurchaseStatusChanged(bool _) =>
        ProcessChanges();

    private void OnCostStoreLevelUpgradeChanged(float _) =>
        ProcessChanges();

    private void OnWalletChanged(float _) =>
        ProcessChanges();
}