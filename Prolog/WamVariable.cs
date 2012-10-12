/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamVariable : WamReferenceTarget
    {
        #region Fields

        private int m_generation;

        private WamReferenceTarget m_target;

        #endregion

        #region Constructors

        public WamVariable(int generation)
        {
            m_generation = generation;

            m_target = null;
        }

        public override WamReferenceTarget Clone()
        {
            WamVariable result = new WamVariable(Generation);

            if (m_target != null)
            {
                result.m_target = m_target.Clone();
            }

            return result;
        }

        #endregion

        #region Public Properties

        public int Generation
        {
            get { return m_generation; }
        }

        public WamReferenceTarget Target
        {
            get { return m_target; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (m_target == null)
            {
                return "_";
            }

            return m_target.ToString();
        }

        public override WamReferenceTarget Dereference()
        {
            if (m_target == null)
            {
                return this;
            }

            return m_target.Dereference();
        }

        public void Bind(WamReferenceTarget target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (m_target != null)
            {
                throw new InvalidOperationException("Attempt to bind to bound variable.");
            }

            m_target = target;
        }

        public void Unbind()
        {
            if (m_target == null)
            {
                throw new InvalidOperationException("Attempt to unbind an unbound variable.");
            }

            m_target = null;
        }

        #endregion

        #region Hidden Members

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            switch (dereferenceType)
            {
                case WamDeferenceTypes.AllVariables:
                    {
                        if (m_target == null)
                        {
                            return new CodeValueObject(null);
                        }

                        return m_target.GetCodeTerm(dereferenceType, mapping);
                    }

                case WamDeferenceTypes.BoundVariables:
                    {
                        if (m_target == null)
                        {
                            return mapping.Lookup(this);
                        }

                        return m_target.GetCodeTerm(dereferenceType, mapping);
                    }

                case WamDeferenceTypes.None:
                    {
                        return mapping.Lookup(this);
                    }

                default:
                    throw new InvalidOperationException(string.Format("Unknown dereferenceType {0}.", dereferenceType));
            }
        }

        #endregion
    }
}
