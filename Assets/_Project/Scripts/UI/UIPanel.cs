using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private List<Button> _openButtons;
    [SerializeField] private List<Button> _closeButtons;
    [SerializeField] private List<Button> _toggleButtons;

    private InputTrailParticle _inputTrailParticle;

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public virtual void Init()
    {
        Subscribe();
    }

    public void SetTrailParticle(InputTrailParticle inputTrailParticle)
    {
        _inputTrailParticle = inputTrailParticle;
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        _inputTrailParticle?.SetActive(false);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        _inputTrailParticle?.SetActive(true);
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
