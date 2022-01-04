// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
using DotSpatial.Controls;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// This provider can be used to open zipped projects.
    /// </summary>
    public class OpenProjectZip : IOpenProjectFileProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        public AppManager App { get; set; }

        /// <inheritdoc/>
        public string Extension => ".zip";

        /// <inheritdoc/>
        public string FileTypeDescription => "Archive File";

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Open(string fileName)
        {
            new LegacyArchiveDeserializer(App).OpenFile(fileName);
            return true;
        }

        #endregion
    }
}