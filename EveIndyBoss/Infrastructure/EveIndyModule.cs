using eZet.EveLib.EveCentralModule;
using EveIndyBoss.Services;
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
            Bind<ITestViewModel>().To<TestViewModel>().InSingletonScope();

            Bind<IViewFor<IShellViewModel>>().To<ShellView>().InSingletonScope();
            Bind<IViewFor<IMenuViewModel>>().To<MenuView>().InSingletonScope();
            Bind<IViewFor<ITestViewModel>>().To<TestView>().InSingletonScope();

            Bind<ICacheThings>().To<InMemoryCache>().InSingletonScope();
            Bind<IFetchMarketData>().To<EveCentralPriceService>();
            Bind<IHavePrices>().To<PriceService>();

            Bind<EveCentral>().ToSelf();
        }
    }
}