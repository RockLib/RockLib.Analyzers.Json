using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class OpenBracketSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _openBrace = new[] { '[' };

        public OpenBracketSyntax()
            : this(null, null)
        {
        }

        public OpenBracketSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(_openBrace, leadingTrivia, trailingTrivia)
        {
        }

        public override bool IsValid => true;

        public override bool IsValueNode => false;

        public OpenBracketSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBracketSyntax(node.LeadingTrivia, node.TrailingTrivia);

        public OpenBracketSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBracketSyntax(node.LeadingTrivia, TrailingTrivia);

        public OpenBracketSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBracketSyntax(LeadingTrivia, node.TrailingTrivia);

        public OpenBracketSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new OpenBracketSyntax(leadingTrivia, TrailingTrivia);

        public OpenBracketSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new OpenBracketSyntax(LeadingTrivia, trailingTrivia);

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
