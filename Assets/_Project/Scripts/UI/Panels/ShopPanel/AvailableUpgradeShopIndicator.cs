using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class AvailableUpgradeShopIndicator : MonoBehaviour, IInjactable
{
    [SerializeField] private GameObject _arrow;

    private IWallet _wallet;
    private IGardensDirector _gardensDirector;
    private IReadOnlyList<Garden> _gardens;

    [Inject]
    public void Construct(IWallet wallet, IGardensDirector gardensDirector)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _gardensDirector = gardensDirector ?? throw new ArgumentNullException(nameof(gardensDirector));
    }

    private void Awake()
    {
        _gardens = _gardensDirector.Gardens;
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