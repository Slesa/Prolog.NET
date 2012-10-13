/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Text;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamCompoundTerm : WamReferenceTarget
    {
        private static WamReferenceTarget[] s_emptyList = new WamReferenceTarget[0];

        private WamCompoundTerm(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            Functor = functor;
            Children = functor.Arity == 0 ? s_emptyList : new WamReferenceTarget[functor.Arity];
        }

        public static WamCompoundTerm Create(Functor functor)
        {
            return new WamCompoundTerm(functor);
        }

        public static WamCompoundTerm Create(CodeCompoundTerm codeCompoundTerm)
        {
            var functor = Functor.Create(codeCompoundTerm.Functor);
            var result = WamCompoundTerm.Create(functor);
            for (var index = 0; index < functor.Arity; ++index)
            {
                result.Children[index] = WamReferenceTarget.Create(codeCompoundTerm.Children[index]);
            }
            return result;
        }

        public override WamReferenceTarget Clone()
        {
            var result = new WamCompoundTerm(Functor);
            for (var index = 0; index < Functor.Arity; ++index)
            {
                result.Children[index] = Children[index].Clone();
            }
            return result;
        }

        public Functor Functor { get; private set; }

        public WamReferenceTarget[] Children { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Functor == Functor.ListFunctor)
            {
                var lhs = Children[0] == null ? "_" : Children[0].ToString();
                var rhs = Children[1] == null ? "_" : Children[1].ToString();

                sb.Append("[");
                sb.Append(lhs);
                if (rhs.StartsWith("[") && rhs.EndsWith("]"))
                {
                    if (rhs == "[]")
                    {
                        // No action required.
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(rhs.Substring(1, rhs.Length - 2));
                    }
                }
                else
                {
                    sb.Append("|");
                    sb.Append(rhs);
                }
                sb.Append("]");
            }
            else if (Functor == Functor.NilFunctor)
            {
                sb.Append("[]");
            }
            else
            {
                sb.Append(Functor.Name);

                if (Functor.Arity > 0)
                {
                    sb.Append("(");

                    string prefix = null;
                    foreach (var child in Children)
                    {
                        sb.Append(prefix); prefix = ",";

                        string value = null;
                        if (child != null)
                        {
                            value = child.ToString();
                        }

                        sb.Append(value);
                    }
                    sb.Append(")");
                }
            }
            return sb.ToString();
        }

        public override WamReferenceTarget Dereference()
        {
            return this;
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            if (Functor == Functor.ListFunctor)
            {
                var head = new List<CodeTerm>();
                CodeTerm tail = null;

                var codeTermHead = Children[0].GetCodeTerm(dereferenceType, mapping);
                var codeTermTail = Children[1].GetCodeTerm(dereferenceType, mapping);

                head.Add(codeTermHead);

                if (codeTermTail.IsCodeList)
                {
                    head.AddRange(codeTermTail.AsCodeList.Head);
                    tail = codeTermTail.AsCodeList.Tail;
                }
                else
                {
                    tail = codeTermTail;
                }
                return new CodeList(head, tail);
            }

            if (Functor.Arity == 0)
            {
                return new CodeCompoundTerm(new CodeFunctor(Functor.Name));
            }

            var children = new List<CodeTerm>();
            foreach (var child in Children)
            {
                children.Add(child.GetCodeTerm(dereferenceType, mapping));
            }
            return new CodeCompoundTerm(new CodeFunctor(Functor.Name, Functor.Arity), children);
        }
    }
}
