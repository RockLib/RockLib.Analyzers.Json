using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class NumberSyntax : VerbatimSyntaxNode
    {
        private readonly Lazy<string> _rawValue;

        public NumberSyntax(IEnumerable<char> rawValue)
            : this(rawValue, null, null)
        {
        }

        public NumberSyntax(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(rawValue, leadingTrivia, trailingTrivia)
        {
            _rawValue = new Lazy<string>(() =>
                rawValue as string
                ?? new string(rawValue.ToArray()));
        }

        public bool HasNegativeSign => _rawValue.Value.StartsWith("-");

        public bool HasFractionPart => _rawValue.Value.Contains(".");

        public bool HasExponentPart => _rawValue.Value.Contains("e") || _rawValue.Value.Contains("E");

        public int GetInt32() => int.Parse(_rawValue.Value);

        public long GetInt64() => long.Parse(_rawValue.Value);

        public double GetDouble() => double.Parse(_rawValue.Value);

        public decimal GetDecimal() => decimal.Parse(_rawValue.Value);

        public NumberSyntax WithValue(int value) =>
            new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

        public NumberSyntax WithValue(long value) =>
            new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

        public NumberSyntax WithValue(double value) =>
            new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

        public NumberSyntax WithValue(decimal value) =>
            new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

        public NumberSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new NumberSyntax(_rawValue.Value, node.LeadingTrivia, node.TrailingTrivia);

        public NumberSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new NumberSyntax(_rawValue.Value, node.LeadingTrivia, TrailingTrivia);

        public NumberSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new NumberSyntax(_rawValue.Value, LeadingTrivia, node.TrailingTrivia);

        public NumberSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new NumberSyntax(_rawValue.Value, leadingTrivia, TrailingTrivia);

        public NumberSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new NumberSyntax(_rawValue.Value, LeadingTrivia, trailingTrivia);

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
    }
}
