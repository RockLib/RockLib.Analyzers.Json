using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class FalseSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _false = "false".ToCharArray();

        public FalseSyntax(IEnumerable<char> falseToken)
            : this(falseToken, null, null)
        {
        }

        public FalseSyntax(IEnumerable<char> falseToken, TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(_false, leadingTrivia, trailingTrivia)
        {
            if (!falseToken.EqualsSlice(_false))
                throw new Exception("Invalid value: " + new string(falseToken.ToArray()));
        }

        public FalseSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new FalseSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

        public FalseSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new FalseSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

        public FalseSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new FalseSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

        public FalseSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new FalseSyntax(RawValue, leadingTrivia, TrailingTrivia);

        public FalseSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new FalseSyntax(RawValue, LeadingTrivia, trailingTrivia);

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
