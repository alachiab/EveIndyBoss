using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace EveIndyBoss.ViewModels
{
    public interface ITestViewModel : IRoutableViewModel
    {
        IScreen HostScreen { get; }
    }

    public class TestViewModel : ReactiveObject, ITestViewModel
    {
        public string UrlPathSegment => "Test";
        public IScreen HostScreen { get; protected set; }

        public TestViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
