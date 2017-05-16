// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Extensions;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Saves the project as zip file.
    /// </summary>
    public class SaveProjectAsZip : ISaveProjectFileProvider
    {
        #region Properties

        /// <summary>
        /// Gets the extension.
        /// </summary>
        public string Extension => ".zip";

        /// <summary>
        /// Gets the file type description.
        /// </summary>
        public string FileTypeDescription => "Archive File";

        #endregion

        #region Methods

        /// <summary>
        /// Saves the graph to a file with the given file name.
        /// </summary>
        /// <param name="fileName">Filename of the resulting file.</param>
        /// <param name="graph">Graph that gets saved.</param>
        public void Save(string fileName, string graph)
        {
            ArchiveSerializer.Save(fileName, graph);
        }

        #endregion
    }
}