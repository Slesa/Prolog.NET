using Prolog.Workbench.Models;
using ReactiveUI;

namespace Prolog.Workbench.ViewModels;

public class MainWindowViewModel : ViewModelBase, IProvideProgram
{
    public MainWindowViewModel()
    {
        Program = new Prolog.Program();
    }
    
    string _statusMsg;
    public string StatusMsg
    {
        get => _statusMsg;
        private set => this.RaiseAndSetIfChanged(ref _statusMsg, value);
    }

    public Prolog.Program Program { get; }
}