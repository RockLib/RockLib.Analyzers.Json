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

        public string RawValueString => new string(RawValue.ToArray());

        public TriviaListSyntax LeadingTrivia { get; }

        public TriviaListSyntax TrailingTrivia { get; }

        public override bool HasTrivia => (LeadingTrivia != null && LeadingTrivia.Children.Count > 0)
            || (TrailingTrivia != null && TrailingTrivia.Children.Count > 0);

        public override IEnumerable<char> GetChars()
        {
            var chars = RawValue;

            if (LeadingTrivia != null)
                chars = LeadingTrivia.GetChars().Concat(chars);

            if (TrailingTrivia != null)
                chars = chars.Concat(TrailingTrivia.GetChars());

            return chars;
        }
    }
}
