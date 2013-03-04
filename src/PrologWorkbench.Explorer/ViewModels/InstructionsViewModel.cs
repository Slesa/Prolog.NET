using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using Prolog;
using PrologWorkbench.Explorer.Events;
using PrologWorkbench.Explorer.Resources;

namespace PrologWorkbench.Explorer.ViewModels
{
    public class InstructionsViewModel
    {
        public InstructionsViewModel(IEventAggregator eventAggregator)
        {
            InstructionStream = new ObservableCollection<PrologInstruction>();
            eventAggregator.GetEvent<ExplorerClauseChangedEvent>().Subscribe(OnExplorerClauseChanged);
        }

        void OnExplorerClauseChanged(Clause clause)
        {
            InstructionStream.Clear();
            if (clause == null) return;
            foreach (var item in clause.PrologInstructionStream) InstructionStream.Add(item);
        }

        public string Title { get { return Strings.InstructionsViewModel_Title; } }

        public ObservableCollection<PrologInstruction> InstructionStream { get; private set; }
    }
}