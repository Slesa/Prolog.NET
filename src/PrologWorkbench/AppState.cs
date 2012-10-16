/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Prolog.Workbench
{
    public sealed class AppState : INotifyPropertyChanged
    {
        internal AppState(App application)
        {
            Application = application;

            Transcript = new Transcript();
            _view = Views.Transcript;
        }

        public App Application { get; private set; }
        public Transcript Transcript { get; private set; }

        Views _view;
        public Views View
        {
            get { return _view; }
            set
            {
                if (value != _view)
                {
                    _view = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("View"));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        bool _statisticsEnabled;
        public bool StatisticsEnabled
        {
            get { return _statisticsEnabled; }
            set
            {
                if (value != _statisticsEnabled)
                {
                    _statisticsEnabled = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("StatisticsEnabled"));
                }
            }
        }

        bool _traceEnabled;
        public bool TraceEnabled
        {
            get { return _traceEnabled; }
            set
            {
                if (value != _traceEnabled)
                {
                    _traceEnabled = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("TraceEnabled"));
                }
            }
        }

        Program _program;
        public Program Program
        {
            get { return _program; }
            set
            {
                if (value != _program)
                {
                    _program = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Program"));
                }
            }
        }

        PrologMachine _machine;
        public PrologMachine Machine
        {
            get { return _machine; }
            set
            {
                if (value != _machine)
                {
                    _machine = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Machine"));
                    if (_machine != null)
                    {
                        _machine.ExecutionComplete += OnMachineExecutionComplete;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnMachineExecutionComplete(object sender, PrologQueryEventArgs e)
        {
            if (e.Results != null)
            {
                var sb = new StringBuilder();

                string prefix = null;
                foreach (var variable in e.Results.Variables)
                {
                    sb.Append(prefix); prefix = System.Environment.NewLine;
                    sb.AppendFormat("{0} = {1}", variable.Name, variable.Text);
                }

                var variables = sb.ToString();
                if (!string.IsNullOrEmpty(variables))
                {
                    Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, variables);
                }
                Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseSuccess);

                if (StatisticsEnabled)
                {
                    Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, string.Format("{0} IC:{1}", Machine.PerformanceStatistics.ElapsedTime, Machine.PerformanceStatistics.InstructionCount));
                }
            }
            else
            {
                Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseFailure);
            }
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
