using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
#if PUBLIC
    public
#else
    internal
#endif
    class ExponentSyntax : VerbatimSyntaxNode
    {
        private static readonly IEnumerable<char> _eLowerCase = new[] { 'e' };
        private static readonly IEnumerable<char> _eUpperCase = new[] { 'E' };

        internal ExponentSyntax(bool lowerCase, IEnumerable<TriviaSyntaxNode> leadingTrivia, IEnumerable<TriviaSyntaxNode> trailingTrivia)
            : base(lowerCase ? _eLowerCase : _eUpperCase, leadingTrivia, trailingTrivia)
        {
        }
    }
}
