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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/19/2010 9:44:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PngInsuficientLengthException
    /// </summary>
    public class PngInsuficientLengthException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PngInsuficientLengthException
        /// </summary>
        public PngInsuficientLengthException(int length, int totalLength, int offset)
            : base(DataStrings.PngInsuficientLengthException.Replace("%S1", length.ToString()).Replace("%S2", totalLength.ToString()).Replace("%S3", offset.ToString()))
        {
        }

        #endregion
    }
}