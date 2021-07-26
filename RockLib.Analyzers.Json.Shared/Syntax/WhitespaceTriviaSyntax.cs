using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class WhitespaceTriviaSyntax : TriviaSyntaxNode
    {
        public WhitespaceTriviaSyntax(IEnumerable<char> rawValue)
            : base(rawValue)
        {
        }

        public SingleLineCommentTriviaSyntax WithWhitespace(string whitespace)
        {
            if (!whitespace.All(char.IsWhiteSpace))
                throw new ArgumentException("Must not contain any non-whitespace characters.", nameof(whitespace));

            return new SingleLineCommentTriviaSyntax(whitespace);
        }
    }
}
