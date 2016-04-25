using System.Windows;
using System.Windows.Controls;
using EveIndyBoss.ViewModels;
using ReactiveUI;

namespace EveIndyBoss.Views
{
    public partial class MenuView : UserControl, IViewFor<IMenuViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IMenuViewModel), typeof(MenuView),
                new PropertyMetadata(null));

        public MenuView(IMenuViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IMenuViewModel)value; }
        }

        public IMenuViewModel ViewModel
        {
            get { return (IMenuViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}