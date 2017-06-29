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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/9/2009 2:07:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration listing the types of image band interpretations supported.
    /// </summary>
    public enum ImageBandType
    {
        /// <summary>
        /// Alpha, Red, Green, Blue 4 bytes per pixel, 4 bands
        /// </summary>
        ARGB,

        /// <summary>
        ///  Red, Green, Blue 3 bytes per pixel, 3 bands
        /// </summary>
        RGB,

        /// <summary>
        /// Gray as one byte per pixel, 1 band
        /// </summary>
        Gray,

        /// <summary>
        /// Colors encoded 1 byte per pixel, 1 band
        /// </summary>
        PalletCoded
    }
}