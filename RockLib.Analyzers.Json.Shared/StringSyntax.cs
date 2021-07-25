using System;
using System.Collections.Generic;
using System.Text;

namespace RockLib.Analyzers
{
#if !PUBLIC
    partial class Json
    {
#endif
#if PUBLIC
        public
#else
        internal
#endif
        class StringSyntax : VerbatimSyntaxNode
        {
            private readonly Lazy<string> _stringValue;

            public StringSyntax(IEnumerable<char> rawValue)
                : this(rawValue, null, null)
            {
            }

            public StringSyntax(IEnumerable<char> rawValue,
                TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(rawValue, leadingTrivia, trailingTrivia)
            {
                _stringValue = new Lazy<string>(() => GetStringValue(rawValue));
            }

            public string Value => _stringValue.Value;

            public StringSyntax WithValue(string value) =>
                new StringSyntax(Escape(value), LeadingTrivia, TrailingTrivia);

            public StringSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new StringSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

            public StringSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new StringSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

            public StringSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new StringSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

            public StringSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new StringSyntax(RawValue, leadingTrivia, TrailingTrivia);

            public StringSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new StringSyntax(RawValue, LeadingTrivia, trailingTrivia);

            protected override JsonSyntaxNode Replace(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
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

                // Skip the opening double quote
                enumerator.MoveNext();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == '\\')
                    {
                        enumerator.MoveNext();

                        switch (enumerator.Current)
                        {
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
                                c.Append("\\u");
                                enumerator.MoveNext();
                                c.Append(enumerator.Current);
                                enumerator.MoveNext();
                                c.Append(enumerator.Current);
                                enumerator.MoveNext();
                                c.Append(enumerator.Current);
                                enumerator.MoveNext();
                                c.Append(enumerator.Current);
                                sb.Append(char.Parse(c.ToString()));
                                break;
                        }
                    }
                    else if (enumerator.Current == '"')
                        break;
                    else
                        sb.Append(enumerator.Current);
                }

                return sb.ToString();
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
#if !PUBLIC
    }
#endif
}
