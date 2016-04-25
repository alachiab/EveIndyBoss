using System.Windows;
using EveIndyBoss.ViewModels;
using EveIndyBoss.Views;
using ReactiveUI;
using Splat;

namespace EveIndyBoss
{
    public partial class App : Application
    {
        public static AppBootstrapper Bootstrapper;
        public static ShellView ShellView;

        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShellView = (ShellView) Locator.Current.GetService<IViewFor<IShellViewModel>>();
            ShellView.Show();
        }
    }
}