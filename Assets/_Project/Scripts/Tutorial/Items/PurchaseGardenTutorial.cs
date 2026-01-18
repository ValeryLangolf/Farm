using TMPro;
using UnityEngine;

public class PurchaseGardenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;
    [SerializeField] private TextMeshProUGUI _text;

    private Garden _garden;
    private UIDirector _uiDirector;

    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _garden = ServiceLocator.Get<GardensDirector>().Gardens[0];
        _text.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _garden.ReadOnlyData.PurchaseStatusChanged += OnPurchaseStatusChanged;
    }

    private void OnDisable()
    {
        _garden.ReadOnlyData.PurchaseStatusChanged -= OnPurchaseStatusChanged;
    }

    public override void Activate()
    {
        _uiDirector.HideUpgradeShopButton();
        _uiDirector.HideUpgradesModeButton();
        _cursor.SetWorldPosition(_garden.transform.position + _cusrsorOffset)
            .SetTouchAnimation()
            .Show();
        _text.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        _tutorial.SetCurrentItem(_nextItem);
        _cursor.Hide();
        _text.gameObject.SetActive(false);
        Destroy(gameObject);
        _nextItem.Activate();
    }

    private void OnPurchaseStatusChanged(bool isChanged)
    {
        if (isChanged)
        {
            Deactivate();
        }
    }
}