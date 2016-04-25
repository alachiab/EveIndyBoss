using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EveIndyBoss.ViewModels;
using ReactiveUI;

namespace EveIndyBoss.Views
{
    public partial class TestView : UserControl, IViewFor<ITestViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel",
            typeof(ITestViewModel), typeof(TestView), new PropertyMetadata(null));

        public TestView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ITestViewModel)value; }
        }

        public ITestViewModel ViewModel
        {
            get { return (ITestViewModel)GetValue(ViewModelProperty); }
            set{SetValue(ViewModelProperty, value);}
        }
    }
}
