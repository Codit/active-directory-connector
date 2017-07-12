using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement
{
    public class NinjectDependencyScope : IDependencyScope
    {
        IResolutionRoot _resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (_resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return _resolver.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (_resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return _resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            IDisposable disposable = _resolver as IDisposable;
            disposable?.Dispose();

            _resolver = null;
        }
    }
}