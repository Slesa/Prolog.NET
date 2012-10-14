/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    public sealed class PerformanceStatistics
    {
        int _startTickCount;
        int _stopTickCount;

        public int InstructionCount { get; private set; }

        public TimeSpan ElapsedTime { get; private set; }

        internal void Start()
        {
            InstructionCount = 0;
            _startTickCount = Environment.TickCount;
        }

        internal void Stop()
        {
            _stopTickCount = Environment.TickCount;
            ElapsedTime = TimeSpan.FromMilliseconds(_stopTickCount - _startTickCount);
        }

        internal void IncrementInstructionCount()
        {
            ++InstructionCount;
        }
    }
}
