using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class JsonSyntaxNode : IJsonSyntaxNode
    {
        private string _jsonDocument;

        public string JsonDocument
        {
            get
            {
                if (_jsonDocument is null)
                    _jsonDocument = new string(GetJsonDocumentChars().ToArray());
                return _jsonDocument;
            }
        }

        public bool HasTrivia => HasLeadingTrivia || HasTrailingTrivia;

        public abstract bool HasLeadingTrivia { get; }

        public abstract bool HasTrailingTrivia { get; }

        public abstract bool IsValid { get; }

        public abstract bool IsValueNode { get; }

        internal abstract IEnumerable<char> GetJsonDocumentChars();

        protected abstract JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList);

        protected abstract JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList);

        protected abstract JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode);

        JsonSyntaxNode IJsonSyntaxNode.WithLeadingTriviaCore(TriviaListSyntax triviaList) =>
            WithLeadingTriviaCore(triviaList);

        JsonSyntaxNode IJsonSyntaxNode.WithTrailingTriviaCore(TriviaListSyntax triviaList) =>
            WithTrailingTriviaCore(triviaList);

        JsonSyntaxNode IJsonSyntaxNode.ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (ReferenceEquals(this, oldNode))
                return newNode;

            return ReplaceCore(oldNode, newNode);
        }

        private string GetDebuggerDisplay() => JsonDocument;
    }
}
