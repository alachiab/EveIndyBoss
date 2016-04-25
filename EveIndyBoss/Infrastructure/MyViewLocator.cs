using System;
using System.Linq;
using System.Reflection;
using ReactiveUI;
using Splat;

namespace EveIndyBoss.Infrastructure
{
    // Have to have this. Is fixed in master branch, but there has been no releases lately :(
    public class MyViewLocator : IViewLocator
    {
        public MyViewLocator(Func<string, string> viewModelToViewFunc = null)
        {
            ViewModelToViewFunc = viewModelToViewFunc ??
                                  (vm => interfaceifyTypeName(vm.Replace("ViewModel", "View")));
        }

        public Func<string, string> ViewModelToViewFunc { get; set; }

        /// <summary>
        /// Returns the View associated with a ViewModel, deriving the name of
        /// the Type via ViewModelToViewFunc, then discovering it via
        /// ServiceLocator.
        /// </summary>
        /// <param name="viewModel">The ViewModel for which to find the
        /// associated View.</param>
        /// <returns>The View for the ViewModel.</returns>
        public IViewFor ResolveView<T>(T viewModel, string contract = null)
            where T : class
        {
            // Given IFooBarViewModel (whose name we derive from T), we'll look 
            // for a few things:
            // * IFooBarView that implements IViewFor
            // * IViewFor<IFooBarViewModel>
            // * IViewFor<FooBarViewModel> (the original behavior in RxUI 3.1)

            var attrs = viewModel.GetType().GetTypeInfo().GetCustomAttributes(typeof (ViewContractAttribute), true);

            if (attrs.Any())
            {
                contract = contract ?? ((ViewContractAttribute) attrs.First()).Contract;
            }

            // IFooBarView that implements IViewFor (or custom ViewModelToViewFunc)
            var typeToFind = ViewModelToViewFunc(viewModel.GetType().AssemblyQualifiedName);

            var ret = attemptToResolveView(Reflection.ReallyFindType(typeToFind, false), contract);
            if (ret != null) return ret;

            // IViewFor<IFooBarViewModel>
            var viewType = typeof (IViewFor<>);
            ret = viewModel.GetType().GetTypeInfo().ImplementedInterfaces
                .Where(
                    implementedInterface =>
                        implementedInterface.GetTypeInfo().ImplementedInterfaces.Contains(typeof (IRoutableViewModel)))
                .Select(
                    routableViewModelInterface =>
                        attemptToResolveView(viewType.MakeGenericType(routableViewModelInterface), contract))
                .FirstOrDefault(x => x != null);
            if (ret != null) return ret;

            // IViewFor<FooBarViewModel> (the original behavior in RxUI 3.1)
            return attemptToResolveView(viewType.MakeGenericType(viewModel.GetType()), contract);
        }

        private IViewFor attemptToResolveView(Type type, string contract)
        {
            if (type == null) return null;

            var ret = default(IViewFor);

            try
            {
                ret = (IViewFor) Locator.Current.GetService(type, contract);
            }
            catch (Exception ex)
            {
                this.Log().ErrorException("Failed to instantiate view: " + type.FullName, ex);
                throw;
            }

            return ret;
        }

        private static string interfaceifyTypeName(string typeName)
        {
            var idxComma = typeName.IndexOf(',');
            var idxPeriod = typeName.LastIndexOf('.', idxComma - 1);
            return typeName.Insert(idxPeriod + 1, "I");
        }
    }
}