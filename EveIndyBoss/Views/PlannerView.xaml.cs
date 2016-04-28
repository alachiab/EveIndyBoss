using System.Windows;
using System.Windows.Controls;
using EveIndyBoss.ViewModels;
using ReactiveUI;

namespace EveIndyBoss.Views
{
    public partial class PlannerView : UserControl, IViewFor<IPlannerViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel",
            typeof (IPlannerViewModel), typeof (PlannerView), new PropertyMetadata(null));

        public PlannerView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.BlueprintGroups, v => v.BlueprintGroups.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.Blueprints, v => v.Blueprints.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.SelectedEfficiencyLevel, v => v.EfficiencyIndicator.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Materials, v => v.ResourceList.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.SelectedQuantity, v => v.QuantityIndicator.Text));
                d(this.OneWayBind(ViewModel, vm => vm.SelectedBlueprint.MaxProductionLimit, v => v.Quantity.Maximum));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.JitaSell, v => v.JitaSell.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.ShippingToStaging, v => v.ShipFromJita.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.StagingSell, v => v.StagingSell.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.SellAtProfit, v => v.ProfitSell.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.TotalCost, v => v.TotalCost.Text));
                d(this.OneWayBind(ViewModel, vm => vm.ProductInfo.TotalCostPerItem, v => v.TotalCostPerItem.Text));

                d(this.Bind(ViewModel, vm => vm.SelectedBlueprintGroup, v => v.BlueprintGroups.SelectedValue));
                d(this.Bind(ViewModel, vm => vm.SelectedBlueprint, v => v.Blueprints.SelectedValue));
                d(this.Bind(ViewModel, vm => vm.SelectedEfficiencyLevel, v => v.Efficiency.Value));
                d(this.Bind(ViewModel, vm => vm.SelectedQuantity, v => v.Quantity.Value));
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IPlannerViewModel) value; }
        }

        public IPlannerViewModel ViewModel
        {
            get { return (IPlannerViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}