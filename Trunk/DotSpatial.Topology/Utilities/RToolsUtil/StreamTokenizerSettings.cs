using System;
using System.Text;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// This contains the settings that control the behavior of the tokenizer.
    /// This is separated from the StreamTokenizer so that common settings
    /// are easy to package and keep together.
    /// </summary>
    [Serializable]
    public class StreamTokenizerSettings
    {
        #region Fields

        private bool _parseHexNumbers;
        private bool _parseNumbers;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StreamTokenizerSettings()
        {
            CharTypes = new byte[StreamTokenizer.NChars + 1];  // plus an EOF entry
            SetDefaults();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public StreamTokenizerSettings(StreamTokenizerSettings other)
        {
            Copy(other);
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is the character type table.  Each byte is bitwise encoded
        /// with the character attributes, such as whether that character is
        /// word or whitespace.
        /// </summary>
        public byte[] CharTypes { get; private set; }

        /// <summary>
        /// Whether or not to check for unterminated quotes and block comments.
        /// If true, and one is encoutered, an exception is thrown of the appropriate type.
        /// </summary>
        public bool DoUntermCheck { get; set; }

        /// <summary>
        /// Whether or not to return comments.
        /// </summary>
        public bool GrabComments { get; set; }

        /// <summary>
        /// Whether or not to return EolTokens on end of line.  Eol tokens will not
        /// break up other tokens which can be multi-line.  For example block comments 
        /// and quotes will not be broken by Eol tokens.  Therefore the number of
        /// Eol tokens does not give you the line count of a stream.
        /// </summary>
        public bool GrabEol { get; set; }

        /// <summary>
        /// Whether or not to return whitespace tokens. If not, they're ignored.
        /// </summary>
        public bool GrabWhitespace { get; set; }

        /// <summary>
        /// Whether or not to parse Hex (0xABCD...) numbers.
        /// This setting is based on the character types table, so this
        /// setting interacts with character type table manipulation.
        /// </summary>
        public bool ParseHexNumbers
        {
            get
            {
                return (_parseHexNumbers);
            }
            set
            {
                _parseHexNumbers = value;
                if (_parseHexNumbers)
                {
                    for (int i = '0'; i <= '9'; i++)
                        CharTypes[i] |= (byte)CharTypeBits.HexDigit;
                    for (int i = 'A'; i <= 'F'; i++)
                        CharTypes[i] |= (byte)CharTypeBits.HexDigit;
                    for (int i = 'a'; i <= 'f'; i++)
                        CharTypes[i] |= (byte)CharTypeBits.HexDigit;
                    CharTypes['x'] |= (byte)CharTypeBits.HexDigit;
                }
                else
                {
                    byte digit = (byte)CharTypeBits.HexDigit;

                    for (int i = 'A'; i <= 'F'; i++)
                    {
                        CharTypes[i] &= (byte)(~digit); // not digit
                    }
                    for (int i = 'a'; i <= 'f'; i++)
                    {
                        CharTypes[i] &= (byte)(~digit); // not digit
                    }
                    CharTypes['x'] &= (byte)(~digit);
                }
            }
        }

        /// <summary>
        /// Whether or not digits are specified as Digit type in the character table.
        /// This setting is based on the character types table, so this setting interacts 
        /// with character type table manipulation.
        /// This setting may become incorrect if you modify the character types table directly.
        /// </summary>
        public bool ParseNumbers
        {
            get { return _parseNumbers; }
            /* dropped for speed, this means this property isn't accurate if
             * character types table is modified directly.
             * 			{ 
                            for (int i = '0'; i <= '9'; i++)
                            {
                                if (!IsCharType((char)i, CharTypeBits.Digit)) 
                                {
                                    return(false);
                                }
                            }

                            return(true); 
                        }
            */
            set
            {
                if (value)
                {
                    for (int i = '0'; i <= '9'; i++)
                        CharTypes[i] |= (byte)CharTypeBits.Digit;
                }
                else
                {
                    byte digit = (byte)CharTypeBits.Digit;
                    for (int i = '0'; i <= '9'; i++)
                        CharTypes[i] &= (byte)(~digit); // not digit
                }
                _parseNumbers = value;
            }
        }

        /// <summary>
        /// Whether or not to look for // comments
        /// </summary>
        public bool SlashSlashComments { get; set; }

        /// <summary>
        /// Whether or not to look for /* */ block comments.
        /// </summary>
        public bool SlashStarComments { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Return a string representation of a character type setting.
        /// Since the type setting is bitwise encoded, a character can have more than one type.
        /// </summary>
        /// <param name="ctype">The character type byte.</param>
        /// <returns>The string representation of the type flags.</returns>
        public string CharTypeToString(byte ctype)
        {
            StringBuilder str = new StringBuilder();

            if (IsCharType(ctype, CharTypeBits.Quote)) str.Append('q');
            if (IsCharType(ctype, CharTypeBits.Comment)) str.Append('m');
            if (IsCharType(ctype, CharTypeBits.Whitespace)) str.Append('w');
            if (IsCharType(ctype, CharTypeBits.Digit)) str.Append('d');
            if (IsCharType(ctype, CharTypeBits.Word)) str.Append('a');
            if (IsCharType(ctype, CharTypeBits.Eof)) str.Append('e');
            if (str.Length == 0)
            {
                str.Append('c');
            }
            return (str.ToString());
        }

        /// <summary>
        /// Specify that a particular character is a comment-starting character.
        /// Character table type manipulation method.
        /// </summary>
        /// <param name="c"></param>
        public void CommentChar(int c)
        {
            CharTypes[c] = (byte)CharTypeBits.Comment;
        }

        /// <summary>
        /// Sets this object to be the same as the specified object.
        /// Note that some settings which are entirely embodied by the character type table.
        /// </summary>
        public void Copy(StreamTokenizerSettings other)
        {
            CharTypes = new byte[StreamTokenizer.NChars + 1];  // plus an EOF entry
            Array.Copy(other.CharTypes, 0, CharTypes, 0, CharTypes.Length);

            GrabWhitespace = other.GrabWhitespace;
            GrabEol = other.GrabEol;
            SlashSlashComments = other.SlashSlashComments;
            SlashStarComments = other.SlashStarComments;
            GrabComments = other.GrabComments;
            DoUntermCheck = other.DoUntermCheck;

            _parseHexNumbers = other._parseHexNumbers;
        }

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
        public void Display(string prefix){}

        /// <summary>
        /// Check whether the specified char type byte has a  particular type flag set.
        /// </summary>
        /// <param name="ctype">The char type byte.</param>
        /// <param name="type">The CharTypeBits entry to compare to.</param>
        /// <returns>bool - true or false</returns>
        public bool IsCharType(byte ctype, CharTypeBits type)
        {
            return ((ctype & (byte)type) != 0);
        }

        /// <summary>
        /// Check whether the specified char has a  particular type flag set.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="type">The CharTypeBits entry to compare to.</param>
        /// <returns>bool - true or false</returns>
        public bool IsCharType(char c, CharTypeBits type)
        {
            return ((CharTypes[c] & (byte)type) != 0);
        }

        /// <summary>
        /// Check whether the specified char has a  particular type flag set.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="type">The CharTypeBits entry to compare to.</param>
        /// <returns>bool - true or false</returns>
        public bool IsCharType(int c, CharTypeBits type)
        {
            return ((CharTypes[c] & (byte)type) != 0);
        }

        /// <summary>
        /// Remove other type settings from a character.
        /// Character table type manipulation method.
        /// </summary>
        /// <param name="c"></param>
        public void OrdinaryChar(int c)
        {
            CharTypes[c] = 0;
        }

        /// <summary>
        /// Remove other type settings from a range of characters.
        /// Character table type manipulation method.
        /// </summary>
        /// <param name="startChar"></param>
        /// <param name="endChar"></param>
        public void OrdinaryChars(int startChar, int endChar)
        {
            for (int i = startChar; i <= endChar; i++)
                CharTypes[i] = 0;
        }

        /// <summary>
        /// Specify that a particular character is a quote character.
        /// Character table type manipulation method.
        /// </summary>
        /// <param name="c"></param>
        public void QuoteChar(int c)
        {
            CharTypes[c] = (byte)CharTypeBits.Quote;
        }

        /// <summary>
        /// Clear the character type settings. This leaves them unset, as opposed to the default.
        /// Use SetDefaults() for default settings.
        /// </summary>
        public void ResetCharTypeTable()
        {
            Array.Clear(CharTypes, 0, CharTypes.Length);
            CharTypes[StreamTokenizer.NChars] = (byte)CharTypeBits.Eof; // last entry for Eof
        }

        /// <summary>
        /// Setup default parse behavior.
        /// This resets to same behavior as on construction.
        /// </summary>
        /// <returns>bool - true for success.</returns>
        public bool SetDefaults()
        {
            SlashStarComments = false;
            GrabComments = false;
            SlashSlashComments = false;
            GrabWhitespace = false;
            DoUntermCheck = true;
            GrabEol = false;

            // setup table
            ResetCharTypeTable();
            ParseNumbers = true;
            ParseHexNumbers = true;
            WordChars('A', 'Z');
            WordChars('a', 'z');
            WhitespaceChars(0, ' ');
            QuoteChar('\'');
            QuoteChar('"');
            WordChars('0', '9');

            return (true);
        }

        /// <summary>
        /// Apply settings which are commonly used for code parsing C-endCapStyle code, including C++, C#, and Java.
        /// </summary>
        /// <returns></returns>
        public bool SetupForCodeParse()
        {
            GrabWhitespace = true;
            GrabComments = true;
            SlashSlashComments = true;
            DoUntermCheck = true;
            SlashStarComments = true;
            WordChar('_');
            ParseNumbers = true;
            ParseHexNumbers = true;
            return (true);
        }

        /// <summary>
        /// Specify that a character is a whitespace character.
        /// Character table type manipulation method.
        /// This type is exclusive with other types.
        /// </summary>
        /// <param name="c">The character.</param>
        public void WhitespaceChar(int c)
        {
            CharTypes[c] = (byte)CharTypeBits.Whitespace;
        }

        /// <summary>
        /// Specify that a range of characters are whitespace characters.
        /// Character table type manipulation method.
        /// This adds the characteristic to the char(s), rather than overwriting other characteristics.
        /// </summary>
        /// <param name="startChar">First character.</param>
        /// <param name="endChar">Last character.</param>
        public void WhitespaceChars(int startChar, int endChar)
        {
            for (int i = startChar; i <= endChar; i++)
                CharTypes[i] = (byte)CharTypeBits.Whitespace;
        }

        /// <summary>
        /// Specify that a particular character is a word character.
        /// Character table type manipulation method.
        /// This adds the type to the char(s), rather than overwriting other types.
        /// </summary>
        /// <param name="c">The character.</param>
        public void WordChar(int c)
        {
            CharTypes[c] |= (byte)CharTypeBits.Word;
        }

        /// <summary>
        /// Specify that a range of characters are word characters.
        /// Character table type manipulation method.
        /// This adds the type to the char(s), rather than overwriting other types.
        /// </summary>
        /// <param name="startChar">First character.</param>
        /// <param name="endChar">Last character.</param>
        public void WordChars(int startChar, int endChar)
        {
            for (int i = startChar; i <= endChar; i++)
                CharTypes[i] |= (byte)CharTypeBits.Word;
        }

        /// <summary>
        /// Specify that a string of characters are word characters.
        /// Character table type manipulation method.
        /// This adds the type to the char(s), rather than overwriting other types.
        /// </summary>
        /// <param name="s"></param>
        public void WordChars(string s)
        {
            for (int i = 0; i < s.Length; i++)
                CharTypes[s[i]] |= (byte)CharTypeBits.Word;
        }

        #endregion
    }
}
