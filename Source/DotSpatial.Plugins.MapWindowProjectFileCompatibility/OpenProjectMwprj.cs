using System.ComponentModel.Composition;

using DotSpatial.Controls;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// This provider can be used to open MapWindow 4 Projects.
    /// </summary>
    public class OpenProjectMwprj : IOpenProjectFileProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        public AppManager App { get; set; }

        /// <inheritdoc/>
        public string Extension => ".mwprj";

        /// <inheritdoc/>
        public string FileTypeDescription => "MapWindow 4 Project";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public bool Open(string fileName)
        {
            new LegacyProjectDeserializer(App.Map).OpenFile(fileName);
            return true;
        }

        #endregion
    }
}