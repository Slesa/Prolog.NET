/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;

using Prolog.Code;

namespace Prolog
{
    public class PrologVariable : INotifyPropertyChanged
    {
        string _register;
        string _name;
        string _fullName;
        string _text = "*";
        CodeTerm _codeTerm;

        internal PrologVariable(PrologVariableList container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            Container = container;
        }

        public PrologVariableList Container { get; private set; }

        public string Register
        {
            get { return _register; }
            internal set
            {
                if (value != _register)
                {
                    _register = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Register"));
                    FullName = GetFullName();
                }
            }
        }

        public string Name
        {
            get { return _name; }
            internal set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));
                    FullName = GetFullName();
                }
            }
        }

        public string FullName
        {
            get { return _fullName; }
            internal set
            {
                if (value != _fullName)
                {
                    _fullName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("FullName"));
                }
            }
        }

        public string Text
        {
            get { return _text; }
            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (value != _text)
                {
                    _text = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public CodeTerm CodeTerm
        {
            get { return _codeTerm; }
            internal set
            {
                if (value != _codeTerm)
                {
                    _codeTerm = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CodeTerm"));
                    Text = _codeTerm == null ? "*" : _codeTerm.ToString();
                }
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) 
                ? string.Format("{0} = {1}", Register, Text) 
                : string.Format("{0}/{1} = {2}", Register, Name, Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        string GetFullName()
        {
            return string.IsNullOrEmpty(Name) ? Register : string.Format("{0}/{1}", Register, Name);
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
