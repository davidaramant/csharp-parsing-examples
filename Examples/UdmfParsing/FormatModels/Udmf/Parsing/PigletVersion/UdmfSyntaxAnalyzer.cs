// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PigletVersion
{
    public sealed class UdmfSyntaxAnalyzer
    {
        public UdmfSyntaxTree Analyze(ILexer lexer)
        {
            var globalAssignments = new List<Assignment>();
            var blocks = new List<Block>();
            var arrayBlocks = new List<ArrayBlock>();

            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.EndOfFile);

                if (idToken.Type == TokenType.EndOfFile) break;

                var name = idToken.TryAsIdentifier().Value;

                var nextToken = lexer.MustReadTokenOfTypes(TokenType.Equal, TokenType.OpenParen);
                if (nextToken.Type == TokenType.Equal)
                {
                    globalAssignments.Add(ParseGlobalAssignment(name, lexer));
                }
                else // Must be a block
                {
                    var firstBlockToken =
                        lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.OpenParen, TokenType.CloseParen);

                    switch (firstBlockToken.Type)
                    {
                        case TokenType.Identifier:
                            var assignmentName = firstBlockToken.TryAsIdentifier().Value;
                            var assignment = ParseAssignment(assignmentName, lexer);
                            blocks.Add(ParseBlock(name, assignment, lexer));
                            break;

                        case TokenType.CloseParen:
                            blocks.Add(new Block(name, Enumerable.Empty<Assignment>()));
                            break;

                        case TokenType.OpenParen:
                            arrayBlocks.Add(ParseArrayBlock(name, lexer));
                            break;

                        default:
                            throw new ParsingException("This can't happen.");
                    }
                }
            }
            return new UdmfSyntaxTree(globalAssignments, blocks, arrayBlocks);
        }

        private static ArrayBlock ParseArrayBlock(Identifier blockName, ILexer lexer)
        {
            var tuples = new List<IReadOnlyList<int>>
            {
                ReadTuple(lexer)
            };

            while (lexer.MustReadTokenOfTypes(TokenType.Comma, TokenType.CloseParen).Type == TokenType.Comma)
            {
                lexer.MustReadTokenOfTypes(TokenType.OpenParen);
                tuples.Add(ReadTuple(lexer));
            }

            return new ArrayBlock(blockName, tuples);
        }

        private static IReadOnlyList<int> ReadTuple(ILexer lexer)
        {
            var tuple = new List<int>();
            // This assumes the opening paren has already been read.

            var firstToken = lexer.MustReadTokenOfTypes(TokenType.Integer, TokenType.CloseParen);
            if (firstToken.Type == TokenType.CloseParen) return tuple;

            tuple.Add(firstToken.TryAsInt().Value);

            while (true)
            {
                var token = lexer.MustReadTokenOfTypes(TokenType.Comma, TokenType.CloseParen);
                if (token.Type == TokenType.CloseParen) return tuple;

                tuple.Add(lexer.MustReadTokenOfTypes(TokenType.Integer).TryAsInt().Value);
            }
        }

        public static Block ParseBlock(Identifier blockName, Assignment firstAssigment, ILexer lexer)
        {
            var assignments = new List<Assignment> { firstAssigment };

            while (true)
            {
                var token = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (token.Type == TokenType.CloseParen)
                {
                    return new Block(blockName, assignments);
                }
                else
                {
                    var assignmentName = token.TryAsIdentifier().Value;
                    assignments.Add(ParseAssignment(assignmentName, lexer));
                }
            }
        }

        public static Assignment ParseAssignment(Identifier name, ILexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.Equal);

            return ParseGlobalAssignment(name, lexer);
        }

        private static Assignment ParseGlobalAssignment(Identifier name, ILexer lexer)
        {
            var valueToken = lexer.MustReadValueToken();

            lexer.MustReadTokenOfTypes(TokenType.Semicolon);

            return new Assignment(name, valueToken);
        }
    }
}