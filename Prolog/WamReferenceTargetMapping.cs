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
        #region Fields

        private Dictionary<WamVariable, CodeVariable> m_variables = new Dictionary<WamVariable, CodeVariable>();
        private int m_count = 0;

        #endregion

        #region Public Methods

        public void Clear()
        {
            m_variables.Clear();
        }

        public CodeVariable Lookup(WamVariable wamVariable)
        {
            if (wamVariable == null)
            {
                throw new ArgumentNullException("wamVariable");
            }

            CodeVariable codeVariable;

            if (!m_variables.TryGetValue(wamVariable, out codeVariable))
            {
                ++m_count;
                codeVariable = new CodeVariable(string.Format("V{0}", m_count));
                m_variables.Add(wamVariable, codeVariable);
            }

            return codeVariable;
        }

        #endregion
    }
}
