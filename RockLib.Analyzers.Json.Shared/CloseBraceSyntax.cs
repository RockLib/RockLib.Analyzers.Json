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
        class CloseBraceSyntax : VerbatimSyntaxNode
        {
            private static readonly IEnumerable<char> _closeBrace = new[] { '}' };

            public CloseBraceSyntax()
                : this(null, null)
            {
            }

            public CloseBraceSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(_closeBrace, leadingTrivia, trailingTrivia)
            {
            }

            public CloseBraceSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBraceSyntax(node.LeadingTrivia, node.TrailingTrivia);

            public CloseBraceSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBraceSyntax(node.LeadingTrivia, TrailingTrivia);

            public CloseBraceSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new CloseBraceSyntax(LeadingTrivia, node.TrailingTrivia);

            public CloseBraceSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new CloseBraceSyntax(leadingTrivia, TrailingTrivia);

            public CloseBraceSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new CloseBraceSyntax(LeadingTrivia, trailingTrivia);

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
