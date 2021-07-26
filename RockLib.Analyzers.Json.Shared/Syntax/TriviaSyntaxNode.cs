using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public abstract class TriviaSyntaxNode : JsonSyntaxNode
    {
        protected TriviaSyntaxNode(IEnumerable<char> rawValue)
        {
            RawValue = rawValue;
        }

        public override bool HasTrivia => false;

        public IEnumerable<char> RawValue { get; }

        public override IEnumerable<char> GetChars() => RawValue;

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode) => this;

        protected override JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList) => this;

        protected override JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList) => this;
    }
}
