using System;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prolog;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Explorer.Resources;

namespace PrologWorkbench.Explorer.ViewModels
{
    public class InstructionsViewModel : NotificationObject
    {
        [Dependency]
        public IProvideCurrentClause CurrentClauseProvider { get; set; }

        public string Title { get { return Strings.InstructionsViewModel_Title; } }

        Clause _selectedClause;
        public Clause SelectedClause
        {
            get
            {
                if (_selectedClause == null)
                {
                    //CurrentClauseProvider.SelectedClause.PropertyChanged += SelectedClauseOnPropertyChanged;
                    //_selectedClause = CurrentClauseProvider.SelectedClause;
                }
                return _selectedClause;
            }
        }

        void SelectedClauseOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaisePropertyChanged(()=>InstructionStream);
        }

        // Binding SelectedClause.PrologInstructionStream, ElementName=ctrlProgram, Mode=OneWay
        PrologInstructionStream _instructionStream;
        public PrologInstructionStream InstructionStream
        {
            get { return SelectedClause.PrologInstructionStream; }
            //set { _instructionStream = value; }
        }
    }
}