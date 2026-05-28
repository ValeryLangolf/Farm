using UnityEngine;
using VContainer;
using VContainer.Unity;

public class FirstLocationLifetimeScope : LifetimeScope
{
    [SerializeField] private GardensDirector _gardensDirector;
    [SerializeField] private UIDirector _uiDirector;
    [SerializeField] private SavesDataConfig _savesDataConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<IGardensDirector>(_gardensDirector);
        builder.RegisterComponent(_uiDirector);

        builder.Register<IWallet, Wallet>(Lifetime.Scoped);
        builder.Register<ICoinCollector, CoinCollector>(Lifetime.Scoped);

        builder.RegisterInstance(_savesDataConfig.GetSavesData());
        builder.Register<ISaver<SavesData>, Saver<SavesData>>(Lifetime.Scoped);
    }
}