using System.Collections.Generic;

public class GardensDirector : IService
{
    private readonly List<Garden> _gardens;

    public GardensDirector(List<Garden> gardens)
    {
        _gardens = gardens;
    }

    public IReadOnlyList<Garden> Gardens => _gardens;
}