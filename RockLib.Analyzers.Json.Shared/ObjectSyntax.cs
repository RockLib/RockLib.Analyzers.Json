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
        class ObjectSyntax : ContainerSyntaxNode
        {
            public ObjectSyntax(OpenBraceSyntax openBrace,
                IReadOnlyList<MemberSyntax> members,
                CloseBraceSyntax closeBrace)
                : base(GetChildren(openBrace, members, closeBrace))
            {
                OpenBrace = openBrace;
                Members = members;
                CloseBrace = closeBrace;
            }

            public OpenBraceSyntax OpenBrace { get; }

            public IReadOnlyList<MemberSyntax> Members { get; }

            public CloseBraceSyntax CloseBrace { get; }

            public ObjectSyntax WithOpenBrace(OpenBraceSyntax openBrace) =>
                new ObjectSyntax(openBrace, Members, CloseBrace);

            public ObjectSyntax WithMembers(IReadOnlyList<MemberSyntax> items) =>
                new ObjectSyntax(OpenBrace, items, CloseBrace);

            public ObjectSyntax WithCloseBrace(CloseBraceSyntax closeBrace) =>
                new ObjectSyntax(OpenBrace, Members, closeBrace);

            protected override JsonSyntaxNode Replace(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
            {
                for (int i = 0; i < Members.Count; i++)
                {
                    var replacementMember = Members[i].ReplaceNode(oldNode, newNode);
                    if (!ReferenceEquals(replacementMember, Members[i]))
                    {
                        var replacementMembers = new MemberSyntax[Members.Count];
                        for (int j = 0; j < replacementMembers.Length; j++)
                        {
                            if (j == i)
                                replacementMembers[j] = replacementMember;
                            else
                                replacementMembers[j] = Members[j];
                        }
                        return WithMembers(replacementMembers);
                    }
                }

                var replacementOpenBrace = OpenBrace.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementOpenBrace, OpenBrace))
                    return WithOpenBrace(replacementOpenBrace);

                var replacementCloseBrace = CloseBrace.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementCloseBrace, CloseBrace))
                    return WithCloseBrace(replacementCloseBrace);

                return this;
            }

            private static IEnumerable<JsonSyntaxNode> GetChildren(
                OpenBraceSyntax openBrace,
                IEnumerable<MemberSyntax> members,
                CloseBraceSyntax closeBrace)
            {
                yield return openBrace;
                foreach (var member in members)
                    yield return member;
                yield return closeBrace;
            }
        }
#if !PUBLIC
    }
#endif
}
