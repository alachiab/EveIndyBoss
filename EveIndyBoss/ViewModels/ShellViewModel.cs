using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel, IShellViewModel
    {
        public ShellViewModel(IScreen screen)
        {
            HostScreen = screen;
            //HostScreen.Router.Navigate.Execute(LoginViewModel);
        }

        public string UrlPathSegment => "Shell";

        public IScreen HostScreen { get; protected set; }
    }

    public interface IShellViewModel
    {
        IScreen HostScreen { get; }
    }
}