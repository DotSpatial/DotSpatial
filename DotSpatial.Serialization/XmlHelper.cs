// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Text;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// A class that helps with xml project serialization
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Some characters create problems in xml format by being interpreted by xml parsers as
        /// content formatting tags.
        /// </summary>
        /// <param name="text">The string text of the unescaped original characters</param>
        /// <returns>The modified string where the characters are replaced</returns>
        public static string EscapeInvalidCharacters(string text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '\'':
                        sb.Append("&apos;");
                        break;
                    case '\"':
                        sb.Append("&quot;");
                        break;
                    case '&':
                        if (IsValidEscapeSequence(text, i))
                            sb.Append(text[i]);
                        else
                            sb.Append("&amp;");
                        break;
                    default:
                        sb.Append(text[i]);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// This converts the invalid characters back to the original, unescaped text
        /// </summary>
        /// <param name="text">The string text of the escaped characters</param>
        /// <returns>The unescaped string text</returns>
        public static string UnEscapeInvalidCharacters(string text)
        {
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&apos;", "'");
            text = text.Replace("&amp;", "&");

            return text;
        }

        /// <summary>
        /// This tests the string in the provided text parameter starting at the specified startIndex.
        /// If the combination of characters builds one of the recognized escape sequences, then this
        /// returns true.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <param name="startIndex">The zero based integer start index.</param>
        /// <returns>A boolean that is true if the startIndex begins an escape sequence.</returns>
        private static bool IsValidEscapeSequence(string text, int startIndex)
        {
            int charactersLeft = text.Length - startIndex;

            if (charactersLeft >= 6)
            {
                return (text[startIndex + 1] == 'q' && text[startIndex + 2] == 'u' && text[startIndex + 3] == 'o' && text[startIndex + 4] == 't' && text[startIndex + 5] == ';') ||
                       (text[startIndex + 1] == 'a' && text[startIndex + 2] == 'p' && text[startIndex + 3] == 'o' && text[startIndex + 4] == 's' && text[startIndex + 5] == ';') ||
                       (text[startIndex + 1] == 'a' && text[startIndex + 2] == 'm' && text[startIndex + 3] == 'p' && text[startIndex + 4] == ';') ||
                       (text[startIndex + 1] == 'l' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';') ||
                       (text[startIndex + 1] == 'g' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';');
            }
            if (charactersLeft >= 5)
            {
                return (text[startIndex + 1] == 'a' && text[startIndex + 2] == 'm' && text[startIndex + 3] == 'p' && text[startIndex + 4] == ';') ||
                       (text[startIndex + 1] == 'l' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';') ||
                       (text[startIndex + 1] == 'g' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';');
            }
            if (charactersLeft >= 4)
            {
                return (text[startIndex + 1] == 'l' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';') ||
                       (text[startIndex + 1] == 'g' && text[startIndex + 2] == 't' && text[startIndex + 3] == ';');
            }

            return false;
        }
    }
}