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

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment => "Menu";
    }
}