/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Prolog.Workbench
{
    public sealed class AppState : INotifyPropertyChanged
    {
        #region Fields

        private App m_application;

        private Transcript m_transcript;
        private Views m_view;
        private bool m_statisticsEnabled;
        private bool m_traceEnabled;

        private Program m_program;
        private PrologMachine m_machine;

        #endregion

        #region Constructors

        internal AppState(App application)
        {
            m_application = application;

            m_transcript = new Transcript();
            m_view = Views.Transcript;
        }

        #endregion

        #region Public Properties

        public App Application
        {
            get { return m_application; }
        }

        public Transcript Transcript
        {
            get { return m_transcript; }
        }

        public Views View
        {
            get { return m_view; }
            set
            {
                if (value != m_view)
                {
                    m_view = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("View"));

                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool StatisticsEnabled
        {
            get { return m_statisticsEnabled; }
            set
            {
                if (value != m_statisticsEnabled)
                {
                    m_statisticsEnabled = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("StatisticsEnabled"));
                }
            }
        }

        public bool TraceEnabled
        {
            get { return m_traceEnabled; }
            set
            {
                if (value != m_traceEnabled)
                {
                    m_traceEnabled = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("TraceEnabled"));
                }
            }
        }

        public Program Program
        {
            get { return m_program; }
            set
            {
                if (value != m_program)
                {
                    m_program = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Program"));
                }
            }
        }

        public PrologMachine Machine
        {
            get { return m_machine; }
            set
            {
                if (value != m_machine)
                {
                    m_machine = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Machine"));

                    if (m_machine != null)
                    {
                        m_machine.ExecutionComplete += Machine_ExecutionComplete;
                    }
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Event Handlers

        void Machine_ExecutionComplete(object sender, PrologQueryEventArgs e)
        {
            if (e.Results != null)
            {
                StringBuilder sb = new StringBuilder();

                string prefix = null;
                foreach (PrologVariable variable in e.Results.Variables)
                {
                    sb.Append(prefix); prefix = System.Environment.NewLine;
                    sb.AppendFormat("{0} = {1}", variable.Name, variable.Text);
                }

                string variables = sb.ToString();
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

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
