/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using System.Globalization;
using Lingua;

namespace PrologWorkbench.Tracer.Helpers
{
    public delegate void WriteTraceLineDelegate(string text);

    public sealed class LinguaTraceListener : TraceListener
    {
        readonly WriteTraceLineDelegate _writeTraceLineDelegate;

        public LinguaTraceListener(WriteTraceLineDelegate writeTraceLineDelegate)
        {
            if (writeTraceLineDelegate == null)
            {
                throw new ArgumentNullException("writeTraceLineDelegate");
            }
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
            _writeTraceLineDelegate(message);
        }

        public override void WriteLine(string message)
        {
            _writeTraceLineDelegate(message);
        }
    }
}
