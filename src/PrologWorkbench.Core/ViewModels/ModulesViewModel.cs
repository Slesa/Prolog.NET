using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.ViewModels
{
    public class ModulesViewModel
    {
        private readonly IUnityContainer _container;
        private IEnumerable<IWorkbenchModule> _modules;

        public ModulesViewModel(IUnityContainer container)
        {
            _container = container;
        }

        public IEnumerable<IWorkbenchModule> Modules
        {
            get
            {
                var modules = _modules ?? (_modules = _container.ResolveAll<IWorkbenchModule>());
                return modules.OrderBy(x => x.Position);
            }
        }
         
    }
}