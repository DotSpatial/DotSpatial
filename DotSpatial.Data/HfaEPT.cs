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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:32:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// EPT ERDAS DATA TYPES
    /// </summary>
    public enum HfaEPT
    {
        /// <summary>
        /// Unsigned 1 bit
        /// </summary>
        U1 = 0,
        /// <summary>
        /// Unsigned 2 bit
        /// </summary>
        U2 = 1,
        /// <summary>
        /// Unsigned 4 bit
        /// </summary>
        U4 = 2,
        /// <summary>
        /// Unsigned 8 bit
        /// </summary>
        U8 = 3,
        /// <summary>
        /// Signed 8 bit
        /// </summary>
        S8 = 4,
        /// <summary>
        /// Unsigned 16 bit
        /// </summary>
        U16 = 5,
        /// <summary>
        /// Signed 16 bit
        /// </summary>
        S16 = 6,
        /// <summary>
        /// Unsigned 32 bit
        /// </summary>
        U32 = 7,
        /// <summary>
        /// Signed 32 bit
        /// </summary>
        S32 = 8,
        /// <summary>
        /// Single precisions 32 bit floating point
        /// </summary>
        Single = 9,
        /// <summary>
        /// Double precision 64 bit floating point
        /// </summary>
        Double = 10,
        /// <summary>
        /// 64 bit character?
        /// </summary>
        Char64 = 11,
        /// <summary>
        /// 128 bit character?
        /// </summary>
        Char128 = 12
    }
}