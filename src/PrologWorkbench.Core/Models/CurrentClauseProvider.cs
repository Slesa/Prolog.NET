using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.Models
{
    public class CurrentClauseProvider : IProvideCurrentClause
    {
        public Clause SelectedClause { get; set; }
        public Procedure SelectedProcedure { get; set; }
    }
}