namespace RockLib.Analyzers.Json
{
    internal interface IJsonSyntaxNode
    {
        JsonSyntaxNode WithLeadingTriviaCore(TriviaListSyntax triviaList);

        JsonSyntaxNode WithTrailingTriviaCore(TriviaListSyntax triviaList);

        JsonSyntaxNode ReplaceCore(JsonSyntaxNode oldNode, JsonSyntaxNode newNode);
    }
}
