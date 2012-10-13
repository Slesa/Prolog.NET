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
        readonly PrologInstructionStream _instructionStream;
        readonly int _index;

        bool _isCurrentLocation;
        bool _isBreakpoint;

        internal PrologInstruction(PrologInstructionStream instructionStream, int index)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }

            _instructionStream = instructionStream;
            _index = index;

            _isCurrentLocation = false;
            _isBreakpoint = false;
        }

        public bool IsCurrentInstruction
        {
            get { return _isCurrentLocation; }
            internal set
            {
                if (value != _isCurrentLocation)
                {
                    _isCurrentLocation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsCurrentInstruction"));
                }
            }
        }

        public bool IsBreakpoint
        {
            get { return _isBreakpoint; }
            internal set
            {
                if (value != _isBreakpoint)
                {
                    _isBreakpoint = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsBreakpoint"));
                }
            }
        }

        public override string ToString()
        {
            return WamInstruction.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        WamInstruction WamInstruction
        {
            get { return _instructionStream.Container.WamInstructionStream[_index]; }
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
