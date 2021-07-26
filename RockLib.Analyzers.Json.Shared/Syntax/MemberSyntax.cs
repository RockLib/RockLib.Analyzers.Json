using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class MemberSyntax : ContainerSyntaxNode
    {
        public MemberSyntax(StringSyntax name, ColonSyntax colon, JsonSyntaxNode value)
            : this(name, colon, value, null)
        {
        }

        public MemberSyntax(StringSyntax name, ColonSyntax colon, JsonSyntaxNode value, CommaSyntax comma)
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

        public MemberSyntax WithName(StringSyntax name) =>
            new MemberSyntax(name, Colon, Value, Comma);

        public MemberSyntax WithColon(ColonSyntax colon) =>
            new MemberSyntax(Name, colon, Value, Comma);

        public MemberSyntax WithValue(JsonSyntaxNode value) =>
            new MemberSyntax(Name, Colon, value, Comma);

        public MemberSyntax WithComma(CommaSyntax comma) =>
            new MemberSyntax(Name, Colon, Value, comma);

        public MemberSyntax WithoutComma() =>
            new MemberSyntax(Name, Colon, Value, null);

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            var replacementValue = Value.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementValue, Value))
                return WithValue(replacementValue);

            var replacementName = Name.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementName, Name))
                return WithName(replacementName);

            var replacementColon = Colon.ReplaceNode(oldNode, newNode);
            if (!ReferenceEquals(replacementColon, Colon))
                return WithColon(replacementColon);

            if (Comma != null)
            {
                var replacementComma = Comma.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementComma, Comma))
                    return WithComma(replacementComma);
            }

            return this;
        }

        private static IEnumerable<JsonSyntaxNode> GetChildren(StringSyntax name,
            ColonSyntax colon, JsonSyntaxNode value, CommaSyntax comma)
        {
            yield return name;
            yield return colon;
            yield return value;
            if (comma != null)
                yield return comma;
        }
    }
}
