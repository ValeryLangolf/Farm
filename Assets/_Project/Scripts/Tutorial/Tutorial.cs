using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialFinger _finger;

    private List<TutorialItem> _items;
    private List<TutorialItem> _remainingItems;
    private bool _isRunning;
    private bool _initilized;
    private int _counter = 0;

    public int Counter => _counter;

    public void SetCounter(int counter)
    {
        if (counter < 0)
            throw new ArgumentOutOfRangeException(nameof(counter), counter, "Значение должно быть положительным");

        if (_initilized == false)
            Init();

        _counter = 0;
        _finger.ResetAll();
        ClearRemainingItems();
        _remainingItems = new(_items);

        int currentItem = counter;

        while (currentItem > 0)
        {
            currentItem--;
            _counter++;
            _remainingItems.Remove(_remainingItems.First());
        }

        if (_isRunning)
            Next();
    }

    public void Run()
    {
        _isRunning = true;
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
        _counter++;
        _remainingItems.Remove(item);
        Next();
    }
}