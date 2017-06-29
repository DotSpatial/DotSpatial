using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Controls;
using Ionic.Zip;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Open MW4 .wma archive file
    /// </summary>
    public class LegacyArchiveDeserializer
    {
        private AppManager _applicationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyArchiveDeserializer"/> class.
        /// </summary>
        /// <param name="_applicationManager">The _application manager.</param>
        public LegacyArchiveDeserializer(AppManager _applicationManager)
        {
            this._applicationManager = _applicationManager;
        }

        private static bool CanSilentlyOverwrite(string selectedPath, ZipFile zip)
        {
            // check for files that might be overridden and warn the user.
            int fileCount = 0;
            foreach (string entryFileName in zip.EntryFileNames)
            {
                if (File.Exists(Path.Combine(selectedPath, entryFileName)))
                    fileCount++;
            }
            if (fileCount > 0)
            {
                DialogResult result = MessageBox.Show("Unpacking will overwrite " + fileCount + " files.", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return result == DialogResult.Yes;
            }
            return true;
        }

        /// <summary>
        /// Opens the MW4 style project file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void OpenFile(string fileName)
        {
            Contract.Requires(!String.IsNullOrEmpty(fileName), "fileName is null or empty.");
            Contract.Requires(_applicationManager != null, "_applicationManager is null.");

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select a location to unpack the archive.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                ZipFile zip = ZipFile.Read(fileName);

                if (!CanSilentlyOverwrite(selectedPath, zip))
                    return;

                try
                {
                    zip.ExtractAll(selectedPath, ExtractExistingFileAction.OverwriteSilently);

                    foreach (var file in Directory.EnumerateFiles(selectedPath))
                    {
                        if (file.EndsWith("mwprj") || file.EndsWith("dspx"))
                        {
                            _applicationManager.SerializationManager.OpenProject(file);
                            break;
                        }
                    }
                }
                catch (ZipException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}