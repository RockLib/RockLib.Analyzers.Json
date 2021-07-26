using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class ContainerSyntaxNode : JsonSyntaxNode
    {
        private readonly IReadOnlyList<JsonSyntaxNode> _children;

        protected ContainerSyntaxNode(IReadOnlyList<JsonSyntaxNode> children)
        {
            if (children != null)
                _children = new List<JsonSyntaxNode>(children);
        }

        public IReadOnlyList<JsonSyntaxNode> Children => _children;

        public override bool HasLeadingTrivia =>
            _children?.FirstOrDefault() is JsonSyntaxNode first
            && (first.HasLeadingTrivia || first is TriviaListSyntax);

        public override bool HasTrailingTrivia =>
            _children?.LastOrDefault() is JsonSyntaxNode last
            && (last.HasTrailingTrivia || last is TriviaListSyntax);

        internal override IEnumerable<char> GetJsonDocumentChars()
        {
            if (_children is null)
                return Enumerable.Empty<char>();

            switch (_children.Count)
            {
                case 0:
                    return Enumerable.Empty<char>();
                case 1:
                    return _children[0].GetJsonDocumentChars();
                default:
                    var chars = _children[0].GetJsonDocumentChars();
                    for (int i = 1; i < _children.Count; i++)
                        chars = chars.Concat(_children[i].GetJsonDocumentChars());
                    return chars;
            }
        }

        protected override JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList)
        {
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    var replacementChild = child.WithLeadingTrivia(triviaList);
                    if (!ReferenceEquals(child, replacementChild))
                        return this.ReplaceNode(child, replacementChild);
                }
            }
            return this;
        }

        protected override JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList)
        {
            if (_children != null)
            {
                foreach (var child in Children.Reverse())
                {
                    var replacementChild = child.WithTrailingTrivia(triviaList);
                    if (!ReferenceEquals(child, replacementChild))
                        return this.ReplaceNode(child, replacementChild);
                }
            }
            return this;
        }
    }
}
