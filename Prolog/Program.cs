/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using Prolog.Code;
using Prolog.Grammar;

namespace Prolog
{
    public sealed class Program : INotifyPropertyChanged
    {
        #region Fields

        private static string s_pragmaOptimizeFunctorName = "optimize";

        private static CodeFunctor s_pragmaOptimizeFunctor = new CodeFunctor(PragmaOptimizeFunctorName, 0);

        private string m_fileName;
        private bool m_isModified;

        private ProgramProcedureList m_procedures;
        private bool m_isOptimized;

        private LibraryList m_libraries;

        #endregion

        #region Constructors

        public Program()
        {
            m_fileName = null;
            m_isModified = false;

            m_procedures = new ProgramProcedureList(this, new ObservableCollection<Procedure>());
            m_isOptimized = false;

            m_libraries = LibraryList.Create();
            m_libraries.Add(Library.Standard);
        }

        public static Program Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            Program program = new Program();
            program.Read(fileName);

            program.FileName = fileName;
            program.IsModified = false;

            return program;
        }

        #endregion

        #region Public Properties

        public static string PragmaOptimizeFunctorName
        {
            get { return s_pragmaOptimizeFunctorName; }
        }

        public static CodeFunctor PragmaOptimizeFunctor
        {
            get { return s_pragmaOptimizeFunctor; }
        }

        public ProgramProcedureList Procedures
        {
            get { return m_procedures; }
        }

        public string FileName
        {
            get { return m_fileName; }
            private set
            {
                if (value == string.Empty)
                {
                    value = null;
                }

                if (value != m_fileName)
                {
                    m_fileName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("FileName"));
                }
            }
        }

        public bool IsModified
        {
            get { return m_isModified; }
            private set
            {
                if (value != m_isModified)
                {
                    m_isModified = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsModified"));
                }
            }
        }

        public bool IsOptimized
        {
            get { return m_isOptimized; }
            set
            {
                if (value != m_isOptimized)
                {
                    m_isOptimized = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsOptimized"));

                    foreach (Procedure procedure in Procedures)
                    {
                        procedure.InvalidateInstructionStream();
                    }

                    Touch();
                }
            }
        }

        public LibraryList Libraries
        {
            get { return m_libraries; }
        }

        #endregion

        #region Public Methods

        public bool Contains(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }
            if (codeSentence.Head == null)
            {
                throw new ArgumentException("Program cannot contain query.", "codeSentence");
            }

            Functor functor = Functor.Create(codeSentence.Head.Functor);
            Procedure procedure;
            if (Procedures.TryGetProcedure(functor, out procedure))
            {
                if (procedure.Clauses.Contains(codeSentence))
                {
                    return true;
                }
            }

            return false;
        }

        public Clause Add(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }
            if (codeSentence.Head == null)
            {
                throw new ArgumentException("Query cannot be added to program.", "codeSentence");
            }

            Functor functor = Functor.Create(codeSentence.Head.Functor);

            if (functor == Functor.PragmaFunctor)
            {
                ProcessPragma(codeSentence);

                return null;
            }
            else
            {
                // Find procedure associated with codeSentence.  Create a new procedure if necessary.
                //
                Procedure procedure;
                if (!Procedures.TryGetProcedure(functor, out procedure))
                {
                    procedure = Procedures.Add(functor);
                }

                // Create clause for codeSentence.
                //
                Clause clause = procedure.Clauses.Add(codeSentence);

                return clause;
            }
        }

        public void Touch()
        {
            IsModified = true;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("File name not specified.");
            }

            Write(FileName);

            IsModified = false;
        }

        public void SaveAs(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            Write(fileName);

            FileName = fileName;
            IsModified = false;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private void Read(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string text = reader.ReadToEnd();

                CodeSentence[] codeSentences = Parser.Parse(text);

                if (codeSentences == null)
                {
                    throw new ApplicationException("Source file is empty or cannot be processed.");
                }

                foreach (CodeSentence codeSentence in codeSentences)
                {
                    Add(codeSentence);
                }
            }
        }

        private void Write(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                string prefix = null;

                if (IsOptimized)
                {
                    writer.Write(prefix); prefix = Environment.NewLine + Environment.NewLine;

                    writer.WriteLine("pragma(optimize,true).");
                }

                foreach (Procedure procedure in Procedures)
                {
                    writer.Write(prefix); prefix = Environment.NewLine + Environment.NewLine;

                    foreach (Clause clause in procedure.Clauses)
                    {
                        writer.WriteLine(clause.ToString());
                    }
                }
            }
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private void ProcessPragma(CodeSentence codeSentence)
        {
            if (codeSentence.Body.Count != 0)
            {
                return;
            }

            CodeCompoundTerm pragma = codeSentence.Head;

            CodeCompoundTerm pragmaName = pragma.Children[0].AsCodeCompoundTerm;
            CodeTerm pragmaArgument = pragma.Children[1];
            if (pragmaName != null
                && pragmaArgument != null)
            {
                if (pragmaName.Functor == PragmaOptimizeFunctor)
                {
                    ProcessOptimizePragma(pragmaArgument);
                }
            }
        }

        private void ProcessOptimizePragma(CodeTerm pragmaArgument)
        {
            CodeValue pragmaValue = pragmaArgument.AsCodeValue;
            if (pragmaValue != null)
            {
                if (pragmaValue.Object.Equals(true))
                {
                    IsOptimized = true;
                }
                if (pragmaValue.Object.Equals(false))
                {
                    IsOptimized = false;
                }
            }
        }

        #endregion
    }
}
