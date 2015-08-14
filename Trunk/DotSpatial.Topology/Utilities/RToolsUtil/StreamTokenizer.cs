using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// A StreamTokenizer similar to Java's.  This breaks an input stream
    /// (coming from a TextReader) into Tokens based on various settings.  The settings
    /// are stored in the TokenizerSettings property, which is a StreamTokenizerSettings instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is configurable in that you can modify TokenizerSettings.CharTypes[] array
    /// to specify which characters are which type, along with other settings
    /// such as whether to look for comments or not.
    /// </para>
    /// <para>
    /// WARNING: This is not internationalized.  This treats all characters beyond
    /// the 7-bit ASCII range (decimal 127) as Word characters.
    /// </para>
    /// <para>
    /// There are two main ways to use this: 1) Parse the entire stream at
    /// once and get an ArrayList of Tokens (see the Tokenize* methods), 
    /// and 2) call NextToken() successively.
    /// This reads from a TextReader, which you can set directly, and this
    /// also provides some convenient methods to parse files and strings.
    /// This returns an Eof token if the end of the input is reached.
    /// </para>
    /// <para>
    /// Here's an example of the NextToken() endCapStyle of use:
    /// <code>
    /// StreamTokenizer tokenizer = new StreamTokenizer();
    /// tokenizer.GrabWhitespace = true;
    /// tokenizer.Verbosity = VerbosityLevel.Debug; // just for debugging
    /// tokenizer.TextReader = File.OpenText(fileName);
    /// Token token;
    /// while (tokenizer.NextToken(out token)) log.Info("Token = '{0}'", token);
    /// </code>
    /// </para>
    /// <para>
    /// Here's an example of the Tokenize... endCapStyle of use:
    /// <code>
    /// StreamTokenizer tokenizer = new StreamTokenizer("some string");
    /// ArrayList tokens = new ArrayList();
    /// if (!tokenizer.Tokenize(tokens)) 
    /// { 
    ///		// error handling
    /// }
    /// foreach (Token t in tokens) Console.WriteLine("t = {0}", t);
    /// </code>
    /// </para>
    /// <para>
    /// Comment delimiters are hardcoded (// and /*), not affected by char type table.
    /// </para>
    /// <para>
    /// This sets line numbers in the tokens it produces.  These numbers are normally
    /// the line on which the token starts.
    /// There is one known caveat, and that is that when GrabWhitespace setting
    /// is true, and a whitespace token contains a newline, that token's line number
    /// will be set to the following line rather than the line on which the token
    /// started.
    /// </para>
    /// </remarks>
    public class StreamTokenizer : IEnumerable<Token>
    {
        #region Fields

        /// <summary>
        /// This is the number of characters in the character table.
        /// </summary>
        public static readonly int NChars = 128;
        private static readonly int Eof = NChars;
        // used to back up in the stream
        private CharBuffer _backString;
        
        // keep track of current line number during parse
        private int _lineNumber;
        // used to collect characters of the current (next to be emitted) token
        private CharBuffer _nextTokenSb;
        
        // for speed, construct these once and re-use
        private CharBuffer _tmpSb;
        private CharBuffer _expSb;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StreamTokenizer()
        {
            Initialize();
        }

        /// <summary>
        /// Construct and set this object's TextReader to the one specified.
        /// </summary>
        /// <param name="sr">The TextReader to read from.</param>
        public StreamTokenizer(TextReader sr)
            : this()
        {
            TextReader = sr;
        }

        /// <summary>
        /// Construct and set a string to tokenize.
        /// </summary>
        /// <param name="str">The string to tokenize.</param>
        public StreamTokenizer(string str) : this(new StringReader(str)) { }

        #endregion

        #region Enums

        /// <summary>
        /// The states of the state machine.
        /// </summary>
        private enum NextTokenState
        {
            Start,
            Whitespace,
            Word,
            Quote,
            EndQuote,
            MaybeNumber, // could be number or word
            MaybeComment, // after first slash, might be comment or not
            MaybeHex, // after 0, may be hex
            HexGot0x, // after 0x, may be hex
            HexNumber,
            LineComment,
            BlockComment,
            EndBlockComment,
            Char,
            Eol,
            Eof,
            Invalid
        }

        #endregion

        #region Properties

        /// <summary>
        /// The settings which govern the behavior of the tokenization.
        /// </summary>
        public StreamTokenizerSettings Settings { get; private set; }

        /// <summary>
        /// This is the TextReader that this object will read from.
        /// Set this to set the input reader for the parse.
        /// </summary>
        public TextReader TextReader { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Display the state of this object.
        /// </summary>
        public void Display()
        {
            Display(string.Empty);
        }

        /// <summary>
        /// Display the state of this object, with a per-line prefix.
        /// </summary>
        /// <param name="prefix">The pre-line prefix.</param>
        public void Display(string prefix)
        {
            if (Settings != null) Settings.Display(prefix + "    ");
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Token> GetEnumerator()
        {
            Token token;
            _lineNumber = 1;

            while (NextToken(out token))
            {
                if (token == null) throw new NullReferenceException("StreamTokenizer: Tokenize: Got a null token from NextToken.");
                yield return token;
            }

            // add the last token returned (EOF)
            yield return token;
        }

        /// <summary>
        /// Get the next token.  The last token will be an EofToken unless
        /// there's an unterminated quote or unterminated block comment
        /// and Settings.DoUntermCheck is true, in which case this throws
        /// an exception of type StreamTokenizerUntermException or sub-class.
        /// </summary>
        /// <param name="token">The output token.</param>
        /// <returns>bool - true for success, false for failure.</returns>
        public bool NextToken(out Token token)
        {
            token = null;
            int thisChar; // current character
            byte ctype; // type of this character

            NextTokenState state = NextTokenState.Start;
            int prevChar = 0; // previous character

            // get previous char from nextTokenSb if there
            // (nextTokenSb is a StringBuilder containing the characters
            //  of the next token to be emitted)
            if (_nextTokenSb.Length > 0)
            {
                prevChar = _nextTokenSb[_nextTokenSb.Length - 1];
                byte prevCtype = Settings.CharTypes[prevChar];
                state = PickNextState(prevCtype, prevChar);
            }

            // extra state for number parse
            int seenDot = 0; // how many .'s in the number
            int seenE = 0; // how many e's or E's have we seen in the number
            bool seenDigit = false; // seen any digits (numbers can start with -)

            // lineNumber can change with each GetNextChar()
            // tokenLineNumber is the line on which the token started
            int tokenLineNumber = _lineNumber;

            // State Machine: Produces a single token.
            // Enter a state based on a single character.
            // Generally, being in a state means we're currently collecting chars in that type of token.
            // We do state machine until it builds a token (Eof is a token), then return that token.
            thisChar = prevChar;  // for first iteration, since prevChar is set to this 
            bool done = false; // optimization
            while (!done)
            {
                prevChar = thisChar;
                thisChar = GetNextChar();
                if (thisChar >= Settings.CharTypes.Length)
                {
                    // greater than 7-bit ascii, treat as word character
                    ctype = (byte)CharTypeBits.Word;
                }
                else ctype = Settings.CharTypes[thisChar];

                // see if we need to change states, or emit a token
                switch (state)
                {
                    case NextTokenState.Start:
                        // RESET
                        state = PickNextState(ctype, thisChar);
                        tokenLineNumber = _lineNumber;
                        break;

                    case NextTokenState.Char:
                        token = new CharToken((char)prevChar, tokenLineNumber);
                        done = true;
                        _nextTokenSb.Length = 0;
                        break;

                    case NextTokenState.Word:
                        if ((!Settings.IsCharType(ctype, CharTypeBits.Word))
                            && (!Settings.IsCharType(ctype, CharTypeBits.Digit)))
                        {
                            // end of word, emit
                            token = new WordToken(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        break;

                    case NextTokenState.Whitespace:
                        if (!Settings.IsCharType(ctype, CharTypeBits.Whitespace)
                            || (Settings.GrabEol && (thisChar == 10)))
                        {
                            // end of whitespace, emit
                            if (Settings.GrabWhitespace)
                            {
                                token = new WhitespaceToken(_nextTokenSb.ToString(), tokenLineNumber);
                                done = true;
                                _nextTokenSb.Length = 0;
                            }
                            else
                            {
                                // RESET
                                _nextTokenSb.Length = 0;
                                tokenLineNumber = _lineNumber;
                                state = PickNextState(ctype, thisChar);
                            }
                        }
                        break;

                    case NextTokenState.EndQuote:
                        // we're now 1 char after end of quote
                        token = new QuoteToken(_nextTokenSb.ToString(), tokenLineNumber);
                        done = true;
                        _nextTokenSb.Length = 0;
                        break;

                    case NextTokenState.Quote:
                        // looking for end quote matching char that started the quote
                        if (thisChar == _nextTokenSb[0])
                        {
                            // handle escaped backslashes: count the immediately prior backslashes 
                            // - even (including 0) means it's not escaped 
                            // - odd means it is escaped 
                            int backSlashCount = 0;
                            for (int i = _nextTokenSb.Length - 1; i >= 0; i--)
                            {
                                if (_nextTokenSb[i] == '\\') backSlashCount++;
                                else break;
                            }

                            if ((backSlashCount % 2) == 0)
                            {
                                state = NextTokenState.EndQuote;
                            }
                        }

                        if ((state != NextTokenState.EndQuote) && (thisChar == Eof))
                        {
                            if (Settings.DoUntermCheck)
                            {
                                _nextTokenSb.Length = 0;
                                throw new StreamTokenizerUntermQuoteException("Unterminated quote");
                            }

                            token = new QuoteToken(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        break;

                    case NextTokenState.MaybeComment:
                        if (thisChar == Eof)
                        {
                            token = new CharToken(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        else
                        {
                            // if we get the right char, we're in a comment
                            if (Settings.SlashSlashComments && (thisChar == '/'))
                                state = NextTokenState.LineComment;
                            else if (Settings.SlashStarComments && (thisChar == '*'))
                                state = NextTokenState.BlockComment;
                            else
                            {
                                token = new CharToken(_nextTokenSb.ToString(), tokenLineNumber);
                                done = true;
                                _nextTokenSb.Length = 0;
                            }
                        }
                        break;

                    case NextTokenState.LineComment:
                        if (thisChar == Eof)
                        {
                            if (Settings.GrabComments)
                            {
                                token = new CommentToken(_nextTokenSb.ToString(), tokenLineNumber);
                                done = true;
                                _nextTokenSb.Length = 0;
                            }
                            else
                            {
                                // RESET
                                _nextTokenSb.Length = 0;
                                tokenLineNumber = _lineNumber;
                                state = PickNextState(ctype, thisChar);
                            }
                        }
                        else
                        {
                            if (thisChar == '\n')
                            {
                                if (Settings.GrabComments)
                                {
                                    token = new CommentToken(_nextTokenSb.ToString(), tokenLineNumber);
                                    done = true;
                                    _nextTokenSb.Length = 0;
                                }
                                else
                                {
                                    // RESET
                                    _nextTokenSb.Length = 0;
                                    tokenLineNumber = _lineNumber;
                                    state = PickNextState(ctype, thisChar);
                                }
                            }
                        }
                        break;

                    case NextTokenState.BlockComment:
                        if (thisChar == Eof)
                        {
                            if (Settings.DoUntermCheck)
                            {
                                _nextTokenSb.Length = 0;
                                throw new StreamTokenizerUntermCommentException("Unterminated comment.");
                            }

                            if (Settings.GrabComments)
                            {
                                token = new CommentToken(_nextTokenSb.ToString(), tokenLineNumber);
                                done = true;
                                _nextTokenSb.Length = 0;
                            }
                            else
                            {
                                // RESET
                                _nextTokenSb.Length = 0;
                                tokenLineNumber = _lineNumber;
                                state = PickNextState(ctype, thisChar);
                            }
                        }
                        else
                        {
                            if ((thisChar == '/') && (prevChar == '*'))
                                state = NextTokenState.EndBlockComment;
                        }
                        break;

                    // special case for 2-character token termination
                    case NextTokenState.EndBlockComment:
                        if (Settings.GrabComments)
                        {
                            token = new CommentToken(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        else
                        {
                            // RESET
                            _nextTokenSb.Length = 0;
                            tokenLineNumber = _lineNumber;
                            state = PickNextState(ctype, thisChar);
                        }
                        break;

                    case NextTokenState.MaybeHex:
                        // previous char was 0
                        if (thisChar != 'x')
                        {
                            // back up and try non-hex
                            // back up to the 0
                            _nextTokenSb.Append((char)thisChar);
                            _backString.Append(_nextTokenSb);
                            _nextTokenSb.Length = 0;

                            // reset state and don't choose MaybeNumber state.
                            // pull char from backString
                            thisChar = _backString[0];
                            _backString.Remove(0, 1);
                            state = PickNextState(Settings.CharTypes[thisChar], thisChar,
                                NextTokenState.MaybeHex);
                        }
                        else state = NextTokenState.HexGot0x;
                        break;

                    case NextTokenState.HexGot0x:
                        if (!Settings.IsCharType(ctype, CharTypeBits.HexDigit))
                        {
                            // got 0x but now a non-hex char
                            // back up to the 0
                            _nextTokenSb.Append((char)thisChar);
                            _backString.Append(_nextTokenSb);
                            _nextTokenSb.Length = 0;

                            // reset state and don't choose MaybeNumber state.
                            // pull char from backString
                            thisChar = _backString[0];
                            _backString.Remove(0, 1);
                            state = PickNextState(Settings.CharTypes[thisChar], thisChar,
                                NextTokenState.MaybeHex);
                        }
                        else state = NextTokenState.HexNumber;
                        break;

                    case NextTokenState.HexNumber:
                        if (!Settings.IsCharType(ctype, CharTypeBits.HexDigit))
                        {
                            // emit the hex number we've collected
                            token = IntToken.ParseHex(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        break;

                    case NextTokenState.MaybeNumber:
                        // Determine whether or not to stop collecting characters for the number parse.
                        // We terminate when it's clear it's not a number or no longer a number.
                        bool term = false;

                        if (Settings.IsCharType(ctype, CharTypeBits.Digit)
                            || Settings.IsCharType(prevChar, CharTypeBits.Digit)) seenDigit = true;

                        // term conditions
                        if (thisChar == '.')
                        {
                            seenDot++;
                            if (seenDot > 1) term = true;  // more than one dot, it aint a number
                        }
                        else if (((thisChar == 'e') || (thisChar == 'E')))
                        {
                            seenE++;
                            if (!seenDigit) term = true;  // e before any digits is bad
                            else if (seenE > 1) term = true;  // more than 1 e is bad
                            else
                            {
                                term = true; // done regardless

                                // scan the exponent, put its characters into nextTokenSb, if there are any
                                char c;
                                _expSb.Clear();
                                _expSb.Append((char)thisChar);
                                if (GrabInt(_expSb, true, out c))
                                {
                                    // we got a good exponent, tack it on
                                    _nextTokenSb.Append(_expSb);
                                    thisChar = c; // and continue after the exponent's characters
                                }
                            }
                        }
                        else if (thisChar == Eof) term = true;
                        // or a char that can't be in a number
                        else if ((!Settings.IsCharType(ctype, CharTypeBits.Digit)
                            && (thisChar != 'e') && (thisChar != 'E')
                            && (thisChar != '-') && (thisChar != '.'))
                            || ((thisChar == '+') && (seenE == 0)))
                        {
                            // it's not a normal number character
                            term = true;
                        }
                        // or a dash not after e
                        else if ((thisChar == '-') && (!((prevChar == 'e') || (prevChar == 'E')))) term = true;

                        if (!term) break;

                        // we are terminating a number, or it wasn't a number
                        if (seenDigit)
                        {
                            if ((_nextTokenSb.IndexOf('.') >= 0) || (_nextTokenSb.IndexOf('e') >= 0)
                               || (_nextTokenSb.IndexOf('E') >= 0) || (_nextTokenSb.Length >= 19)) // probably too large for Int64, use float
                            {
                                token = new FloatToken(_nextTokenSb.ToString(), tokenLineNumber);
                            }
                            else
                                token = new IntToken(_nextTokenSb.ToString(), tokenLineNumber);
                            done = true;
                            _nextTokenSb.Length = 0;
                        }
                        else
                        {
                            // -whatever or -.whatever
                            // didn't see any digits, must have gotten here by a leading -
                            // and no digits after it
                            // back up to -, pick next state excluding numbers
                            _nextTokenSb.Append((char)thisChar);
                            _backString.Append(_nextTokenSb);
                            _nextTokenSb.Length = 0;

                            // restart on the - and don't choose MaybeNumber state
                            // pull char from backString
                            thisChar = _backString[0];
                            _backString.Remove(0, 1);
                            state = PickNextState(Settings.CharTypes[thisChar], thisChar, NextTokenState.MaybeNumber);
                        }

                        break;

                    case NextTokenState.Eol:
                        // tokenLineNumber - 1 because the newline char is on the previous line
                        token = new EolToken(tokenLineNumber - 1);
                        done = true;
                        _nextTokenSb.Length = 0;
                        break;

                    case NextTokenState.Eof:
                        token = new EofToken(tokenLineNumber);
                        done = true;
                        _nextTokenSb.Length = 0;
                        return (false);

                    case NextTokenState.Invalid:
                    default:
                        // not a good sign, some unrepresented state?
                        return (false);
                }

                // use a StringBuilder to accumulate characters which are part of this token
                if (thisChar != Eof) _nextTokenSb.Append((char)thisChar);
            }

            return (true);
        }

        /// <summary>
        /// Parse the rest of the stream and put all the tokens
        /// in the input ArrayList. This resets the line number to 1.
        /// </summary>
        /// <param name="tokens">The ArrayList to append to.</param>
        /// <returns>bool - true for success</returns>
        public bool Tokenize(IList<Token> tokens)
        {
            Token token;
            _lineNumber = 1;

            while (NextToken(out token))
            {
                if (token == null) throw new NullReferenceException("StreamTokenizer: Tokenize: Got a null token from NextToken.");
                tokens.Add(token);
            }

            // add the last token returned (EOF)
            tokens.Add(token);
            return (true);
        }

        /// <summary>
        /// Parse all tokens from the specified file, put
        /// them into the input ArrayList.
        /// </summary>
        /// <param name="fileName">The file to read.</param>
        /// <param name="tokens">The ArrayList to put tokens in.</param>
        /// <returns>bool - true for success, false for failure.</returns>
        public bool TokenizeFile(string fileName, IList<Token> tokens)
        {
            FileInfo fi = new FileInfo(fileName);
            FileStream fr = null;
            try
            {
                fr = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                TextReader = new StreamReader(fr);
            }
            catch (DirectoryNotFoundException)
            {
            }
            try
            {
                if (!Tokenize(tokens))
                {
                    TextReader.Close();
                    if (fr != null) fr.Close();
                    return false;
                }
            }
            catch (StreamTokenizerUntermException e)
            {
                TextReader.Close();
                if (fr != null) fr.Close();
                throw e;
            }

            if (TextReader != null) TextReader.Close();
            if (fr != null) fr.Close();
            return true;
        }

        /// <summary>
        /// Tokenize a file completely and return the tokens in a Token[].
        /// </summary>
        /// <param name="fileName">The file to tokenize.</param>
        /// <returns>A Token[] with all tokens.</returns>
        public Token[] TokenizeFile(string fileName)
        {
            var list = new List<Token>();
            if (!TokenizeFile(fileName, list)) return null;
            return list.Count > 0 ? list.ToArray() : null;
        }

        /// <summary>
        /// Parse all tokens from the specified TextReader, put
        /// them into the input ArrayList.
        /// </summary>
        /// <param name="tr">The TextReader to read from.</param>
        /// <param name="tokens">The ArrayList to append to.</param>
        /// <returns>bool - true for success, false for failure.</returns>
        public bool TokenizeReader(TextReader tr, IList<Token> tokens)
        {
            TextReader = tr;
            return (Tokenize(tokens));
        }

        /// <summary>
        /// Parse all tokens from the specified Stream, put
        /// them into the input ArrayList.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="tokens">The ArrayList to put tokens in.</param>
        /// <returns>bool - true for success, false for failure.</returns>
        public bool TokenizeStream(Stream s, IList<Token> tokens)
        {
            TextReader = new StreamReader(s);
            return (Tokenize(tokens));
        }

        /// <summary>
        /// Parse all tokens from the specified string, put
        /// them into the input ArrayList.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tokens">The ArrayList to put tokens in.</param>
        /// <returns>bool - true for success, false for failure.</returns>
        public bool TokenizeString(string str, IList<Token> tokens)
        {
            TextReader = new StringReader(str);
            return (Tokenize(tokens));
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Read the next character from the stream, or from backString
        /// if we backed up.
        /// </summary>
        /// <returns>The next character.</returns>
        private int GetNextChar()
        {
            int c;

            // consume from backString if possible
            if (_backString.Length > 0)
            {
                c = _backString[0];
                _backString.Remove(0, 1);
                return (c);
            }

            if (TextReader == null) return (Eof);

            try
            {
                while ((c = TextReader.Read()) == 13) { } // skip LF (13)
            }
            catch (Exception)
            {
                return (Eof);
            }

            if (c == 10)
                _lineNumber++;
            else if (c < 0)
                c = Eof;

            return (c);
        }

        /// <summary>
        /// Starting from current stream location, scan forward
        /// over an int.  Determine whether it's an integer or not.  If so, 
        /// push the integer characters to the specified CharBuffer.  
        /// If not, put them in backString (essentially leave the
        /// stream as it was) and return false.
        /// <para>
        /// If it was an int, the stream is left 1 character after the
        /// end of the int, and that character is output in the thisChar parameter.
        /// </para>
        /// <para>The formats for integers are: 1, +1, and -1</para>
        /// The + and - signs are included in the output buffer.
        /// </summary>
        /// <param name="sb">The CharBuffer to append to.</param>
        /// <param name="allowPlus">Whether or not to consider + to be part
        /// of an integer.</param>
        /// <param name="thisChar">The last character read by this method.</param>
        /// <returns>true for parsed an int, false for not an int</returns>
        private bool GrabInt(CharBuffer sb, bool allowPlus, out char thisChar)
        {
            _tmpSb.Clear(); // use tmp CharBuffer

            // first character can be -, maybe can be + depending on arg
            thisChar = (char)GetNextChar();
            if (thisChar == Eof) return false;
            else if (thisChar == '+')
            {
                if (allowPlus)
                    _tmpSb.Append(thisChar);
                else
                {
                    _backString.Append(thisChar);
                    return false;
                }
            }
            else if (thisChar == '-')
            {
                _tmpSb.Append(thisChar);
            }
            else if (Settings.IsCharType(thisChar, CharTypeBits.Digit))
            {
                // a digit, back this out so we can handle it in loop below
                _backString.Append(thisChar);
            }
            else
            {
                // not a number starter
                _backString.Append(thisChar);
                return false;
            }

            // rest of chars have to be digits
            bool gotInt = false;
            while (((thisChar = (char)GetNextChar()) != Eof) && (Settings.IsCharType(thisChar, CharTypeBits.Digit)))
            {
                gotInt = true;
                _tmpSb.Append(thisChar);
            }

            if (gotInt)
            {
                sb.Append(_tmpSb);
                return true;
            }

            // didn't get any chars after first 
            _backString.Append(_tmpSb); // put + or - back on
            if (thisChar != Eof) _backString.Append(thisChar);
            return false;
        }

        /// <summary>
        /// Utility function, things common to constructors.
        /// </summary>
        void Initialize()
        {
            _backString = new CharBuffer(32);
            _nextTokenSb = new CharBuffer(1024);

            InitializeStream();
            Settings = new StreamTokenizerSettings();
            Settings.SetDefaults();

            _expSb = new CharBuffer();
            _tmpSb = new CharBuffer();
        }

        /// <summary>
        /// Clear the stream settings.
        /// </summary>
        void InitializeStream()
        {
            _lineNumber = 1; // base 1 line numbers
            TextReader = null;
        }

        /// <summary>
        /// Pick the next state given just a single character.  This is used
        /// at the start of a new token.
        /// </summary>
        /// <param name="ctype">The type of the character.</param>
        /// <param name="c">The character.</param>
        /// <returns>The state.</returns>
        private NextTokenState PickNextState(byte ctype, int c)
        {
            return PickNextState(ctype, c, NextTokenState.Start);
        }

        /// <summary>
        /// Pick the next state given just a single character.  This is used
        /// at the start of a new token.
        /// </summary>
        /// <param name="ctype">The type of the character.</param>
        /// <param name="c">The character.</param>
        /// <param name="excludeState">Exclude this state from the possible next state.</param>
        /// <returns>The state.</returns>
        private NextTokenState PickNextState(byte ctype, int c, NextTokenState excludeState)
        {
            if (c == '/')
                return (NextTokenState.MaybeComment); // overrides all other cats
            else if ((excludeState != NextTokenState.MaybeHex) && Settings.ParseHexNumbers && (c == '0'))
                return (NextTokenState.MaybeHex);
            else if ((excludeState != NextTokenState.MaybeNumber) && Settings.ParseNumbers
                    && (Settings.IsCharType(ctype, CharTypeBits.Digit) || (c == '-') || (c == '.')))
                return (NextTokenState.MaybeNumber);
            else if (Settings.IsCharType(ctype, CharTypeBits.Word)) return (NextTokenState.Word);
            else if (Settings.GrabEol && (c == 10)) return (NextTokenState.Eol);
            else if (Settings.IsCharType(ctype, CharTypeBits.Whitespace)) return (NextTokenState.Whitespace);
            else if (Settings.IsCharType(ctype, CharTypeBits.Comment)) return (NextTokenState.LineComment);
            else if (Settings.IsCharType(ctype, CharTypeBits.Quote)) return (NextTokenState.Quote);
            else if ((c == Eof) || (Settings.IsCharType(ctype, CharTypeBits.Eof))) return (NextTokenState.Eof);
            return (NextTokenState.Char);
        }

        #endregion
    }
}


