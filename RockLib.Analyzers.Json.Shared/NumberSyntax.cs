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
        class NumberSyntax : VerbatimSyntaxNode
        {
            private readonly string _rawValue;

            public NumberSyntax(string rawValue)
                : this(rawValue, null, null)
            {
            }

            public NumberSyntax(string rawValue,
                TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(rawValue, leadingTrivia, trailingTrivia)
            {
                _rawValue = rawValue;
            }

            public bool HasNegativeSign => _rawValue.StartsWith("-");

            public bool HasFractionPart => _rawValue.Contains(".");

            public bool HasExponentPart => _rawValue.Contains("e") || _rawValue.Contains("E");

            public int GetInt32() => int.Parse(_rawValue);

            public long GetInt64() => long.Parse(_rawValue);

            public double GetDouble() => double.Parse(_rawValue);

            public decimal GetDecimal() => decimal.Parse(_rawValue);

            public NumberSyntax WithValue(int value) =>
                new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

            public NumberSyntax WithValue(long value) =>
                new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

            public NumberSyntax WithValue(double value) =>
                new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

            public NumberSyntax WithValue(decimal value) =>
                new NumberSyntax(value.ToString(), LeadingTrivia, TrailingTrivia);

            public NumberSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new NumberSyntax(_rawValue, node.LeadingTrivia, node.TrailingTrivia);

            public NumberSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new NumberSyntax(_rawValue, node.LeadingTrivia, TrailingTrivia);

            public NumberSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new NumberSyntax(_rawValue, LeadingTrivia, node.TrailingTrivia);

            public NumberSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new NumberSyntax(_rawValue, leadingTrivia, TrailingTrivia);

            public NumberSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new NumberSyntax(_rawValue, LeadingTrivia, trailingTrivia);

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
        }
#if !PUBLIC
    }
#endif
}
