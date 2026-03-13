using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSavesData", menuName = "ScriptableObject/NewSavesData", order = 0)]
public class SavesDataConfig : ScriptableObject
{
    [SerializeField] private SavesData _initialSavesData;

    public SavesData GetSavesData()
    {
        if( _initialSavesData == null )
            throw new NullReferenceException(nameof(_initialSavesData));

        return new(_initialSavesData);
    }
}