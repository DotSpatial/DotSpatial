// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LayerProvider.
    /// </summary>
    public interface ILayerProvider
    {
        #region Properties

        /// <summary>
        /// Gets a basic description of what your provider does.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        string DialogReadFilter { get; }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        string DialogWriteFilter { get; }

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control. Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="fileName">A string specifying the complete path and extension of the file to open.</param>
        /// <param name="inRam">A boolean that, if ture, will request that the data be loaded into memory.</param>
        /// <param name="container">Any valid IContainer that should have the new layer automatically added to it.</param>
        /// <param name="progressHandler">An IProgressHandler interface for status messages.</param>
        /// <returns>A List of IDataSets to be added to the Map. These can also be groups of datasets.</returns>
        ILayer OpenLayer(string fileName, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler);

        #endregion
    }
}