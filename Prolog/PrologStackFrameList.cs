/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;

namespace Prolog
{
    /// <summary>
    /// Represents a stack of <see cref="PrologStackFrame"/> objects.
    /// </summary>
    /// <remarks>Defines the current runtime call stack of a <see cref="PrologMachine"/>.
    /// </remarks>
    public sealed class PrologStackFrameList : ReadableList<PrologStackFrame>, INotifyPropertyChanged
    {
        #region Fields

        private PrologMachine m_machine;

        private PrologStackFrame m_currentStackFrame;

        #endregion

        #region Constructors

        internal PrologStackFrameList(PrologMachine machine)
        {
            if (machine == null)
            {
                throw new ArgumentNullException("machine");
            }

            m_machine = machine;
        }

        #endregion

        #region Public Properties

        public PrologMachine Machine
        {
            get { return m_machine; }
        }

        public PrologStackFrame CurrentStackFrame
        {
            get { return m_currentStackFrame; }
            internal set
            {
                if (value != m_currentStackFrame)
                {
                    m_currentStackFrame = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CurrentStackFrame"));
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        #region Internal Members

        internal PrologStackFrame Push()
        {
            PrologStackFrame item = new PrologStackFrame(this, Items.Count);

            Items.Add(item);

            return item;
        }

        internal PrologStackFrame Pop()
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("List is empty.");
            }

            int index = Items.Count - 1;

            PrologStackFrame item = Items[index];
            if (item == CurrentStackFrame)
            {
                CurrentStackFrame = null;
            }
            Items.RemoveAt(index);

            return item;
        }

        #endregion

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
