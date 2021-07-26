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
            if (items != null)
                Items = new List<ArrayItemSyntax>(items);
            CloseBracket = closeBracket;
        }

        public OpenBracketSyntax OpenBracket { get; }

        public IReadOnlyList<ArrayItemSyntax> Items { get; }

        public CloseBracketSyntax CloseBracket { get; }

        public override bool IsValid
        {
            get
            {
                if (OpenBracket is null || CloseBracket is null)
                    return false;

                // All but the last item needs a comma
                if (Items is null || Items.Count < 2)
                    return true;

                for (int i = 0; i < Items.Count - 1; i++)
                    if (Items[i].Comma is null)
                        return false;

                return true;
            }
        }

        public override bool IsValueNode => true;

        public ArraySyntax WithOpenBracket(OpenBracketSyntax openBracket) =>
            new ArraySyntax(openBracket, Items, CloseBracket);

        public ArraySyntax WithItems(IReadOnlyList<ArrayItemSyntax> items) =>
            new ArraySyntax(OpenBracket, items, CloseBracket);

        public ArraySyntax WithCloseBracket(CloseBracketSyntax closeBracket) =>
            new ArraySyntax(OpenBracket, Items, closeBracket);

        // TODO: Add InsertItem, RemoveItem

        public ArraySyntax AddItem(ArrayItemSyntax item)
        {
            if (Items is null)
                return WithItems(new[] { item });

            var items = new ArrayItemSyntax[Items.Count + 1];

            if (Items.Count > 0
                && Items[0].Value is VerbatimSyntaxNode verbatimNode
                && verbatimNode.LeadingTrivia != null)
            {
                item = item.WithLeadingTrivia(verbatimNode.LeadingTrivia);
            }

            for (int i = 0; i < Items.Count; i++)
                items[i] = Items[i];

            items[Items.Count] = item;

            if (items.Length > 1 && items[items.Length - 2].Comma is null)
                items[items.Length - 2] = items[items.Length - 2].WithComma(new CommaSyntax());

            return WithItems(items);
        }

        protected override ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child)
        {
            return AddItem((ArrayItemSyntax)child);
        }

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (Items != null)
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
            }

            if (OpenBracket != null)
            {
                var replacementOpenBracket = OpenBracket.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementOpenBracket, OpenBracket))
                    return WithOpenBracket(replacementOpenBracket);
            }

            if (CloseBracket != null)
            {
                var replacementCloseBracket = CloseBracket.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementCloseBracket, CloseBracket))
                    return WithCloseBracket(replacementCloseBracket);
            }

            return this;
        }

        private static IReadOnlyList<JsonSyntaxNode> GetChildren(
            OpenBracketSyntax openBracket,
            IEnumerable<ArrayItemSyntax> items,
            CloseBracketSyntax closeBracket)
        {
            var list = new List<JsonSyntaxNode>();
            if (openBracket != null)
                list.Add(openBracket);
            if (items != null)
                foreach (var item in items)
                    list.Add(item);
            if (closeBracket != null)
                list.Add(closeBracket);
            return list;
        }
    }
}
