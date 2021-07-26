using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ColonSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _colon = new[] { ':' };

        public ColonSyntax()
            : this(null, null)
        {
        }

        public ColonSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(_colon, leadingTrivia, trailingTrivia)
        {
        }

        public ColonSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new ColonSyntax(node.LeadingTrivia, node.TrailingTrivia);

        public ColonSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new ColonSyntax(node.LeadingTrivia, TrailingTrivia);

        public ColonSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new ColonSyntax(LeadingTrivia, node.TrailingTrivia);

        public ColonSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new ColonSyntax(leadingTrivia, TrailingTrivia);

        public ColonSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new ColonSyntax(LeadingTrivia, trailingTrivia);

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
