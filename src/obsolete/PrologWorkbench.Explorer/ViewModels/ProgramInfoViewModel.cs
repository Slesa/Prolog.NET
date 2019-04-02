using System.ComponentModel;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Explorer.Events;

namespace PrologWorkbench.Explorer.ViewModels
{
    public class ProgramInfoViewModel : ProgramViewModelBase
    {
        public ProgramInfoViewModel(IProvideProgram programProvider, IEventAggregator eventAggregator) 
            : base(programProvider, eventAggregator)
        {
            PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("SelectedClause")) return;
            _eventAggregator.GetEvent<ExplorerClauseChangedEvent>().Publish(SelectedClause);
        }
    }
}