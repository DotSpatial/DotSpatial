// -----------------------------------------------------------------------
// <copyright file="PackageManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Controls.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using DotSpatial.Controls.Properties;

    /// <summary>
    /// The PackageManager performs file based operations on packages.
    /// </summary>
    public static class PackageManager
    {
        /// <summary>
        /// For internal use only. (Method signature may change)
        /// </summary>
        /// <param name="appManager"></param>
        /// <param name="extensionName"></param>
        /// <param name="folderName"></param>
        public static void MarkForUninstallation(this AppManager appManager, string extensionName, string folderName)
        {
            // Deactivate the extension.
            var ext = appManager.Extensions.Where(t => t.Name == extensionName).FirstOrDefault();
            if (ext != null && ext.IsActive)
                ext.Deactivate();

            // Add it to a list to be delete on application restart.
            string path = Path.Combine(AppManager.PackageDirectory, folderName);
            Settings.Default.UninstalledExtensions.Add(path);
            Settings.Default.Save();
        }

        internal static void DeleteUninstalledPackages()
        {
            if (Settings.Default.UninstalledExtensions.Count > 0)
            {
                try
                {
                    foreach (string extension in Settings.Default.UninstalledExtensions)
                    {
                        string path = Path.Combine(AppManager.AbsolutePathToExtensions, extension);
                        DeleteDirectory(path);
                    }
                }
                catch (ArgumentException) { }
                catch (DirectoryNotFoundException) { }
                catch (IOException) { }
                catch (NotSupportedException) { }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    // We don't want to end up in a situation where we continually try to delete files and we cannot.
                    Settings.Default.UninstalledExtensions.Clear();
                    Settings.Default.Save();
                }
            }

        }

        public static void TryDeleteAllPackages()
        {
            if (Directory.Exists(DotSpatial.Controls.AppManager.AbsolutePathToExtensions))
            {
                try
                {
                    DeleteDirectory(DotSpatial.Controls.AppManager.AbsolutePathToExtensions);
                }
                catch (ArgumentException) { }
                catch (DirectoryNotFoundException) { }
                catch (IOException) { }
                catch (NotSupportedException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        /// <summary>
        /// Deletes the directory and any files, recursively.
        /// </summary>
        /// <param name="path">The path.</param>
        internal static void DeleteDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                // Prevent errors caused by readonly files.
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(path, false);
        }
    }
}
