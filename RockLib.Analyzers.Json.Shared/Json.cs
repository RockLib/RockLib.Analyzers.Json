namespace RockLib.Analyzers
{
#if PUBLIC
    public static
#else
    internal static partial
#endif
    class Json
    {
        public static TRoot ReplaceNode<TRoot>(this TRoot root, JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
            where TRoot : JsonSyntaxNode
        {
            if (oldNode == newNode)
                return root;

            return (TRoot)((IJsonSyntaxNode)root).ReplaceCore(oldNode, newNode);
        }
    }
}
