using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class OpenBraceSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _openBrace = new[] { '{' };

        public OpenBraceSyntax()
            : this(null, null)
        {
        }

        public OpenBraceSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(_openBrace, leadingTrivia, trailingTrivia)
        {
        }

        public override bool IsValid => true;

        public override bool IsValueNode => false;

        public OpenBraceSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBraceSyntax(node.LeadingTrivia, node.TrailingTrivia);

        public OpenBraceSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBraceSyntax(node.LeadingTrivia, TrailingTrivia);

        public OpenBraceSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new OpenBraceSyntax(LeadingTrivia, node.TrailingTrivia);

        public OpenBraceSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new OpenBraceSyntax(leadingTrivia, TrailingTrivia);

        public OpenBraceSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new OpenBraceSyntax(LeadingTrivia, trailingTrivia);

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
