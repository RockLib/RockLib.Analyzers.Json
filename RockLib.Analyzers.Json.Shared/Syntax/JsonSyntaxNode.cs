using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class JsonSyntaxNode : IJsonSyntaxNode
    {
        public abstract IEnumerable<char> GetChars();

        public abstract bool HasTrivia { get; }

        public string FormattedValue => new string(GetChars().ToArray());

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

        protected abstract JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList);

        protected abstract JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList);

        protected abstract JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode);
    }
}
