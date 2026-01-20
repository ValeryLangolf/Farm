using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialFinger _finger;

    private SavesData _data;
    private List<TutorialItem> _items;
    private List<TutorialItem> _remainingItems;
    private bool _isRunning;
    private bool _initilized;

    public void SetData(SavesData data)
    {
        if (_initilized == false)
            Init();

        _data = data;
        _finger.ResetAll();
        ClearRemainingItems();
        _remainingItems = new(_items);

        int currentItem = data.TutorialCounter;

        while (currentItem > 0)
        {
            currentItem--;
            _remainingItems.Remove(_remainingItems.First());
        }

        if (_isRunning)
            Next();
    }

    public void Run()
    {
        _isRunning = true;

        if (_data != null)
            Next();
    }

    private void Init()
    {
        _items = GetComponentsInChildren<TutorialItem>(true).ToList();

        foreach (TutorialItem item in _items)
            item.Hide();

        _finger.Init();
        _initilized = true;
    }

    private void Next()
    {
        if (_remainingItems.Count > 0)
        {
            TutorialItem item = _remainingItems.First();
            item.Deactivated += OnItemDeactivated;
            item.SetActive(true);
            item.Activate();
        }
    }

    private void ClearRemainingItems()
    {
        if (_remainingItems == null || _remainingItems.Count == 0)
            return;

        List<TutorialItem> items = new(_remainingItems);

        foreach (TutorialItem item in items)
        {
            if (item.IsActive)
            {
                item.Deactivated -= OnItemDeactivated;
                item.Deactivate();
                item.Hide();
            }
        }

        _remainingItems.Clear();
    }

    private void OnItemDeactivated(TutorialItem item)
    {
        item.Deactivated -= OnItemDeactivated;
        item.Hide();
        _remainingItems.Remove(item);
        _data.TutorialCounter++;
        Next();
    }
}