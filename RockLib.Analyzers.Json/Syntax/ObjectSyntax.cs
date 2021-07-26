using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class ObjectSyntax : ExpandableContainerSyntaxNode
    {
        public ObjectSyntax(OpenBraceSyntax openBrace,
            IReadOnlyList<ObjectMemberSyntax> members,
            CloseBraceSyntax closeBrace)
            : base(GetChildren(openBrace, members, closeBrace))
        {
            OpenBrace = openBrace;
            Members = new List<ObjectMemberSyntax>(members);
            CloseBrace = closeBrace;
        }

        public OpenBraceSyntax OpenBrace { get; }

        public IReadOnlyList<ObjectMemberSyntax> Members { get; }

        public CloseBraceSyntax CloseBrace { get; }

        public override bool IsValid
        {
            get
            {
                if (OpenBrace is null || OpenBrace is null)
                    return false;

                // All but the last member needs a comma
                if (Members is null || Members.Count < 2)
                    return true;

                for (int i = 0; i < Members.Count - 1; i++)
                    if (Members[i].Comma is null)
                        return false;

                return true;
            }
        }

        public override bool IsValueNode => true;

        public ObjectSyntax WithOpenBrace(OpenBraceSyntax openBrace) =>
            new ObjectSyntax(openBrace, Members, CloseBrace);

        public ObjectSyntax WithMembers(IReadOnlyList<ObjectMemberSyntax> members) =>
            new ObjectSyntax(OpenBrace, members, CloseBrace);

        public ObjectSyntax WithCloseBrace(CloseBraceSyntax closeBrace) =>
            new ObjectSyntax(OpenBrace, Members, closeBrace);

        // TODO: Add InsertMember, RemoveMember

        public ObjectSyntax AddMember(ObjectMemberSyntax member)
        {
            if (Members is null)
                return WithMembers(new[] { member });

            var members = new ObjectMemberSyntax[Members.Count + 1];

            if (Members.Count > 0 && Members[0].Name.LeadingTrivia != null)
                member = member.WithLeadingTrivia(Members[0].Name.LeadingTrivia);

            for (int i = 0; i < Members.Count; i++)
                members[i] = Members[i];

            members[Members.Count] = member;

            if (members.Length > 1 && members[members.Length - 2].Comma is null)
                members[members.Length - 2] = members[members.Length - 2].WithComma(new CommaSyntax());

            return WithMembers(members);
        }

        protected override ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child)
        {
            return AddMember((ObjectMemberSyntax)child);
        }

        protected override JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
        {
            if (Members != null)
            {
                for (int i = 0; i < Members.Count; i++)
                {
                    var replacementMember = Members[i].ReplaceNode(oldNode, newNode);
                    if (!ReferenceEquals(replacementMember, Members[i]))
                    {
                        var replacementMembers = new ObjectMemberSyntax[Members.Count];
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
            }

            if (OpenBrace != null)
            {
                var replacementOpenBrace = OpenBrace.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementOpenBrace, OpenBrace))
                    return WithOpenBrace(replacementOpenBrace);
            }

            if (CloseBrace != null)
            {
                var replacementCloseBrace = CloseBrace.ReplaceNode(oldNode, newNode);
                if (!ReferenceEquals(replacementCloseBrace, CloseBrace))
                    return WithCloseBrace(replacementCloseBrace);
            }

            return this;
        }

        private static IReadOnlyList<JsonSyntaxNode> GetChildren(
            OpenBraceSyntax openBrace,
            IEnumerable<ObjectMemberSyntax> members,
            CloseBraceSyntax closeBrace)
        {
            var list = new List<JsonSyntaxNode>();
            if (openBrace != null)
                list.Add(openBrace);
            if (members != null)
                foreach (var member in members)
                    list.Add(member);
            if (closeBrace != null)
                list.Add(closeBrace);
            return list;
        }
    }
}
