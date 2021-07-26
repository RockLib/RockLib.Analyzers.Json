using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public class MultiLineCommentTriviaSyntax : TriviaSyntaxNode
    {
        public MultiLineCommentTriviaSyntax(IEnumerable<char> rawValue)
            : base(rawValue)
        {
        }

        public MultiLineCommentTriviaSyntax WithText(string text) =>
            new MultiLineCommentTriviaSyntax(GetCommentedText(text));

        private static IEnumerable<char> GetCommentedText(string text)
        {
            yield return '/';
            yield return '*';

            foreach (var c in text)
                yield return c;

            yield return '*';
            yield return '/';
        }
    }
}
