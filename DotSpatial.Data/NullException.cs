// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 4:41:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// NullExceptioncs
    /// </summary>
    public class NullException : ArgumentNullException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the NullLogException, but does not set the message
        /// or log the exception.
        /// </summary>
        public NullException()
        {
        }

        /// <summary>
        /// Creates a new instance of NullException
        /// </summary>
        public NullException(string parameterName)
            : base(DataStrings.Argument_Null_S.Replace("%S", parameterName))
        {
        }

        #endregion
    }
}