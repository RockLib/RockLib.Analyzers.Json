﻿using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class TrueSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _true = "true".ToCharArray();

        public TrueSyntax(IEnumerable<char> trueToken)
            : this(trueToken, null, null)
        {
        }

        public TrueSyntax(IEnumerable<char> trueToken, TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(trueToken, leadingTrivia, trailingTrivia)
        {
        }

        public override bool IsValid => RawValue != null && RawValue.EqualsSlice(_true);

        public override bool IsValueNode => true;

        public TrueSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new TrueSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

        public TrueSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new TrueSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

        public TrueSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new TrueSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

        public TrueSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new TrueSyntax(RawValue, leadingTrivia, TrailingTrivia);

        public TrueSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new TrueSyntax(RawValue, LeadingTrivia, trailingTrivia);

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
