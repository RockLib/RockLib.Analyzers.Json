using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        abstract class ContainerSyntaxNode : JsonSyntaxNode

        {
            private readonly IReadOnlyList<JsonSyntaxNode> _children;

            protected ContainerSyntaxNode(IEnumerable<JsonSyntaxNode> children)
            {
                if (children is null)
                    throw new ArgumentNullException(nameof(children));

                _children = new ReadOnlyCollection<JsonSyntaxNode>(children as IList<JsonSyntaxNode> ?? children.ToList());
            }

            public virtual IReadOnlyList<JsonSyntaxNode> Children => _children;

            public override bool HasTrivia => false;

            public override IEnumerable<char> GetChars()
            {
                switch (_children.Count)
                {
                    case 0:
                        return Enumerable.Empty<char>();
                    case 1:
                        return _children[0].GetChars();
                    default:
                        var chars = _children[0].GetChars();
                        for (int i = 1; i < _children.Count; i++)
                            chars = chars.Concat(_children[i].GetChars());
                        return chars;
                }
            }
        }
#if !PUBLIC
    }
#endif
}
