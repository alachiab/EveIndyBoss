using System;
using System.Data;
using System.Data.SQLite;
using eZet.EveLib.EveCentralModule;
using eZet.EveLib.EveMarketDataModule;
using EveIndyBoss.Converters;
using EveIndyBoss.Services;
using EveIndyBoss.Services.StaticData;
using EveIndyBoss.ViewModels;
using EveIndyBoss.Views;
using Ninject.Modules;
using ReactiveUI;

namespace EveIndyBoss.Infrastructure
{
    public class EveIndyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IShellViewModel>().To<ShellViewModel>().InSingletonScope();
            Bind<IMenuViewModel>().To<MenuViewModel>().InSingletonScope();
            Bind<IPlannerViewModel>().To<PlannerViewModel>().InSingletonScope();

            Bind<IViewFor<IShellViewModel>>().To<ShellView>().InSingletonScope();
            Bind<IViewFor<IMenuViewModel>>().To<MenuView>().InSingletonScope();
            Bind<IViewFor<IPlannerViewModel>>().To<PlannerView>().InSingletonScope();

            Bind<ICacheThings>().To<InMemoryCache>().InSingletonScope();
            Bind<IFetchMarketData>().To<EveCentralPriceService>();
            Bind<IHavePrices>().To<PriceService>();
            Bind<IProvideStaticData>().To<StaticDataService>().InSingletonScope();
            Bind<ICalculateMaterials>().To<MaterialCalculator>();

            Bind<IBindingTypeConverter>().To<IskConverter>();

            Bind<Func<IDbConnection>>()
                .ToMethod(context => 
                    () => new SQLiteConnection($"Data Source={Environment.CurrentDirectory}\\staticdata.db")
                );

            Bind<EveCentral>().ToSelf();
            Bind<EveMarketData>().ToSelf();
        }
    }
}