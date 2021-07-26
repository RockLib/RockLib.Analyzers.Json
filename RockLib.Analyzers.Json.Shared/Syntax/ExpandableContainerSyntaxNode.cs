using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public abstract class ExpandableContainerSyntaxNode : ContainerSyntaxNode, IExpandableContainerSyntaxNode
    {
        protected ExpandableContainerSyntaxNode(IEnumerable<JsonSyntaxNode> children)
            : base(children)
        {
        }

        ExpandableContainerSyntaxNode IExpandableContainerSyntaxNode.AddChildCore(JsonSyntaxNode child) =>
            AddChildCore(child);

        protected abstract ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child);
    }
}
