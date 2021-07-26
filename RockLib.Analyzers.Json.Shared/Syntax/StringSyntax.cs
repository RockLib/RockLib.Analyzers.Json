using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public abstract class StringSyntax : VerbatimSyntaxNode
    {
        protected StringSyntax(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia,
            TriviaListSyntax trailingTrivia)
            : base(rawValue, leadingTrivia, trailingTrivia)
        {
            if (!rawValue.EndsWith('"'))
                throw new Exception("Invalid value: " + new string(rawValue.ToArray()));
        }

        public abstract string Value { get; }
    }
}
