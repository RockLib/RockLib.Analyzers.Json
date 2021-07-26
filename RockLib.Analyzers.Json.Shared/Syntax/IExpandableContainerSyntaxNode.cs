namespace RockLib.Analyzers.Json
{
    internal interface IExpandableContainerSyntaxNode
    {
        ExpandableContainerSyntaxNode AddChildCore(JsonSyntaxNode child);
    }
}
