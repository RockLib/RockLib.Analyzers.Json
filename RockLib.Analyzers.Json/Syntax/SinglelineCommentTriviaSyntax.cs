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

        public override bool IsValid => IsSingleLineComment(RawValue);

        public override bool IsValueNode => true;

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

        private static bool IsSingleLineComment(IEnumerable<char> value)
        {
            if (value is null)
                return false;

            var enumerator = value.GetEnumerator();
            try
            {
                if (enumerator.MoveNext()
                    && enumerator.Current == '/'
                    && enumerator.MoveNext()
                    && enumerator.Current == '/')
                {
                    while (enumerator.MoveNext())
                        if (enumerator.Current == '\r' || enumerator.Current == '\n')
                            return false;
                    return true;
                }
                return false;
            }
            finally
            {
                enumerator.Dispose();
            }
        }
    }
}
