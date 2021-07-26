namespace RockLib.Analyzers.Json
{
    public enum JsonTokenType : byte
    {
        None,
        Whitespace,
        SingleLineComment,
        MultiLineComment,
        ObjectStart,
        ObjectEnd,
        ArrayStart,
        ArrayEnd,
        MemberSeparator,
        ItemSeparator,
        EscapedString,
        NonEscapedString,
        Number,
        True,
        False,
        Null
    }
}
