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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 5:57:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// ValidationType
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// No validation will be performed
        /// </summary>
        None,
        /// <summary>
        /// Any string value, including null will be accepted
        /// </summary>
        String,
        /// <summary>
        /// Only values that can be parsed to byte values will be accepted
        /// </summary>
        Byte,
        /// <summary>
        /// Only values that can be parsed to short values will be accepted
        /// </summary>
        Short,
        /// <summary>
        /// Only values that can be parsed to integer values will be accepted
        /// </summary>
        Integer,
        /// <summary>
        /// Only values that can be parsed to float values will be accepted
        /// </summary>
        Float,
        /// <summary>
        /// Only values that can be parsed to double values will be accepted
        /// </summary>
        Double,
        /// <summary>
        /// Only values that can be parsed to positive short values will be accepted
        /// </summary>
        PositiveShort,
        /// <summary>
        /// Only values that can be parsed to positive integer values will be accepted
        /// </summary>
        PositiveInteger,
        /// <summary>
        /// Only values that can be parsed to positive float values will be accepted
        /// </summary>
        PositiveFloat,
        /// <summary>
        /// Only values that can be parsed to positive double values will be accepted
        /// </summary>
        PositiveDouble,
    }
}