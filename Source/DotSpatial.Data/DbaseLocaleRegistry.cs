// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the SharpMap (v2) project.
//
// The Initial Developer to integrate this code into DotSpatial is Arnold Engelmann.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// | Arnold Engelmann     | 01/18/2013 | Copied from SharpMap v2 for use in DotSpatial
// | Arnold Engelmann     | 01/22/2013 | Changed reverse lookup of LDID to only require Encoding
// ********************************************************************************************************

using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// A registry used to map CLR cultures and encodings to dBase language driver id (LDID) encoding specifiers.
    /// </summary>
    /// <remarks>
    /// Lookup values are taken from Esri's <a href="http://downloads.esri.com/support/documentation/pad_/ArcPad_RefGuide_1105.pdf"> ArcPad Reference Guide</a>
    /// </remarks>
    internal static class DbaseLocaleRegistry
    {
        #region Fields

        private static readonly Dictionary<byte, CultureWithEncoding> DbaseToEncoding = new Dictionary<byte, CultureWithEncoding>();

        private static readonly Dictionary<Encoding, byte> EncodingToDbase = new Dictionary<Encoding, byte>();

        #endregion

        #region Constructors

        static DbaseLocaleRegistry()
        {
            SetupDbaseToEncodingMap();
            SetupEncodingToDbaseMap();
        }

        #endregion

        /// <summary>
        /// Specifies which code page to use in a <see cref="TextInfo"/>
        /// instance.
        /// </summary>
        private enum CodePageChoice
        {
            Custom,
            Oem,
            Ansi,
            Mac
        }

        #region Methods

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> associated with the given dBase LDID.
        /// </summary>
        /// <param name="dBaseEncoding">The language driver id (LDID) to get a CultureInfo for.</param>
        /// <returns>A <see cref="CultureInfo"/> which uses the encoding represented by <paramref name="dBaseEncoding"/> by default.</returns>
        public static CultureInfo GetCulture(byte dBaseEncoding)
        {
            CultureWithEncoding pair;
            return DbaseToEncoding.TryGetValue(dBaseEncoding, out pair) ? pair.CultureInfo : CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets the <see cref="Encoding"/> which matches the  given dBase LDID.
        /// </summary>
        /// <param name="dBaseEncoding">The language driver id (LDID) to get the <see cref="Encoding"/> for.</param>
        /// <returns>An <see cref="Encoding"/> which corresponds to the the <paramref name="dBaseEncoding"/> code established by Esri.</returns>
        public static Encoding GetEncoding(byte dBaseEncoding)
        {
            CultureWithEncoding pair;
            return DbaseToEncoding.TryGetValue(dBaseEncoding, out pair) ? pair.Encoding : Encoding.Default;
        }

        /// <summary>
        /// Gets a language driver id (LDID) for the given encoding.
        /// </summary>
        /// <param name="encoding">The Encoding used to lookup the LDID.</param>
        /// <returns>A language driver code used to specify the encoding used to write text in the dBase file.</returns>
        /// <remarks>0x57 (Windows ANSI) is returned as a default if there is no associated LDID for the given encoding.</remarks>
        public static byte GetLanguageDriverId(Encoding encoding)
        {
            byte ldid;
            return EncodingToDbase.TryGetValue(encoding, out ldid) ? ldid : (byte)0x57;
        }

        // Values from the ArcPad reference guide.
        // Found here: http://downloads.esri.com/support/documentation/pad_/ArcPad_RefGuide_1105.pdf
        private static void SetupDbaseToEncodingMap()
        {
            DbaseToEncoding[0x01] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Oem); // DOS USA code page 437
            DbaseToEncoding[0x02] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), Encoding.GetEncoding(850)); // DOS Multilingual code page 850
            DbaseToEncoding[0x03] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Ansi); // Windows ANSI code page 1252
            DbaseToEncoding[0x04] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Mac); // Macintosh US English
            DbaseToEncoding[0x08] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1030), Encoding.GetEncoding(865)); // Danish OEM
            DbaseToEncoding[0x09] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1043), Encoding.GetEncoding(437)); // Dutch OEM
            DbaseToEncoding[0x0A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1043), CodePageChoice.Oem); // Dutch OEM Secondary codepage
            DbaseToEncoding[0x0B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1035), Encoding.GetEncoding(437)); // Finnish OEM
            DbaseToEncoding[0x0D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1036), Encoding.GetEncoding(437)); // French OEM
            DbaseToEncoding[0x0E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1036), CodePageChoice.Oem); // French OEM Secondary codepage
            DbaseToEncoding[0x0F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1031), Encoding.GetEncoding(437)); // German OEM
            DbaseToEncoding[0x10] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1031), CodePageChoice.Oem); // German OEM Secondary codepage
            DbaseToEncoding[0x11] = new CultureWithEncoding(CultureInfo.GetCultureInfo(16), Encoding.GetEncoding(437)); // Italian OEM
            DbaseToEncoding[0x12] = new CultureWithEncoding(CultureInfo.GetCultureInfo(16), CodePageChoice.Oem); // Italian OEM Secondary codepage
            DbaseToEncoding[0x13] = new CultureWithEncoding(CultureInfo.GetCultureInfo(17), CodePageChoice.Oem); // Japanese Shift-JIS
            DbaseToEncoding[0x14] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), CodePageChoice.Oem); // Spanish OEM secondary codepage
            DbaseToEncoding[0x15] = new CultureWithEncoding(CultureInfo.GetCultureInfo(29), Encoding.GetEncoding(437)); // Swedish OEM
            DbaseToEncoding[0x16] = new CultureWithEncoding(CultureInfo.GetCultureInfo(29), CodePageChoice.Oem); // Swedish OEM secondary codepage
            DbaseToEncoding[0x17] = new CultureWithEncoding(CultureInfo.GetCultureInfo(20), Encoding.GetEncoding(865)); // Norwegian OEM
            DbaseToEncoding[0x18] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), Encoding.GetEncoding(437)); // Spanish OEM
            DbaseToEncoding[0x19] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2057), Encoding.GetEncoding(437)); // English OEM (Britain)
            DbaseToEncoding[0x1A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2057), CodePageChoice.Oem); // English OEM (Britain) secondary codepage
            DbaseToEncoding[0x1B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Oem); // English OEM (U.S.)
            DbaseToEncoding[0x1C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(3084), Encoding.GetEncoding(863)); // French OEM (Canada)
            DbaseToEncoding[0x1D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(12), CodePageChoice.Oem); // French OEM secondary codepage
            DbaseToEncoding[0x1F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(5), CodePageChoice.Oem); // Czech OEM
            DbaseToEncoding[0x22] = new CultureWithEncoding(CultureInfo.GetCultureInfo(14), CodePageChoice.Oem); // Hungarian OEM
            DbaseToEncoding[0x23] = new CultureWithEncoding(CultureInfo.GetCultureInfo(21), CodePageChoice.Oem); // Polish OEM
            DbaseToEncoding[0x24] = new CultureWithEncoding(CultureInfo.GetCultureInfo(22), Encoding.GetEncoding(860)); // Portuguese OEM
            DbaseToEncoding[0x25] = new CultureWithEncoding(CultureInfo.GetCultureInfo(22), CodePageChoice.Oem); // Portuguese OEM secondary codepage
            DbaseToEncoding[0x26] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Oem); // Russian OEM
            DbaseToEncoding[0x37] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), Encoding.GetEncoding(850)); // English OEM (U.S.) secondary codepage
            DbaseToEncoding[0x40] = new CultureWithEncoding(CultureInfo.GetCultureInfo(24), CodePageChoice.Oem); // Romanian OEM
            DbaseToEncoding[0x4D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(4), CodePageChoice.Oem); // Chinese GBK (PRC)
            DbaseToEncoding[0x4E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1042), CodePageChoice.Oem); // Korean (ANSI/OEM)
            DbaseToEncoding[0x4F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1028), CodePageChoice.Oem); // Chinese Big5 (Taiwan)
            DbaseToEncoding[0x50] = new CultureWithEncoding(CultureInfo.GetCultureInfo(30), CodePageChoice.Oem); // Thai (ANSI/OEM)
            DbaseToEncoding[0x57] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Ansi); // ANSI
            DbaseToEncoding[0x58] = new CultureWithEncoding(CultureInfo.InvariantCulture, CodePageChoice.Ansi); // Western European ANSI
            DbaseToEncoding[0x59] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), CodePageChoice.Ansi); // Spanish ANSI
            DbaseToEncoding[0x64] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(852)); // Eastern European MS–DOS
            DbaseToEncoding[0x65] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Oem); // Russian MS–DOS
            DbaseToEncoding[0x66] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(865)); // Nordic MS–DOS
            DbaseToEncoding[0x67] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(861)); // Icelandic MS–DOS

            DbaseToEncoding[0x6B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Oem); // Turkish MS–DOS
            DbaseToEncoding[0x6C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(3084), Encoding.GetEncoding(863)); // French–Canadian MS–DOS
            DbaseToEncoding[0x78] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1028), CodePageChoice.Oem); // Taiwan Big 5
            DbaseToEncoding[0x79] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1042), CodePageChoice.Oem); // Hangul (Wansung)
            DbaseToEncoding[0x7A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2052), CodePageChoice.Oem); // PRC GBK
            DbaseToEncoding[0x7B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(17), CodePageChoice.Oem); // Japanese Shift-JIS
            DbaseToEncoding[0x7C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(30), CodePageChoice.Oem); // Thai Windows/MS–DOS
            DbaseToEncoding[0x7D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(13), CodePageChoice.Ansi); // Hebrew Windows
            DbaseToEncoding[0x7E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1), CodePageChoice.Ansi); // Arabic Windows
            DbaseToEncoding[0x86] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Oem); // Greek OEM
            DbaseToEncoding[0x87] = new CultureWithEncoding(CultureInfo.GetCultureInfo(27), CodePageChoice.Oem); // Slovenian OEM
            DbaseToEncoding[0x88] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Oem); // Turkish OEM
            DbaseToEncoding[0xC8] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(1250)); // Eastern European Windows
            DbaseToEncoding[0xC9] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Ansi); // Russian Windows
            DbaseToEncoding[0xCA] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Ansi); // Turkish Windows
            DbaseToEncoding[0xCB] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Ansi); // Greek Windows
            DbaseToEncoding[0xCC] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(1257)); // Baltic Windows

            if (!Mono.IsRunningOnMono())
            {
                DbaseToEncoding[0x6A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Oem); // Greek MS–DOS (437G) -m
                DbaseToEncoding[0x96] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Mac); // Russian Macintosh -m
                DbaseToEncoding[0x97] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(10029)); // Eastern European Macintosh -m
                DbaseToEncoding[0x98] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Mac); // Greek Macintosh -m
            }
        }

        private static void SetupEncodingToDbaseMap()
        {
            foreach (KeyValuePair<byte, CultureWithEncoding> item in DbaseToEncoding)
            {
                // These encodings are duplicated. When a dBase file is created,
                // these will not be used as the LDID since it would result in an ambiguous lookup.
                if (item.Key == 0x01 || item.Key == 0x02 || item.Key == 0x03 || item.Key == 0x26 || item.Key == 0x6C || item.Key == 0x78 || item.Key == 0x79 || item.Key == 0x7B || item.Key == 0x7C || item.Key == 0x86 || item.Key == 0x88)
                {
                    continue;
                }

                Encoding encoding = item.Value.Encoding;
                EncodingToDbase[encoding] = item.Key;
            }
        }

        #endregion

        #region Classes

        /// <summary>
        /// Represents a culture and encoding pair.
        /// </summary>
        private struct CultureWithEncoding
        {
            private readonly Encoding _encoding;
            private readonly CodePageChoice _codePageChoice;

            internal CultureWithEncoding(CultureInfo cultureInfo, CodePageChoice codePageChoice)
            {
                CultureInfo = cultureInfo;
                _codePageChoice = codePageChoice;
                _encoding = null;
            }

            internal CultureWithEncoding(CultureInfo cultureInfo, Encoding encoding)
            {
                CultureInfo = cultureInfo;
                _encoding = encoding;
                _codePageChoice = CodePageChoice.Custom;
            }

            public CultureInfo CultureInfo { get; }

            public Encoding Encoding
            {
                get
                {
                    if (_encoding == null)
                    {
                        switch (_codePageChoice)
                        {
                            case CodePageChoice.Ansi: return Encoding.GetEncoding(CultureInfo.TextInfo.ANSICodePage);
                            case CodePageChoice.Mac: return Encoding.GetEncoding(CultureInfo.TextInfo.MacCodePage);
                            default: return Encoding.GetEncoding(CultureInfo.TextInfo.OEMCodePage);
                        }
                    }

                    return _encoding;
                }
            }
        }

        #endregion
    }
}