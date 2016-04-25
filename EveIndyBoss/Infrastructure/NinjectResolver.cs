using System;
using System.Collections.Generic;
using Ninject;
using ReactiveUI;
using Splat;

namespace EveIndyBoss.Infrastructure
{
    public class NinjectResolver : IMutableDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType, string contract = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contract))
                    return _kernel.Get(serviceType);

                return _kernel.Get(serviceType, contract);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contract))
                    return _kernel.GetAll(serviceType);

                return _kernel.GetAll(serviceType, contract);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (string.IsNullOrWhiteSpace(contract))
                _kernel.Bind(serviceType).ToMethod(x => factory());
            else
                _kernel.Bind(serviceType).ToMethod(x => factory()).Named(contract);
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            // this method is not used by RxUI
            throw new NotImplementedException();
        }
    }
}