/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

using Prolog.Code;

namespace Prolog
{
    internal class WamReferenceTargetMapping
    {
        private readonly Dictionary<WamVariable, CodeVariable> _variables = new Dictionary<WamVariable, CodeVariable>();
        private int _count = 0;

        public void Clear()
        {
            _variables.Clear();
        }

        public CodeVariable Lookup(WamVariable wamVariable)
        {
            if (wamVariable == null)
            {
                throw new ArgumentNullException("wamVariable");
            }

            CodeVariable codeVariable;
            if (!_variables.TryGetValue(wamVariable, out codeVariable))
            {
                ++_count;
                codeVariable = new CodeVariable(string.Format("V{0}", _count));
                _variables.Add(wamVariable, codeVariable);
            }
            return codeVariable;
        }
    }
}
