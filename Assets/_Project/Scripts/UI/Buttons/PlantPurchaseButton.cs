using TMPro;
using UnityEngine;

public class PlantPurchaseButton : ButtonClickHandler
{
    [SerializeField] private Garden _garden;
    [SerializeField] private TextMeshProUGUI _priceText;

    private IWallet _wallet;
    private IReadOnlyGardenData _data;
    private float _lastPrice;

    protected override void Awake()
    {
        base.Awake();

        _wallet = ServiceLocator.Get<IWallet>();
        _data = _garden.ReadOnlyData;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnPlantsPriceToUpgradeChanged(_data.PlantsPriceToUpgrade);
        _data.PlantsPriceToUpgradeChanged += OnPlantsPriceToUpgradeChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _data.PlantsPriceToUpgradeChanged -= OnPlantsPriceToUpgradeChanged;
    }

    protected override void OnClick()
    {
        base.OnClick();

        _garden.UpgradePlantsCount();
    }

    private void OnPlantsPriceToUpgradeChanged(float price)
    {
        if (MoneyFormatter.NeedUpdateText(out string formattedText, _lastPrice, price))
        {
            _lastPrice = price;
            _priceText.text = $"{formattedText}{Constants.DollarChar}";
        }

        bool canBuy = _wallet.CanSpend(_data.PlantsPriceToUpgrade);
        SetInteractable(canBuy);
    }
}