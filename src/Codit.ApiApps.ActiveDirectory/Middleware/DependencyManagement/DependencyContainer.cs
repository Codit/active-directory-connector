using Ninject;

namespace Codit.ApiApps.ActiveDirectory.Middleware.DependencyManagement
{
    public class DependencyContainer
    {
        private static StandardKernel _instance;

        /// <summary>
        /// Gets the instance of the dependency container
        /// </summary>
        public static StandardKernel Instance => _instance ?? (_instance = new StandardKernel());
    }
}