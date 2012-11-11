using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class VariableListViewModel
    {
    }

    public class ArgumentsVariableListViewModel : VariableListViewModel
    {
        readonly IProvideMachine _machineProvider;

        public ArgumentsVariableListViewModel(IProvideMachine machineProvider)
        {
            _machineProvider = machineProvider;
        }

        public string Title { get { return Resources.Strings.VariablesListViewModel_ArgumentsList; } }

        public PrologVariableList Variables { get { return _machineProvider.Machine.Arguments; } }
    }

    public class TemporaryVariableListViewModel : VariableListViewModel
    {
        readonly IProvideMachine _machineProvider;

        public TemporaryVariableListViewModel(IProvideMachine machineProvider)
        {
            _machineProvider = machineProvider;
        }

        public string Title { get { return Resources.Strings.VariablesListViewModel_TemporariesList; } }

        public PrologVariableList Variables { get { return _machineProvider.Machine.TemporaryVariables; } }
    }

    public class PermanentVariablesListViewModel : VariableListViewModel
    {
        public string Title { get { return Resources.Strings.VariablesListViewModel_PermanentsList; } }
    }
}