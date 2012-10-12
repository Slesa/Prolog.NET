/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;

namespace Prolog
{
    /// <summary>
    /// Represents a single instruction supported by a <see cref="PrologMachine"/>.
    /// </summary>
    public sealed class PrologInstruction : INotifyPropertyChanged
    {
        #region Fields

        private PrologInstructionStream m_instructionStream;
        private int m_index;

        private bool m_isCurrentLocation;
        private bool m_isBreakpoint;

        #endregion

        #region Constructors

        internal PrologInstruction(PrologInstructionStream instructionStream, int index)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }

            m_instructionStream = instructionStream;
            m_index = index;

            m_isCurrentLocation = false;
            m_isBreakpoint = false;
        }

        #endregion

        #region Public Properties

        public bool IsCurrentInstruction
        {
            get { return m_isCurrentLocation; }
            internal set
            {
                if (value != m_isCurrentLocation)
                {
                    m_isCurrentLocation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsCurrentInstruction"));
                }
            }
        }

        public bool IsBreakpoint
        {
            get { return m_isBreakpoint; }
            internal set
            {
                if (value != m_isBreakpoint)
                {
                    m_isBreakpoint = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsBreakpoint"));
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return WamInstruction.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private WamInstruction WamInstruction
        {
            get { return m_instructionStream.Container.WamInstructionStream[m_index]; }
        }

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
