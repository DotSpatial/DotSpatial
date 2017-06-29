// -----------------------------------------------------------------------
// <copyright file="DataManagement.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

namespace MapWinGeoProc
{
    //using MapWinUtility;

    /// <summary>
    /// The DataManagement namespace will contain basic file handling routines
    /// such as copy and delete, as well as some more complex methods for appending and merging..
    /// </summary>
    public class DataManagement
    {
        #region CopyShapefile()

        /// <summary>
        /// Copies a shapefile and all associated files.
        /// </summary>
        /// <param name="oldShapefilePath">Full path to the original shapefile (including .shp extension).</param>
        /// <param name="newShapefilePath">Full path to where the copy should be saved (including .shp extension).</param>
        /// <returns>False if an error was encoutered, true otherwise.</returns>
        public static bool CopyShapefile(string oldShapefilePath, string newShapefilePath)
        {
            Debug.WriteLine("CopyShapefile(oldShapefilePath" + oldShapefilePath + ",\n" +
                            "         newShapefilePath" + newShapefilePath + ")");
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
            //todo:
            //DeleteShapefile(newShapefilePath);

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

        #endregion

        //    #region RenameGrid()
        //    /// <summary>
        //    /// Rename a grid (or move it's path) and all associated files.
        //    /// </summary>
        //    /// <param name="oldGridPath">The full path to the original grid file (including extension).</param>
        //    /// <param name="newGridPath">The full path to the new grid file (including extension).</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    public static bool RenameGrid(string oldGridPath, string newGridPath)
        //    {
        //        Debug.WriteLine("RenameGrid(oldGridPath: " + oldGridPath + ",\n" +
        //                                 "           newGridPath: " + newGridPath + ")");
        //        if (CopyGrid(oldGridPath, newGridPath))
        //        {
        //            if (DeleteGrid(oldGridPath))
        //            {
        //                Debug.WriteLine("Finished RenameGrid");
        //                return true;
        //            }

        //            Debug.WriteLine("DeleteGrid returned false; Rename " + oldGridPath + " failed.");
        //            return false;
        //        }

        //        Debug.WriteLine("CopyGrid returned false; Rename " + oldGridPath + " failed.");
        //        return false;
        //    }
        //    #endregion

        //    #region RenameShapefile()
        //    /// <summary>
        //    /// Rename a shapefile and all associated files.
        //    /// </summary>
        //    /// <param name="oldShapefilePath">Full path to the original shapefile (including the .shp extension).</param>
        //    /// <param name="newShapefilePath">New path to the shapefile (including the .shp extension).</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    public static bool RenameShapefile(string oldShapefilePath, string newShapefilePath)
        //    {
        //        Debug.WriteLine("RenameShapefile(oldShapefilePath: " + oldShapefilePath + ",\n" +
        //                                 "                newSahpefilePath: " + newShapefilePath + ")");
        //        if (CopyShapefile(oldShapefilePath, newShapefilePath))
        //        {
        //            if (DeleteShapefile(oldShapefilePath))
        //            {
        //                Debug.WriteLine("Finished RenameShapefile");
        //                return true;
        //            }

        //            Debug.WriteLine("DeleteShapefile returned false; RenameShapefile failed.");
        //            return false;
        //        }

        //        Debug.WriteLine("CopyShapefile returned false; RenameShapefile failed.");
        //        return false;
        //    }
        //    #endregion

        //    #region CopyGrid()
        //    /// <summary>
        //    /// Copies a grid and all associated files from one destination path to another.
        //    /// </summary>
        //    /// <param name="oldGridPath">The original path to the grid (including extension).</param>
        //    /// <param name="newGridPath">The path to where the grid copy should be (including extension).</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    /// <remarks>Won't work for Grid formats that are directory names like an Esri grid format yet</remarks>
        //    public static bool CopyGrid(string oldGridPath, string newGridPath)
        //    {
        //        Debug.WriteLine("CopyGrid(oldGridPath" + oldGridPath + ",\n" +
        //                                 "         newGridPath" + newGridPath + ")");
        //        if (string.IsNullOrEmpty(oldGridPath))
        //        {
        //            Trace.WriteLine("Argument Exception: oldGridPath cannot be null.");
        //            return false;
        //        }

        //        if (string.IsNullOrEmpty(newGridPath))
        //        {
        //            Trace.WriteLine("Argument Exception: newGridPath cannot be null.");
        //            return false;
        //        }

        //        if (!File.Exists(oldGridPath))
        //        {
        //            if (Path.GetExtension(oldGridPath) == string.Empty)
        //            {
        //                if (Directory.Exists(oldGridPath))
        //                {
        //                    // TODO: Esri GRID format handling.
        //                    Trace.WriteLine(
        //                        "Argument Exception: Esri grids that are directories are not currently supported.");
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                Trace.WriteLine("Input grid does not exists: " + oldGridPath);
        //            }
        //        }

        //        DeleteGrid(newGridPath);

        //        // Copy the files that make up a grid file:

        //        // .grd
        //        TryCopy(oldGridPath, newGridPath);

        //        // .bmp
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".bmp"), Path.ChangeExtension(newGridPath, ".bmp"));

        //        // .bpw
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".bpw"), Path.ChangeExtension(newGridPath, ".bpw"));

        //        // .mwleg
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".mwleg"), Path.ChangeExtension(newGridPath, ".mwleg"));

        //        // .prj
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".prj"), Path.ChangeExtension(newGridPath, ".prj"));

        //        // .aux
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".aux"), Path.ChangeExtension(newGridPath, ".aux"));

        //        // .rrd
        //        TryCopy(Path.ChangeExtension(oldGridPath, ".rrd"), Path.ChangeExtension(newGridPath, ".rrd"));

        //        Debug.WriteLine("Finished CopyGrid");
        //        return true;
        //    }
        //    #endregion

        //    /// <summary>
        //    /// Attempts to delete a file.
        //    /// </summary>
        //    /// <param name="file">The filename to delete</param>
        //    /// <returns>Boolean, false if the file is null or the directory doesn't exist or the file doesn't exist</returns>
        //    public static bool TryDelete(string file)
        //    {
        //        Debug.WriteLine("TryDelete(file: " + file + ")");
        //        if (string.IsNullOrEmpty(file))
        //        {
        //            Trace.WriteLine("File cannot be null.\nTryDelete = false");
        //            return false;
        //        }

        //        var directory = Path.GetDirectoryName(file);
        //        if (string.IsNullOrEmpty(directory))
        //        {
        //            Trace.WriteLine("Directory cannot be null.\nTryDelete = false");
        //            return false;
        //        }

        //        if (!Directory.Exists(directory))
        //        {
        //            Trace.WriteLine("Specified Directory: " + directory + " did not exist.\nTryDelete = false");
        //            return false;
        //        }

        //        if (!File.Exists(file))
        //        {
        //            Trace.WriteLine("Specified File: " + file + " did not exist.\nTryDelete = false");
        //            return false;
        //        }

        //        try
        //        {
        //            File.Delete(file);
        //        }
        //        catch (Exception ex)
        //        {
        //            Trace.WriteLine("Exception trying to delete " + file + ":\n" + ex.Message + ".\nTryDelete = false");
        //            return false;
        //        }

        //        Debug.WriteLine("Finished TryDelete.");
        //        return true;
        //    }

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
                    fl.Attributes = (fl.Attributes & (FileAttributes.Archive & FileAttributes.ReadOnly));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception thrown while copying " + oldName + " to " + newName + ":\n" + ex.Message);
            }

            return false;
        }

        //    /// <summary>
        //    /// Copies the .mwleg file from the input grid to the output grid.
        //    /// </summary>
        //    /// /// <param name="inputGf">The name of the input grid</param>
        //    /// <param name="resultGf">The name of the output grid</param>
        //    /// <returns>true on success</returns>
        //    public static bool CopyGridLegend(string inputGf, string resultGf)
        //    {
        //        Debug.WriteLine("CopyGridLegend(inputGF: " + inputGf + ", resultGF:" + resultGf);
        //        var legendFile = Path.ChangeExtension(inputGf, ".mwleg");
        //        if (File.Exists(legendFile))
        //        {
        //            return TryCopy(legendFile, Path.ChangeExtension(resultGf, ".mwleg"));
        //        }

        //        // Paul Meems Nov. 8 2010
        //        // Fill the error message when the file doesn't exists:
        //        Trace.WriteLine("mwleg file doesn't exists: " + legendFile);
        //        return false;
        //    }

        //    #region DeleteGrid()
        //    /// <summary>
        //    /// Deletes the grid and associated  files (.bmp, .bpw, .mwleg, .prj).
        //    /// </summary>
        //    /// <param name="gridPath">Full path to the grid file, including extension.</param>
        //    /// <returns>Boolean, False if gridPath is null, the directory doesn't exist, or any of the files exist but could not be deleted.</returns>
        //    public static bool DeleteGrid(string gridPath)
        //    {
        //        Debug.WriteLine("In DeleteGrid(gridPath" + gridPath + ",\n");
        //        if (string.IsNullOrEmpty(gridPath))
        //        {
        //            Trace.WriteLine("gridPath cannot be null.");
        //            return false;
        //        }

        //        var dir = Path.GetDirectoryName(gridPath);
        //        if (dir != null)
        //        {
        //            if (!Directory.Exists(dir))
        //            {
        //                Trace.WriteLine("The specified directory: " + dir + " does not exist.");
        //                return false;
        //            }
        //        }

        //        // .grd
        //        TryDelete(gridPath);

        //        // .bmp
        //        TryDelete(Path.ChangeExtension(gridPath, ".bmp"));

        //        // .bpw
        //        TryDelete(Path.ChangeExtension(gridPath, ".bpw"));

        //        // .mwleg
        //        TryDelete(Path.ChangeExtension(gridPath, ".mwleg"));

        //        // .prj
        //        TryDelete(Path.ChangeExtension(gridPath, ".prj"));

        //        // .aux
        //        TryDelete(Path.ChangeExtension(gridPath, ".aux"));

        //        // .rrd
        //        TryDelete(Path.ChangeExtension(gridPath, ".rrd"));

        //        return true;
        //    }
        //    #endregion

        //    #region DeleteShapefile()
        //    /// <summary>
        //    /// Deletes shapefile and associated files (.shx, .dbf, .prj).
        //    /// </summary>
        //    /// <param name="shapefilePath">Full path to shapefile, including .shp extension</param>
        //    /// <returns>True on success</returns>
        //    public static bool DeleteShapefile(string shapefilePath)
        //    {
        //        Debug.WriteLine("In DeleteShapefile(shapefilePath: " + shapefilePath + ")");
        //        if (string.IsNullOrEmpty(shapefilePath))
        //        {
        //            Trace.WriteLine("Shapefile path cannot be null.");
        //            return false;
        //        }

        //        var dir = Path.GetDirectoryName(shapefilePath);

        //        if (dir != null)
        //        {
        //            if (!Directory.Exists(dir))
        //            {
        //                Trace.WriteLine("The specified directory: " + dir + " does not exist.");
        //                return false;
        //            }
        //        }

        //        // .shp
        //        TryDelete(shapefilePath);

        //        // .shx
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".shx"));

        //        // .dbf
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".dbf"));

        //        // .prj
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".prj"));

        //        // .spx
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".spx"));

        //        // .sbn
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".sbn"));

        //        // .mwsr
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".mwsr"));

        //        // .lbl
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".lbl"));

        //        // .xml
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".xml"));

        //        // .shp.xml
        //        TryDelete(Path.ChangeExtension(shapefilePath, ".shp.xml"));

        //        Debug.WriteLine("Finished DeleteShapefile");
        //        return true;
        //    }
        //    #endregion

        //    #region ChangeGridFormat

        //    /// <summary>
        //    /// Change the grid format?
        //    /// </summary>
        //    /// <param name="origFilename">Original grid filename</param>
        //    /// <param name="newFilename">Output grid filename</param>
        //    /// <param name="newFileType">Specifies the original file format of the grid</param>
        //    /// <param name="newFileFormat">Specifies the new file format</param>
        //    /// <param name="multFactor">Like Extrusion, this multiplies the Z value</param>
        //    /// <returns>True on success</returns>
        //    public static bool ChangeGridFormat(string origFilename, string newFilename, MapWinGIS.GridFileType newFileType, MapWinGIS.GridDataType newFileFormat, float multFactor)
        //    {
        //        var hasErrors = false;

        //        var grid = new MapWinGIS.Grid();
        //        grid.Open(origFilename, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);

        //        Debug.WriteLine("Writing Grid to New Format");

        //        // If we're multiplying by a factor, must
        //        // create the new grid and actually do it ourselves.
        //        // Otherwise, can save directly
        //        // Jiri Kadlec 1-28-2009 we still neet to create a new grid when the data or file type is different.
        //        if (multFactor == 1 && newFileFormat == grid.DataType)
        //        {
        //            Debug.WriteLine("Saving directly to new format");
        //            if (!grid.Save(newFilename, newFileType, null))
        //            {
        //                Trace.WriteLine("Error in saving grid: " + grid.get_ErrorMsg(grid.LastErrorCode));
        //                hasErrors = true;
        //            }
        //        }
        //        else
        //        {
        //            Debug.WriteLine("Saving to new format with mult. factor: " + multFactor);

        //            var hdr = new MapWinGIS.GridHeader();
        //            hdr.CopyFrom(grid.Header);

        //            var newgrid = new MapWinGIS.Grid();
        //            if (!newgrid.CreateNew(newFilename, hdr, newFileFormat, hdr.NodataValue, true, newFileType, null))
        //            {
        //                Trace.WriteLine("Unable to create new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
        //                return false;
        //            }

        //            var ncols = grid.Header.NumberCols;
        //            var nrows = grid.Header.NumberRows;
        //            var oneRow = new float[ncols + 1];
        //            for (var i = 0; i < nrows; i++)
        //            {
        //                grid.GetFloatWindow(i, i, 0, ncols, oneRow[0]);
        //                // CWG 2/2/2011 little point doing this if multFactor is 1
        //                if (multFactor != 1)
        //                {
        //                    for (var z = 0; z < ncols; z++)
        //                    {
        //                        oneRow[z] *= multFactor;
        //                    }
        //                }

        //                newgrid.PutFloatWindow(i, i, 0, ncols, oneRow[0]);
        //            }

        //            if (!newgrid.Save(newFilename, newFileType, null))
        //            {
        //                Trace.WriteLine("Error in saving new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
        //                hasErrors = true;
        //            }

        //            if (!newgrid.Close())
        //            {
        //                Trace.WriteLine("Error in closing new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
        //                hasErrors = true;
        //            }
        //        }

        //        return !hasErrors;
        //    }
        //    #endregion

        //    /// <summary>
        //    /// Not Implemented
        //    /// Appends one shapefile to another. No intersection/union operation is performed
        //    /// on overlapping shapes. The input shapefile is overwritten.
        //    /// </summary>
        //    /// <param name="inputSFPath">Full path to the input shapefile.</param>
        //    /// <param name="appendSFPath">Full path to the shapefile that needs to be appended to the input shapefile.</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    /// <remarks>This function is not yet implemented.</remarks>
        //    public static bool AppendShapefile(string inputSFPath, string appendSFPath)
        //    {
        //        // TODO: Implement this function
        //        Error.ClearErrorLog();
        //        Trace.WriteLine("This function is not yet implemented.");

        //        // PM Added NotImplementedException:
        //        throw new NotImplementedException();

        //        // return false;
        //    }

        //    /// <summary>
        //    /// Not Implemented
        //    /// Removes shapes from the shapefile that contain a specified attribute.
        //    /// </summary>
        //    /// <param name="inputSFPath">The full path to the input shapefile.</param>
        //    /// <param name="resultSFPath">The full path to the resulting shapefile.</param>
        //    /// <param name="fieldID">The ID value for which field in the input shapefile will be considered.</param>
        //    /// <param name="attributeLoc">The location of an attribute value to compare against for removing shapes.</param>
        //    /// <param name="compOperation">The comparison method to use (==, !=, >=, etc).</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    /// <remarks>This function is not yet implemented.</remarks>
        //    public static bool DissolveShapefile(string inputSFPath, string resultSFPath, int fieldID, int attributeLoc, int compOperation)
        //    {
        //        // TODO: Implement this function
        //        Error.ClearErrorLog();
        //        Trace.WriteLine("This function is not yet implemented.");

        //        // PM Added NotImplementedException:
        //        throw new NotImplementedException();

        //        // return false;
        //    }

        //    /// <summary>
        //    /// Not Implemented
        //    /// Combines two grids into one.
        //    /// </summary>
        //    /// <param name="grid1Path">Full path to the first grid.</param>
        //    /// <param name="grid2Path">Full path to the second grid.</param>
        //    /// <param name="resultGridPath">Full path to the result grid.</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    /// <remarks>This function is not yet implemented.</remarks>
        //    public static bool MergeGrids(string grid1Path, string grid2Path, string resultGridPath)
        //    {
        //        // TODO: Implement this function
        //        Error.ClearErrorLog();
        //        Trace.WriteLine("This function is not yet implemented.");

        //        // PM Added NotImplementedException:
        //        throw new NotImplementedException();

        //        // return false;
        //    }

        //    /// <summary>
        //    /// Not Implemented
        //    /// Combines two shapefiles into one.
        //    /// </summary>
        //    /// <param name="sfPath1">Full path to the first shapefile.</param>
        //    /// <param name="sfPath2">Full path to the second shapefile.</param>
        //    /// <param name="resultSFPath">Full path to the result shapefile.</param>
        //    /// <param name="mergeOperation">Indicates whether Union or Intersect should be performed on overlapping shapes.</param>
        //    /// <param name="tableOperation">Indicates how table data should be combined.</param>
        //    /// <returns>False if an error was encountered, true otherwise.</returns>
        //    /// <remarks>This function is not yet implemented.</remarks>
        //    public static bool MergeShapefiles(string sfPath1, string sfPath2, string resultSFPath, int mergeOperation, int tableOperation)
        //    {
        //        // TODO: Implement this function
        //        Error.ClearErrorLog();
        //        Trace.WriteLine("This function is not yet implemented.");

        //        // PM Added NotImplementedException:
        //        throw new NotImplementedException();

        //        // return false;
        //    }

        //    /// <summary>
        //    /// Uses dialogs to obtain input and output information for processing files
        //    /// </summary>
        //    public static void MergeShapefiles()
        //    {
        //        Debug.WriteLine("MergeShapefiles()");
        //        var merger = new clsMergeShapefiles();
        //        merger.DoMergeShapefiles();
        //        Debug.WriteLine("Exit MergeShapefiles()");
        //    }
        //}

        internal static void DeleteGrid(string tmpClipPath)
        {
            // might need the real method sometime...
        }
    }
}