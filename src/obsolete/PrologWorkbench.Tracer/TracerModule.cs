﻿using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Tracer.Resources;
using PrologWorkbench.Tracer.ViewModels;
using PrologWorkbench.Tracer.Views;

namespace PrologWorkbench.Tracer
{
    public class TracerModule : IModule, IWorkbenchModule
    {
        public static readonly string TagTracerModule = "TracerModule";

        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public TracerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<TraceViewModel>();
            _container.RegisterInstance<IWorkbenchModule>(TagTracerModule, this);

            _container.RegisterType<TraceView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 40; } }
        public string Icon { get { return "/PrologWorkbench.Tracer;component/Resources/Tracer.png"; } }
        public string Title { get { return Strings.TracerModule_Title; } }
        public string ToolTip { get { return Strings.TracerModule_ToolTip; } }
        public Control View { get { return _container.Resolve<TraceView>(); } }
    }
}
