using System;
using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class SingleLineCommentTriviaSyntax : TriviaSyntaxNode
    {
        public SingleLineCommentTriviaSyntax(IEnumerable<char> rawValue)
            : base(rawValue)
        {
        }

        public SingleLineCommentTriviaSyntax WithText(string text) =>
            new SingleLineCommentTriviaSyntax(GetCommentedText(text));

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
}
