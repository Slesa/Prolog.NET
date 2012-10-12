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
        #region Fields

        private static WamReferenceTarget[] s_emptyList = new WamReferenceTarget[0];

        private Functor m_functor;
        private WamReferenceTarget[] m_children;

        #endregion

        #region Constructors

        private WamCompoundTerm(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            m_functor = functor;
            if (functor.Arity == 0)
            {
                m_children = s_emptyList;
            }
            else
            {
                m_children = new WamReferenceTarget[functor.Arity];
            }
        }

        public static WamCompoundTerm Create(Functor functor)
        {
            return new WamCompoundTerm(functor);
        }

        public static WamCompoundTerm Create(CodeCompoundTerm codeCompoundTerm)
        {
            Functor functor = Functor.Create(codeCompoundTerm.Functor);

            WamCompoundTerm result = WamCompoundTerm.Create(functor);

            for (int index = 0; index < functor.Arity; ++index)
            {
                result.Children[index] = WamReferenceTarget.Create(codeCompoundTerm.Children[index]);
            }

            return result;
        }

        public override WamReferenceTarget Clone()
        {
            WamCompoundTerm result = new WamCompoundTerm(Functor);

            for (int index = 0; index < Functor.Arity; ++index)
            {
                result.Children[index] = Children[index].Clone();
            }

            return result;
        }

        #endregion

        #region Public Properties

        public Functor Functor
        {
            get { return m_functor; }
        }

        public WamReferenceTarget[] Children
        {
            get { return m_children; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Functor == Functor.ListFunctor)
            {
                string lhs;
                if (Children[0] == null)
                {
                    lhs = "_";
                }
                else
                {
                    lhs = Children[0].ToString();
                }

                string rhs;
                if (Children[1] == null)
                {
                    rhs = "_";
                }
                else
                {
                    rhs = Children[1].ToString();
                }

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
                    foreach (WamReferenceTarget child in Children)
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

        #endregion

        #region Hidden Members

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            if (Functor == Functor.ListFunctor)
            {
                List<CodeTerm> head = new List<CodeTerm>();
                CodeTerm tail = null;

                CodeTerm codeTermHead = Children[0].GetCodeTerm(dereferenceType, mapping);
                CodeTerm codeTermTail = Children[1].GetCodeTerm(dereferenceType, mapping);

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
            else
            {
                if (Functor.Arity == 0)
                {
                    return new CodeCompoundTerm(new CodeFunctor(Functor.Name));
                }
                else
                {
                    List<CodeTerm> children = new List<CodeTerm>();
                    foreach (WamReferenceTarget child in Children)
                    {
                        children.Add(child.GetCodeTerm(dereferenceType, mapping));
                    }

                    return new CodeCompoundTerm(new CodeFunctor(Functor.Name, Functor.Arity), children);
                }
            }
        }

        #endregion
    }
}
