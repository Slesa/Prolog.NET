/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

namespace Prolog
{
    /// <summary>
    /// Represents a list of <see cref="PrologVariable"/> objects.
    /// </summary>
    public sealed class PrologVariableList : ReadableList<PrologVariable>
    {
        #region Fields

        private IPrologVariableListContainer m_container;

        #endregion

        #region Constructors

        internal PrologVariableList()
        {
            m_container = null;
        }

        internal PrologVariableList(IPrologVariableListContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            m_container = container;
        }

        #endregion

        #region Public Properties

        public PrologVariable this[string register]
        {
            get
            {
                foreach (PrologVariable item in this)
                {
                    if (item.Register == register)
                    {
                        return item;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        #endregion

        #region Public Methods

        public bool TryGetValue(string register, out PrologVariable value)
        {
            foreach (PrologVariable item in this)
            {
                if (item.Register == register)
                {
                    value = item;
                    return true;
                }
            }

            value = null;
            return false;
        }

        #endregion

        #region Internal Members

        internal IPrologVariableListContainer Container
        {
            get { return m_container; }
        }

        internal void Clear()
        {
            Items.Clear();
        }

        internal PrologVariable Add(string register)
        {
            PrologVariable variable = new PrologVariable(this);
            variable.Register = register;

            Items.Add(variable);

            return variable;
        }

        internal void Remove(string register)
        {
            foreach (PrologVariable variable in this)
            {
                if (variable.Register == register)
                {
                    Items.Remove(variable);
                    return;
                }
            }
        }

        internal void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        internal void Synchronize(PrologVariableList variables)
        {
            HashSet<string> registers = new HashSet<string>();
            foreach (PrologVariable variable in this)
            {
                registers.Add(variable.Register);
            }

            // Add/update variables in specified list.
            //
            foreach (PrologVariable variable in variables)
            {
                PrologVariable thisVariable;
                if (registers.Contains(variable.Register))
                {
                    thisVariable = this[variable.Register];
                    registers.Remove(variable.Register);
                }
                else
                {
                    thisVariable = Add(variable.Register);
                    thisVariable.Name = variable.Name;
                }

                if (variable.CodeTerm != null)
                {
                    thisVariable.CodeTerm = variable.CodeTerm;
                }
                else
                {
                    thisVariable.Text = variable.Text;
                }
            }

            // Remove variables not in specified list.
            //
            foreach (string register in registers)
            {
                Remove(register);
            }
        }

        #endregion
    }
}
