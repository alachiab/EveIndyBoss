using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public interface IShellViewModel : IRoutableViewModel
    {
        IMenuViewModel MenuVm { get; set; }
    }

    public class ShellViewModel : ReactiveObject, IShellViewModel
    {
        public ShellViewModel(IScreen screen, IMenuViewModel menuVm, IPlannerViewModel plannerVm)
        {
            HostScreen = screen;
            MenuVm = menuVm;
            PlannerVm = plannerVm;
            HostScreen.Router.Navigate.Execute(PlannerVm);
        }

        public IPlannerViewModel PlannerVm { get; set; }
        public string UrlPathSegment => "Shell";
        public IScreen HostScreen { get; protected set; }
        public IMenuViewModel MenuVm { get; set; }
    }
}