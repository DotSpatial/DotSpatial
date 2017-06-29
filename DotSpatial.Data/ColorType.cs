// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 3:45:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// ColorType
    /// </summary>
    public enum ColorType : byte
    {
        /// <summary>
        /// Each pixel is a greyscale sample
        /// </summary>
        Greyscale = 0,
        /// <summary>
        /// Each pixel is an RGB triple
        /// </summary>
        Truecolor = 2,
        /// <summary>
        /// Each pixel is a palette index
        /// </summary>
        Indexed = 3,
        /// <summary>
        /// Each pixel is a greyscale sample followed by an alpha sample
        /// </summary>
        GreyscaleAlpha = 4,
        /// <summary>
        /// EAch pixel is an RGB triple followed by an alhpa sample
        /// </summary>
        TruecolorAlpha = 6
    }
}