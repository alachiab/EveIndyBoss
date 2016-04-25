using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public interface IMenuViewModel : IRoutableViewModel
    {
    }

    public class MenuViewModel : ReactiveObject, IMenuViewModel
    {
        public MenuViewModel()
        {
        }

        public string UrlPathSegment => "Menu";
        public IScreen HostScreen { get; }
    }
}