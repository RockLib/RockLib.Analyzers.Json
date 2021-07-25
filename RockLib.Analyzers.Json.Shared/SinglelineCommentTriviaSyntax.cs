using System;
using System.Collections.Generic;

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
        class SinglelineCommentTriviaSyntax : TriviaSyntaxNode
        {
            public SinglelineCommentTriviaSyntax(IEnumerable<char> rawValue)
                : base(rawValue)
            {
            }

            public SinglelineCommentTriviaSyntax WithText(string text) =>
                new SinglelineCommentTriviaSyntax(GetCommentedText(text));

            private static IEnumerable<char> GetCommentedText(string text)
            {
                yield return '/';
                yield return '/';

                foreach (var c in text)
                {
                    switch (c)
                    {
                        case '\n':
                        case '\r':
                        throw new ArgumentException("Cannot have any newline characters.", nameof(text));
                    }
                    yield return c;
                }
            }
        }
#if !PUBLIC
    }
#endif
}
