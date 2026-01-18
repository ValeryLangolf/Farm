using TMPro;
using UnityEngine;

public class PlantPurchaseButton : ButtonClickHandler
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Transform _center;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;
    private float _lastPrice;

    public Transform Center => _center;

    protected override void Awake()
    {
        base.Awake();

        _wallet = ServiceLocator.Get<IWallet>();
        _data = _garden.ReadOnlyData;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnWalletChanged();
        _wallet.Changed += (_) => OnWalletChanged();

        OnPlantsPriceToUpgradeChanged();
        _data.PlantsPriceToUpgradeChanged += (_) => OnPlantsPriceToUpgradeChanged();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _wallet.Changed -= (_) => OnWalletChanged();
        _data.PlantsPriceToUpgradeChanged -= (_) => OnPlantsPriceToUpgradeChanged();
    }

    protected override void OnClick()
    {
        base.OnClick();

        _garden.UpgradePlantsCount();
    }

    private void ProcessChanged()
    {
        if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastPrice, _data.PlantsPriceToUpgrade))
        {
            _lastPrice = _data.PlantsPriceToUpgrade;
            _priceText.text = $"{formattedText}{Constants.DollarChar}";
        }

        bool canBuy = _wallet.CanSpend(_data.PlantsPriceToUpgrade);
        SetInteractable(canBuy);
    }

    private void OnWalletChanged() =>
        ProcessChanged();

    private void OnPlantsPriceToUpgradeChanged() =>
        ProcessChanged();
}