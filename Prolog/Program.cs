/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using Prolog.Code;

namespace Prolog
{
    public sealed class Program : INotifyPropertyChanged
    {
        string _fileName;
        bool _isModified;
        bool _isOptimized;

        public Program()
        {
            _fileName = null;
            _isModified = false;

            Procedures = new ProgramProcedureList(this, new ObservableCollection<Procedure>());
            _isOptimized = false;

            Libraries = LibraryList.Create();
            Libraries.Add(Library.Standard);
        }

        static Program()
        {
            PragmaOptimizeFunctor = new CodeFunctor(PragmaOptimizeFunctorName, 0);
            PragmaOptimizeFunctorName = "optimize";
        }

        public static Program Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            var program = new Program();
            program.Read(fileName);

            program.FileName = fileName;
            program.IsModified = false;

            return program;
        }

        public static string PragmaOptimizeFunctorName { get; private set; }
        public static CodeFunctor PragmaOptimizeFunctor { get; private set; }
        public ProgramProcedureList Procedures { get; private set; }

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                if (value == string.Empty)
                {
                    value = null;
                }

                if (value != _fileName)
                {
                    _fileName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("FileName"));
                }
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
            private set
            {
                if (value != _isModified)
                {
                    _isModified = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsModified"));
                }
            }
        }

        public bool IsOptimized
        {
            get { return _isOptimized; }
            set
            {
                if (value != _isOptimized)
                {
                    _isOptimized = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsOptimized"));

                    foreach (var procedure in Procedures)
                    {
                        procedure.InvalidateInstructionStream();
                    }
                    Touch();
                }
            }
        }

        public LibraryList Libraries { get; private set; }

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

            var functor = Functor.Create(codeSentence.Head.Functor);
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
            var functor = Functor.Create(codeSentence.Head.Functor);
            if (functor == Functor.PragmaFunctor)
            {
                ProcessPragma(codeSentence);
                return null;
            }

            // Find procedure associated with codeSentence.  Create a new procedure if necessary.
            //
            Procedure procedure;
            if (!Procedures.TryGetProcedure(functor, out procedure))
            {
                procedure = Procedures.Add(functor);
            }

            // Create clause for codeSentence.
            //
            var clause = procedure.Clauses.Add(codeSentence);
            return clause;

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

        public event PropertyChangedEventHandler PropertyChanged;

        void Read(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var text = reader.ReadToEnd();
                var codeSentences = Parser.Parse(text);
                if (codeSentences == null)
                {
                    throw new ApplicationException("Source file is empty or cannot be processed.");
                }
                foreach (var codeSentence in codeSentences)
                {
                    Add(codeSentence);
                }
            }
        }

        void Write(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                string prefix = null;
                if (IsOptimized)
                {
                    writer.Write(prefix); prefix = Environment.NewLine + Environment.NewLine;
                    writer.WriteLine("pragma(optimize,true).");
                }
                foreach (var procedure in Procedures)
                {
                    writer.Write(prefix); prefix = Environment.NewLine + Environment.NewLine;
                    foreach (var clause in procedure.Clauses)
                    {
                        writer.WriteLine(clause.ToString());
                    }
                }
            }
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        void ProcessPragma(CodeSentence codeSentence)
        {
            if (codeSentence.Body.Count != 0)
            {
                return;
            }
            var pragma = codeSentence.Head;
            var pragmaName = pragma.Children[0].AsCodeCompoundTerm;
            var pragmaArgument = pragma.Children[1];
            if (pragmaName == null || pragmaArgument == null) return;
            if (pragmaName.Functor == PragmaOptimizeFunctor)
            {
                ProcessOptimizePragma(pragmaArgument);
            }
        }

        void ProcessOptimizePragma(CodeTerm pragmaArgument)
        {
            var pragmaValue = pragmaArgument.AsCodeValue;
            if (pragmaValue == null) return;
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
}
