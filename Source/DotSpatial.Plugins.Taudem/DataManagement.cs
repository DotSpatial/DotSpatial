// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;

namespace DotSpatial.Plugins.Taudem
{
    /// <summary>
    /// The DataManagement namespace will contain basic file handling routines
    /// such as copy and delete, as well as some more complex methods for appending and merging..
    /// </summary>
    public class DataManagement
    {
        #region Methods

        /// <summary>
        /// Copies a shapefile and all associated files.
        /// </summary>
        /// <param name="oldShapefilePath">Full path to the original shapefile (including .shp extension).</param>
        /// <param name="newShapefilePath">Full path to where the copy should be saved (including .shp extension).</param>
        /// <returns>False if an error was encoutered, true otherwise.</returns>
        public static bool CopyShapefile(string oldShapefilePath, string newShapefilePath)
        {
            Debug.WriteLine("CopyShapefile(oldShapefilePath" + oldShapefilePath + ",\n" + "         newShapefilePath" + newShapefilePath + ")");
            if (string.IsNullOrEmpty(oldShapefilePath))
            {
                Trace.WriteLine("Argument Exception: oldShapefilePath cannot be null.");
                return false;
            }

            if (string.IsNullOrEmpty(newShapefilePath))
            {
                Trace.WriteLine("Argument Exception: newShapefilePath cannot be null.");
                return false;
            }

            if (!File.Exists(oldShapefilePath))
            {
                Trace.WriteLine("Input shapefile does not exists: " + oldShapefilePath);
                return false;
            }

            // todo:
            // DeleteShapefile(newShapefilePath);

            // Copy the files that make up a shape file:

            // .shp
            TryCopy(oldShapefilePath, newShapefilePath);

            // .shx
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".shx"), Path.ChangeExtension(newShapefilePath, ".shx"));

            // .dbf
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".dbf"), Path.ChangeExtension(newShapefilePath, ".dbf"));

            // .spx
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".spx"), Path.ChangeExtension(newShapefilePath, ".spx"));

            // .prj
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".prj"), Path.ChangeExtension(newShapefilePath, ".prj"));

            // .sbn
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".prj"), Path.ChangeExtension(newShapefilePath, ".sbn"));

            // .xml
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".xml"), Path.ChangeExtension(newShapefilePath, ".xml"));

            // .shp.xml
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".shp.xml"), Path.ChangeExtension(newShapefilePath, ".shp.xml"));

            Debug.WriteLine("Finished CopyShapefile");

            return true;
        }

        /// <summary>
        /// Attempts to copy a file
        /// </summary>
        /// <param name="oldName">The old filename</param>
        /// <param name="newName">The new filename</param>
        /// <returns>True on success</returns>
        public static bool TryCopy(string oldName, string newName)
        {
            try
            {
                if (File.Exists(oldName))
                {
                    File.Copy(oldName, newName, true);
                    var fl = new FileInfo(newName);
                    fl.Attributes = fl.Attributes & (FileAttributes.Archive & FileAttributes.ReadOnly);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception thrown while copying " + oldName + " to " + newName + ":\n" + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Deletes the grid.
        /// </summary>
        /// <param name="tmpClipPath">Path of the grid that should be deleted.</param>
        internal static void DeleteGrid(string tmpClipPath)
        {
            // might need the real method sometime...
        }

        #endregion
    }
}