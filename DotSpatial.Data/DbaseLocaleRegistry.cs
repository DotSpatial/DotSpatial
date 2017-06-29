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

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// A registry used to map CLR cultures and encodings to 
    /// dBase language driver id (LDID) encoding specifiers.
    /// </summary>
    /// <remarks>
    /// Lookup values are taken from Esri's 
    /// <a href="http://downloads.esri.com/support/documentation/pad_/ArcPad_RefGuide_1105.pdf">
    /// ArcPad Reference Guide</a>
    /// </remarks>
    internal static class DbaseLocaleRegistry
    {
        #region Nested types
        /// <summary>
        /// Specifies which code page to use in a <see cref="TextInfo"/>
        /// instance.
        /// </summary>
        enum CodePageChoice
        {
            Custom,
            Oem,
            Ansi,
            Mac
        }

        /// <summary>
        /// Represents a culture and encoding pair.
        /// </summary>
        struct CultureWithEncoding
        {
            private readonly Encoding _encoding;
            public readonly CultureInfo CultureInfo;
            public readonly CodePageChoice CodePageChoice;

            internal CultureWithEncoding(CultureInfo cultureInfo, CodePageChoice codePageChoice)
            {
                CultureInfo = cultureInfo;
                CodePageChoice = codePageChoice;
                _encoding = null;
            }

            internal CultureWithEncoding(CultureInfo cultureInfo, Encoding encoding)
            {
                CultureInfo = cultureInfo;
                _encoding = encoding;
                CodePageChoice = CodePageChoice.Custom;
            }

            public Encoding Encoding
            {
                get
                {
                    if (_encoding == null)
                    {
                        switch (CodePageChoice)
                        {
                            case CodePageChoice.Ansi:
                                return Encoding.GetEncoding(CultureInfo.TextInfo.ANSICodePage);
                            case CodePageChoice.Mac:
                                return Encoding.GetEncoding(CultureInfo.TextInfo.MacCodePage);
                            default:
                                return Encoding.GetEncoding(CultureInfo.TextInfo.OEMCodePage);
                        }
                    }
                    else
                    {
                        return _encoding;
                    }
                }
            }
        }
        #endregion

        private static readonly Dictionary<Byte, CultureWithEncoding> _dbaseToEncoding
            = new Dictionary<Byte, CultureWithEncoding>();
        private static readonly Dictionary<Encoding, Byte> _encodingToDbase
            = new Dictionary<Encoding, Byte>();

        static DbaseLocaleRegistry()
        {
            setupDbaseToEncodingMap();
            setupEncodingToDbaseMap();
        }

        // Values from the ArcPad reference guide.
        // Found here: http://downloads.esri.com/support/documentation/pad_/ArcPad_RefGuide_1105.pdf
        private static void setupDbaseToEncodingMap()
        {
            _dbaseToEncoding[0x01] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Oem); //DOS USA code page 437 
            _dbaseToEncoding[0x02] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), Encoding.GetEncoding(850)); // DOS Multilingual code page 850 
            _dbaseToEncoding[0x03] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Ansi); // Windows ANSI code page 1252 
            _dbaseToEncoding[0x04] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Mac); // Macintosh US English 
            _dbaseToEncoding[0x08] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1030), Encoding.GetEncoding(865)); // Danish OEM
            _dbaseToEncoding[0x09] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1043), Encoding.GetEncoding(437)); // Dutch OEM
            _dbaseToEncoding[0x0A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1043), CodePageChoice.Oem); // Dutch OEM Secondary codepage
            _dbaseToEncoding[0x0B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1035), Encoding.GetEncoding(437)); // Finnish OEM
            _dbaseToEncoding[0x0D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1036), Encoding.GetEncoding(437)); // French OEM
            _dbaseToEncoding[0x0E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1036), CodePageChoice.Oem); // French OEM Secondary codepage
            _dbaseToEncoding[0x0F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1031), Encoding.GetEncoding(437)); // German OEM
            _dbaseToEncoding[0x10] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1031), CodePageChoice.Oem); // German OEM Secondary codepage
            _dbaseToEncoding[0x11] = new CultureWithEncoding(CultureInfo.GetCultureInfo(16), Encoding.GetEncoding(437)); // Italian OEM
            _dbaseToEncoding[0x12] = new CultureWithEncoding(CultureInfo.GetCultureInfo(16), CodePageChoice.Oem); // Italian OEM Secondary codepage
            _dbaseToEncoding[0x13] = new CultureWithEncoding(CultureInfo.GetCultureInfo(17), CodePageChoice.Oem); // Japanese Shift-JIS
            _dbaseToEncoding[0x14] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), CodePageChoice.Oem); // Spanish OEM secondary codepage
            _dbaseToEncoding[0x15] = new CultureWithEncoding(CultureInfo.GetCultureInfo(29), Encoding.GetEncoding(437)); // Swedish OEM
            _dbaseToEncoding[0x16] = new CultureWithEncoding(CultureInfo.GetCultureInfo(29), CodePageChoice.Oem); // Swedish OEM secondary codepage
            _dbaseToEncoding[0x17] = new CultureWithEncoding(CultureInfo.GetCultureInfo(20), Encoding.GetEncoding(865)); // Norwegian OEM
            _dbaseToEncoding[0x18] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), Encoding.GetEncoding(437)); // Spanish OEM
            _dbaseToEncoding[0x19] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2057), Encoding.GetEncoding(437)); // English OEM (Britain)
            _dbaseToEncoding[0x1A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2057), CodePageChoice.Oem); // English OEM (Britain) secondary codepage
            _dbaseToEncoding[0x1B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Oem); // English OEM (U.S.)
            _dbaseToEncoding[0x1C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(3084), Encoding.GetEncoding(863)); // French OEM (Canada)
            _dbaseToEncoding[0x1D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(12), CodePageChoice.Oem); // French OEM secondary codepage
            _dbaseToEncoding[0x1F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(5), CodePageChoice.Oem); // Czech OEM
            _dbaseToEncoding[0x22] = new CultureWithEncoding(CultureInfo.GetCultureInfo(14), CodePageChoice.Oem); // Hungarian OEM
            _dbaseToEncoding[0x23] = new CultureWithEncoding(CultureInfo.GetCultureInfo(21), CodePageChoice.Oem); // Polish OEM
            _dbaseToEncoding[0x24] = new CultureWithEncoding(CultureInfo.GetCultureInfo(22), Encoding.GetEncoding(860)); // Portuguese OEM
            _dbaseToEncoding[0x25] = new CultureWithEncoding(CultureInfo.GetCultureInfo(22), CodePageChoice.Oem); // Portuguese OEM secondary codepage
            _dbaseToEncoding[0x26] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Oem); // Russian OEM
            _dbaseToEncoding[0x37] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), Encoding.GetEncoding(850)); // English OEM (U.S.) secondary codepage
            _dbaseToEncoding[0x40] = new CultureWithEncoding(CultureInfo.GetCultureInfo(24), CodePageChoice.Oem); // Romanian OEM
            _dbaseToEncoding[0x4D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(4), CodePageChoice.Oem); // Chinese GBK (PRC)
            _dbaseToEncoding[0x4E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1042), CodePageChoice.Oem); // Korean (ANSI/OEM)
            _dbaseToEncoding[0x4F] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1028), CodePageChoice.Oem); // Chinese Big5 (Taiwan)
            _dbaseToEncoding[0x50] = new CultureWithEncoding(CultureInfo.GetCultureInfo(30), CodePageChoice.Oem); // Thai (ANSI/OEM)
            _dbaseToEncoding[0x57] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1033), CodePageChoice.Ansi); // ANSI
            _dbaseToEncoding[0x58] = new CultureWithEncoding(CultureInfo.InvariantCulture, CodePageChoice.Ansi); // Western European ANSI
            _dbaseToEncoding[0x59] = new CultureWithEncoding(CultureInfo.GetCultureInfo(10), CodePageChoice.Ansi); // Spanish ANSI
            _dbaseToEncoding[0x64] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(852)); // Eastern European MS–DOS
            _dbaseToEncoding[0x65] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Oem); // Russian MS–DOS
            _dbaseToEncoding[0x66] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(865)); // Nordic MS–DOS
            _dbaseToEncoding[0x67] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(861)); // Icelandic MS–DOS
            //_dbaseToEncoding[0x68] = Encoding.GetEncoding(895); // Kamenicky (Czech) MS-DOS 
            //_dbaseToEncoding[0x69] = Encoding.GetEncoding(620); // Mazovia (Polish) MS-DOS 
            _dbaseToEncoding[0x6B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Oem); // Turkish MS–DOS
            _dbaseToEncoding[0x6C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(3084), Encoding.GetEncoding(863)); // French–Canadian MS–DOS
            _dbaseToEncoding[0x78] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1028), CodePageChoice.Oem); // Taiwan Big 5
            _dbaseToEncoding[0x79] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1042), CodePageChoice.Oem); // Hangul (Wansung)
            _dbaseToEncoding[0x7A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(2052), CodePageChoice.Oem); // PRC GBK
            _dbaseToEncoding[0x7B] = new CultureWithEncoding(CultureInfo.GetCultureInfo(17), CodePageChoice.Oem); // Japanese Shift-JIS
            _dbaseToEncoding[0x7C] = new CultureWithEncoding(CultureInfo.GetCultureInfo(30), CodePageChoice.Oem); // Thai Windows/MS–DOS
            _dbaseToEncoding[0x7D] = new CultureWithEncoding(CultureInfo.GetCultureInfo(13), CodePageChoice.Ansi); // Hebrew Windows 
            _dbaseToEncoding[0x7E] = new CultureWithEncoding(CultureInfo.GetCultureInfo(1), CodePageChoice.Ansi); // Arabic Windows 
            _dbaseToEncoding[0x86] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Oem); // Greek OEM
            _dbaseToEncoding[0x87] = new CultureWithEncoding(CultureInfo.GetCultureInfo(27), CodePageChoice.Oem); // Slovenian OEM
            _dbaseToEncoding[0x88] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Oem); // Turkish OEM
            _dbaseToEncoding[0xC8] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(1250)); // Eastern European Windows
            _dbaseToEncoding[0xC9] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Ansi); // Russian Windows
            _dbaseToEncoding[0xCA] = new CultureWithEncoding(CultureInfo.GetCultureInfo(31), CodePageChoice.Ansi); // Turkish Windows
            _dbaseToEncoding[0xCB] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Ansi); // Greek Windows
            _dbaseToEncoding[0xCC] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(1257)); // Baltic Windows

            if (!Mono.Mono.IsRunningOnMono())
            {
                _dbaseToEncoding[0x6A] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Oem); // Greek MS–DOS (437G) -m
                _dbaseToEncoding[0x96] = new CultureWithEncoding(CultureInfo.GetCultureInfo(25), CodePageChoice.Mac); // Russian Macintosh -m
                _dbaseToEncoding[0x97] = new CultureWithEncoding(CultureInfo.InvariantCulture, Encoding.GetEncoding(10029)); // Eastern European Macintosh -m
                _dbaseToEncoding[0x98] = new CultureWithEncoding(CultureInfo.GetCultureInfo(8), CodePageChoice.Mac); // Greek Macintosh -m
            }
        }

        private static void setupEncodingToDbaseMap()
        {
            foreach (KeyValuePair<Byte, CultureWithEncoding> item in _dbaseToEncoding)
            {
                // These encodings are duplicated. When a dBase file is created,
                // these will not be used as the LDID since it would result in an ambiguous lookup.
                if (item.Key == 0x01 || item.Key == 0x02 || item.Key == 0x03 || item.Key == 0x26 ||
                    item.Key == 0x6C || item.Key == 0x78 || item.Key == 0x79 || item.Key == 0x7B ||
                    item.Key == 0x7C || item.Key == 0x86 || item.Key == 0x88)
                {
                    continue;
                }

                Encoding encoding = item.Value.Encoding;
                _encodingToDbase[encoding] = item.Key;
            }
        }

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> associated with the 
        /// given dBase LDID.
        /// </summary>
        /// <param name="dBaseEncoding">
        /// The language driver id (LDID) to get a CultureInfo for.
        /// </param>
        /// <returns>
        /// A <see cref="CultureInfo"/> which uses the encoding represented by 
        /// <paramref name="dBaseEncoding"/> by default.
        /// </returns>
        public static CultureInfo GetCulture(Byte dBaseEncoding)
        {
            CultureWithEncoding pair;
            return _dbaseToEncoding.TryGetValue(dBaseEncoding, out pair) ? pair.CultureInfo : CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets the <see cref="Encoding"/> which matches the 
        /// given dBase LDID.
        /// </summary>
        /// <param name="dBaseEncoding">
        /// The language driver id (LDID) to get the <see cref="Encoding"/> for.
        /// </param>
        /// <returns>
        /// An <see cref="Encoding"/> which corresponds to the the
        /// <paramref name="dBaseEncoding"/> code established by Esri.
        /// </returns>
        public static Encoding GetEncoding(Byte dBaseEncoding)
        {
            CultureWithEncoding pair;
            return _dbaseToEncoding.TryGetValue(dBaseEncoding, out pair) ? pair.Encoding : Encoding.Default;
        }

        /// <summary>
        /// Gets a language driver id (LDID) for the given encoding.
        /// </summary>
        /// <param name="encoding">The Encoding used to lookup the LDID.</param>
        /// <returns>A language driver code used to specify the encoding used to write text in the dBase file.</returns>
        /// <remarks>0x57 (Windows ANSI) is returned as a default if there is no associated LDID for the given encoding.</remarks>
        public static Byte GetLanguageDriverId(Encoding encoding)
        {
            Byte ldid;
            return _encodingToDbase.TryGetValue(encoding, out ldid) ? ldid : (Byte)0x57;
        }
    }
}