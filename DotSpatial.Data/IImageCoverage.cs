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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:46:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// An Image Coverage just consists of several images that can be thought of as a single image region.
    /// Queries for pixel values for a region will simply return the first value in the set that is not
    /// completely transparent.
    /// </summary>
    public interface IImageCoverage : IImageSet
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of tiles.
        /// </summary>
        List<IImageData> Images
        {
            get;
            set;
        }

        #endregion
    }
}