﻿using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class NullSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _null = "null".ToCharArray();

        public NullSyntax(IEnumerable<char> nullToken)
            : this(nullToken, null, null)
        {
        }

        public NullSyntax(IEnumerable<char> nullToken, TriviaListSyntax leadingTrivia, TriviaListSyntax trailingTrivia)
            : base(nullToken, leadingTrivia, trailingTrivia)
        {
        }

        public override bool IsValid => RawValue != null && RawValue.EqualsSlice(_null);

        public override bool IsValueNode => true;

        public NullSyntax WithTriviaFrom(VerbatimSyntaxNode node) =>
            new NullSyntax(RawValue, node.LeadingTrivia, node.TrailingTrivia);

        public NullSyntax WithLeadingTriviaFrom(VerbatimSyntaxNode node) =>
            new NullSyntax(RawValue, node.LeadingTrivia, TrailingTrivia);

        public NullSyntax WithTrailingTriviaFrom(VerbatimSyntaxNode node) =>
            new NullSyntax(RawValue, LeadingTrivia, node.TrailingTrivia);

        public NullSyntax WithLeadingTrivia(TriviaListSyntax leadingTrivia) =>
            new NullSyntax(RawValue, leadingTrivia, TrailingTrivia);

        public NullSyntax WithTrailingTrivia(TriviaListSyntax trailingTrivia) =>
            new NullSyntax(RawValue, LeadingTrivia, trailingTrivia);

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
