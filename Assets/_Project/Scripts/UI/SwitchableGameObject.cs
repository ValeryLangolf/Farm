using UnityEngine;

public class SwitchableGameObject : MonoBehaviour
{
    [SerializeField] private GameObject _inActive;
    [SerializeField] private GameObject _active;

    public void UpdateState(bool isActive)
    {
        _inActive.SetActive(isActive == false);
        _active.SetActive(isActive);
    }
}