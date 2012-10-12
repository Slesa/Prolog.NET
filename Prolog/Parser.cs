/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Reflection;

using Lingua;
using Prolog.Code;
using Prolog.Grammar;

namespace Prolog
{
    /// <summary>
    /// Represents a parser that recognizes Prolog source code text.
    /// </summary>
    public sealed class Parser
    {
        #region Fields

        private static Parser s_singleton;

        private IGrammar m_grammar;
        private ITerminalReader m_terminalReader;
        private IParser m_parser;

        #endregion

        #region Constructors

        private Parser()
        {
            Lingua.Grammar grammar = new Lingua.Grammar();
            grammar.Load(Assembly.GetCallingAssembly(), "Prolog");
            grammar.LoadRules(Assembly.GetCallingAssembly(), "Prolog");
            grammar.Resolve();
            m_grammar = grammar;

            TerminalReaderGenerator terminalReaderGenerator = new TerminalReaderGenerator();
            TerminalReaderGeneratorResult terminalReaderGeneratorResult = terminalReaderGenerator.GenerateTerminalReader(m_grammar);
            m_terminalReader = terminalReaderGeneratorResult.TerminalReader;

            ParserGenerator parserGenerator = new ParserGenerator();
            ParserGeneratorResult parserGeneratorResult = parserGenerator.GenerateParser(m_grammar);
            m_parser = parserGeneratorResult.Parser;
        }

        public static Parser Singleton
        {
            get
            {
                if (s_singleton == null)
                {
                    s_singleton = new Parser();
                }

                return s_singleton;
            }
        }

        #endregion

        #region Public Properties

        public IGrammar Grammar
        {
            get { return m_grammar; }
        }

        #endregion

        #region Public Methods

        public static CodeSentence[] Parse(string text)
        {
            return Singleton.ParseText(text);
        }

        #endregion

        #region Internal Methods

        internal CodeSentence[] ParseText(string text)
        {
            m_terminalReader.Open(text);

            Grammar.Program program = (Grammar.Program)m_parser.Parse(m_terminalReader);
            if (program == null)
            {
                return null;
            }

            return program.CodeSentences.ToArray();
        }

        #endregion
    }
}
