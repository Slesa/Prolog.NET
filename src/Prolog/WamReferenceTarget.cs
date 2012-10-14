/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using Prolog.Code;

namespace Prolog
{
    internal abstract class WamReferenceTarget
    {
        public static WamReferenceTarget Create(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            var codeValue = codeTerm as CodeValue;
            if (codeValue != null)
            {
                return WamValue.Create(codeValue);
            }
            var codeCompoundTerm = codeTerm as CodeCompoundTerm;
            if (codeCompoundTerm != null)
            {
                return WamCompoundTerm.Create(codeCompoundTerm);
            }
            throw new ArgumentException("Invalid CodeTerm type.");
        }

        public static WamReferenceTarget Create(IEnumerable<WamReferenceTarget> items)
        {
            WamCompoundTerm result = null;

            // Iterate through collection and add each item to the list.
            //
            WamCompoundTerm currentListItem = null;
            foreach (var item in items)
            {
                if (currentListItem == null)
                {
                    result = WamCompoundTerm.Create(Functor.ListFunctor);
                    currentListItem = result;
                }
                else
                {
                    currentListItem.Children[1] = WamCompoundTerm.Create(Functor.ListFunctor);
                    currentListItem = (WamCompoundTerm)(currentListItem.Children[1]);
                }
                currentListItem.Children[0] = item;
            }
            if (currentListItem != null)
            {
                currentListItem.Children[1] = WamCompoundTerm.Create(Functor.NilFunctor);
            }

            // If there were no items, create a nil list.
            //
            return result ?? WamCompoundTerm.Create(Functor.NilFunctor);
        }

        public abstract WamReferenceTarget Clone();
        public abstract WamReferenceTarget Dereference();

        public CodeTerm GetCodeTerm()
        {
            return GetCodeTermBase(WamDeferenceTypes.AllVariables, null);
        }

        public CodeTerm GetCodeTerm(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return GetCodeTermBase(dereferenceType, mapping);
        }

        protected abstract CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping);
    }
}
