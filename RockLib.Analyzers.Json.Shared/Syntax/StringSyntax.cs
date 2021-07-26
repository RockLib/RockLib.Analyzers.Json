using System;
using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
    public abstract class StringSyntax : VerbatimSyntaxNode
    {
        protected StringSyntax(IEnumerable<char> rawValue,
            TriviaListSyntax leadingTrivia,
            TriviaListSyntax trailingTrivia)
            : base(rawValue, leadingTrivia, trailingTrivia)
        {
            if (rawValue is null)
                throw new ArgumentNullException(nameof(rawValue));
        }

        public abstract string Value { get; }

        public override bool IsValid
        {
            get
            {
                if (RawValue is null)
                    return false;

                try
                {
                    _ = Value;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override bool IsValueNode => true;
    }
}
