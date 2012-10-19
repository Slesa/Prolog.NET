using System.IO;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Prolog.Workbench.Helpers;
using Prolog.Workbench.Views;

namespace Prolog.Workbench
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
            //return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            RegisterShellObjects();
        }

        void RegisterShellObjects()
        {
            //RegisterTypeIfMissing(typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), true);

            RegisterTypeIfMissing(typeof(ILoggerFacade), typeof(Log4NetLogger), true);
            RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
        }
    }
}