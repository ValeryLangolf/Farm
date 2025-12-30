using UnityEngine;

public class TrailParticleFactory : IFactory<TrailParticle>
{
    private readonly TrailParticle _prefab;

    public TrailParticleFactory(TrailParticle prefab)
    {
        _prefab = prefab;
    }

    public TrailParticle Create()
    {
        return Object.Instantiate(_prefab);
    }
}