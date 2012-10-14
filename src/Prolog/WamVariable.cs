/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamVariable : WamReferenceTarget
    {
        public WamVariable(int generation)
        {
            Generation = generation;
            Target = null;
        }

        public override WamReferenceTarget Clone()
        {
            var result = new WamVariable(Generation);
            if (Target != null)
            {
                result.Target = Target.Clone();
            }
            return result;
        }

        public int Generation { get; private set; }
        public WamReferenceTarget Target { get; private set; }

        public override string ToString()
        {
            return Target == null ? "_" : Target.ToString();
        }

        public override WamReferenceTarget Dereference()
        {
            return Target == null ? this : Target.Dereference();
        }

        public void Bind(WamReferenceTarget target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (Target != null)
            {
                throw new InvalidOperationException("Attempt to bind to bound variable.");
            }
            Target = target;
        }

        public void Unbind()
        {
            if (Target == null)
            {
                throw new InvalidOperationException("Attempt to unbind an unbound variable.");
            }
            Target = null;
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            switch (dereferenceType)
            {
                case WamDeferenceTypes.AllVariables:
                    {
                        return Target == null ? new CodeValueObject(null) : Target.GetCodeTerm(dereferenceType, mapping);
                    }
                case WamDeferenceTypes.BoundVariables:
                    {
                        return Target == null ? mapping.Lookup(this) : Target.GetCodeTerm(dereferenceType, mapping);
                    }
                case WamDeferenceTypes.None:
                    {
                        return mapping.Lookup(this);
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown dereferenceType {0}.", dereferenceType));
            }
        }
    }
}
