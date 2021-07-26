using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ArrayItemSyntax : ContainerSyntaxNode
    {
        public ArrayItemSyntax(JsonSyntaxNode value)
            : this(value, null)
        {
        }

        public ArrayItemSyntax(JsonSyntaxNode value, CommaSyntax comma)
            : base(GetChildren(value, comma))
        {
            Value = value;
            Comma = comma;
        }

        public JsonSyntaxNode Value { get; }

        public CommaSyntax Comma { get; }

        public ArrayItemSyntax WithValue(JsonSyntaxNode value) =>
            new ArrayItemSyntax(value, Comma);

        public ArrayItemSyntax WithComma(CommaSyntax comma) =>
            new ArrayItemSyntax(Value, comma);

        public ArrayItemSyntax WithoutComma() =>
            new ArrayItemSyntax(Value, null);

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            var replacementValue = Value.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementValue, Value))
                return WithValue(replacementValue);

            if (Comma != null)
            {
                var replacementComma = Comma.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementComma, Comma))
                    return WithComma(replacementComma);
            }

            return this;
        }

        private static IEnumerable<JsonSyntaxNode> GetChildren(JsonSyntaxNode value, CommaSyntax comma)
        {
            yield return value;
            if (comma != null)
                yield return comma;
        }
    }
}
