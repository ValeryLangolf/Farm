using System.Collections.Generic;

public interface IGardensDirector
{
    public IReadOnlyList<Garden> Gardens { get; }

    public List<SavedGardenData> GetGardensData();

    public void SetData(List<SavedGardenData> datas);
}