using UnityEngine;

public interface IPagedItem
{
    GameObject GameObject { get; }

    bool HasData { get; }

    void SetData(object data);

    void ClearData();
}