using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialItem _first;


    private TutorialItem _currentItem;

    private void Start()
    {
        _currentItem = _first;
        ActivateCurrentItem();
    }

    public void SetCurrentItem(TutorialItem item)
    {
        _currentItem = item;
    }

    private void ActivateCurrentItem()
    {
        _currentItem.Activate();
    }
}
