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
        #region Fields

        private PrologVariableList m_container;

        private string m_register;
        private string m_name;
        private string m_fullName;

        private string m_text = "*";
        private CodeTerm m_codeTerm;

        #endregion

        #region Constructors

        internal PrologVariable(PrologVariableList container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            m_container = container;
        }

        #endregion

        #region Public Properties

        public PrologVariableList Container
        {
            get { return m_container; }
        }

        public string Register
        {
            get { return m_register; }
            internal set
            {
                if (value != m_register)
                {
                    m_register = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));

                    FullName = GetFullName();
                }
            }
        }

        public string Name
        {
            get { return m_name; }
            internal set
            {
                if (value != m_name)
                {
                    m_name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));

                    FullName = GetFullName();
                }
            }
        }

        public string FullName
        {
            get { return m_fullName; }
            internal set
            {
                if (value != m_fullName)
                {
                    m_fullName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("FullName"));
                }
            }
        }

        public string Text
        {
            get { return m_text; }
            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (value != m_text)
                {
                    m_text = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public CodeTerm CodeTerm
        {
            get { return m_codeTerm; }
            internal set
            {
                if (value != m_codeTerm)
                {
                    m_codeTerm = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CodeTerm"));

                    if (m_codeTerm == null)
                    {
                        Text = "*";
                    }
                    else
                    {
                        Text = m_codeTerm.ToString();
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Format("{0} = {1}", Register, Text);
            }
            else
            {
                return string.Format("{0}/{1} = {2}", Register, Name, Text);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private string GetFullName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Register;
            }
            else
            {
                return string.Format("{0}/{1}", Register, Name);
            }
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
