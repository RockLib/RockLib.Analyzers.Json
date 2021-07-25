using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers
{
#if !PUBLIC
    partial class Json
    {
#endif
#if PUBLIC
        public
#else
        internal
#endif
        abstract class JsonSyntaxNode : IJsonSyntaxNode
        {
            public abstract IEnumerable<char> GetChars();

            public abstract bool HasTrivia { get; }

            public override string ToString() => new string(GetChars().ToArray());

            JsonSyntaxNode IJsonSyntaxNode.ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
            {
                if (ReferenceEquals(this, oldNode))
                    return newNode;

                return Replace(oldNode, newNode);
            }

            protected abstract JsonSyntaxNode Replace(JsonSyntaxNode oldNode, JsonSyntaxNode newNode);
        }

        private interface IJsonSyntaxNode
        {
            JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode);
        }
#if !PUBLIC
    }
#endif
}
