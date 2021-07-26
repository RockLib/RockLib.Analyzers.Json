using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class ContainerSyntaxNode : JsonSyntaxNode
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

        // TODO: Add WithChild method (and extension method?)
        // Or maybe there needs to be an "expandable container" node that has the WithChild method?

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

        protected override JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList)
        {
            foreach (var child in Children)
            {
                var replacementChild = child.WithLeadingTrivia(triviaList);
                if (!ReferenceEquals(child, replacementChild))
                    return this.ReplaceNode(child, replacementChild);
            }
            return this;
        }

        protected override JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList)
        {
            foreach (var child in Children.Reverse())
            {
                var replacementChild = child.WithTrailingTrivia(triviaList);
                if (!ReferenceEquals(child, replacementChild))
                    return this.ReplaceNode(child, replacementChild);
            }
            return this;
        }
    }
}
