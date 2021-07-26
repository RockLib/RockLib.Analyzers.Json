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

        public override bool IsValid =>
            Value != null
            && Value.IsValueNode;

        public override bool IsValueNode => false;

        public ArrayItemSyntax WithValue(JsonSyntaxNode value) =>
            new ArrayItemSyntax(value, Comma);

        public ArrayItemSyntax WithComma(CommaSyntax comma) =>
            new ArrayItemSyntax(Value, comma);

        public ArrayItemSyntax WithoutComma() =>
            new ArrayItemSyntax(Value, null);

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (Value != null)
            {
                var replacementValue = Value.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementValue, Value))
                    return WithValue(replacementValue);
            }

            if (Comma != null)
            {
                var replacementComma = Comma.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementComma, Comma))
                    return WithComma(replacementComma);
            }

            return this;
        }

        private static IReadOnlyList<JsonSyntaxNode> GetChildren(JsonSyntaxNode value, CommaSyntax comma)
        {
            var list = new List<JsonSyntaxNode>();
            if (value != null)
                list.Add(value);
            if (comma != null)
                list.Add(comma);
            return list;
        }
    }
}
