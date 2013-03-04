using Microsoft.Practices.Prism.Events;

namespace PrologWorkbench.Explorer.Events
{
    internal class ExplorerClauseChangedEvent : CompositePresentationEvent<Prolog.Clause>{ }
}