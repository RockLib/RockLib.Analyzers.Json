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
        class FalseSyntax : VerbatimSyntaxNode
        {
            private static readonly IEnumerable<char> _false = "false".ToCharArray();

            public FalseSyntax()
                : this(null, null)
            {
            }

            public FalseSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(_false, leadingTrivia, trailingTrivia)
            {
            }

            public FalseSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new FalseSyntax(node.LeadingTrivia, node.TrailingTrivia);

            public FalseSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new FalseSyntax(node.LeadingTrivia, TrailingTrivia);

            public FalseSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new FalseSyntax(LeadingTrivia, node.TrailingTrivia);

            public FalseSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new FalseSyntax(leadingTrivia, TrailingTrivia);

            public FalseSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new FalseSyntax(LeadingTrivia, trailingTrivia);

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
