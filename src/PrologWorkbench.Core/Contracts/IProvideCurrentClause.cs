using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public interface IProvideCurrentClause
    {
        Clause SelectedClause { get; set; }
        Procedure SelectedProcedure { get; set; } 
    }
}