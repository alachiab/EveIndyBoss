using System.Windows;
using EveIndyBoss.ViewModels;
using ReactiveUI;

namespace EveIndyBoss.Views
{
    public partial class ShellView : IViewFor<IShellViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IShellViewModel), typeof(ShellView),
                new PropertyMetadata(null));

        public ShellView(IShellViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.Bind(ViewModel, vm => vm.HostScreen.Router, v => v.ContentView.Router);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IShellViewModel)value; }
        }

        public IShellViewModel ViewModel
        {
            get { return (IShellViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value);}
        }
    }
}