using UnityEngine;

public class SwipeGardenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private Garden _garden;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;

    private UIDirector _uiDirector;
    private IWallet _wallet;

    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _wallet = ServiceLocator.Get<IWallet>();
    }

    private void OnEnable()
    {
        _wallet.Changed += OnWalletChanged;
    }

    private void OnDisable()
    {
        _garden.ReadOnlyData.StorageFullnessChanged -= OnWalletChanged;
    }

    public override void Activate()
    {
        _uiDirector.HideUpgradesButtons();
        _cursor.SetWorldPosition(_garden.transform.position + _cusrsorOffset)
            .SetSwipeAnimation()
            .Show();
    }

    public override void Deactivate()
    {
        _tutorial.SetCurrentItem(_nextItem);
        _cursor.Hide();
        _nextItem.Activate();
        Destroy(gameObject);
    }

    private void OnWalletChanged(float obj)
    {
        float targetCount = 3f;

        if (obj > targetCount)
        {
            Deactivate();
        }
    }
}
