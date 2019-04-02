﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Core.ViewModels
{
    public class ModuleViewModel : NotificationObject
    {
        public ModuleViewModel(IWorkbenchModule module)
        {
            Icon = module.Icon;
            Title = module.Title;
            Position = module.Position;
            ToolTip = module.ToolTip;
            View = module.View;
        }

        bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        public string Icon { get; private set; }
        public string Title { get; private set; }
        public string ToolTip { get; private set; }
        public int Position { get; private set; }
        public Control View { get; private set; }
    }

    public class ModulesViewModel
    {
        public static readonly string TagMainRegion = "MainRegion";
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;
        private IEnumerable<ModuleViewModel> _modules;

        public ModulesViewModel(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            eventAggregator.GetEvent<ActivateModuleEvent>().Subscribe(OnActivateModule);
        }

        void OnActivateModule(string moduleName)
        {
            var module = Modules.FirstOrDefault(m=>m.View.Name.Equals(moduleName));
            if (module == null) return;
            module.IsChecked = true;
        }

        public IEnumerable<ModuleViewModel> Modules
        {
            get
            {
                if(_modules==null)
                {
                    var modules = _container.ResolveAll<IWorkbenchModule>().Select(x => new ModuleViewModel(x)).ToList();
                    foreach(var module in modules)
                    {
                        module.PropertyChanged += OnModuleSelected;
                        _regionManager.Regions[TagMainRegion].Add(module.View, module.Title);
                    }
                    modules.First().IsChecked = true;
                    _modules = modules.OrderBy(x => x.Position);
                }
                return _modules;
            }
        }

        void OnModuleSelected(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("IsChecked")) return;

            var module = sender as ModuleViewModel;
            if (module == null) return;
            if (!module.IsChecked) return;

            _regionManager.Regions[TagMainRegion].Activate(module.View);
        }

    }
}