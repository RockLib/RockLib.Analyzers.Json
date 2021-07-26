using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public static class JsonExtensions
    {
        public static TRoot WithLeadingTrivia<TRoot>(this TRoot node, TriviaListSyntax triviaList)
            where TRoot : JsonSyntaxNode
        {
            return (TRoot)((IJsonSyntaxNode)node).WithLeadingTriviaCore(triviaList);
        }

        public static TRoot WithTrailingTrivia<TRoot>(this TRoot node, TriviaListSyntax triviaList)
            where TRoot : JsonSyntaxNode
        {
            return (TRoot)((IJsonSyntaxNode)node).WithTrailingTriviaCore(triviaList);
        }

        public static TRoot ReplaceNode<TRoot>(this TRoot root, JsonSyntaxNode oldNode, JsonSyntaxNode newNode)
            where TRoot : JsonSyntaxNode
        {
            if (oldNode == newNode)
                return root;

            return (TRoot)((IJsonSyntaxNode)root).ReplaceCore(oldNode, newNode);
        }

        public static bool EndsWith(this IEnumerable<char> value, char endChar)
        {
            char last = default(char);

            foreach (var c in value)
                last = c;

            return last == endChar;
        }

        public static bool EqualsSlice(this IEnumerable<char> value, IEnumerable<char> comparison)
        {
            var valueEnumerator = value.GetEnumerator();
            var comparisonEnumerator = comparison.GetEnumerator();

            while (true)
            {
                var valueHasCurrent = valueEnumerator.MoveNext();
                var comparisonHasCurrent = comparisonEnumerator.MoveNext();
                
                // Different number of characters
                if (valueHasCurrent != comparisonHasCurrent)
                    return false;

                // We made it to the end of the slice
                if (!valueHasCurrent)
                    return true;

                // The values are different
                if (valueEnumerator.Current != comparisonEnumerator.Current)
                    return false;
            }
        }

        public static IEnumerable<char> Slice(this string value, int start)
        {
            var end = value.Length;

            for (int i = start; i < end; i++)
                yield return value[i];
        }

        public static IEnumerable<char> Slice(this string value, int start, int length)
        {
            var end = start + length;

            for (int i = start; i < end; i++)
                yield return value[i];
        }

        public static IEnumerable<char> Slice(this IEnumerable<char> value, int start)
        {
            var enumerator = value.GetEnumerator();

            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (i >= start)
                    yield return enumerator.Current;
            }
        }

        public static IEnumerable<char> Slice(this IEnumerable<char> value, int start, int length)
        {
            var enumerator = value.GetEnumerator();

            for (int i = 0; enumerator.MoveNext() && i < length; i++)
            {
                if (i >= start)
                    yield return enumerator.Current;
            }
        }
    }
}
