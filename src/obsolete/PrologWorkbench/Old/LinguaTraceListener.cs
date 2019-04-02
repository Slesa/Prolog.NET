/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;

using Lingua;

namespace Prolog.Workbench
{
    public sealed class LinguaTraceListener : TraceListener
    {
        readonly Dispatcher _dispatcher;
        readonly WriteTraceLineDelegate _writeTraceLineDelegate;

        public LinguaTraceListener(Dispatcher dispatcher, WriteTraceLineDelegate writeTraceLineDelegate)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }
            if (writeTraceLineDelegate == null)
            {
                throw new ArgumentNullException("writeTraceLineDelegate");
            }

            _dispatcher = dispatcher;
            _writeTraceLineDelegate = writeTraceLineDelegate;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            WriteLine(id.ToString(CultureInfo.InvariantCulture));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            var linguaTraceId = (LinguaTraceId)id;
            WriteLine(string.Format("({0}) {1}", linguaTraceId, message));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            var linguaTraceId = (LinguaTraceId)id;
            WriteLine(string.Format("({0}) {1}", linguaTraceId, string.Format(format, args)));
        }

        public override void Write(string message)
        {
            _dispatcher.Invoke(_writeTraceLineDelegate, new object[] { message });
        }

        public override void WriteLine(string message)
        {
            _dispatcher.Invoke(_writeTraceLineDelegate, new object[] { message });
        }
    }
}
