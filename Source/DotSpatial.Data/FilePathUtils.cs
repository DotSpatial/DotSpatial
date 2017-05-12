using System;
using System.Collections.Specialized;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Contains misc utils for file paths.
    /// </summary>
    public static class FilePathUtils
    {
        #region Methods

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException">Occurs when the toPath is NULL</exception>
        // http://weblogs.asp.net/pwelter34/archive/2006/02/08/create-a-relative-path-code-snippet.aspx
        public static string RelativePathTo(string toPath)
        {
            string fromDirectory = Directory.GetCurrentDirectory();

            if (toPath == null) throw new ArgumentNullException(nameof(toPath));

            if (Path.IsPathRooted(fromDirectory) && Path.IsPathRooted(toPath))
            {
                if (string.Compare(Path.GetPathRoot(fromDirectory), Path.GetPathRoot(toPath), true) != 0) return toPath;
            }

            StringCollection relativePath = new StringCollection();
            string[] fromDirectories = fromDirectory.Split(Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(Path.DirectorySeparatorChar);

            int length = Math.Min(fromDirectories.Length, toDirectories.Length);

            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x], toDirectories[x], true) != 0) break;

                lastCommonRoot = x;
            }

            if (lastCommonRoot == -1) return toPath;

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
            {
                if (fromDirectories[x].Length > 0) relativePath.Add("..");
            }

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
            {
                relativePath.Add(toDirectories[x]);
            }

            // create relative path
            string[] relativeParts = new string[relativePath.Count];
            relativePath.CopyTo(relativeParts, 0);

            return string.Join(Path.DirectorySeparatorChar.ToString(), relativeParts);
        }

        #endregion
    }
}