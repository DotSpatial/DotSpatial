using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

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
            /* Supposed next directory hierarchy:
              
              - UnitTestProjectFolder
              -- bin
              --- Debug(Release)
              ---- ExecutingAssembly location
              - TestFiles             
             
             */

            var executingAssemblyFile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            var executingDirectory = Path.GetDirectoryName(executingAssemblyFile);
            Debug.Assert(executingDirectory != null);
            var ds_solutionPath = Path.GetFullPath(Path.Combine(executingDirectory, @"..\..\.."));
            Debug.Assert(Directory.Exists(ds_solutionPath));
            Debug.Assert(ds_solutionPath != null);
            return Path.Combine(ds_solutionPath,
                                "TestFiles",
                                relativePath);
        }
    }
}
