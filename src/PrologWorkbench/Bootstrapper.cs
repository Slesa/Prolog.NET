using System.IO;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Prolog.Workbench.Helpers;
using Prolog.Workbench.Views;
using PrologWorkbench.Core;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;
using PrologWorkbench.Editor;
using PrologWorkbench.Tracer;

namespace Prolog.Workbench
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            //return ServiceLocator.Current.GetInstance<MainWindow>();
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            RegisterShellObjects();
        }

        private void RegisterShellObjects()
        {
            //RegisterTypeIfMissing(typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), true);

            RegisterTypeIfMissing(typeof (ILoggerFacade), typeof (Log4NetLogger), true);
            RegisterTypeIfMissing(typeof (IRegionManager), typeof (RegionManager), true);
            RegisterTypeIfMissing(typeof (IProvideProgram), typeof (ProgramProvider), true);
        }

        protected override void ConfigureModuleCatalog()
        {
            var coreModule = typeof (CoreModule);
            ModuleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = coreModule.Name,
                                            ModuleType = coreModule.AssemblyQualifiedName
                                        });
            var editorModule = typeof (EditorModule);
            ModuleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = editorModule.Name,
                                            ModuleType = editorModule.AssemblyQualifiedName
                                        });
            var tracerModule = typeof (TracerModule);
            ModuleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = tracerModule.Name,
                                            ModuleType = tracerModule.AssemblyQualifiedName
                                        });
        }
    }
}