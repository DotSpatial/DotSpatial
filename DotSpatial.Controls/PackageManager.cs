// -----------------------------------------------------------------------
// <copyright file="PackageManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Controls.Extensions
{
    using System;
    using System.IO;
    using DotSpatial.Controls.Properties;
    using DotSpatial.Extensions;

    /// <summary>
    /// The PackageManager performs file based operations on packages.
    /// </summary>
    public static class PackageManager
    {

        /// <summary>
        /// Marks the package for removal.
        /// </summary>
        /// <param name="appManager">The app manager.</param>
        /// <param name="path">The path.</param>
        public static void MarkPackageForRemoval(this AppManager appManager, string path)
        {
            // Add it to a list to be delete on application restart.
            Settings.Default.PackagesToRemove.Add(Path.Combine(AppManager.PackageDirectory, path));
            Settings.Default.Save();
        }

        /// <summary>
        /// Marks the extension for removal. This will leave any dependencies.
        /// </summary>
        /// <param name="appManager">The AppManager.</param>
        /// <param name="path">Name of the file.</param>
        public static void MarkExtensionForRemoval(this AppManager appManager, string path)
        {
            // Add it to a list to be delete on application restart.
            Settings.Default.ExtensionsToRemove.Add(path);
            Settings.Default.Save();
        }

        /// <summary>
        /// Ensures the extension is deactivated.
        /// </summary>
        /// <param name="appManager">The AppManager.</param>
        /// <param name="extensionName">Name of the extension.</param>
        /// <returns></returns>
        public static IExtension EnsureDeactivated(this AppManager appManager, string extensionName)
        {
            var ext = appManager.GetExtension(extensionName);
            if (ext != null && ext.IsActive)
                ext.Deactivate();

            return ext;
        }

        /// <summary>
        /// Removes the pending packages and extensions.
        /// </summary>
        internal static void RemovePendingPackagesAndExtensions()
        {
            if (Settings.Default.PackagesToRemove.Count > 0 || Settings.Default.ExtensionsToRemove.Count > 0)
            {
                foreach (string extension in Settings.Default.PackagesToRemove)
                {
                    string path = Path.Combine(AppManager.AbsolutePathToExtensions, extension);
                    TryDeleteDirectory(path);
                }

                foreach (string extension in Settings.Default.ExtensionsToRemove)
                {
                    TryDeleteFile(extension);
                }

                // We don't want to end up in a situation where we continually try to delete files and we cannot.
                Settings.Default.PackagesToRemove.Clear();
                Settings.Default.ExtensionsToRemove.Clear();
                Settings.Default.Save();
            }
        }

        private static void TryDeleteDirectory(string path)
        {
            try
            {
                DeleteDirectory(path);
            }
            catch (ArgumentException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
            catch (NotSupportedException) { }
            catch (UnauthorizedAccessException) { }
        }

        private static void TryDeleteFile(string path)
        {
            try
            {
                // Prevent errors caused by readonly files.
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
            catch (ArgumentException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
            catch (NotSupportedException) { }
            catch (UnauthorizedAccessException) { }
        }

        /// <summary>
        /// Deletes everything in the DotSpatial.Controls.AppManager.AbsolutePathToExtensions folder.
        /// </summary>
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
