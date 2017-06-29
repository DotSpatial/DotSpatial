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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/22/2008 10:22:19 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ILayerCollection2
    /// </summary>
    public interface ILayerCollection : ILayerEventList<ILayer>, IDisposable, IDisposeLock
    {
        /// <summary>
        /// Gets or sets the ParentGroup for this layer collection, even if that parent group
        /// is not actually a map frame.
        /// </summary>
        IGroup ParentGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MapFrame for this layer collection.
        /// </summary>
        IFrame MapFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the currently active layer.
        /// </summary>
        ILayer SelectedLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Given a base name, this increments a number for appending
        /// if the name already exists in the collection.
        /// </summary>
        /// <param name="baseName">The string base name to start with</param>
        /// <returns>The base name modified by a number making it unique in the collection</returns>
        string UnusedName(string baseName);
    }
}