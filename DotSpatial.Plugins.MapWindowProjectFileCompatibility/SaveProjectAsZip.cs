using DotSpatial.Extensions;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    public class SaveProjectAsZip : ISaveProjectFileProvider
    {
        #region ISaveProjectFileProvider Members

        public void Save(string fileName, string graph)
        {
            ArchiveSerializer.Save(fileName, graph);
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