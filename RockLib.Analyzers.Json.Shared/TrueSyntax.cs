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
        class TrueSyntax : VerbatimSyntaxNode
        {
            private static readonly IEnumerable<char> _true = "true".ToCharArray();

            public TrueSyntax()
                : this(null, null)
            {
            }

            public TrueSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(_true, leadingTrivia, trailingTrivia)
            {
            }

            public TrueSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new TrueSyntax(node.LeadingTrivia, node.TrailingTrivia);

            public TrueSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new TrueSyntax(node.LeadingTrivia, TrailingTrivia);

            public TrueSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new TrueSyntax(LeadingTrivia, node.TrailingTrivia);

            public TrueSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new TrueSyntax(leadingTrivia, TrailingTrivia);

            public TrueSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new TrueSyntax(LeadingTrivia, trailingTrivia);

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
