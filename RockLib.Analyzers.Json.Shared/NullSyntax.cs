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
        class NullSyntax : VerbatimSyntaxNode
        {
            private static readonly IEnumerable<char> _null = "null".ToCharArray();

            public NullSyntax()
                : this(null, null)
            {
            }

            public NullSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
                : base(_null, leadingTrivia, trailingTrivia)
            {
            }

            public NullSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
                new NullSyntax(node.LeadingTrivia, node.TrailingTrivia);

            public NullSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
                new NullSyntax(node.LeadingTrivia, TrailingTrivia);

            public NullSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
                new NullSyntax(LeadingTrivia, node.TrailingTrivia);

            public NullSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
                new NullSyntax(leadingTrivia, TrailingTrivia);

            public NullSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
                new NullSyntax(LeadingTrivia, trailingTrivia);

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
