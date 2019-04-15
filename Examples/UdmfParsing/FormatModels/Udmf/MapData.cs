// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using Pidgin;
using Superpower;

namespace SectorDirector.Core.FormatModels.Udmf
{
    public sealed partial class MapData
    {
        public static MapData LoadFromUsingPidgin(TextReader reader)
        {
            var result = Parsing.PidginVersion.UdmfParser.TranslationUnit.ParseOrThrow(reader);
            return Parsing.PidginVersion.UdmfSemanticAnalyzer.Process(result);
        }

        public static MapData LoadFromUsingPidgin(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFromUsingPidgin(textReader);
            }
        }

        
        public static MapData LoadFromUsingSuperpower(TextReader reader)
        {
            var text = reader.ReadToEnd();

            var lexerResult = Parsing.SuperpowerVersion.UdmfTokenizerRules.Tokenizer.Tokenize(text);
            var result = Parsing.SuperpowerVersion.UdmfParser.GlobalExpressions.Parse(lexerResult);

            return Parsing.SuperpowerVersion.UdmfSemanticAnalyzer.Process(result);
        }

        public static MapData LoadFromUsingSuperpower(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFromUsingSuperpower(textReader);
            }
        }

        public static MapData LoadFromUsingPiglet(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new Parsing.PigletVersion.UdmfSyntaxAnalyzer();
                return Parsing.PigletVersion.UdmfParser.Parse(sa.Analyze(new Parsing.PigletVersion.UdmfLexer(textReader)));
            }
        }

        public static MapData LoadFromUsingHime(TextReader reader)
        {
            var lexer = new Parsing.HimeVersion.UdmfLexer(reader);
            var parser = new Parsing.HimeVersion.UdmfParser(lexer);
            return Parsing.HimeVersion.UdmfSemanticAnalyzer.Process(parser.Parse());
        }

        public static MapData LoadFromUsingHime(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFromUsingHime(textReader);
            }
        }
    }
}