// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 4:04:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Serialization.Properties;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// ReadOnlyException
    /// </summary>
    public class ReadOnlyException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of ReadOnlyException
        /// </summary>
        public ReadOnlyException()
            : base(Resources.ReadOnly)
        {
        }

        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="message"></param>
        public ReadOnlyException(string message)
            : base(message)
        {
        }

        #endregion
    }
}