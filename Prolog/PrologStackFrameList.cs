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
        PrologStackFrame _currentStackFrame;

        internal PrologStackFrameList(PrologMachine machine)
        {
            if (machine == null)
            {
                throw new ArgumentNullException("machine");
            }

            Machine = machine;
        }

        public PrologMachine Machine { get; private set; }

        public PrologStackFrame CurrentStackFrame
        {
            get { return _currentStackFrame; }
            internal set
            {
                if (value != _currentStackFrame)
                {
                    _currentStackFrame = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CurrentStackFrame"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal PrologStackFrame Push()
        {
            var item = new PrologStackFrame(this, Items.Count);
            Items.Add(item);
            return item;
        }

        internal PrologStackFrame Pop()
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("List is empty.");
            }

            var index = Items.Count - 1;
            var item = Items[index];
            if (item == CurrentStackFrame)
            {
                CurrentStackFrame = null;
            }
            Items.RemoveAt(index);
            return item;
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
