using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Analyzers.Json
{
    public class JsonReader
    {
        private readonly string _buffer;

        private JsonTokenType _tokenType;
        private int _currentIndex;
        private int _start;
        private int _length;

        public JsonReader(string json)
        {
            _buffer = json;
            _tokenType = JsonTokenType.None;
            _currentIndex = -1;
            _start = -1;
            _length = 0;
        }

        public JsonTokenType TokenType => _tokenType;

        public int CurrentIndex => _currentIndex;

        public int CurrentLength => _length;

        public IEnumerable<char> Current => _buffer.Slice(_start, _length);

        public string CurrentString => new string(Current.ToArray());

        public bool Read()
        {
            if (++_currentIndex >= _buffer.Length)
            {
                _tokenType = JsonTokenType.None;
                return false;
            }

            switch (_buffer[_currentIndex])
            {
                case '{':
                    _tokenType = JsonTokenType.ObjectStart;
                    _start = _currentIndex;
                    _length = 1;
                    break;
                case '}':
                    _tokenType = JsonTokenType.ObjectEnd;
                    _start = _currentIndex;
                    _length = 1;
                    break;
                case '[':
                    _tokenType = JsonTokenType.ArrayStart;
                    _length = 1;
                    break;
                case ']':
                    _tokenType = JsonTokenType.ArrayEnd;
                    _start = _currentIndex;
                    _length = 1;
                    break;
                case ',':
                    _tokenType = JsonTokenType.ItemSeparator;
                    _start = _currentIndex;
                    _length = 1;
                    break;
                case ':':
                    _tokenType = JsonTokenType.MemberSeparator;
                    _start = _currentIndex;
                    _length = 1;
                    break;
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    ReadWhitespace();
                    break;
                case '"':
                    ReadString();
                    break;
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumber();
                    break;
                case 't':
                    ReadLiteral("true", JsonTokenType.True);
                    break;
                case 'f':
                    ReadLiteral("false", JsonTokenType.False);
                    break;
                case 'n':
                    ReadLiteral("null", JsonTokenType.Null);
                    break;
                case '/':
                    ReadComment();
                    break;
                default:
                    throw new Exception($"Unknown token '{_buffer[_currentIndex]}' at index {_currentIndex}.");
            }

            return true;
        }

        private void ReadWhitespace()
        {
            _tokenType = JsonTokenType.Whitespace;
            _start = _currentIndex;
            _length = 0;

            while (true)
            {
                _length++;

                if (_currentIndex + 1 >= _buffer.Length)
                    break;
                else if (char.IsWhiteSpace(_buffer[_currentIndex + 1]))
                    _currentIndex++;
                else
                    break;
            }
        }

        private void ReadString()
        {
            _tokenType = JsonTokenType.NonEscapedString;
            _start = _currentIndex;

            _currentIndex++;
            _length = 1;

            while (true)
            {
                if (_currentIndex + 1 >= _buffer.Length)
                    return;

                switch (_buffer[_currentIndex])
                {
                    default:
                        _currentIndex++;
                        _length++;
                        break;
                    case '"':
                        _length++;
                        return;
                    case '\\':
                        _tokenType = JsonTokenType.EscapedString;
                        _currentIndex++;
                        _length++;

                        switch (_buffer[_currentIndex])
                        {
                            case '"':
                            case '\\':
                            case '/':
                            case 'b':
                            case 'f':
                            case 'n':
                            case 'r':
                            case 't':
                                if (_currentIndex + 1 >= _buffer.Length)
                                    return;

                                _currentIndex++;
                                _length++;
                                break;
                            case 'u':
                                _currentIndex++;
                                _length++;

                                for (int i = 0; i < 4; i++)
                                {
                                    if (_currentIndex + 1 >= _buffer.Length)
                                        return;

                                    switch (_buffer[_currentIndex])
                                    {
                                        default:
                                            throw new Exception("Parse error. Expected hex digit.");
                                        case '0':
                                        case '1':
                                        case '2':
                                        case '3':
                                        case '4':
                                        case '5':
                                        case '6':
                                        case '7':
                                        case '8':
                                        case '9':
                                        case 'a':
                                        case 'b':
                                        case 'c':
                                        case 'd':
                                        case 'e':
                                        case 'f':
                                        case 'A':
                                        case 'B':
                                        case 'C':
                                        case 'D':
                                        case 'E':
                                        case 'F':
                                            _currentIndex++;
                                            _length++;
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void ReadNumber()
        {
            _tokenType = JsonTokenType.Number;
            _start = _currentIndex;
            _length = 0;

            if (_buffer[_currentIndex] == '-')
            {
                _currentIndex++;
                _length++;
            }

            ReadDigits();

            if (_buffer[_currentIndex] == '.')
            {
                _currentIndex++;
                _length++;
                ReadDigits();
            }

            var current = _buffer[_currentIndex];
            if (current == 'e' || current == 'E')
            {
                _currentIndex++;
                _length++;

                current = _buffer[_currentIndex];
                if (current == '-' || current == '+')
                {
                    _currentIndex++;
                    _length++;
                }
                ReadDigits();
            }
        }

        private void ReadDigits()
        {
            while (true)
            {
                if (_currentIndex + 1 >= _buffer.Length)
                    return;

                if (char.IsDigit(_buffer[_currentIndex]))
                {
                    _currentIndex++;
                    _length++;
                }
                else
                    return;
            }
        }

        private void ReadLiteral(string literalValue, JsonTokenType tokenType)
        {
            _tokenType = tokenType;
            _start = _currentIndex;
            _length = 0;

            for (int i = 0; i < literalValue.Length; i++)
            {
                if (_buffer[_currentIndex] != literalValue[i])
                    return;

                _length++;

                if (_length == literalValue.Length || _currentIndex + 1 >= _buffer.Length)
                    return;

                _currentIndex++;
            }
        }

        private void ReadComment()
        {
            _start = _currentIndex;
            _length = 1;

            if (_currentIndex + 1 >= _buffer.Length)
                return;

            _currentIndex++;

            switch (_buffer[_currentIndex])
            {
                case '/':
                    ReadSingleLineComment();
                    break;
                case '*':
                    ReadMultiLineComment();
                    break;
                default:
                    return;
            }
        }

        private void ReadSingleLineComment()
        {
            _tokenType = JsonTokenType.SingleLineComment;
            _length++;

            if (_currentIndex + 1 >= _buffer.Length)
                return;

            _currentIndex++;

            while (true)
            {
                if (_currentIndex + 1 >= _buffer.Length)
                    return;

                switch (_buffer[_currentIndex])
                {
                    default:
                        _currentIndex++;
                        _length++;
                        break;
                    case '\r':
                    case '\n':
                        _currentIndex--;
                        return;
                }
            }
        }

        private void ReadMultiLineComment()
        {
            _tokenType = JsonTokenType.MultiLineComment;
            _length++;

            if (_currentIndex + 1 >= _buffer.Length)
                return;

            _currentIndex++;

            while (true)
            {
                if (_currentIndex + 1 >= _buffer.Length)
                    return;

                switch (_buffer[_currentIndex])
                {
                    default:
                        _currentIndex++;
                        _length++;
                        break;
                    case '*':
                        _length++;

                        if (_currentIndex + 1 >= _buffer.Length)
                            return;

                        _currentIndex++;

                        if (_buffer[_currentIndex] == '/')
                        {
                            _length++;
                            return;
                        }
                        else
                        {
                            _currentIndex--;
                            break;
                        }
                }
            }
        }

        //public string UnescapeStringValue()
        //{
        //    var sb = new StringBuilder(); // TODO: Use object pool

        //    for (int i = _start; i < _start + _length; i++)
        //    {
        //        if (_buffer[i] != '\\')
        //            sb.Append(_buffer[i]);
        //        else
        //        {
        //            switch (_buffer[i + 1])
        //            {
        //                case '"':
        //                case '\\':
        //                case '/':
        //                    sb.Append(_buffer[i + 1]);
        //                    break;
        //                case 'b':
        //                    sb.Append('\b');
        //                    break;
        //                case 'f':
        //                    sb.Append('\f');
        //                    break;
        //                case 'n':
        //                    sb.Append('\n');
        //                    break;
        //                case 'r':
        //                    sb.Append('\r');
        //                    break;
        //                case 't':
        //                    sb.Append('\t');
        //                    break;
        //                case 'u':
        //                    sb.Append(char.Parse(new string(_buffer.Slice(i, 6).ToArray())));
        //                    break;
        //            }
        //        }
        //    }

        //    return sb.ToString();
        //}
    }
}
