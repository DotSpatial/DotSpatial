using System;
using System.Diagnostics;
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

        /// <summary>
        /// Get path to test file
        /// </summary>
        /// <param name="relativePath">Relative path inside TestFiles folder</param>
        /// <returns>Full path to test file</returns>
        public static string PathToTestFile(string relativePath)
        {
            var ds_solutionPath = Environment.GetEnvironmentVariable("DS_SOLUTION_DIR", EnvironmentVariableTarget.User);
            Debug.Assert(ds_solutionPath != null);
            Debug.Assert(Directory.Exists(ds_solutionPath));
            return Path.Combine(ds_solutionPath,
                                "TestFiles",
                                relativePath);
        }
    }
}
