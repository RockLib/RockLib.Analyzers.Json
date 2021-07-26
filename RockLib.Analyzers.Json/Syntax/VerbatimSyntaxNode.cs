using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class VerbatimSyntaxNode : JsonSyntaxNode
    {
        protected VerbatimSyntaxNode(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia,
            TriviaListSyntax trailingTrivia)
        {
            RawValue = rawValue;
            LeadingTrivia = leadingTrivia;
            TrailingTrivia = trailingTrivia;
        }

        public IEnumerable<char> RawValue { get; }

        public string RawValueString => RawValue is null ? null : new string(RawValue.ToArray());

        public TriviaListSyntax LeadingTrivia { get; }

        public TriviaListSyntax TrailingTrivia { get; }

        public override bool HasLeadingTrivia => LeadingTrivia != null && LeadingTrivia.Items.Count > 0;

        public override bool HasTrailingTrivia => TrailingTrivia != null && TrailingTrivia.Items.Count > 0;

        internal override IEnumerable<char> GetJsonDocumentChars()
        {
            var chars = RawValue;

            if (LeadingTrivia != null)
            {
                var triviaChars = LeadingTrivia.GetJsonDocumentChars();
                if (chars is null)
                    chars = triviaChars;
                else if (!ReferenceEquals(triviaChars, Enumerable.Empty<char>()))
                    chars = triviaChars.Concat(chars);
            }

            if (TrailingTrivia != null)
            {
                var triviaChars = TrailingTrivia.GetJsonDocumentChars();
                if (chars is null)
                    chars = triviaChars;
                else if (!ReferenceEquals(triviaChars, Enumerable.Empty<char>()))
                    chars = chars.Concat(triviaChars);
            }

            return chars ?? Enumerable.Empty<char>();
        }
    }
}
