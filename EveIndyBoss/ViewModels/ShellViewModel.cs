using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public interface IShellViewModel : IRoutableViewModel
    {
        IScreen HostScreen { get; }
        IMenuViewModel MenuVm { get; set; }
    }

    public class ShellViewModel : ReactiveObject, IShellViewModel
    {
        public ShellViewModel(IScreen screen, IMenuViewModel menuVm, ITestViewModel testVm)
        {
            HostScreen = screen;
            MenuVm = menuVm;
            TestVm = testVm;
            HostScreen.Router.Navigate.Execute(TestVm);
        }

        public ITestViewModel TestVm { get; set; }
        public string UrlPathSegment => "Shell";
        public IScreen HostScreen { get; protected set; }
        public IMenuViewModel MenuVm { get; set; }
    }
}