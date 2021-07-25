using System.Collections.Generic;

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
        class CloseBracketSyntax : VerbatimSyntaxNode
        {
            private static readonly IEnumerable<char> _closeBracket = new[] { ']' };

            public CloseBracketSyntax()
                : this(null, null)
            {
            }

            public CloseBracketSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(_closeBracket, leadingTrivia, trailingTrivia)
            {
            }

            public CloseBracketSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBracketSyntax(node.LeadingTrivia, node.TrailingTrivia);

            public CloseBracketSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBracketSyntax(node.LeadingTrivia, TrailingTrivia);

            public CloseBracketSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBracketSyntax(LeadingTrivia, node.TrailingTrivia);

            public CloseBracketSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new CloseBracketSyntax(leadingTrivia, TrailingTrivia);

            public CloseBracketSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new CloseBracketSyntax(LeadingTrivia, trailingTrivia);

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
