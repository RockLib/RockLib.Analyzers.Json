using System;
using System.Collections.Generic;
using System.Linq;

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
        class WhitespaceTriviaSyntax : TriviaSyntaxNode
        {
            public WhitespaceTriviaSyntax(IEnumerable<char> rawValue)
                : base(rawValue)
            {
            }

            public SinglelineCommentTriviaSyntax WithWhitespace(string whitespace)
            {
                if (!whitespace.All(char.IsWhiteSpace))
                    throw new ArgumentException("Must not contain any non-whitespace characters.", nameof(whitespace));

                return new SinglelineCommentTriviaSyntax(whitespace);
            }
        }
#if !PUBLIC
    }
#endif
}
