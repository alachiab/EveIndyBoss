using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conditions;
using EveIndyBoss.Models;
using EveIndyBoss.Models.StaticData;
using EveIndyBoss.Services;
using EveIndyBoss.Services.StaticData;
using ReactiveUI;
using Splat;

namespace EveIndyBoss.ViewModels
{
    public interface IPlannerViewModel : IRoutableViewModel
    {
        IEnumerable<Blueprint> Blueprints { get; }
        IEnumerable<BlueprintGroup> BlueprintGroups { get; }
        IEnumerable<MaterialForProduction> Materials { get; }
        BlueprintGroup SelectedBlueprintGroup { get; set; }
        Blueprint SelectedBlueprint { get; set; }
        int SelectedEfficiencyLevel { get; set; }
        int SelectedQuantity { get; set; }
        ProductInfo ProductInfo { get; }
    }

    public class PlannerViewModel : ReactiveObject, IPlannerViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<BlueprintGroup>> _blueprintGroups;
        private readonly ObservableAsPropertyHelper<IEnumerable<Blueprint>> _blueprints;
        private readonly ObservableAsPropertyHelper<IEnumerable<MaterialForProduction>> _materialList;
        private readonly ICalculateMaterials _materials;
        private readonly ObservableAsPropertyHelper<IEnumerable<MaterialForProduction>> _materialsBase;
        private readonly ObservableAsPropertyHelper<ProductInfo> _productInfo;
        private readonly IProvideStaticData _staticData;
        private Blueprint _selecBlueprint = new Blueprint();
        private BlueprintGroup _selecBlueprintGroup = new BlueprintGroup();
        private int _selectedEfficiencyLevel;
        private int _selectedQuantity;

        public PlannerViewModel(IScreen screen, IProvideStaticData staticData, ICalculateMaterials materials)
        {
            HostScreen = screen;
            _staticData = staticData;
            _materials = materials;

            var canExecuteLoadBlueprints = this.WhenAny(vm => vm.SelectedBlueprintGroup, change => change.Value != null);
            LoadBlueprints = ReactiveCommand.CreateAsyncTask(canExecuteLoadBlueprints,
                _ => _staticData.GetBlueprintsAsync(SelectedBlueprintGroup.GroupId), RxApp.MainThreadScheduler);
            LoadBlueprints.ToProperty(this, x => x.Blueprints, out _blueprints, new List<Blueprint>(),
                RxApp.MainThreadScheduler);
            LoadBlueprints.ThrownExceptions.Subscribe(te => { this.Log().Error(te); });

            LoadBlueprintGroups = ReactiveCommand.CreateAsyncTask(_ => _staticData.GetBlueprintGroupsAsync());
            LoadBlueprintGroups.ToProperty(this, x => x.BlueprintGroups, out _blueprintGroups,
                new List<BlueprintGroup>(), RxApp.MainThreadScheduler);
            LoadBlueprintGroups.ThrownExceptions.Subscribe(te => { this.Log().Error(te); });

            var canExecuteGenerateBaseMatList = this.WhenAny(vm => vm.SelectedBlueprint, change => change.Value != null);
            GenerateBaseMaterialList = ReactiveCommand.CreateAsyncTask(canExecuteGenerateBaseMatList,
                _ => _materials.GetCalculationBase(SelectedBlueprint), RxApp.MainThreadScheduler);
            GenerateBaseMaterialList.ToProperty(this, x => x.MaterialsBase, out _materialsBase,
                new List<MaterialForProduction>());
            GenerateBaseMaterialList.ThrownExceptions.Subscribe(te => { this.Log().Error(te); });

            var a = this.WhenAnyValue(x => x.MaterialsBase, x => x.SelectedEfficiencyLevel, x => x.SelectedQuantity,
                CalculateMats);
            a.ToProperty(this, x => x.Materials, out _materialList);

            var b = this.WhenAnyValue(x => x.Materials, x => CalculateStuff(x, SelectedBlueprint, SelectedQuantity));
            b.ToProperty(this, x => x.ProductInfo, out _productInfo);

            Condition.Ensures(_staticData).IsNotNull();

            this.WhenAnyValue(vm => vm.SelectedBlueprintGroup).InvokeCommand(this, vm => vm.LoadBlueprints);
            this.WhenAnyValue(vm => vm.SelectedBlueprint).InvokeCommand(this, vm => vm.GenerateBaseMaterialList);
            this.WhenAnyValue(vm => vm.SelectedEfficiencyLevel).InvokeCommand(this, vm => vm.GenerateBaseMaterialList);

            LoadBlueprintGroups.Execute(null);
        }

        private ReactiveCommand<IEnumerable<Blueprint>> LoadBlueprints { get; }
        private ReactiveCommand<IEnumerable<BlueprintGroup>> LoadBlueprintGroups { get; }
        private ReactiveCommand<IEnumerable<MaterialForProduction>> GenerateBaseMaterialList { get; }
        public IEnumerable<MaterialForProduction> MaterialsBase => _materialsBase.Value;
        public ProductInfo ProductInfo => _productInfo.Value;

        public BlueprintGroup SelectedBlueprintGroup
        {
            get { return _selecBlueprintGroup; }
            set { this.RaiseAndSetIfChanged(ref _selecBlueprintGroup, value); }
        }

        public Blueprint SelectedBlueprint
        {
            get { return _selecBlueprint; }
            set { this.RaiseAndSetIfChanged(ref _selecBlueprint, value); }
        }

        public int SelectedEfficiencyLevel
        {
            get { return _selectedEfficiencyLevel; }
            set { this.RaiseAndSetIfChanged(ref _selectedEfficiencyLevel, value); }
        }

        public int SelectedQuantity
        {
            get { return _selectedQuantity; }
            set { this.RaiseAndSetIfChanged(ref _selectedQuantity, value); }
        }

        public string UrlPathSegment => "Test";
        public IScreen HostScreen { get; protected set; }
        public IEnumerable<Blueprint> Blueprints => _blueprints.Value;
        public IEnumerable<BlueprintGroup> BlueprintGroups => _blueprintGroups.Value;
        public IEnumerable<MaterialForProduction> Materials => _materialList.Value;

        private ProductInfo CalculateStuff(IEnumerable<MaterialForProduction> mats, Blueprint selectedBlueprint, int quantity)
        {
            if(mats == null || selectedBlueprint == null || selectedBlueprint.OutputTypeId == 0 || quantity == 0)
                return new ProductInfo();

            return _materials.CalculateStuff(mats.ToList(), selectedBlueprint.OutputTypeId, quantity);
        }

        private IEnumerable<MaterialForProduction> CalculateMats(IEnumerable<MaterialForProduction> mats, int efficiency,
            int quantity)
        {
            return _materials.Calculate(mats.ToList(), efficiency, quantity);
        }
    }
}