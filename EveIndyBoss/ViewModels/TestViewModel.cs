using EveIndyBoss.Services;
using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public interface ITestViewModel : IRoutableViewModel
    {
        IScreen HostScreen { get; }
    }

    public class TestViewModel : ReactiveObject, ITestViewModel
    {
        private readonly IProvideStaticData _staticData;

        public TestViewModel(IScreen screen, IProvideStaticData staticData)
        {
            HostScreen = screen;
            _staticData = staticData;
            var a = _staticData.Test().Result;
        }

        public string UrlPathSegment => "Test";
        public IScreen HostScreen { get; protected set; }
    }
}