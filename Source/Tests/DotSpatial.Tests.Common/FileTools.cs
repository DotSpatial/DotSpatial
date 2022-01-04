// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;

namespace DotSpatial.Tests.Common
{
    /// <summary>
    /// FileTools.
    /// </summary>
    public static class FileTools
    {
        /// <summary>
        /// Deletes the shp, dbf, shx and prj of the given shapefile.
        /// </summary>
        /// <param name="fileName">Name of the shapefile that should be deleted.</param>
        public static void DeleteShapeFile(string fileName)
        {
            if (fileName != null)
            {
                File.Delete(Path.ChangeExtension(fileName, ".shp"));
                File.Delete(Path.ChangeExtension(fileName, ".dbf"));
                File.Delete(Path.ChangeExtension(fileName, ".shx"));
                File.Delete(Path.ChangeExtension(fileName, ".prj"));
            }
        }

        /// <summary>
        /// Creates a uniquely named, zero-byte temporary file on disk and returns the full
        /// path of that file.
        /// </summary>
        /// <param name="extension">Extension that should be used.</param>
        /// <returns>The path of the created file.</returns>
        public static string GetTempFileName(string extension)
        {
            var tmpFile = Path.GetTempFileName();
            if (Path.GetExtension(tmpFile) == extension)
            {
                return tmpFile;
            }

            var toReturn = Path.ChangeExtension(tmpFile, extension);
            File.Delete(tmpFile);
            return toReturn;
        }
    }
}
