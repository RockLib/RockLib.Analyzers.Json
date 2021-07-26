using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RockLib.Analyzers.Json
{
    public class EscapedStringSyntax : StringSyntax
    {
        private readonly Lazy<string> _stringValue;

        public EscapedStringSyntax(IEnumerable<char> rawValue)
            : this(rawValue, null, null)
        {
        }

        public EscapedStringSyntax(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(rawValue, leadingTrivia, trailingTrivia)
        {
            _stringValue = new Lazy<string>(() => GetStringValue(rawValue));
        }

        public override string Value => _stringValue.Value;

        public EscapedStringSyntax WithValue(string value) =>
            new EscapedStringSyntax(Escape(value), LeadingTrivia, TrailingTrivia);

        public EscapedStringSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new EscapedStringSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

        public EscapedStringSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new EscapedStringSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

        public EscapedStringSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new EscapedStringSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

        public EscapedStringSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new EscapedStringSyntax(RawValue, leadingTrivia, TrailingTrivia);

        public EscapedStringSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new EscapedStringSyntax(RawValue, LeadingTrivia, trailingTrivia);

        protected override JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList) =>
            WithLeadingTrivia(triviaList);

        protected override JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList) =>
            WithTrailingTrivia(triviaList);

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (LeadingTrivia != null)
            {
                var replacementLeadingTrivia = LeadingTrivia.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementLeadingTrivia, LeadingTrivia))
                    return WithLeadingTrivia(replacementLeadingTrivia);
            }

            if (TrailingTrivia != null)
            {
                var replacementTrailingTrivia = TrailingTrivia.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementTrailingTrivia, TrailingTrivia))
                    return WithTrailingTrivia(replacementTrailingTrivia);
            }

            return this;
        }

        private static string GetStringValue(IEnumerable<char> value)
        {
            var sb = new StringBuilder(); // TODO: Use object pool
            var enumerator = value.GetEnumerator();

            // Skip the opening quote
            enumerator.MoveNext();

            if (enumerator.Current != '"')
                throw new Exception("Missing open quote.");

            var closeQuoteFound = false;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == '\\')
                {
                    enumerator.MoveNext();

                    switch (enumerator.Current)
                    {
                        default:
                        case '"':
                        case '\\':
                        case '/':
                            sb.Append(enumerator.Current);
                            break;
                        case 'b':
                            sb.Append('\b');
                            break;
                        case 'f':
                            sb.Append('\f');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case 'u':
                            var c = new StringBuilder(); // TODO: Use object pool

                            enumerator.MoveNext();
                            if (!IsHexDigit(enumerator.Current))
                                throw new Exception("Invalid hex digit: " + enumerator.Current);
                            c.Append(enumerator.Current);

                            enumerator.MoveNext();
                            if (!IsHexDigit(enumerator.Current))
                                throw new Exception("Invalid hex digit: " + enumerator.Current);
                            c.Append(enumerator.Current);

                            enumerator.MoveNext();
                            if (!IsHexDigit(enumerator.Current))
                                throw new Exception("Invalid hex digit: " + enumerator.Current);
                            c.Append(enumerator.Current);

                            enumerator.MoveNext();
                            if (!IsHexDigit(enumerator.Current))
                                throw new Exception("Invalid hex digit: " + enumerator.Current);
                            c.Append(enumerator.Current);

                            sb.Append((char)int.Parse(c.ToString(), NumberStyles.HexNumber));
                            break;
                    }
                }
                else if (enumerator.Current == '"')
                {
                    closeQuoteFound = true;
                    break;
                }
                else
                    sb.Append(enumerator.Current);
            }

            if (!closeQuoteFound)
                throw new Exception("Missing close quote.");

            return sb.ToString();
        }

        private static bool IsHexDigit(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                    return true;
                default:
                    return false;
            }
        }

        private static string Escape(string value)
        {
            var sb = new StringBuilder(); // TODO: Use object pool

            foreach (var c in value)
            {
                switch (c)
                {
                    default:
                        sb.Append(c);
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
