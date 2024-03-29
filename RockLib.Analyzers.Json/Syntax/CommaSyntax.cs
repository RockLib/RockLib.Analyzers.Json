﻿using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class CommaSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _comma = new[] { ',' };

        public CommaSyntax()
            : this(null, null)
        {
        }

        public CommaSyntax(TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(_comma, leadingTrivia, trailingTrivia)
        {
        }

        public override bool IsValid => true;

        public override bool IsValueNode => false;

        public CommaSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new CommaSyntax(node.LeadingTrivia, node.TrailingTrivia);

        public CommaSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new CommaSyntax(node.LeadingTrivia, TrailingTrivia);

        public CommaSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new CommaSyntax(LeadingTrivia, node.TrailingTrivia);

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
