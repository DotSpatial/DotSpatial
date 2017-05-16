// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;

namespace DotSpatial.Data
{
    /// <summary>
    /// IDataProvider is what you implement to expand the data handling methods of DotSpatial
    /// </summary>
    [InheritedExport]
    public interface IDataProvider
    {
        #region Properties

        /// <summary>
        /// Gets a basic description of what your provider does.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimited
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceded by the name provided
        /// on this object.
        /// </summary>
        string DialogReadFilter { get; }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceded by the name provided on this object.
        /// In addition, the same extension mapping will be used in order to pair a string driver code to the
        /// extension.
        /// </summary>
        string DialogWriteFilter { get; }

        /// <summary>
        /// Gets a preferably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the progress handler to use.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control. Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="name">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>A List of IDataSets to be added to the Map. These can also be groups of datasets.</returns>
        IDataSet Open(string name);

        #endregion
    }
}