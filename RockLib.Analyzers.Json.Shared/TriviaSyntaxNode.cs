using System.Collections.Generic;

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
        abstract class TriviaSyntaxNode : JsonSyntaxNode
        {
            protected TriviaSyntaxNode(IEnumerable<char> rawValue)
            {
                RawValue = rawValue;
            }

            public override bool HasTrivia => false;

            public IEnumerable<char> RawValue { get; }

            public override IEnumerable<char> GetChars() => RawValue;

            protected override JsonSyntaxNode Replace(JsonSyntaxNode oldNode, JsonSyntaxNode newNode) => this;
        }
#if !PUBLIC
    }
#endif
}
