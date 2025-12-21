using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private List<Button> _openButtons;
    [SerializeField] private List<Button> _closeButtons;
    [SerializeField] private List<Button> _toggleButtons;

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public virtual void Init()
    {
        Subscribe();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ChangeVisibility()
    {
        if (gameObject.activeSelf)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Subscribe()
    {
        if(_openButtons.Count > 0)
        {
            foreach(Button button in _openButtons)
            {
                button.onClick.AddListener(Open);
            }
        }

        if (_closeButtons.Count > 0)
        {
            foreach (Button button in _closeButtons)
            {
                button.onClick.AddListener(Close);
            }
        }

        if (_toggleButtons.Count > 0)
        {
            foreach (Button button in _toggleButtons)
            {
                button.onClick.AddListener(ChangeVisibility);
            }
        }
    }

    private void UnSubscribe()
    {
        if (_openButtons.Count > 0)
        {
            foreach (Button button in _openButtons)
            {
                button.onClick.RemoveAllListeners(); ;
            }
        }

        if (_closeButtons.Count > 0)
        {
            foreach (Button button in _closeButtons)
            {
                button.onClick.RemoveAllListeners(); ;
            }
        }

        if (_toggleButtons.Count > 0)
        {
            foreach (Button button in _toggleButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}
