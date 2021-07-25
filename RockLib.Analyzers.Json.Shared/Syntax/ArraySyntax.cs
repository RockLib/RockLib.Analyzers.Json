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
        class ArraySyntax : ContainerSyntaxNode
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

            protected override JsonSyntaxNode Replace(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
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
#if !PUBLIC
    }
#endif
}
