// ********************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a plugin for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/10/2008 11:44:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// GDalException
    /// </summary>
    public class GdalException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class using Gdal.GetLastErrorMsg.
        /// </summary>
        public GdalException()
            : base(Gdal.GetLastErrorMsg())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GdalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GdalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}