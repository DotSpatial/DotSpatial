// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// OgrDataProvider.
    /// </summary>
    public class OgrDataProvider : IDataProvider
    {
        #region Constructors

        static OgrDataProvider()
        {
            GdalConfiguration.ConfigureOgr();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description of the Vector Provider.
        /// </summary>
        public string Description => "GDAL/OGR Vector";

        /// <summary>
        /// Gets the dialog filter to use when opening a file.
        /// </summary>
        public string DialogReadFilter => "OGR Vectors|*.shp;*.kml;*.dxf";

        /// <summary>
        /// Gets the dialog filter to use when saving to a file.
        /// </summary>
        public string DialogWriteFilter => string.Empty;

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public string Name => "Ogr Vector Provider";

        /// <summary>
        /// Gets or sets the progress handler that gets updated with progress information.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">File t hat gets opened.</param>
        /// <returns>Content of the file as data set.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return new MultiLayerDataSet(fileName);
        }

        #endregion
    }
}