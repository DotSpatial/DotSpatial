using System.ComponentModel.Composition;
using DotSpatial.Controls;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    public class OpenProjectZip : IOpenProjectFileProvider
    {
        /// <summary>
        /// Gets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        public AppManager App { get; set; }

        #region IOpenProjectFileProvider Members

        public bool Open(string fileName)
        {
            new LegacyArchiveDeserializer(App).OpenFile(fileName);
            return true;
        }

        public string Extension
        {
            get { return ".zip"; }
        }

        public string FileTypeDescription
        {
            get { return "Archive File"; }
        }

        #endregion
    }
}