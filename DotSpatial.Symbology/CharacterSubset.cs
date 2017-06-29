// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2009 11:53:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// CharacterSubsets
    /// </summary>
    public enum CharacterSubset
    {
        /// <summary>
        /// Basic Latin
        /// </summary>
        Basic_Latin = 32,
        /// <summary>
        /// Latin-1 Supplement
        /// </summary>
        Latin1_Supplement = 160,
        /// <summary>
        /// Latin Extended-A
        /// </summary>
        Latin_Extended_A = 256,
        /// <summary>
        /// Latin Extended-B
        /// </summary>
        Latin_Extended_B = 384,
        /// <summary>
        /// IPA Extensions
        /// </summary>
        IPA_Extensions = 592,
        /// <summary>
        /// Spacing Modifier Letters
        /// </summary>
        Spacing_Modifier_Letters = 688,
        /// <summary>
        /// Combining Diacritical Marks
        /// </summary>
        Combining_Diacritical_Marks = 768,
        /// <summary>
        /// Greek and Coptic
        /// </summary>
        Greek_and_Coptic = 884,
        /// <summary>
        /// Cyrillic
        /// </summary>
        Cyrillic = 1025,
        /// <summary>
        /// Armenian
        /// </summary>
        Armenian = 1329,
        /// <summary>
        /// Hebrew
        /// </summary>
        Hebrew = 1425,
        /// <summary>
        /// Arabic
        /// </summary>
        Arabic = 1548,
        /// <summary>
        /// Devanagari
        /// </summary>
        Devanagari = 2305,
        /// <summary>
        /// Bengali
        /// </summary>
        Bengali = 2433,
        /// <summary>
        /// Gumukhi
        /// </summary>
        Gumukhi = 2562,
        /// <summary>
        /// Gujarati
        /// </summary>
        Gujarati = 2689,
        /// <summary>
        /// Oriya
        /// </summary>
        Oriya = 2817,
        /// <summary>
        /// Tamil
        /// </summary>
        Tamil = 2946,
        /// <summary>
        /// Teluga
        /// </summary>
        Teluga = 3073,
        /// <summary>
        /// Kanada
        /// </summary>
        Kannada = 3202,
        /// <summary>
        /// Malayalam
        /// </summary>
        Malayalam = 3330,
        /// <summary>
        /// Thai
        /// </summary>
        Thai = 3585,
        /// <summary>
        /// Lao
        /// </summary>
        Lao = 3713,
        /// <summary>
        /// Tibetan
        /// </summary>
        Tibetan = 3840,
        /// <summary>
        /// Georgian
        /// </summary>
        Georgian = 4256,
        /// <summary>
        /// Hangul Jamo 4352
        /// </summary>
        Hangul_Jamo = 4352,
        /// <summary>
        /// Latin Extended Additional
        /// </summary>
        Latin_Extended_Additional = 7680,
        /// <summary>
        /// Greek Extended
        /// </summary>
        Greek_Extended = 7936,
        /// <summary>
        /// General Punctuation
        /// </summary>
        General_Punctuation = 8192,
        /// <summary>
        /// Superscripts and Subscripts
        /// </summary>
        Superscripts_and_Subscripts = 8304,
        /// <summary>
        /// Currency Symbols
        /// </summary>
        Currency_Symbols = 8352,
        /// <summary>
        /// Combining Diacritical Marks for Symbols
        /// </summary>
        Combining_Diacritical_Marks_for_Symbols = 8400,
        /// <summary>
        /// Letterlike Symbols
        /// </summary>
        Letterlike_Symbols = 8448,
        /// <summary>
        /// Number Forms
        /// </summary>
        Number_Forms = 8531,
        /// <summary>
        /// Arrows
        /// </summary>
        Arrows = 8592,
        /// <summary>
        /// Mathematical Operators
        /// </summary>
        Mathematical_Operators = 8704,
        /// <summary>
        /// Miscellaneous Technical
        /// </summary>
        Miscellaneous_Technical = 8960,
        /// <summary>
        /// Control Pictures
        /// </summary>
        Control_Pictures = 9216,
        /// <summary>
        /// Optical Character Recognition
        /// </summary>
        Optical_Character_Recognition = 9280,
        /// <summary>
        /// Enclosed Alphanumerics
        /// </summary>
        Enclosed_Alphanumerics = 9312,
        /// <summary>
        /// Box Drawing
        /// </summary>
        Box_Drawing = 9472,
        /// <summary>
        /// Block Elements
        /// </summary>
        Block_Elements = 9600,
        /// <summary>
        /// Geometric Shapes
        /// </summary>
        Geometric_Shapes = 9632,
        /// <summary>
        /// Miscellaneous Symbols
        /// </summary>
        Miscellaneous_Symbols = 9728,
        /// <summary>
        /// Dingbats
        /// </summary>
        Dingbats = 9985,
        /// <summary>
        /// CJK Symbols and Punctuation
        /// </summary>
        CJK_Symbols_and_Puctuation = 12288,
        /// <summary>
        /// Hiragana
        /// </summary>
        Hiragana = 12353,
        /// <summary>
        /// Katakana
        /// </summary>
        Katakana = 12449,
        /// <summary>
        /// Bopomofo
        /// </summary>
        Bopomofo = 12549,
        /// <summary>
        /// Hangul Compatibility
        /// </summary>
        Hangul_Compatibility = 12593,
        /// <summary>
        /// Kanbun
        /// </summary>
        Kanbun = 12688,
        /// <summary>
        /// Enclosed CJK Letters and Months
        /// </summary>
        Enclosed_CJK_Letters_and_Months = 12800,
        /// <summary>
        /// CJK Compatibility
        /// </summary>
        CJK_Compatibility = 13056,
        /// <summary>
        /// CJK Unified Ideographs
        /// </summary>
        CJK_Unified_Ideographs = 19968,
        /// <summary>
        /// Hangul Syllables
        /// </summary>
        Hangul_Syllables = 44032,
        /// <summary>
        /// Private Use Area
        /// </summary>
        Private_Use_Area = 59393,
        /// <summary>
        /// CJK Compatibility Ideographs
        /// </summary>
        CJK_Compatibility_Ideographs = 63744,
        /// <summary>
        /// Alphabetic Presentation Forms
        /// </summary>
        Alphabetic_Presentation_Forms = 64256,
        /// <summary>
        /// Arabic Presentation Forms
        /// </summary>
        Arabic_Presentation_Forms = 64336,
        /// <summary>
        /// Combining Half Marks
        /// </summary>
        Combining_Half_Marks = 65056,
        /// <summary>
        /// CJK Compatibility Forms
        /// </summary>
        CJK_Compatibility_Forms = 65072,
        /// <summary>
        /// Small Form Variants
        /// </summary>
        Small_Form_Variants = 65104,
        /// <summary>
        /// Arabic Presentation Forms-B
        /// </summary>
        Arabic_Presentation_Forms_B = 65136,
        /// <summary>
        /// Halfwidth and fullwidth Forms
        /// </summary>
        Halfwidth_and_fullwidth_Forms = 65281,
        /// <summary>
        /// Specials
        /// </summary>
        Specials = 65532
    }
}