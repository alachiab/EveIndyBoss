using eZet.EveLib.EveCentralModule;
using EveIndyBoss.Services;
using EveIndyBoss.ViewModels;
using EveIndyBoss.Views;
using ReactiveUI;
using Splat;

namespace EveIndyBoss
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        // https://github.com/kondaskondas/FirstsStepsRUI
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            RegisterParts(dependencyResolver);

            LogHost.Default.Level = LogLevel.Debug;
        }

        public RoutingState Router { get; }

        private void RegisterParts(IMutableDependencyResolver resolver)
        {
            resolver.RegisterLazySingleton(() => this, typeof (IScreen));
            resolver.RegisterLazySingleton(() =>
                new ShellViewModel(Locator.Current.GetService<IScreen>()),
                typeof (IShellViewModel));
            resolver.RegisterLazySingleton(() =>
                new ShellView(Locator.Current.GetService<IShellViewModel>()),
                typeof (IViewFor<IShellViewModel>));

            resolver.RegisterLazySingleton(() => new InMemoryCache(), typeof (ICacheThings));
            resolver.RegisterLazySingleton(() => new EveCentral(), typeof(EveCentral));
            //dependencyResolver.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            //dependencyResolver.RegisterConstant(new EddnService(), typeof(IEddnService));

            //dependencyResolver.Register(() => new MainListViewModel(), typeof(IMainListViewModel));
            //dependencyResolver.Register(() => new SaveService(), typeof(ISaveService));
            //dependencyResolver.Register(() => new CalculateLootService(), typeof(ICalculateLoot));
            //dependencyResolver.Register(() => new EdscService(), typeof(IQueryEdsc));
        }
    }
}