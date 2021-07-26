using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ArraySyntax : ExpandableContainerSyntaxNode
    {
        public ArraySyntax(OpenBracketSyntax openBracket,
            IReadOnlyList<ArrayItemSyntax> items,
            CloseBracketSyntax closeBracket)
            : base(GetChildren(openBracket, items, closeBracket))
        {
            OpenBracket = openBracket;
            Items = items;
            CloseBracket = closeBracket;
        }

        public OpenBracketSyntax OpenBracket { get; }

        public IReadOnlyList<ArrayItemSyntax> Items { get; }

        public CloseBracketSyntax CloseBracket { get; }

        public ArraySyntax WithOpenBracket(OpenBracketSyntax openBracket) =>
            new ArraySyntax(openBracket, Items, CloseBracket);

        public ArraySyntax WithItems(IReadOnlyList<ArrayItemSyntax> items) =>
            new ArraySyntax(OpenBracket, items, CloseBracket);

        public ArraySyntax WithCloseBracket(CloseBracketSyntax closeBracket) =>
            new ArraySyntax(OpenBracket, Items, closeBracket);

        // TODO: Add InsertItem, RemoveItem

        public ArraySyntax AddItem(ArrayItemSyntax item)
        {
            var items = new ArrayItemSyntax[Items.Count + 1];

            for (int i = 0; i < Items.Count; i++)
                items[i] = Items[i];

            items[Items.Count] = item;

            if (items.Length > 1 && items[items.Length - 2].Comma == null)
            {
                if (items.Length > 2)
                {
                    items[items.Length - 2] = items[items.Length - 2].WithComma(items[items.Length - 3].Comma);
                }
                else
                    items[items.Length - 2] = items[items.Length - 2].WithComma(new CommaSyntax());
            }

            return WithItems(items);
        }

        protected override ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child)
        {
            return AddItem((ArrayItemSyntax)child);
        }

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var replacementItem = Items[i].ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementItem, Items[i]))
                {
                    var replacementItems = new ArrayItemSyntax[Items.Count];
                    for (int j = 0; j < replacementItems.Length; j++)
                    {
                        if (j == i)
                            replacementItems[j] = replacementItem;
                        else
                            replacementItems[j] = Items[j];
                    }
                    return WithItems(replacementItems);
                }
            }

            var replacementOpenBracket = OpenBracket.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementOpenBracket, OpenBracket))
                return WithOpenBracket(replacementOpenBracket);

            var replacementCloseBracket = CloseBracket.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementCloseBracket, CloseBracket))
                return WithCloseBracket(replacementCloseBracket);

            return this;
        }

        private static IEnumerable<JsonSyntaxNode> GetChildren(
            OpenBracketSyntax openBracket,
            IEnumerable<ArrayItemSyntax> arrayItems,
            CloseBracketSyntax closeBracket)
        {
            yield return openBracket;
            foreach (var item in arrayItems)
                yield return item;
            yield return closeBracket;
        }
    }
}
