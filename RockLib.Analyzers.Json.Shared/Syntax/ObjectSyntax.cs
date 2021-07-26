using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ObjectSyntax : ExpandableContainerSyntaxNode
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

        // TODO: Add InsertMember, RemoveMember

        public ObjectSyntax AddMember(MemberSyntax member)
        {
            var members = new MemberSyntax[Members.Count + 1];

            if (Members.Count > 1 && Members[0].Name.LeadingTrivia != null)
                member = member.WithLeadingTrivia(Members[0].Name.LeadingTrivia);

            for (int i = 0; i < Members.Count; i++)
                members[i] = Members[i];

            members[Members.Count] = member;

            if (members.Length > 1 && members[members.Length - 2].Comma == null)
                members[members.Length - 2] = members[members.Length - 2].WithComma(new CommaSyntax());

            return WithMembers(members);
        }

        protected override ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child)
        {
            return AddMember((MemberSyntax)child);
        }

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
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
}
