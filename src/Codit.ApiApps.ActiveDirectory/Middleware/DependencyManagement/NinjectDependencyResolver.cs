using System.Web.Http.Dependencies;

namespace Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement
{
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        public NinjectDependencyResolver() : base(DependencyContainer.Instance)
        {
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(DependencyContainer.Instance.BeginBlock());
        }
    }
}