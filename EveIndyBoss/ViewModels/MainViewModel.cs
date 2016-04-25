using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        public MainViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public IScreen HostScreen { get; protected set; }
    }

    public interface IMainViewModel
    {
    }
}