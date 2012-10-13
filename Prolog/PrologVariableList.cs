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
        readonly IPrologVariableListContainer _container;

        internal PrologVariableList()
        {
            _container = null;
        }

        internal PrologVariableList(IPrologVariableListContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
        }

        public PrologVariable this[string register]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Register == register)
                    {
                        return item;
                    }
                }
                throw new KeyNotFoundException();
            }
        }

        public bool TryGetValue(string register, out PrologVariable value)
        {
            foreach (var item in this)
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

        internal IPrologVariableListContainer Container
        {
            get { return _container; }
        }

        internal void Clear()
        {
            Items.Clear();
        }

        internal PrologVariable Add(string register)
        {
            var variable = new PrologVariable(this) {Register = register};
            Items.Add(variable);
            return variable;
        }

        internal void Remove(string register)
        {
            foreach (var variable in this)
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
            var registers = new HashSet<string>();
            foreach (var variable in this)
            {
                registers.Add(variable.Register);
            }

            // Add/update variables in specified list.
            //
            foreach (var variable in variables)
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
            foreach (var register in registers)
            {
                Remove(register);
            }
        }
    }
}
