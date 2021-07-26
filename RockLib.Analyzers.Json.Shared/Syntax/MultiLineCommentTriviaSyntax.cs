using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class MultiLineCommentTriviaSyntax : TriviaSyntaxNode
    {
        public MultiLineCommentTriviaSyntax(IEnumerable<char> rawValue)
            : base(rawValue)
        {
        }

        public override bool IsValid =>
            RawValue != null
            && StartsWithMultiLineComment(RawValue)
            && StartsWithMultiLineComment(RawValue.Reverse());

        public override bool IsValueNode => true;

        public MultiLineCommentTriviaSyntax WithCommentText(string commentText) =>
            new MultiLineCommentTriviaSyntax(GetMultiLineCommentString(commentText));

        private static IEnumerable<char> GetMultiLineCommentString(string commentText)
        {
            yield return '/';
            yield return '*';

            foreach (var c in commentText)
                yield return c;

            yield return '*';
            yield return '/';
        }

        private static bool StartsWithMultiLineComment(IEnumerable<char> value)
        {
            var enumerator = value.GetEnumerator();
            try
            {
                return enumerator.MoveNext()
                    && enumerator.Current == '/'
                    && enumerator.MoveNext()
                    && enumerator.Current == '*';
            }
            finally
            {
                enumerator.Dispose();
            }
        }
    }
}
