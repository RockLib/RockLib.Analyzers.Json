using System;
using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class TriviaListSyntax : ContainerSyntaxNode
    {
        public TriviaListSyntax(IReadOnlyList<TriviaSyntaxNode> items)
            : base(items)
        {
            if (items != null)
                Items = new List<TriviaSyntaxNode>(items);
        }

        public IReadOnlyList<TriviaSyntaxNode> Items { get; }

        public override bool IsValid => true;

        public override bool IsValueNode => false;

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (Items is null)
                return this;

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

        public static implicit operator TriviaListSyntax(TriviaSyntaxNode triviaSyntax)
        {
            if (triviaSyntax is null)
                throw new ArgumentNullException(nameof(triviaSyntax));

            return new TriviaListSyntax(new[] { triviaSyntax });
        }

        public static implicit operator TriviaListSyntax(TriviaSyntaxNode[] triviaSyntaxArray)
        {
            if (triviaSyntaxArray is null)
                throw new ArgumentNullException(nameof(triviaSyntaxArray));

            return new TriviaListSyntax(triviaSyntaxArray);
        }

        public static implicit operator TriviaListSyntax(List<TriviaSyntaxNode> triviaSyntaxList)
        {
            if (triviaSyntaxList is null)
                throw new ArgumentNullException(nameof(triviaSyntaxList));

            return new TriviaListSyntax(triviaSyntaxList);
        }
    }
}
