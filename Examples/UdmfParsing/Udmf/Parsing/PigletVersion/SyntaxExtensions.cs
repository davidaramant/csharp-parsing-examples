// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.PigletVersion
{
    public static class SyntaxExtensions
    {
        public static Token MustReadValueToken(this ILexer lexer)
        {
            return lexer.MustReadTokenOfTypes(
                TokenType.BooleanFalse,
                TokenType.BooleanTrue,
                TokenType.Double,
                TokenType.Integer,
                TokenType.String);
        }

        public static Identifier MustReadIdentifier(this ILexer lexer)
        {
            return new Identifier((string)lexer.MustReadTokenOfTypes(TokenType.Identifier).Value);
        }

        public static ushort MustReadUshort(this ILexer lexer)
        {
            return (ushort)lexer.MustReadTokenOfTypes(TokenType.Integer).Value;
        }

        public static Maybe<Identifier> TryAsIdentifier(this Token token)
        {
            if (token.Type == TokenType.Identifier)
            {
                return new Identifier((string)token.Value).ToMaybe();
            }
            return Maybe<Identifier>.Nothing;
        }

        public static Identifier AsIdentifier(this Token token)
        {
            return token.TryAsIdentifier().OrElse(()=> new ParsingException($"Expected '{TokenType.Identifier}' but got '{token.Type}"));
        }

        public static Maybe<string> TryAsString(this Token token)
        {
            if (token.Type == TokenType.String)
            {
                return ((string)token.Value).ToMaybe();
            }
            return Maybe<string>.Nothing;
        }

        public static string AsString(this Token token)
        {
            return token.TryAsString().OrElse(() => new ParsingException($"Expected 'string' but got '{token.Type}'"));
        }

        public static Maybe<int> TryAsInt(this Token token)
        {
            if (token.Type == TokenType.Integer)
            {
                return ((int)token.Value).ToMaybe();
            }
            return Maybe<int>.Nothing;
        }

        public static int AsInt(this Token token)
        {
            return token.TryAsInt().OrElse(() => new ParsingException($"Expected 'int' but got '{token.Type}'"));
        }

        public static Maybe<ushort> AsUshort(this Token token)
        {
            var intToken = token.TryAsInt();
            if (intToken.HasValue)
            {
                return token.TryAsInt().Select(num => (ushort)num);
            }
            throw new ParsingException("Expected ushort.");
        }

        public static Maybe<double> TryAsDouble(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Integer:
                    return ((double)(int)token.Value).ToMaybe();
                case TokenType.Double:
                    return ((double)token.Value).ToMaybe();
                default:
                    return Maybe<double>.Nothing;
            }
        }

        public static Maybe<bool> TryAsBool(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.BooleanFalse:
                    return false.ToMaybe();
                case TokenType.BooleanTrue:
                    return true.ToMaybe();
                default:
                    return Maybe<bool>.Nothing;
            }
        }

        public static Token DequeueOfType(this Queue<Token> tokenQueue, params TokenType[] types)
        {
            if (!tokenQueue.Any())
            {
                throw new ParsingException($"Expected token type of {string.Join(", ", types)}");
            }
            var token = tokenQueue.Dequeue();
            if (!types.Contains(token.Type))
            {
                throw new ParsingException($"Expected token type of {string.Join(", ", types)} but got '{token.Type}'");
            }
            return token;
        }

        public static void SetRequiredString(this Maybe<Token> maybeToken, Action<string> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new ParsingException($"{parameterName} was not set on {blockName}")).
                TryAsString().
                OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a string.")));
        }

        public static void SetRequiredDouble(this Maybe<Token> maybeToken, Action<double> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new ParsingException($"{parameterName} was not set on {blockName}")).
                TryAsDouble().
                OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a floating point value.")));
        }

        public static void SetRequiredInteger(this Maybe<Token> maybeToken, Action<int> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new ParsingException($"{parameterName} was not set on {blockName}")).
                TryAsInt().
                OrElse(() => new ParsingException($"{parameterName} in {blockName} was not an integer.")));
        }

        public static void SetRequiredBoolean(this Maybe<Token> maybeToken, Action<bool> setter, string blockName, string parameterName)
        {
            setter(
                maybeToken.
                OrElse(() => new ParsingException($"{parameterName} was not set on {blockName}")).
                TryAsBool().
                OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a boolean.")));
        }

        public static void SetOptionalString(this Maybe<Token> maybeToken, Action<string> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsString().
                    OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a string.")))
                .Do(setter);
        }

        public static void SetOptionalDouble(this Maybe<Token> maybeToken, Action<double> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsDouble().
                    OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a floating point value.")))
                .Do(setter);
        }

        public static void SetOptionalInteger(this Maybe<Token> maybeToken, Action<int> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsInt().
                    OrElse(() => new ParsingException($"{parameterName} in {blockName} was not an integer.")))
                .Do(setter);
        }

        public static void SetOptionalBoolean(this Maybe<Token> maybeToken, Action<bool> setter, string blockName, string parameterName)
        {
            maybeToken.
                Select(token =>
                    token.TryAsBool().
                    OrElse(() => new ParsingException($"{parameterName} in {blockName} was not a boolean.")))
                .Do(setter);
        }
    }
}