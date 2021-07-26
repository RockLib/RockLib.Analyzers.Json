using System;
using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public static class JsonDocument
    {
        public static JsonSyntaxNode Parse(string json)
        {
            var reader = new JsonReader(json);

            JsonSyntaxNode rootNode = null;
            var localTrivia = new List<TriviaSyntaxNode>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.Whitespace:
                        localTrivia.Add(new WhitespaceTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.SingleLineComment:
                        localTrivia.Add(new SingleLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.MultiLineComment:
                        localTrivia.Add(new MultiLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.ArrayStart:
                        ArraySyntax arraySyntax = ParseArray(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = arraySyntax;
                        break;
                    case JsonTokenType.ObjectStart:
                        ObjectSyntax objectSyntax = ParseObject(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = objectSyntax;
                        break;
                    case JsonTokenType.NonEscapedString:
                        var nonEscapedString = ParseNonEscapedStringSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = nonEscapedString;
                        break;
                    case JsonTokenType.EscapedString:
                        var escapedString = ParseEscapedStringSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = escapedString;
                        break;
                    case JsonTokenType.Number:
                        var number = ParseNumberSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = number;
                        break;
                    case JsonTokenType.True:
                        var trueSyntax = ParseTrueSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = trueSyntax;
                        break;
                    case JsonTokenType.False:
                        var falseSyntax = ParseFalseSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = falseSyntax;
                        break;
                    case JsonTokenType.Null:
                        var nullSyntax = ParseNullSyntax(reader, localTrivia);

                        if (rootNode is null)
                            rootNode = nullSyntax;
                        break;
                }
            }

            if (localTrivia.Count > 0)
                rootNode = rootNode.WithTrailingTrivia(localTrivia);

            return rootNode;
        }

        private static NonEscapedStringSyntax ParseNonEscapedStringSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            NonEscapedStringSyntax nonEscapedString;
            if (localTrivia.Count > 0)
            {
                nonEscapedString = new NonEscapedStringSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                nonEscapedString = new NonEscapedStringSyntax(reader.Current);
            return nonEscapedString;
        }

        private static EscapedStringSyntax ParseEscapedStringSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            EscapedStringSyntax escapedString;
            if (localTrivia.Count > 0)
            {
                escapedString = new EscapedStringSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                escapedString = new EscapedStringSyntax(reader.Current);
            return escapedString;
        }

        private static NumberSyntax ParseNumberSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            NumberSyntax number;
            if (localTrivia.Count > 0)
            {
                number = new NumberSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                number = new NumberSyntax(reader.Current);
            return number;
        }

        private static TrueSyntax ParseTrueSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            TrueSyntax trueSyntax;
            if (localTrivia.Count > 0)
            {
                trueSyntax = new TrueSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                trueSyntax = new TrueSyntax(reader.Current);
            return trueSyntax;
        }

        private static FalseSyntax ParseFalseSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            FalseSyntax falseSyntax;
            if (localTrivia.Count > 0)
            {
                falseSyntax = new FalseSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                falseSyntax = new FalseSyntax(reader.Current);
            return falseSyntax;
        }

        private static NullSyntax ParseNullSyntax(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            NullSyntax nullSyntax;
            if (localTrivia.Count > 0)
            {
                nullSyntax = new NullSyntax(reader.Current, localTrivia, null);
                localTrivia.Clear();
            }
            else
                nullSyntax = new NullSyntax(reader.Current);
            return nullSyntax;
        }

        private static ArraySyntax ParseArray(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            var openBracket = GetOpenBracket(localTrivia);

            var items = new List<ArrayItemSyntax>();
            JsonSyntaxNode currentItem = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    default:
                        if (currentItem is null)
                            throw new Exception("Parse error. Expected array item, but was unexpected token:" + reader.CurrentString);
                        throw new Exception("Parse error. Expected comma, but was unexpected token:" + reader.CurrentString);
                    case JsonTokenType.Whitespace:
                        localTrivia.Add(new WhitespaceTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.SingleLineComment:
                        localTrivia.Add(new SingleLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.MultiLineComment:
                        localTrivia.Add(new MultiLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.ItemSeparator:
                        if (currentItem is null)
                            throw new Exception("Parse error. Expected array item, but was comma.");

                        items.Add(new ArrayItemSyntax(currentItem, GetComma(localTrivia)));
                        currentItem = null;
                        break;
                    case JsonTokenType.ArrayEnd:
                        if (currentItem != null)
                            items.Add(new ArrayItemSyntax(currentItem));

                        return new ArraySyntax(openBracket, items, GetCloseBracket(localTrivia));
                    case JsonTokenType.ArrayStart:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was open bracket.");

                        currentItem = ParseArray(reader, localTrivia);
                        break;
                    case JsonTokenType.ObjectStart:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was open brace.");

                        currentItem = ParseObject(reader, localTrivia);
                        break;
                    case JsonTokenType.NonEscapedString:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was non-escaped string.");

                        currentItem = ParseNonEscapedStringSyntax(reader, localTrivia);
                        break;
                    case JsonTokenType.EscapedString:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was escaped string.");

                        currentItem = ParseEscapedStringSyntax(reader, localTrivia);
                        break;
                    case JsonTokenType.Number:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was number.");

                        currentItem = ParseNumberSyntax(reader, localTrivia);
                        break;
                    case JsonTokenType.True:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was 'true'.");

                        currentItem = ParseTrueSyntax(reader, localTrivia);
                        break;
                    case JsonTokenType.False:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was 'false'.");

                        currentItem = ParseFalseSyntax(reader, localTrivia);
                        break;
                    case JsonTokenType.Null:
                        if (currentItem != null)
                            throw new Exception("Parse error. Expected comma, but was 'null'.");

                        currentItem = ParseNullSyntax(reader, localTrivia);
                        break;
                }
            }

            throw new Exception("Parse error. Expected close bracket but was never found.");
        }

        private static ObjectSyntax ParseObject(JsonReader reader, List<TriviaSyntaxNode> localTrivia)
        {
            var openBrace = GetOpenBrace(localTrivia);

            var members = new List<ObjectMemberSyntax>();

            StringSyntax memberNameSyntax = null;
            ColonSyntax colonSyntax = null;
            JsonSyntaxNode memberValueSyntax = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    default:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was unexpected token:" + reader.CurrentString);
                        if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was unexpected token:" + reader.CurrentString);
                        if (memberValueSyntax is null)
                            throw new Exception("Parse error. Expected member value, but was unexpected token:" + reader.CurrentString);
                        throw new Exception("Parse error. Expected comma, but was unexpected token:" + reader.CurrentString);
                    case JsonTokenType.Whitespace:
                        localTrivia.Add(new WhitespaceTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.SingleLineComment:
                        localTrivia.Add(new SingleLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.MultiLineComment:
                        localTrivia.Add(new MultiLineCommentTriviaSyntax(reader.Current));
                        break;
                    case JsonTokenType.ItemSeparator:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was comma.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was comma.");
                        else if (memberValueSyntax is null)
                            throw new Exception("Parse error. Expected member value, but was comma.");

                        members.Add(new ObjectMemberSyntax(memberNameSyntax, colonSyntax, memberValueSyntax, GetComma(localTrivia)));
                        memberNameSyntax = null;
                        colonSyntax = null;
                        memberValueSyntax = null;
                        break;
                    case JsonTokenType.ObjectEnd:
                        if (memberNameSyntax != null)
                        {
                            if (colonSyntax is null)
                                throw new Exception("Parse error. Expected colon, but was close brace.");
                            else if (memberValueSyntax is null)
                                throw new Exception("Parse error. Expected member value, but was close brace.");
                            else
                                members.Add(new ObjectMemberSyntax(memberNameSyntax, colonSyntax, memberValueSyntax));
                        }

                        return new ObjectSyntax(openBrace, members, GetCloseBrace(localTrivia));
                    case JsonTokenType.NonEscapedString:
                        if (memberNameSyntax is null)
                            memberNameSyntax = ParseNonEscapedStringSyntax(reader, localTrivia);
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was escaped string.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseNonEscapedStringSyntax(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was escaped string.");
                        break;
                    case JsonTokenType.EscapedString:
                        if (memberNameSyntax is null)
                            memberNameSyntax = ParseEscapedStringSyntax(reader, localTrivia);
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was non-escaped string.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseEscapedStringSyntax(reader, localTrivia);
                        else throw new Exception("Parse error. Expected comma, but was non-escaped string.");
                        break;
                    case JsonTokenType.MemberSeparator:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was colon.");
                        else if (colonSyntax is null)
                            colonSyntax = GetColon(localTrivia);
                        else if (memberValueSyntax is null)
                            throw new Exception("Parse error. Expected member value, but was colon.");
                        else
                            throw new Exception("Parse error. Expected comma, but was colon.");
                        break;
                    case JsonTokenType.ArrayStart:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseArray(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                    case JsonTokenType.ObjectStart:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseObject(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                    case JsonTokenType.Number:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseNumberSyntax(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                    case JsonTokenType.True:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseTrueSyntax(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                    case JsonTokenType.False:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseFalseSyntax(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                    case JsonTokenType.Null:
                        if (memberNameSyntax is null)
                            throw new Exception("Parse error. Expected member name, but was open bracket.");
                        else if (colonSyntax is null)
                            throw new Exception("Parse error. Expected colon, but was open bracket.");
                        else if (memberValueSyntax is null)
                            memberValueSyntax = ParseNullSyntax(reader, localTrivia);
                        else
                            throw new Exception("Parse error. Expected comma, but was open bracket.");
                        break;
                }
            }

            throw new Exception("Parse error. Expected close brace but was never found.");
        }

        private static CommaSyntax GetComma(List<TriviaSyntaxNode> localTrivia)
        {
            CommaSyntax comma;
            if (localTrivia.Count > 0)
            {
                comma = new CommaSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                comma = new CommaSyntax();
            return comma;
        }

        private static ColonSyntax GetColon(List<TriviaSyntaxNode> localTrivia)
        {
            ColonSyntax colon;
            if (localTrivia.Count > 0)
            {
                colon = new ColonSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                colon = new ColonSyntax();
            return colon;
        }

        private static OpenBracketSyntax GetOpenBracket(List<TriviaSyntaxNode> localTrivia)
        {
            OpenBracketSyntax openBracket;
            if (localTrivia.Count > 0)
            {
                openBracket = new OpenBracketSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                openBracket = new OpenBracketSyntax();
            return openBracket;
        }

        private static CloseBracketSyntax GetCloseBracket(List<TriviaSyntaxNode> localTrivia)
        {
            CloseBracketSyntax closeBracket;
            if (localTrivia.Count > 0)
            {
                closeBracket = new CloseBracketSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                closeBracket = new CloseBracketSyntax();
            return closeBracket;
        }

        private static OpenBraceSyntax GetOpenBrace(List<TriviaSyntaxNode> localTrivia)
        {
            OpenBraceSyntax openBrace;
            if (localTrivia.Count > 0)
            {
                openBrace = new OpenBraceSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                openBrace = new OpenBraceSyntax();
            return openBrace;
        }

        private static CloseBraceSyntax GetCloseBrace(List<TriviaSyntaxNode> localTrivia)
        {
            CloseBraceSyntax closeBrace;
            if (localTrivia.Count > 0)
            {
                closeBrace = new CloseBraceSyntax(localTrivia, null);
                localTrivia.Clear();
            }
            else
                closeBrace = new CloseBraceSyntax();
            return closeBrace;
        }
    }
}
