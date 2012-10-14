/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Reflection;

using Lingua;
using Prolog.Code;

namespace Prolog
{
    /// <summary>
    /// Represents a parser that recognizes Prolog source code text.
    /// </summary>
    public sealed class Parser
    {
        static Parser _singleton;
        readonly ITerminalReader _terminalReader;
        readonly IParser _parser;

        Parser()
        {
            var grammar = new Lingua.Grammar();
            grammar.Load(Assembly.GetCallingAssembly(), "Prolog");
            grammar.LoadRules(Assembly.GetCallingAssembly(), "Prolog");
            grammar.Resolve();
            Grammar = grammar;

            var terminalReaderGenerator = new TerminalReaderGenerator();
            var terminalReaderGeneratorResult = terminalReaderGenerator.GenerateTerminalReader(Grammar);
            _terminalReader = terminalReaderGeneratorResult.TerminalReader;

            var parserGenerator = new ParserGenerator();
            var parserGeneratorResult = parserGenerator.GenerateParser(Grammar);
            _parser = parserGeneratorResult.Parser;
        }

        public static Parser Singleton
        {
            get { return _singleton ?? (_singleton = new Parser()); }
        }

        public IGrammar Grammar { get; private set; }

        public static CodeSentence[] Parse(string text)
        {
            return Singleton.ParseText(text);
        }

        internal CodeSentence[] ParseText(string text)
        {
            _terminalReader.Open(text);

            var program = (Grammar.Program)_parser.Parse(_terminalReader);
            return program == null ? null : program.CodeSentences.ToArray();
        }
    }
}
