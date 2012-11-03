using Lingua;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using PrologWorkbench.Tracer.Helpers;

namespace PrologWorkbench.Tracer.ViewModels
{
    public class TraceViewModel : NotificationObject
    {
        LinguaTraceListener _linguaTraceListener;

        public TraceViewModel()
        {
            EnableTraceCommand = new DelegateCommand(OnEnableTrace);
            ClearTraceCommand = new DelegateCommand(OnClearTrace, CanClearTrace);
        }

        public string Title { get { return Resources.Strings.TraceViewModel_Title; } }

        public DelegateCommand EnableTraceCommand { get; private set; }
        public DelegateCommand ClearTraceCommand { get; private set; }

        string _traces;
        public string Traces
        {
            get { return _traces; }
            set
            {
                if (value == _traces) return;
                _traces = value;
                RaisePropertyChanged(() => Traces);
                ClearTraceCommand.RaiseCanExecuteChanged();
            }
        }

        bool _traceEnabled;
        public bool TraceEnabled
        {
            get { return _traceEnabled; }
            set
            {
                if(value==_traceEnabled) return;
                _traceEnabled = value;
                RaisePropertyChanged(()=>TraceEnabled);
                EnableTraceCommand.RaiseCanExecuteChanged();
            }
        }

        bool CanClearTrace()
        {
            return !string.IsNullOrEmpty(Traces);
        }

        void OnClearTrace()
        {
            Traces = string.Empty;
        }

        void OnEnableTrace()
        {
            if (TraceEnabled)
            {
                if (_linguaTraceListener == null)
                {
                    _linguaTraceListener = new LinguaTraceListener(WriteTraceLine);
                    LinguaTrace.TraceSource.Listeners.Add(_linguaTraceListener);
                }
            }
            else
            {
                if (_linguaTraceListener != null)
                {
                    LinguaTrace.TraceSource.Listeners.Remove(_linguaTraceListener);
                    _linguaTraceListener = null;
                }
            }
        }

        void WriteTraceLine(string text)
        {
            Traces += text + System.Environment.NewLine;
        }
    }
}