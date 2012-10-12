/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Reflection;

using Lingua;
using Prolog.Code;
using Prolog.Grammar;

namespace Prolog
{
    public sealed class PerformanceStatistics
    {
        #region Fields

        private int m_startTickCount;
        private int m_stopTickCount;
        private TimeSpan m_elapsedTime;

        private int m_instructionCount;

        #endregion

        #region Public Properties

        public int InstructionCount
        {
            get { return m_instructionCount; }
        }

        public TimeSpan ElapsedTime
        {
            get { return m_elapsedTime; }
        }

        #endregion

        #region Public Methods

        internal void Start()
        {
            m_instructionCount = 0;

            m_startTickCount = Environment.TickCount;
        }

        internal void Stop()
        {
            m_stopTickCount = Environment.TickCount;

            m_elapsedTime = TimeSpan.FromMilliseconds(m_stopTickCount - m_startTickCount);
        }

        internal void IncrementInstructionCount()
        {
            ++m_instructionCount;
        }

        #endregion
    }
}
