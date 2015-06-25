using System.IO;

namespace DotSpatial.Tests.Common
{
    public static class FileTools
    {
        public static void DeleteShapeFile(string fileName)
        {
            File.Delete(Path.ChangeExtension(fileName, ".shp"));
            File.Delete(Path.ChangeExtension(fileName, ".dbf"));
            File.Delete(Path.ChangeExtension(fileName, ".shx"));
            File.Delete(Path.ChangeExtension(fileName, ".prj"));
        }

        public static string GetTempFileName(string extension)
        {
            var tmpFile = Path.GetTempFileName();
            if (Path.GetExtension(tmpFile) == extension) return tmpFile;
            var toReturn = Path.ChangeExtension(tmpFile, extension);
            File.Delete(tmpFile);
            return toReturn;
        }
    }
}
