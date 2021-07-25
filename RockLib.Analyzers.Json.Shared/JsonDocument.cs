using System;
using System.Collections.Generic;

namespace RockLib.Analyzers.Json
{
#if PUBLIC
    public
#else
    internal
#endif
    class JsonDocument
    {
        private readonly string _json;
        private readonly char[] _jsonCharArray;
        private readonly int _jsonCharsLength;

        internal JsonDocument(string json)
        {
            _json = json;
            _jsonCharArray = _json.ToCharArray();
            _jsonCharsLength = _jsonCharArray.Length;
        }

        public string Json => _json;

        public IEnumerable<char> Slice(int start)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Cannot be negative.");
            if (start >= _jsonCharsLength)
                throw new ArgumentOutOfRangeException(nameof(start), "Cannot be greater than or equal to the length of the json string.");

            var end = _jsonCharsLength;
            for (int i = start; i < end; i++)
                yield return _jsonCharArray[i];
        }

        public IEnumerable<char> Slice(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Cannot be negative.");
            if (start >= _jsonCharsLength)
                throw new ArgumentOutOfRangeException(nameof(start), "Cannot be greater than or equal to the length of the json string.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Cannot be negative.");

            var end = start + length;

            if (end >= _jsonCharsLength)
                throw new ArgumentOutOfRangeException(nameof(length), "The sum of the 'start' and 'length' parameters cannot be greater than or equal to the length of the json string.");

            for (int i = start; i < end; i++)
                yield return _jsonCharArray[i];
        }
    }
}
