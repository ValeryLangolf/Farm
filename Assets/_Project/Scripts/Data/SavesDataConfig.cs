using UnityEngine;

[CreateAssetMenu(fileName = "NewSavesData", menuName = "ScriptableObject/NewSavesData", order = 0)]
public class SavesDataConfig : ScriptableObject
{
    [SerializeField] private SavesData _initialSavesData;

    public SavesData SavesData => _initialSavesData == null ? new () : new (_initialSavesData);
}