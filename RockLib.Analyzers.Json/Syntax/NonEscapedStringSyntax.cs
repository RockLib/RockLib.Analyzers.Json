using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class NonEscapedStringSyntax : StringSyntax
    {
        private readonly Lazy<string> _stringValue;

        public NonEscapedStringSyntax(IEnumerable<char> rawValue)
            : this(rawValue, null, null)
        {
        }

        public NonEscapedStringSyntax(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(rawValue, leadingTrivia, trailingTrivia)
        {
            _stringValue = new Lazy<string>(() =>
            {
                var charArray = rawValue.ToArray();

                if (charArray.Length < 1 || charArray[0] != '"')
                    throw new Exception("Missing open quote.");

                if (charArray.Length < 2 || charArray[charArray.Length - 1] != '"')
                    throw new Exception("Missing close quote.");

                var stringValue = new string(charArray, 1, charArray.Length - 2);

                if (stringValue.Any(IsSpecialCharacter))
                    throw new Exception("Cannot contain any special characters.");

                return stringValue;
            });
        }

        public override string Value => _stringValue.Value;

        public NonEscapedStringSyntax WithValue(string value) =>
            new NonEscapedStringSyntax(value, LeadingTrivia, TrailingTrivia);

        public NonEscapedStringSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new NonEscapedStringSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

        public NonEscapedStringSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new NonEscapedStringSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

        public NonEscapedStringSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new NonEscapedStringSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

        public NonEscapedStringSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new NonEscapedStringSyntax(RawValue, leadingTrivia, TrailingTrivia);

        public NonEscapedStringSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new NonEscapedStringSyntax(RawValue, LeadingTrivia, trailingTrivia);

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

        private static bool IsSpecialCharacter(char c)
        {
            switch (c)
            {
                default:
                    return false;
                case '"':
                case '\\':
                case '/':
                case '\b':
                case '\f':
                case '\n':
                case '\r':
                case '\t':
                    return true;
            }
        }
    }
}
