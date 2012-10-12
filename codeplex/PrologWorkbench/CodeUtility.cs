/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.IO;

using Prolog.Code;

namespace Prolog.Workbench
{
    public static class CodeUtility
    {
        #region Public Methods

        public static void Write(CodeSentence codeSentence, int indentation, TextWriter wtr)
        {
            wtr.WriteLine("{0}CodeSentence", Indentation(indentation));
            Write(codeSentence.Head, indentation+1, wtr);
            foreach (CodeCompoundTerm item in codeSentence.Body)
            {
                Write(item, indentation + 1, wtr);
            }

        }

        public static void Write(CodeTerm codeTerm, int indentation, TextWriter wtr)
        {
            CodeCompoundTerm codeCompoundTerm = codeTerm as CodeCompoundTerm;
            if (codeCompoundTerm != null)
            {
                Write(codeCompoundTerm, indentation, wtr);
                return;
            }

            CodeVariable codeVariable = codeTerm as CodeVariable;
            if (codeVariable != null)
            {
                Write(codeVariable, indentation, wtr);
                return;
            }

            CodeValue codeValue = codeTerm as CodeValue;
            if (codeValue != null)
            {
                Write(codeValue, indentation, wtr);
                return;
            }
        }

        public static void Write(CodeCompoundTerm codeCompoundTerm, int indentation, TextWriter wtr)
        {
            wtr.WriteLine("{0}{1}/{2} - CodeCompoundTerm", Indentation(indentation), codeCompoundTerm.Functor.Name, codeCompoundTerm.Functor.Arity);
            foreach (CodeTerm codeTerm in codeCompoundTerm.Children)
            {
                Write(codeTerm, indentation + 1, wtr);
            }
        }

        public static void Write(CodeVariable codeVariable, int indentation, TextWriter wtr)
        {
            wtr.WriteLine("{0}{1} - CodeVariable", Indentation(indentation), codeVariable.Name);
        }

        public static void Write(CodeValue codeValue, int indentation, TextWriter wtr)
        {
            wtr.WriteLine("{0}{1} - CodeValue", Indentation(indentation), codeValue.ToString());
        }

        #endregion

        #region Hidden Members

        public static string Indentation(int indentation)
        {
            return new string(' ', indentation * 3);
        }

        #endregion
    }
}
