using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ObjectMemberSyntax : ContainerSyntaxNode
    {
        public ObjectMemberSyntax(StringSyntax name, ColonSyntax colon, JsonSyntaxNode value)
            : this(name, colon, value, null)
        {
        }

        public ObjectMemberSyntax(StringSyntax name, ColonSyntax colon, JsonSyntaxNode value, CommaSyntax comma)
            : base(GetChildren(name, colon, value, comma))
        {
            Name = name;
            Colon = colon;
            Value = value;
            Comma = comma;
        }

        public StringSyntax Name { get; }

        public ColonSyntax Colon { get; }

        public JsonSyntaxNode Value { get; }

        public CommaSyntax Comma { get; }

        public override bool IsValid =>
            Name != null
            && Colon != null
            && Value != null
            && Value.IsValueNode;

        public override bool IsValueNode => false;

        public ObjectMemberSyntax WithName(StringSyntax name) =>
            new ObjectMemberSyntax(name, Colon, Value, Comma);

        public ObjectMemberSyntax WithColon(ColonSyntax colon) =>
            new ObjectMemberSyntax(Name, colon, Value, Comma);

        public ObjectMemberSyntax WithValue(JsonSyntaxNode value) =>
            new ObjectMemberSyntax(Name, Colon, value, Comma);

        public ObjectMemberSyntax WithComma(CommaSyntax comma) =>
            new ObjectMemberSyntax(Name, Colon, Value, comma);

        public ObjectMemberSyntax WithoutComma() =>
            new ObjectMemberSyntax(Name, Colon, Value, null);

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (Value != null)
            {
                var replacementValue = Value.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementValue, Value))
                    return WithValue(replacementValue);
            }

            if (Name != null)
            {
                var replacementName = Name.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementName, Name))
                    return WithName(replacementName);
            }

            if (Colon != null)
            {
                var replacementColon = Colon.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementColon, Colon))
                    return WithColon(replacementColon);
            }

            if (Comma != null)
            {
                var replacementComma = Comma.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementComma, Comma))
                    return WithComma(replacementComma);
            }

            return this;
        }

        private static IReadOnlyList<JsonSyntaxNode> GetChildren(StringSyntax name,
                ColonSyntax colon, JsonSyntaxNode value, CommaSyntax comma)
        {
            var list = new List<JsonSyntaxNode>();
            if (name != null)
                list.Add(name);
            if (colon != null)
                list.Add(colon);
            if (value != null)
                list.Add(value);
            if (comma != null)
                list.Add(comma);
            return list;
        }
    }
}
