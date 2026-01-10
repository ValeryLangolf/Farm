using System.Collections;
using UnityEngine;

public class UpgradeModeOpenTutorial : TutorialItem
{
    [SerializeField] private TutorialItem _nextItem;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private OpenerUpgradePanelButton _button;
    [SerializeField] private TutorialCursor _cursor;
    [SerializeField] private Vector3 _cusrsorOffset;

    private UIDirector _uiDirector;
    private IWallet _wallet;
    private bool _isActivated = false;

    private void Awake()
    {
        _uiDirector = ServiceLocator.Get<UIDirector>();
        _wallet = ServiceLocator.Get<IWallet>();
        _button = _uiDirector.OpenerUpgradePanelButton;
    }

    private void OnEnable()
    {
        _button.Clicked += OnUpgradePanelOpened;
    }

    private void OnDisable()
    {
        _button.Clicked -= OnUpgradePanelOpened;
    }

    private void Update()
    {
        if(_isActivated == false && _wallet.Amount > 15)
        {
            _isActivated = true;
            Activate();
        }
    }

    public override void Activate()
    {
        
        if(_button.TryGetComponent<RectTransform>(out var rectTransform) == false)
        {
            return;
        }

        _uiDirector.ShowUpgradeModeButton();

        float x = rectTransform.position.x - rectTransform.rect.width / 2; //Как-то надо рассчитать центр кнопки
        Vector3 newPosition = new(x, rectTransform.position.y);

        _cursor.SetScreenPosition(newPosition + _cusrsorOffset)
            .SetTouchAnimation()
            .Show();
    }

    public override void Deactivate()
    {
        _cursor.Hide();
        Destroy(gameObject);
       // _nextItem.Activate();
    }

    private void OnUpgradePanelOpened(ButtonClickHandler _)
    {
        Deactivate();
    }
    
}
