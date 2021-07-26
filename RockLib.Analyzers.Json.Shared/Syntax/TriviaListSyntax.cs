using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class TriviaListSyntax : ContainerSyntaxNode
    {
        public TriviaListSyntax(IReadOnlyList<TriviaSyntaxNode> items)
            : base(items)
        {
            Items = items;
        }

        public IReadOnlyList<TriviaSyntaxNode> Items { get; }

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var replacementItem = Items[i].ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementItem, Items[i]))
                {
                    var replacementItems = new TriviaSyntaxNode[Items.Count];
                    for (int j = 0; j < replacementItems.Length; j++)
                    {
                        if (j == i)
                            replacementItems[j] = replacementItem;
                        else
                            replacementItems[j] = Items[j];
                    }
                    return new TriviaListSyntax(replacementItems);
                }
            }

            return this;
        }

        public static implicit operator TriviaListSyntax(TriviaSyntaxNode triviaSyntax) => new TriviaListSyntax(new[] { triviaSyntax });

        public static implicit operator TriviaListSyntax(TriviaSyntaxNode[] triviaSyntax) => new TriviaListSyntax(triviaSyntax);

        public static implicit operator TriviaListSyntax(List<TriviaSyntaxNode> triviaSyntax) => new TriviaListSyntax(triviaSyntax);
    }
}
