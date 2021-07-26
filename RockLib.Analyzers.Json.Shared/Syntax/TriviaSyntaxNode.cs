using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class TriviaSyntaxNode : JsonSyntaxNode
    {
        protected TriviaSyntaxNode(IEnumerable<char> rawValue)
        {
            RawValue = rawValue;
        }

        public override bool HasLeadingTrivia => false;
        
        public override bool HasTrailingTrivia => false;

        public IEnumerable<char> RawValue { get; }

        internal override IEnumerable<char> GetJsonDocumentChars() => RawValue ?? Enumerable.Empty<char>();

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode) => this;

        protected override JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList) => this;

        protected override JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList) => this;
    }
}
