//********************************************************************************************************
//File name: Hydrology.cs
//Description: Public class, provides methods for watershed delineation.
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
//you may not use this file except in compliance with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
//ANY KIND, either express or implied. See the License for the specific language governing rights and
//limitations under the License.
//
//The Original Code is MapWindow Open Source Hydrology, including the Fill algorithm that was
//developed by Ted Dunsford at Idaho State University.  This algorithm is currently being
//prepared for publication in Computers and Geosciences.
//
//Contributor(s): (Open source contributors should list themselves and their modifications here).
//7/7/2006 Ted Dunsford established this new environment for an improved hydrology toolkit.
//2/11/2009 Chris George rewrote BuildJoinedBasins to (dramatically) improve speed
//20/7/2010 Chris George made corrections to deal correctly with zero length links
//TODO the Magnitude field in the network shapefile is currently incorrectly calculated
//23/1/2011 Chris George replaced use of binary grids with GeoTiffs for TauDEM V5
//26/1/2011 Chris George replaced old Taudem and some Hydrology functions with TauDEM V5
//********************************************************************************************************

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Plugins.Taudem;
using DotSpatial.Topology;

namespace MapWinGeoProc
{
    //using System.Windows.Forms;
    //using MapWindow.Interfaces.Geometries;
    //using MapWinGeoProc.Dialogs;
    //using MapWinGeoProc.Pitfill;
    //using MapWinGeoProc.Topology2D;
    //using MapWinGIS;

    /// <summary>
    /// The Hydrology algorithms are especially designed for working with DEMs in the context
    /// of modeling hydrological processes.
    /// </summary>
    public class Hydrology
    {
        #region  --------- Fill ---------------

        /// <summary>
        /// Fills depressions in an image
        /// - Files specified by parameters
        /// - Progress and status messages will be sent back via IProgressHandler
        /// - Frames will be sized to default values
        /// </summary>
        /// <param name="SourceFile">String filename of unfilled DEM</param>
        /// <param name="DestFile">String filename of output file</param>
        /// <param name="progress">The progress.</param>
        /// <remarks>
        /// Images too large to process all at once are broken down into a framework.
        /// A frame represents what will be loaded into memory at any given time.
        /// </remarks>
        public static void Fill(string SourceFile, string DestFile, IProgressHandler progress)
        {
            // 2000 width
            // 1000 height
            //MapWinUtility.Logger.Dbg("Fill(SourceFile: " + SourceFile + ",\n" +
            //                         "     DestFile: " + DestFile + ",\n" +
            //                         "     IProgressHandler");
            File_Fill(SourceFile, DestFile, true, false, 10000, 2000, progress);
        }

        //        /// <summary>
        //        /// An overload of the Fill function which will generate a GeoprocDialog for the Fill function
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static void Fill(IProgressHandler callback)
        //        {
        //           Trace.WriteLine("Fill(IProgressHandler) -> DoFillDiag");
        //            DoFillDiag(false, callback);
        //        }

        //        /// <summary>
        //        /// An overload of the Fill function which will generate a GeoprocDialog for the Fill function
        //        /// </summary>
        //        /// <returns></returns>
        //        public static void Fill()
        //        {
        //           Trace.WriteLine("Fill() -> DoFillDiag");
        //            DoFillDiag(false, null);
        //        }

        //        /// <summary>
        //        /// This overload does not take a callback, but allows the user to show the progress dialog.
        //        /// </summary>
        //        /// <param name="ShowProgressDialog">Boolean, true will display the progress dialog.</param>
        //        public static void Fill(bool ShowProgressDialog)
        //        {
        //           Trace.WriteLine("Fill(ShowProgressDialog: " + ShowProgressDialog.ToString() + ") -> DoFillDiag");
        //            DoFillDiag(ShowProgressDialog, null);
        //        }

        //        /// <summary>Fills pitholes</summary>
        //        /// <param name="showProgressDialog">Show the progress dialog</param>
        //        /// <param name="callback">The callback</param>
        //        private static void DoFillDiag(bool showProgressDialog, IProgressHandler callback)
        //        {
        //           Trace.WriteLine(string.Format("DoFillDiag(showProgressDialog: {0}, IProgressHandler)", showProgressDialog));
        //            var fillDiag = new GeoProcDialog();
        //            var demElem = fillDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            var fillResElem = fillDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);

        //            fillDiag.Text = @"Sub-basins to Shapefile Conversion";
        //            fillDiag.DialogHelpTitle = "Sub-basins to Shapefile Conversion";
        //            fillDiag.DialogHelpText = "This function will generate a polygon shapefile of sub-basins from a sub-basin grid and D8 grid.";
        //            fillDiag.Height = 200;
        //            fillDiag.HelpPanelVisible = false;

        //            demElem.Caption = "Digital Elevation Model Grid Path";
        //            demElem.HelpButtonVisible = false;

        //            fillResElem.Caption = "Pit-Filled DEM Grid Path";
        //            fillResElem.HelpButtonVisible = false;

        //            if (fillDiag.ShowDialog() == DialogResult.OK)
        //            {
        //                // Defaults to Overwriting silently (not showing a messagebox for overwriting destination.)
        //                // Defaults to NOT showing the progress dialog
        //                // Default Width = 10,000
        //                // Default Height = 2,000
        //                File_Fill(demElem.Filename, fillResElem.Filename, true, showProgressDialog, 10000, 2000, callback);
        //            }

        //            return;
        //        }

        //        //         |------------------------- FILE NAMES | DEFAULT FRAMES -----------------------------|

        //        /// <summary>
        //        /// Fills depressions in an image.
        //        /// - File names obtained through parameters
        //        /// - Progress will be shown through a dialog if ShowProgress is true
        //        /// - Frames will be sized to default values
        //        /// </summary>
        //        /// <param name="SourceFile">String the full path of a source DEM to fill</param>
        //        /// <param name="DestFile">String, the full path of the output filled dem</param>
        //        /// <param name="ShowProgressDialog">
        //        /// Boolean.  Function will display a dialog depicting progress if true.
        //        /// </param>
        //        ///
        //        /// <remarks>
        //        /// Images too large to process all at once are broken down into a framework.
        //        /// A frame represents what will be loaded into memory at any given time.
        //        /// </remarks>
        //        public static void Fill(string SourceFile, string DestFile, bool ShowProgressDialog)
        //        {
        //           Trace.WriteLine("Fill(SourceFile: " + SourceFile + ",\n" +
        //                                     "     DestFile: " + DestFile + ",\n" +
        //                                     "     ShowProgressDialog: " + ShowProgressDialog.ToString());
        //            // 2000 width
        //            // 1000 height
        //            File_Fill(SourceFile, DestFile, true, ShowProgressDialog, 10000, 2000, null);
        //        }
        //        /// <summary>
        //        /// Fills the depressions in the SourceFile and saves the output to DestFile
        //        /// </summary>
        //        /// <param name="SourceFile">String filename: Input file</param>
        //        /// <param name="DestFile">String filename: Output file</param>
        //        /// <param name="Overwrite">Boolean, true to overwrite destfile and associated files silently, false to show a messagebox.</param>
        //        /// <param name="ShowProgressDialog">Boolean, True to display the progress dialog with status grid</param>
        //        /// <param name="FrameWidth">Integer, The width of the smaller grids to use</param>
        //        /// <param name="FrameHeight">Integer, The height of the sub-images to use</param>
        //        /// <param name="IProgressHandler">A MapWinGIS.IProgressHandler for status messages</param>
        //        public static void Fill(string SourceFile, string DestFile, bool Overwrite, bool ShowProgressDialog, int FrameWidth, int FrameHeight, MapWinGIS.IProgressHandler IProgressHandler)
        //        {
        //           Trace.WriteLine("Fill(SourceFile: " + SourceFile + ",\n" +
        //                                     "     DestFile: " + DestFile + ",\n" +
        //                                     "     Overwrite: " + Overwrite.ToString() + ",\n" +
        //                                     "     ShowProgressDialog: " + ShowProgressDialog.ToString() + ",\n" +
        //                                     "     FrameWidth: " + FrameWidth.ToString() + ",\n" +
        //                                     "     FrameHeight: " + FrameHeight.ToString() + ",\n" +
        //                                     "     IProgressHandler)");
        //            File_Fill(SourceFile, DestFile, Overwrite, ShowProgressDialog, FrameWidth, FrameHeight, IProgressHandler);
        //        }

        //        //         |------------------------- GRIDS | DEFAULT FRAMES -----------------------------|
        //        /// <summary>
        //        /// /// Fills depressions in an image.
        //        /// - Grids obtained through parameters
        //        /// - Progress and status messages will be sent back via IProgressHandler
        //        /// - Frames will be sized to default values
        //        /// </summary>
        //        /// <param name="mwSourceGrid">Raster object to Fill</param>
        //        /// <param name="mwDestGrid">
        //        /// Raster object Output.
        //        /// Specifies output filename and extents.  Everything else will be obtained from original grid.
        //        /// </param>
        //        /// <param name="IProgressHandler">A MapWinGIS IProgressHandler to receive errors and status messages</param>
        //        /// /// <remarks>
        //        /// Images too large to process all at once are broken down into a framework.
        //        /// A frame represents what will be loaded into memory at any given time.
        //        /// </remarks>
        //        public static void Fill(Raster mwSourceGrid, Raster mwDestGrid, MapWinGIS.IProgressHandler IProgressHandler)
        //        {
        //            // 10000 Frame Width
        //            // 2000 Frame Height
        //           Trace.WriteLine("Fill(sourceGrid: " + mwSourceGrid.Filename + ",\n" +
        //                                     "      mwDestGrid: " + mwDestGrid.Filename + ",\n" +
        //                                     "      IProgressHandler");
        //            DoFill(mwSourceGrid, mwDestGrid, false, 10000, 2000, IProgressHandler);
        //        }

        //        /// <summary>
        //        /// /// Fills depressions in an image.
        //        /// - Grids obtained through parameters
        //        /// - Progress will be shown through a dialog if ShowProgress is true
        //        /// - Frames will be sized to default values
        //        /// </summary>
        //        /// <param name="mwSourceGrid">Raster object to Fill</param>
        //        /// <param name="mwDestGrid">
        //        /// Raster object Output.
        //        /// Specifies output filename and extents.  Everything else will be obtained from original grid.
        //        /// </param>
        //        /// <param name="ShowProgressDialog">
        //        /// Boolean.  Function will display a dialog depicting progress if true.
        //        /// </param>
        //        /// /// <remarks>
        //        /// Images too large to process all at once are broken down into a framework.
        //        /// A frame represents what will be loaded into memory at any given time.
        //        /// </remarks>
        //        public static void Fill(Raster mwSourceGrid, Raster mwDestGrid, bool ShowProgressDialog)
        //        {
        //            // 10000 width
        //            // 2000 height
        //           Trace.WriteLine("Fill(sourceGrid: " + mwSourceGrid.Filename + ",\n" +
        //                                     "      mwDestGrid: " + mwDestGrid.Filename + ",\n" +
        //                                     "      ShowProgressDialog: " + ShowProgressDialog.ToString());
        //            DoFill(mwSourceGrid, mwDestGrid, ShowProgressDialog, 10000, 2000, null);
        //        }

        /// <summary>
        /// Internal File handling
        /// </summary>
        /// <param name="sourceFile">The Source File. </param>
        /// <param name="destFile">The Dest File. </param>
        /// <param name="overwrite">The Overwrite. </param>
        /// <param name="showProgressDialog">The Show Progress Dialog. </param>
        /// <param name="frameWidth">The Frame Width. </param>
        /// <param name="frameHeight">The Frame Height. </param>
        /// <param name="callBack">The CallBack. </param>
        private static void File_Fill(string sourceFile, string destFile, bool overwrite, bool showProgressDialog, int frameWidth, int frameHeight, IProgressHandler callBack)
        {
            Trace.WriteLine("Fill(sourceGrid: " + sourceFile + ",\n" +
                            "     mwDestFile: " + destFile + ",\n" +
                            "     Overwrite: " + overwrite + ",\n" +
                            "     ShowProgressDialog: " + showProgressDialog + ",\n" +
                            "     FrameWidth: " + frameWidth + ", \n" +
                            "     FrameHeight: " + frameHeight + ", \n" +
                            "     IProgressHandler");

            if (callBack != null)
            {
                callBack.Progress("Status", 0, "Opening Files");
            }

            var pars = "-z " + Quote(sourceFile) + " -fel " + Quote(destFile);
            var result = RunTaudem("PitRemove.exe", pars, 1, false);
            //todo use tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput,

            if (result != 0)
            {
                MessageBox.Show(
                    "TauDEM Error " + result,
                    "TauDEM Error " + result,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }

            //var sourceRaster = Raster.Open(sourceFile);

            //if (File.Exists(destFile))
            //{
            //    // Delete any existing files for our output grid
            //    var bmp = Path.ChangeExtension(destFile, "bmp");
            //    var bpw = Path.ChangeExtension(destFile, "bpw");
            //    var prj = Path.ChangeExtension(destFile, "prj");
            //    var mwleg = Path.ChangeExtension(destFile, "mwleg");

            //    if (overwrite == false)
            //    {
            //        if (File.Exists(bmp) || File.Exists(bpw) ||
            //            File.Exists(prj) || File.Exists(mwleg))
            //        {
            //            if (MessageBox.Show("The output file exists, or associated files of the same name exist.  Do you wish to delete the existing files?\n", "Output Files Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            //            {
            //                return;
            //            }

            //            // This ensures mapwindow will recognize the new image as a new file.

            //            if (File.Exists(bmp))
            //            {
            //                File.Delete(bmp);
            //            }

            //            if (File.Exists(bpw))
            //            {
            //                File.Delete(bpw);
            //            }

            //            if (File.Exists(prj))
            //            {
            //                File.Delete(prj);
            //            }

            //            if (File.Exists(mwleg))
            //            {
            //                File.Delete(mwleg);
            //            }
            //        }
            //        else
            //        {
            //            if (MessageBox.Show(
            //                "The output file already exists. Do you wish to delete it?",
            //                "Destination File Already Exists",
            //                MessageBoxButtons.YesNo,
            //                MessageBoxIcon.Warning) == DialogResult.No)
            //            {
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (File.Exists(bmp))
            //        {
            //            File.Delete(bmp);
            //        }
            //    }

            //    File.Delete(destFile);
            //}

            //if (callBack != null)
            //{
            //    callBack.Progress("Status", 0, "Creating Output File");
            //}

            //var destinationRaster = Raster.CreateRaster(destFile, sourceRaster.DriverCode, sourceRaster.NumColumnsInFile, sourceRaster.NumRowsInFile, sourceRaster.NumBands, sourceRaster.DataType, new[] { string.Empty });

            // Trying something new... Copy first and only edit a few cells: 2-12-07
            //if (newNumColumns * newNumRows > 64000000)
            //{
            //    if (Path.GetExtension(sourceFile) != Path.GetExtension(destFile))
            //    {
            //        throw new ArgumentException("Images this large must have an output type that matches the input type.");
            //    }

            //    File.Copy(sourceFile, destFile);
            //    destGrid.Open(destFile, GridDataType.FloatDataType, true, GridFileType.UseExtension, callBack);
            //}
            //else
            //{
            // This allows for more versitile file types, but tends to lock up if the file is too large.
            // res = destGrid.CreateNew(destFile, newHeader, GridDataType.FloatDataType, float.MaxValue, true, GridFileType.UseExtension, callBack);
            //}

            //DoFill(sourceGrid, destGrid, showProgressDialog, frameWidth, frameHeight, callBack);

            //res = destGrid.Save(destFile, RasterFileType.UseExtension, callBack);

            //sourceRaster.Close();
            //destinationRaster.Close();
        }

        /*
        public const int UP = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;
        public const int RIGHT = 4;
        */

        /// <summary>
        /// Performs a Pitfill.  Returns true unless the operation was canceled by the dialog
        /// </summary>
        /// <param name="sourceGrid">The source Grid.</param>
        /// <param name="destGrid">The dest Grid.</param>
        /// <param name="showProgressDialog">The show Progress Dialog.</param>
        /// <param name="frameWidth">The frame Width.</param>
        /// <param name="frameHeight">The frame Height.</param>
        /// <param name="progress">The progress.</param>
        private static void DoFill(IRaster sourceGrid, IRaster destGrid, bool showProgressDialog, int frameWidth, int frameHeight, IProgressHandler progress)
        {
            Trace.WriteLine(
                "DoFill(sourceGrid: '" + sourceGrid.Filename + "\n" + "', mwDestGrid: '" + destGrid.Filename + "\n"
                + "', ShowProgressDialog: " + showProgressDialog + "\n" + ", FrameWidth: " + frameWidth + "\n"
                + ", FrameHeight: " + frameHeight + ", callback);\n");

            //var numRows = sourceGrid.NumRows;
            //var numCols = sourceGrid.NumColumns;
            //                                      Trace.WriteLine("Assigning Edges");

            //// We only need to do this if we are using the "create new" strategy
            //if (numRows * numCols < 64000000)
            //{
            //    // assume that we can store at least one whole row in memory at once
            //    var rowVals = new float[numCols];
            //    var colVals = new float[numRows];

            //    // Copy first row
            //    sourceGrid.GetFloatWindow(0, 0, 0, numCols - 1, rowVals[0]);
            //    destGrid.PutFloatWindow(0, 0, 0, numCols - 1, rowVals[0]);

            //    // Copy last row
            //    sourceGrid.GetFloatWindow(numRows - 1, numRows - 1, 0, numCols - 1, rowVals[0]);
            //    destGrid.PutFloatWindow(numRows - 1, numRows - 1, 0, numCols - 1, rowVals[0]);
            //    Trace.WriteLine("Left Edge...");
            //    sourceGrid.GetFloatWindow(1, numRows - 2, 0, 0, colVals[0]);
            //    destGrid.PutFloatWindow(1, numRows - 2, 0, 0, colVals[0]);

            //    // Copy last column
            //    Trace.WriteLine("Right Edge...");
            //    sourceGrid.GetFloatWindow(1, numRows - 2, numCols - 1, numCols - 1, colVals[0]);
            //    destGrid.PutFloatWindow(1, numRows - 2, numCols - 1, numCols - 1, colVals[0]);
            //}

            //myFramework.InitDependencyFiles(destGrid.Filename);

            //Trace.WriteLine("Beginning Algorithm");
            //var myFrame = myFramework.First_Frame();
            //var total = myFramework.NumFramesTall * myFramework.NumFramesWide;
            //var count = 0;
            //var algTime = new TimeSpan();
            //do
            //{
            //    if (myFrame.HasDependencies)
            //    {
            //        var dependencies = myFrame.get_Dependencies();

            //        var depsExist = new bool[5];
            //        myFrame.Status = Frame.StatusType.tkBeingEvaluated1;
            //        myProgressDialog.ReDraw();
            //        if (myProgressDialog.IsCanceled)
            //        {
            //            myFramework.DeleteFiles();
            //            throw new ApplicationException("User Interrupt.");
            //        }

            //        if (myProgressDialog.IsPaused)
            //        {
            //            while (myProgressDialog.IsPaused)
            //            {
            //                Application.DoEvents();
            //            }
            //        }

            //        Trace.WriteLine(string.Format("Frame: [{0}][{1}] - ", myFrame.X, myFrame.Y));
            //        algTime += Algorithm.FloodDependencies(
            //            sourceGrid, destGrid, dependencies, myFrame, depsExist, myProgressDialog);
            //        myFrame.Status = Frame.StatusType.tkEvaluatedNoDependencies;
            //    }

            //    myFrame = myFramework.Next_Frame();
            //    count += 1;
            //    myProgressDialog.Progress = count * 100 / total;
            //}
            //while (myFramework.HasDependencies);

            //Trace.WriteLine(
            //    string.Format("TIME: {0}, {1}, {2}, {3}", algTime.Hours, algTime.Minutes, algTime.Seconds.ToString(), algTime.Milliseconds));
            //myProgressDialog.ReDraw();
            //myFramework.DeleteFiles();
            //myProgressDialog.Hide();
            //Application.DoEvents();
        }

        #endregion

        //Class level variables.  These variables are shared by the functions, or can be used
        //to preserve information between function calls, in the event of future dependency.
        //To indicate that they are class level, I will mark them with m_

        //private class Times
        //{
        //    DateTime t_StartTime;
        //    DateTime t_StartOfLast;
        //    TimeSpan t_TotalSpan;
        //    TimeSpan t_LastSpan;
        //    public DateTime StartTime
        //    {
        //        get
        //        {
        //            return t_StartTime;
        //        }
        //        set
        //        {
        //            t_StartTime = value;
        //        }
        //    }//End StartTime
        //    public DateTime StartOfLast
        //    {
        //        get
        //        {
        //            return t_StartOfLast;
        //        }
        //        set
        //        {
        //            t_StartOfLast = value;
        //        }
        //    }//End StartOfLast
        //    //Returns a statuslike string showing the timespan
        //    public string TotalSpan
        //    {
        //        get
        //        {
        //            t_TotalSpan = System.DateTime.Now - t_StartTime;
        //            return t_TotalSpan.ToString();
        //        }
        //    }
        //    public string LastTime
        //    {
        //        get
        //        {
        //            t_LastSpan = System.DateTime.Now - t_StartOfLast;
        //            return t_LastSpan.ToString();
        //        }
        //    }
        //}//End of Times class

        ///// <summary>
        ///// This is a dataformat to hold some strings with error information
        ///// </summary>
        //public struct ErrorLog
        //{
        //    /// <summary>
        //    /// A stringbuilder with log of errors
        //    /// </summary>
        //    public System.Text.StringBuilder log;
        //    /// <summary>
        //    /// The last error thrown in Hydrology
        //    /// </summary>
        //    public string LastErrorMessage;
        //    /// <summary>
        //    /// The last function called
        //    /// </summary>
        //    public string LastFunctionCalled;
        //    /// <summary>
        //    /// The last progress message before the error
        //    /// </summary>
        //    public string LastLocation;
        //    /// <summary>
        //    /// Clears the local error information
        //    /// </summary>
        //    public void clear()
        //    {
        //        log = new System.Text.StringBuilder();
        //        LastErrorMessage = String.Empty;
        //        LastFunctionCalled = String.Empty;
        //        LastLocation = String.Empty;
        //    }
        //}
        //ErrorLog m_ErrorLog;

        //// These are some properties for Hydrology that I plan for other functions to use as well.
        //#region Shared Properties

        ///// <summary>
        ///// A string holding the last error generated in this class
        ///// </summary>
        //public ErrorLog LastErrorInfo
        //{
        //    get
        //    {
        //        return m_ErrorLog;
        //    }
        //}

        //#endregion

        //        #region General Support Functions

        //        private void ReportProgress(string KeyOfSender, int Percent, string Message, MapWinGIS.IProgressHandler IProgressHandler)
        //        {
        //            m_ErrorLog.LastLocation = Message;
        //            if (IProgressHandler == null) return; // IProgressHandler is where we are sending progress
        //            if (Percent < 0) Percent = 0;
        //            if (Percent > 100) Percent = 100; //ProgressBars don't like overflow percentages
        //            IProgressHandler.Progress(KeyOfSender, Percent, Message);

        //        }//End Report Progress

        //        // Handles IProgressHandler as well as taking advantage of the Global Error object
        //        private void SetError(string Message)
        //        {
        //            Error.SetErrorMsg(Message);
        //            m_ErrorLog.LastErrorMessage = Message;
        //            m_ErrorLog.log.Append(Message);
        //            // IProgressHandler.Error("Error", Message);
        //        }//End SetError

        //        // Clears errors from both error tracking systems
        //        private void ClearErrors()
        //        {
        //            m_ErrorLog.clear();
        //            Error.ClearErrorLog();
        //        }

        // Puts quotation marks round a string

        #region DSNode enum

        public enum DSNode
        {
            Outlet,
            Inlet,
            Reservoir,
            PointSource,
        }

        #endregion

        #region ElevationUnits enum

        /// <summary>
        /// Enum for elevation units used in DEMs
        /// </summary>
        public enum ElevationUnits
        {
            /// <summary>
            /// Meters = 0
            /// </summary>
            meters,
            /// <summary>
            /// Centimeters = 1
            /// </summary>
            centimeters,
            /// <summary>
            /// Feet = 2
            /// </summary>
            feet
        }

        #endregion

        private static string Quote(string s)
        {
            return "\"" + s + "\"";
        }

        //        #endregion

        //        #region Taudem and Custom Hydrology

        //        #region "Focus Mask"
        //        /// <summary>
        //        /// A hydrology function to clip by a mask grid
        //        /// </summary>
        //        /// <param name="demPath">Path to the DEM to clip</param>
        //        /// <param name="maskGridPath">Path of the mask grid</param>
        //        /// <param name="maskResultPath">Result grid</param>
        //        /// <param name="callback">IProgressHandler for progress</param>
        //        /// <returns></returns>
        //        public static int Mask(string demPath, string maskGridPath, string maskResultPath, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("Mask( demPath: " + demPath + ", \n" +
        //                                     "      MaskGridPath: " + maskGridPath + ",\n" +
        //                                     "      MaskResultPath: " + maskResultPath + ",\n" +
        //                                     "      IProgressHandler);\n");
        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, "Masking Grid");
        //            }

        //            DataManagement.DeleteGrid(maskResultPath);

        //            var g = new Grid();
        //            g.Open(maskGridPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);

        //            // This is OK for speed, but don't do something like make a new grid with this "reference copied" header.
        //            var maskHead = g.Header;
        //            var tmpShape = getShapeFromExtents(getGridExtents(g));
        //            if (DotSpatial.Analysis.ClipRaster.ClipRasterWithPolygon(demPath, tmpShape, maskResultPath))
        //            {
        //                var oldperc = 0;
        //                var gridMasked = new Grid();
        //                gridMasked.Open(maskResultPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);

        //                // This is OK for speed, but don't do something like make a new grid with this "reference copied" header.
        //                var maskedHead = gridMasked.Header;
        //                for (var row = 0; row <= maskHead.NumberRows - 1; row++)
        //                {
        //                    if (maskHead.NumberRows > 0)
        //                    {
        //                        var newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(maskHead.NumberRows)) * 100);
        //                        if (newperc > oldperc)
        //                        {
        //                            if (callback != null)
        //                            {
        //                                callback.Progress("Status", newperc, "Masking Grid");
        //                            }

        //                            oldperc = newperc;
        //                        }
        //                    }

        //                    for (var col = 0; col <= maskHead.NumberCols - 1; col++)
        //                    {
        //                        double currX;
        //                        double currY;
        //                        g.CellToProj(col, row, out currX, out currY);
        //                        int currCol;
        //                        int currRow;
        //                        gridMasked.ProjToCell(currX, currY, out currCol, out currRow);
        //                        if (double.Parse(g.get_Value(col, row).ToString()) == double.Parse(maskHead.NodataValue.ToString()))
        //                        {
        //                            gridMasked.set_Value(currCol, currRow, maskedHead.NodataValue);
        //                        }
        //                    }
        //                }

        //                // CWG 23/1/2011 changed to GeoTiff for Taudem V5
        //                gridMasked.Save(maskResultPath, GridFileType.GeoTiff, callback);
        //                gridMasked.Close();
        //            }

        //            g.Close();

        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, string.Empty);
        //            }

        //           Trace.WriteLine("Finished Mask");
        //            return 0;
        //        }

        //        /// <summary>
        //        /// A hydrology function to clip by shapefile
        //        /// </summary>
        //        /// <param name="demPath">Path to the dem to clip</param>
        //        /// <param name="maskShapePath">path to the shape path</param>
        //        /// <param name="selectedIndexesList">Arraylist of selected integer indexes from shapefile</param>
        //        /// <param name="maskResultPath">Result of the masking</param>
        //        /// <param name="callback">IProgressHandler for progress</param>
        //        /// <returns></returns>
        //        public static int Mask(string demPath, string maskShapePath, ArrayList selectedIndexesList, string maskResultPath, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("Mask( demPath: " + demPath + ", \n" +
        //                                     "      MaskShapePath: " + maskShapePath + ",\n" +
        //                                     "      MaskResultPath: " + maskResultPath + ",\n" +
        //                                     "      IProgressHandler);\n");
        //            var result = -1;
        //            var oldperc = 0;

        //            if (selectedIndexesList.Count > 0)
        //            {
        //                if (callback != null)
        //                {
        //                    callback.Progress("Status", 0, "Masking Grid");
        //                }

        //                DataManagement.DeleteGrid(maskResultPath);

        //                var demGrid = new Grid();
        //                demGrid.Open(demPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);
        //                var projStr = demGrid.Projection.ToString();
        //                demGrid.Close();

        //
        //                var sf = new Shapefile();
        //                sf.Open(maskShapePath, null);
        //                Shape tmpShape;
        //                if (selectedIndexesList.Count == 1)
        //                {
        //                    tmpShape = sf.get_Shape((int)selectedIndexesList[0]);
        //                    if (DotSpatial.Analysis.ClipRaster.ClipRasterWithPolygon(demPath, tmpShape, maskResultPath))
        //                    {
        //                        result = 0;
        //                    }
        //                }
        //                else if (selectedIndexesList.Count > 1)
        //                {
        //                    var grids = new Grid[selectedIndexesList.Count];
        //                    for (var i = 0; i <= selectedIndexesList.Count - 1; i++)
        //                    {
        //                        if (selectedIndexesList.Count > 0)
        //                        {
        //                            var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(selectedIndexesList.Count)) * 100);
        //                            if (newperc > oldperc)
        //                            {
        //                                if (callback != null)
        //                                {
        //                                    callback.Progress("Status", newperc, "Masking Grid");
        //                                }

        //                                oldperc = newperc;
        //                            }
        //                        }

        //                        var maskclipPath = string.Format(
        //                            "{0}{1}{2}_{3}.tif",
        //                            Path.GetDirectoryName(maskResultPath),
        //                            Path.DirectorySeparatorChar,
        //                            Path.GetFileNameWithoutExtension(maskResultPath),
        //                            i);
        //                        tmpShape = sf.get_Shape((int)selectedIndexesList[i]);
        //                        if (!DotSpatial.Analysis.ClipRaster.ClipRasterWithPolygon(demPath, tmpShape, maskclipPath))
        //                        {
        //                            continue;
        //                        }

        //                        var selectedGrid = new Grid();
        //                        selectedGrid.Open(maskclipPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);
        //                        selectedGrid.Projection.ToString() = projStr;
        //                        grids[i] = selectedGrid;
        //                    }

        //                    var u = new MapWinGIS.Utils();
        //                    var g = u.GridMerge(grids, maskResultPath, true, GridFileType.UseExtension, null);
        //                    g.Save(maskResultPath, GridFileType.UseExtension, callback);
        //                    g.Close();

        //                    for (var i = 0; i <= selectedIndexesList.Count - 1; i++)
        //                    {
        //                        var tmpPath = grids[i].Filename;
        //                        grids[i].Close();
        //                        DataManagement.DeleteGrid(tmpPath);

        //                    }
        //                }

        //                sf.Close();

        //                result = 0;

        //                if (callback != null)
        //                {
        //                    callback.Progress("Status", 0, string.Empty);
        //                }
        //            }

        //           Trace.WriteLine("Finished Mask");
        //            return result;
        //        }

        //        /// <summary>
        //        /// A hydrology function to clip by extents
        //        /// </summary>
        //        /// <param name="demPath">Path to the grid to mask</param>
        //        /// <param name="maskExtents">Path to the extents to mask by</param>
        //        /// <param name="maskResultPath">Path to the Resulting masked file</param>
        //        /// <param name="callback">IProgressHandler for progress</param>
        //        /// <returns>0 on success, -1 on fail</returns>
        //        public static int Mask(string demPath, Extents maskExtents, string maskResultPath, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("Mask( demPath: " + demPath + ", \n" +
        //                                     "      MaskExtents: " + maskExtents + ",\n" +
        //                                     "      MaskResultPath: " + maskResultPath + ",\n" +
        //                                     "      IProgressHandler);\n");
        //            var result = -1;
        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, "Masking Grid");
        //            }

        //            DataManagement.DeleteGrid(maskResultPath);

        //            var tmpShape = getShapeFromExtents(maskExtents);
        //            if (DotSpatial.Analysis.ClipRaster.ClipRasterWithPolygon(demPath, tmpShape, maskResultPath))
        //            {
        //                result = 0;
        //            }

        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, string.Empty);
        //            }

        //           Trace.WriteLine("Finished Mask");
        //            return result;
        //        }
        //        #endregion

        //        #region "Canyon Burn-in"
        //        /// <summary>
        //        /// Hydrology burnin function to lower DEM cell values along a given stream polyline
        //        /// </summary>
        //        /// <param name="streamNetShapePath">Path to the stream polyline</param>
        //        /// <param name="demPath">Path to the DEM to burn</param>
        //        /// <param name="burnedDemResultPath">Resultant burned DEM</param>
        //        /// <param name="callback">IProgressHandler for progress</param>
        //        /// <returns>0 on success -1 on fail</returns>
        //        /// <remarks>Paul Meems, 22-Aug-2011, minor changes to make the code more readable.</remarks>
        //        public static int CanyonBurnin(string streamNetShapePath, string demPath, string burnedDemResultPath, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("CanyonBurnin( StreamNetShapePath: " + streamNetShapePath + ", \n" +
        //                                     "      demPath: " + demPath + ",\n" +
        //                                     "      BurnedDEMResultPath: " + burnedDemResultPath + ",\n" +
        //                                     "      IProgressHandler);\n");
        //            var lineShape = new Shapefile();
        //            var gridDem = new Grid();
        //            var gridTrack = new Grid();
        //            double setVal;
        //            int i;
        //            Shape tmpShape;

        //            var oldperc = 0;

        //            // Used if assigning a flat value or subtracting
        //            const bool Subval = true;

        //            if (Subval)
        //            {
        //                setVal = 50;
        //            }
        //            else
        //            {
        //                setVal = double.Parse(gridDem.Minimum.ToString()) - 1;
        //            }

        //            gridDem.Open(demPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);
        //            gridDem.Save(burnedDemResultPath, GridFileType.UseExtension, null);

        //            var nodata = double.Parse(gridDem.Header.NodataValue.ToString());

        //            // CWG 23/1/2011 changed to GeoTiff for Taudem V5
        //            var strGridTrack = string.Format(
        //                "{0}{1}{2}_track.tif",
        //                Path.GetDirectoryName(demPath),
        //                Path.DirectorySeparatorChar,
        //                Path.GetFileNameWithoutExtension(demPath));

        //            // Chris M 12/14/2006 -- do not create a new grid with a direct reference to an old grid's header
        //            // gridTrack.CreateNew(strGridTrack, gridDem.Header, RasterDataType.DoubleDataType, nodata,true, RasterFileType.UseExtension,callback);
        //            var newHeader = new GridHeader();
        //            newHeader.CopyFrom(gridDem.Header);
        //            gridTrack.CreateNew(strGridTrack, newHeader, GridDataType.DoubleDataType, nodata, true, GridFileType.UseExtension, callback);

        //            var ext = getGridExtents(gridDem);
        //            var gridExt = new Envelope(ext);
        //            lineShape.Open(streamNetShapePath, callback);

        //            for (i = 0; i < lineShape.NumRows(); i++)
        //            {
        //                if (lineShape.NumRows() > 1)
        //                {
        //                    var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(lineShape.NumRows())) * 100);
        //                    if (newperc > oldperc)
        //                    {
        //                        if (callback != null)
        //                        {
        //                            callback.Progress("Status", newperc, "Burn-in");
        //                        }

        //                        oldperc = newperc;
        //                    }
        //                }

        //                tmpShape = lineShape.get_Shape(i);
        //                var shapeExt = new Envelope(tmpShape.Extents);

        //                // TODO: Use new GEOS methods:
        //                if (gridExt.Intersects(shapeExt))
        //                {
        //                    int j;
        //                    for (j = 1; j <= tmpShape.numPoints - 1; j++)
        //                    {
        //                        int x0;
        //                        int y0;
        //                        gridDem.ProjToCell(tmpShape.get_Point(j - 1).x, tmpShape.get_Point(j - 1).y, out x0, out y0);

        //                        int x1;
        //                        int y1;
        //                        gridDem.ProjToCell(tmpShape.get_Point(j).x, tmpShape.get_Point(j).y, out x1, out y1);

        //                        var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        //                        int swap;
        //                        if (steep)
        //                        {
        //                            swap = x0;
        //                            x0 = y0;
        //                            y0 = swap;
        //                            swap = x1;
        //                            x1 = y1;
        //                            y1 = swap;
        //                        }

        //                        if (x0 > x1)
        //                        {
        //                            swap = x0;
        //                            x0 = x1;
        //                            x1 = swap;
        //                            swap = y0;
        //                            y0 = y1;
        //                            y1 = swap;
        //                        }

        //                        var deltax = x1 - x0;
        //                        var deltay = Math.Abs(y1 - y0);
        //                        var err = 0;
        //                        var deltaerr = deltay;
        //                        var y = y0;
        //                        int ystep;
        //                        if (y0 < y1)
        //                        {
        //                            ystep = 1;
        //                        }
        //                        else
        //                        {
        //                            ystep = -1;
        //                        }

        //                        int x;
        //                        for (x = x0; x <= x1; x++)
        //                        {
        //                            double currVal;
        //                            if (steep)
        //                            {
        //                                if (double.Parse(gridTrack.get_Value(y, x).ToString()) != 1.0)
        //                                {
        //                                    currVal = double.Parse(gridDem.get_Value(y, x).ToString());
        //                                    if (nodata < 0)
        //                                    {
        //                                        if (currVal > nodata)
        //                                        {
        //                                            gridTrack.set_Value(y, x, 1);
        //                                            if (Subval)
        //                                            {
        //                                                gridDem.set_Value(y, x, currVal - setVal);
        //                                            }
        //                                            else
        //                                            {
        //                                                gridDem.set_Value(y, x, setVal);
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (currVal < nodata)
        //                                        {
        //                                            gridTrack.set_Value(y, x, 1);
        //                                            if (Subval)
        //                                            {
        //                                                gridDem.set_Value(y, x, currVal - setVal);
        //                                            }
        //                                            else
        //                                            {
        //                                                gridDem.set_Value(y, x, setVal);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (double.Parse(gridTrack.get_Value(x, y).ToString()) != 1.0)
        //                                {
        //                                    currVal = double.Parse(gridDem.get_Value(x, y).ToString());
        //                                    if (nodata < 0)
        //                                    {
        //                                        if (currVal > nodata)
        //                                        {
        //                                            gridTrack.set_Value(x, y, 1);
        //                                            if (Subval)
        //                                            {
        //                                                gridDem.set_Value(x, y, currVal - setVal);
        //                                            }
        //                                            else
        //                                            {
        //                                                gridDem.set_Value(x, y, setVal);
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (currVal < nodata)
        //                                        {
        //                                            gridTrack.set_Value(y, x, 1);
        //                                            if (Subval)
        //                                            {
        //                                                gridDem.set_Value(y, x, currVal - setVal);
        //                                            }
        //                                            else
        //                                            {
        //                                                gridDem.set_Value(y, x, setVal);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            err = err + deltaerr;
        //                            if (2 * err < deltax)
        //                            {
        //                                continue;
        //                            }

        //                            y = y + ystep;
        //                            err = err - deltax;
        //                        }
        //                    }
        //                }

        //               Trace.WriteLine("Finished Mask");
        //            }

        //            gridDem.Save(burnedDemResultPath, GridFileType.UseExtension, null);
        //            const int Result = 0;
        //            lineShape.Close();

        //            gridDem.Close();

        //            gridTrack.Close();

        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, string.Empty);
        //            }

        //            DataManagement.DeleteGrid(strGridTrack);
        //           Trace.WriteLine("Finished CanyonBurnin");
        //            return Result;
        //        }
        //        #endregion

        //        #region "AreaD8"
        /// <summary>
        /// Uses TauDEM V5
        /// Generates an area D8 grid which shows the paths of highest flow and can be used to delineate stream networks.
        /// </summary>
        /// <param name="d8Path">Path to a D8 grid to be converted into an area D8 grid.</param>
        /// <param name="outletsPath">Optional path to a point shape file which is used to designate outlet points on a grid. If this path is given, the resulting area D8 grid will only include values for those areas of the grid which flow into the outlet points given. All other portions of the grid will be set to 0.</param>
        /// <param name="areaD8ResultPath">Path to an area D8 output grid, </param>
        /// <param name="useOutlets">Boolean true for using outlets in delineation d8 areas</param>
        /// <param name="useEdgeContamCheck">Boolean true to ignore off-grid contributing area</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"> A callback object for internal status messages</param>
        /// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
        public static int AreaD8(string d8Path, string outletsPath, string areaD8ResultPath, bool useOutlets, bool useEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("AreaD8(d8Path: " + d8Path + ",\n" +
                                      "       outletsPath: " + outletsPath + ",\n" +
                                      "       AreaD8ResultPath: " + areaD8ResultPath + ",\n" +
                                      "       useOutlets: " + useOutlets + ",\n" +
                                      "       useEdgeContamCheck: " + useEdgeContamCheck + "\n" +
                                      "       NumProcesses: " + numProcesses + "\n" +
                                      "       ShowTaudemOutput: " + showTaudemOutput + "\n" +
                                      "       callback)");

            // DataManagement.DeleteGrid(areaD8ResultPath);

            if (callback != null)
            {
                callback.Progress("Status", 0, "D8 Area");
            }

            var pars = "-p " + Quote(d8Path) + " -ad8 " + Quote(areaD8ResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            if (!useEdgeContamCheck)
            {
                pars += " -nc";
            }

            var result = RunTaudem("AreaD8.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                MessageBox.Show("TauDEM Error " + result, "TauDEM Error " + result, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // CopyProjectionFromGrid(d8Path, areaD8ResultPath);
            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            Trace.WriteLine("Finished AreaD8");
            return result;
        }

        //        /// <summary>
        //        /// An overload of the AreaD8 function which will generate a GeoprocDialog for the AreaD8 function
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int AreaD8(IProgressHandler callback)
        //        {
        //            return DoAreaD8Diag(callback);
        //        }

        //        /// <summary>
        //        /// An overload of the AreaD8 function which will generate a GeoprocDialog for the AreaD8 function
        //        /// </summary>
        //        /// <returns></returns>
        //        public static int AreaD8()
        //        {
        //            return DoAreaD8Diag(null);
        //        }

        //        private static int DoAreaD8Diag(IProgressHandler callback)
        //        {
        //           Trace.WriteLine("DoAreaD8Diag(callback)");
        //            var ad8Diag = new GeoProcDialog();
        //            var d8Elem = ad8Diag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            var useOutletsElem = ad8Diag.Add_BooleanElement();
        //            var outletsElem = ad8Diag.Add_FileElement(GeoProcDialog.ElementTypes.OpenShapefile);
        //            var useEdgeElem = ad8Diag.Add_BooleanElement();
        //            var ad8ResElem = ad8Diag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            var numProcessesElem = ad8Diag.Add_TextElement();
        //            var showTaudemOutputElem = ad8Diag.Add_BooleanElement();

        //            ad8Diag.Text = @"D8 Flow Accumulation/Contributing Area";
        //            ad8Diag.HelpTitle = "D8 Flow Accumulation/Contributing Area";
        //            ad8Diag.HelpText = "This function will generate a grid of flow accumulations from a given D8 flow direction grid.";
        //            ad8Diag.Height = 350;
        //            ad8Diag.HelpPanelVisible = false;

        //            d8Elem.Caption = "D8 Flow Direction Grid Path";
        //            d8Elem.HelpButtonVisible = false;

        //            useOutletsElem.Caption = "Use Outlets/Inlets Shapefile";
        //            useOutletsElem.Text = @"Use Outlets/Inlets";
        //            useOutletsElem.HelpButtonVisible = false;

        //            outletsElem.Caption = "Outlets/Inlets Point Shapefile Path (optional)";
        //            outletsElem.HelpButtonVisible = false;
        //            outletsElem.HaltOnEmpty = false;

        //            useEdgeElem.Caption = "Check for Edge Contamination";
        //            useEdgeElem.Text = @"Use Edge Contamination Check";
        //            useEdgeElem.HelpButtonVisible = false;

        //            ad8ResElem.Caption = "D8 Contributing Area Grid Result Path";
        //            ad8ResElem.HelpButtonVisible = false;

        //            numProcessesElem.Caption = "Number of processes";
        //            numProcessesElem.Text = "8";
        //            numProcessesElem.HelpButtonVisible = false;

        //            showTaudemOutputElem.Caption = "Show Taudem output";
        //            showTaudemOutputElem.Text = @"Show Taudem output";
        //            showTaudemOutputElem.HelpButtonVisible = false;

        //            if (ad8Diag.ShowDialog() == DialogResult.OK)
        //            {
        //                return AreaD8(
        //                    d8Elem.Filename,
        //                    outletsElem.Filename,
        //                    ad8ResElem.Filename,
        //                    useOutletsElem.Value,
        //                    useEdgeElem.Value,
        //                    int.Parse(numProcessesElem.Text),
        //                    showTaudemOutputElem.Value,
        //                    callback);
        //            }

        //           Trace.WriteLine("Finished DoAreaD8Diag");
        //            return -2;
        //        }
        //        #endregion

        //        #region "WeightedAreaD8"
        //        /// <summary>
        //        /// Uses TauDEM V5
        //        /// Generates an area D8 grid which shows the paths of highest flow and can be used to delineate stream networks.
        //        /// </summary>
        //        /// <param name="d8Path">Path to a D8 grid to be converted into an area D8 grid.</param>
        //        /// <param name="weightPath">Path to a weight gid to be used in place of the d8 path weights</param>
        //        /// <param name="outletsPath">Optional path to a point shape file which is used to designate outlet points on a grid. If this path is given, the resulting area D8 grid will only include values for those areas of the grid which flow into the outlet points given. All other portions of the grid will be set to 0.</param>
        //        /// <param name="areaD8ResultPath">Path to an area D8 output grid, </param>
        //        /// <param name="useOutlets">Boolean true for using outlets in delineation d8 areas</param>
        //        /// <param name="useEdgeContamCheck">Boolean true to ignore off-grid contributing area</param>
        //        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        //        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        //        /// <param name="callback"> A callback object for internal status messages</param>
        //        /// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
        //        public static int WeightedAreaD8(string d8Path, string weightPath, string outletsPath, string areaD8ResultPath, bool useOutlets, bool useEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("WeightedAreaD8(d8Path: " + d8Path + ",\n" +
        //                                     "       WeightPath: " + weightPath + ",\n" +
        //                                     "       outletsPath: " + outletsPath + ",\n" +
        //                                     "       AreaD8ResultPath: " + areaD8ResultPath + ",\n" +
        //                                     "       useOutlets: " + useOutlets.ToString() + ",\n" +
        //                                     "       useEdgeContamCheck: " + useEdgeContamCheck.ToString() + "\n" +
        //                                     "       NumProcesses: " + numProcesses.ToString() + "\n" +
        //                                     "       ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
        //                                     "       callback)");

        //            var result = -1;
        //            DataManagement.DeleteGrid(areaD8ResultPath);
        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, "Weighted Area D8");
        //            }

        //            var pars = "-p " + Quote(d8Path) + " -ad8 " + Quote(areaD8ResultPath);
        //            if (useOutlets)
        //            {
        //                pars += " -o " + Quote(outletsPath);
        //            }

        //            pars += " -wg " + Quote(weightPath);
        //            if (!useEdgeContamCheck)
        //            {
        //                pars += " -nc";
        //            }

        //            result = RunTaudem("AreaD8.exe", pars, numProcesses, showTaudemOutput);
        //            if (result != 0)
        //            {
        //                MessageBox.Show(
        //                    "TauDEM Error " + result,
        //                    "TauDEM Error " + result,
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error,
        //                    DialogResult.OK);
        //            }

        //            CopyProjectionFromGrid(d8Path, areaD8ResultPath);
        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, string.Empty);
        //            }

        //           Trace.WriteLine("Finished Weighted AreaD8");
        //            return result;
        //        }

        //        #endregion

        //        #region "Dinf"
        /// <summary>A function to call the Taudem d-infinity calculations</summary>
        /// <param name="pitFillPath"></param>
        /// <param name="dInfResultPath"></param>
        /// <param name="dInfSlopeResultPath"></param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int DInf(string pitFillPath, string dInfResultPath, string dInfSlopeResultPath, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("Dinf(pitFillPath: " + pitFillPath + ",\n" +
                                      "       DInfResultPath: " + dInfResultPath + ",\n" +
                                      "       DInfSlopeResultPath: " + dInfSlopeResultPath + ",\n" +
                                      "       NumProcesses: " + numProcesses.ToString() + "\n" +
                                      "       ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
                                      "       callback)");

            int result = -1;
            if (callback != null)
            {
                callback.Progress("Status", 0, "D-inf Flow Directions");
            }

            //DataManagement.DeleteGrid(dInfResultPath);
            //DataManagement.DeleteGrid(dInfSlopeResultPath);

            var pars = "-ang " + Quote(dInfResultPath) + " -slp " + Quote(dInfSlopeResultPath) + " -fel "
                       + Quote(pitFillPath);

            result = RunTaudem("DinfFlowDir.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                MessageBox.Show("TauDEM Error " + result, "TauDEM Error " + result, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //CopyProjectionFromGrid(pitFillPath, dInfResultPath);
            //CopyProjectionFromGrid(pitFillPath, dInfSlopeResultPath);
            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            Trace.WriteLine("Finished DInf");
            return result;
        }

        //        /// <summary>
        //        /// An overload of Dinf which will generate a GeoprocDialog and execute the dinf from that.
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int DInf(IProgressHandler callback)
        //        {
        //            return DoDInfDiag(callback);
        //        }

        //        /// <summary>
        //        /// An overload of Dinf which will generate a GeoprocDialog and execute the dinf from that.
        //        /// </summary>
        //        /// <returns></returns>
        //        public static int DInf()
        //        {
        //            return DoDInfDiag(null);
        //        }

        //        private static int DoDInfDiag(IProgressHandler callback)
        //        {
        //            var dinfDiag = new GeoProcDialog();
        //            var pitFillElem = dinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            var dinfResElem = dinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            var dinfSlopeResElem = dinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            var numProcessesElem = dinfDiag.Add_TextElement();
        //            var showTaudemOutputElem = dinfDiag.Add_BooleanElement();

        //            dinfDiag.Text = @"D-Infinity Flow Directions and Slopes";
        //            dinfDiag.HelpTitle = "D-Infinity Flow Directions and Slopes";
        //            dinfDiag.HelpText = "This function will generate a grid of D-infinity flow directions and a grid of slopes, given a pit-filled DEM.";
        //            dinfDiag.Height = 250;
        //            dinfDiag.HelpPanelVisible = false;

        //            pitFillElem.Caption = "Pit Filled DEM Path";
        //            pitFillElem.HelpButtonVisible = false;

        //            dinfResElem.Caption = "D-Infinity Flow Direction Grid Result Path";
        //            dinfResElem.HelpButtonVisible = false;

        //            dinfSlopeResElem.Caption = "D-Infinity Slope Grid Result Path";
        //            dinfSlopeResElem.HelpButtonVisible = false;

        //            numProcessesElem.Caption = "Number of processes";
        //            numProcessesElem.Text = "8";
        //            numProcessesElem.HelpButtonVisible = false;

        //            showTaudemOutputElem.Caption = "Show Taudem output";
        //            showTaudemOutputElem.Text = @"Show Taudem output";
        //            showTaudemOutputElem.HelpButtonVisible = false;

        //            if (dinfDiag.ShowDialog() == DialogResult.OK)
        //            {
        //                return DInf(pitFillElem.Filename, dinfResElem.Filename, dinfSlopeResElem.Filename, int.Parse(numProcessesElem.Text), showTaudemOutputElem.Value, callback);
        //            }

        //            return -2;
        //        }
        //        #endregion

        //        #region "AreaDinf"
        //        /// <summary>
        //        /// Function to call the Taudem AreaDinf function
        //        /// </summary>
        //        /// <param name="dInfPath"></param>
        //        /// <param name="outletsPath"></param>
        //        /// <param name="areaDInfResultPath"></param>
        //        /// <param name="useOutlets"></param>
        //        /// <param name="useEdgeContamCheck"></param>
        //        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        //        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int AreaDInf(string dInfPath, string outletsPath, string areaDInfResultPath, bool useOutlets, bool useEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("AreaDInf(DInfPath: " + dInfPath + "\n " +
        //                                     "outletsPath: " + outletsPath + "\n" +
        //                                     "AreaDInfResultPath: " + areaDInfResultPath + "\n" +
        //                                     "useOutlets: " + useOutlets.ToString() + "\n" +
        //                                     "useEdgeContamCheck: " + useEdgeContamCheck + "\n" +
        //                                     "NumProcesses: " + numProcesses + "\n" +
        //                                     "ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
        //                                     "callback)");

        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, "D-inf Area");
        //            }

        //            DataManagement.DeleteGrid(areaDInfResultPath);
        //            var pars = "-ang " + Quote(dInfPath) + " -sca " + Quote(areaDInfResultPath);
        //            if (useOutlets)
        //            {
        //                pars += " -o " + Quote(outletsPath);
        //            }

        //            if (!useEdgeContamCheck)
        //            {
        //                pars += " -nc";
        //            }

        //            var result = RunTaudem("AreaDinf.exe", pars, numProcesses, showTaudemOutput);
        //            if (result != 0)
        //            {
        //                MessageBox.Show(
        //                    "TauDEM Error " + result,
        //                    "TauDEM Error " + result,
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error,
        //                    DialogResult.OK);
        //            }

        //            CopyProjectionFromGrid(dInfPath, areaDInfResultPath);
        //            if (callback != null)
        //            {
        //                callback.Progress("Status", 0, string.Empty);
        //            }

        //           Trace.WriteLine("Finished AreaDInf");
        //            return result;
        //        }

        //        /// <summary>
        //        /// An overload of the AreaDInf function which will generate a GeoprocDialog for the AreaDInf function
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int AreaDInf(IProgressHandler callback)
        //        {
        //            return DoAreaDInfDiag(callback);
        //        }

        //        /// <summary>
        //        /// An overload of the AreaDInf function which will generate a GeoprocDialog for the AreaDInf function
        //        /// </summary>
        //        /// <returns></returns>
        //        public static int AreaDInf()
        //        {
        //            return DoAreaDInfDiag(null);
        //        }

        //        private static int DoAreaDInfDiag(IProgressHandler callback)
        //        {
        //            var adinfDiag = new GeoProcDialog();
        //            var dinfElem = adinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            var useOutletsElem = adinfDiag.Add_BooleanElement();
        //            var outletsElem = adinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenShapefile);
        //            var useEdgeElem = adinfDiag.Add_BooleanElement();
        //            var adinfResElem = adinfDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            var numProcessesElem = adinfDiag.Add_TextElement();
        //            var showTaudemOutputElem = adinfDiag.Add_BooleanElement();

        //            adinfDiag.Text = @"D-Infinity Flow Accumulation/Contributing Area";
        //            adinfDiag.HelpTitle = "D-Infinity Flow Accumulation/Contributing Area";
        //            adinfDiag.HelpText = "This function will generate a grid of flow accumulations from a given D-Infinity flow direction grid.";
        //            adinfDiag.Height = 350;
        //            adinfDiag.HelpPanelVisible = false;

        //            dinfElem.Caption = "D-Infinity Flow Direction Grid Path";
        //            dinfElem.HelpButtonVisible = false;

        //            useOutletsElem.Caption = "Use Outlets/Inlets Shapefile";
        //            useOutletsElem.Text = @"Use Outlets/Inlets";
        //            useOutletsElem.HelpButtonVisible = false;

        //            outletsElem.Caption = "Outlets/Inlets Point Shapefile Path (optional)";
        //            outletsElem.HelpButtonVisible = false;
        //            outletsElem.HaltOnEmpty = false;

        //            useEdgeElem.Caption = "Check for Edge Contamination";
        //            useEdgeElem.Text = @"Use Edge Contamination Check";
        //            useEdgeElem.HelpButtonVisible = false;

        //            adinfResElem.Caption = "D-Infinity Contributing Area Grid Result Path";
        //            adinfResElem.HelpButtonVisible = false;

        //            numProcessesElem.Caption = "Number of processes";
        //            numProcessesElem.Text = "8";
        //            numProcessesElem.HelpButtonVisible = false;

        //            showTaudemOutputElem.Caption = "Show Taudem output";
        //            showTaudemOutputElem.Text = @"Show Taudem output";
        //            showTaudemOutputElem.HelpButtonVisible = false;

        //            if (adinfDiag.ShowDialog() == DialogResult.OK)
        //            {
        //                return AreaDInf(dinfElem.Filename, outletsElem.Filename, adinfResElem.Filename, useOutletsElem.Value, useEdgeElem.Value, int.Parse(numProcessesElem.Text), showTaudemOutputElem.Value, callback);
        //            }

        //            return -2;
        //        }
        //        #endregion

        //        #region "Delin Stream Grids"
        /// <summary>
        /// A function which will make appropriate calls to Taudem in order to form the network grids used in delineation
        /// </summary>
        /// <param name="DemGridPath"></param>
        /// <param name="PitFillPath"></param>
        /// <param name="D8Path"></param>
        /// <param name="D8SlopePath"></param>
        /// <param name="AreaD8Path"></param>
        /// <param name="OutletsPath"></param>
        /// <param name="StrahlOrdResultPath"></param>
        /// <param name="LongestUpslopeResultPath"></param>
        /// <param name="TotalUpslopeResultPath"></param>
        /// <param name="StreamGridResultPath"></param>
        /// <param name="StreamOrdResultPath"></param>
        /// <param name="TreeDatResultPath"></param>
        /// <param name="CoordDatResultPath"></param>
        /// <param name="StreamShapeResultPath"></param>
        /// <param name="WatershedGridResultPath"></param>
        /// <param name="Threshold"></param>
        /// <param name="UseOutlets"></param>
        /// <param name="UseEdgeContamCheck"></param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int DelinStreamGrids(string DemGridPath, string PitFillPath, string D8Path, string D8SlopePath, string AreaD8Path, string OutletsPath, string StrahlOrdResultPath, string LongestUpslopeResultPath, string TotalUpslopeResultPath, string StreamGridResultPath, string StreamOrdResultPath, string TreeDatResultPath, string CoordDatResultPath, string StreamShapeResultPath, string WatershedGridResultPath, int Threshold, bool UseOutlets, bool UseEdgeContamCheck, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            return RunDelinStreamGrids(DemGridPath, PitFillPath, D8Path, D8SlopePath, AreaD8Path, String.Empty, OutletsPath, StrahlOrdResultPath, LongestUpslopeResultPath, TotalUpslopeResultPath, StreamGridResultPath, StreamOrdResultPath, TreeDatResultPath, CoordDatResultPath, StreamShapeResultPath, WatershedGridResultPath, Threshold, UseOutlets, UseEdgeContamCheck, false, numProcesses, showTaudemOutput, callback);
        }

        /// <summary>
        /// An overload of delinStreamGrids which allows the use of Dinf in the delineation
        /// </summary>
        /// <param name="DemGridPath"></param>
        /// <param name="PitFillPath"></param>
        /// <param name="D8Path"></param>
        /// <param name="D8SlopePath"></param>
        /// <param name="AreaD8Path"></param>
        /// <param name="AreaDInfPath"></param>
        /// <param name="OutletsPath"></param>
        /// <param name="StrahlOrdResultPath"></param>
        /// <param name="LongestUpslopeResultPath"></param>
        /// <param name="TotalUpslopeResultPath"></param>
        /// <param name="StreamGridResultPath"></param>
        /// <param name="StreamOrdResultPath"></param>
        /// <param name="TreeDatResultPath"></param>
        /// <param name="CoordDatResultPath"></param>
        /// <param name="StreamShapeResultPath"></param>
        /// <param name="WatershedGridResultPath"></param>
        /// <param name="Threshold"></param>
        /// <param name="UseOutlets"></param>
        /// <param name="UseEdgeContamCheck"></param>
        /// <param name="UseDinf"></param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int DelinStreamGrids(string DemGridPath, string PitFillPath, string D8Path, string D8SlopePath, string AreaD8Path, string AreaDInfPath, string OutletsPath, string StrahlOrdResultPath, string LongestUpslopeResultPath, string TotalUpslopeResultPath, string StreamGridResultPath, string StreamOrdResultPath, string TreeDatResultPath, string CoordDatResultPath, string StreamShapeResultPath, string WatershedGridResultPath, int Threshold, bool UseOutlets, bool UseEdgeContamCheck, bool UseDinf, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            return RunDelinStreamGrids(DemGridPath, PitFillPath, D8Path, D8SlopePath, AreaD8Path, AreaDInfPath, OutletsPath, StrahlOrdResultPath, LongestUpslopeResultPath, TotalUpslopeResultPath, StreamGridResultPath, StreamOrdResultPath, TreeDatResultPath, CoordDatResultPath, StreamShapeResultPath, WatershedGridResultPath, Threshold, UseOutlets, UseEdgeContamCheck, UseDinf, numProcesses, showTaudemOutput, callback);
        }

        //        /// <summary>
        //        /// An overload of the DelinStreamGrids function which will generate a GeoprocDialog for the DelinStreamGrids function
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int DelinStreamGrids(IProgressHandler callback)
        //        {
        //            return doDelinStreamGridsDiag(callback);
        //        }

        //        /// <summary>
        //        /// An overload of the DelinStreamGrids function which will generate a GeoprocDialog for the DelinStreamGrids function
        //        /// </summary>
        //        /// <returns></returns>
        //        public static int DelinStreamGrids()
        //        {
        //            return doDelinStreamGridsDiag(null);
        //        }

        //        private static int doDelinStreamGridsDiag(IProgressHandler callback)
        //        {
        //            GeoProcDialog delinstreamDiag = new GeoProcDialog();
        //            FileElement demElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement fillElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement d8Elem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement d8slpElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement aread8Elem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            BooleanElement usedinfElem = delinstreamDiag.Add_BooleanElement();
        //            FileElement areadinfElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            BooleanElement useOutletsElem = delinstreamDiag.Add_BooleanElement();
        //            FileElement outletsElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            BooleanElement useEdgeElem = delinstreamDiag.Add_BooleanElement();
        //            TextElement threshElem = delinstreamDiag.Add_TextElement();
        //            FileElement strahlResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement longestResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement totalResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement streamGridResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement streamOrdResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement streamShapeResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveShapefile);
        //            FileElement wshedGridResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //            FileElement treeDatResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveFile);
        //            FileElement coordDatResElem = delinstreamDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveFile);
        //            TextElement numProcessesElem = delinstreamDiag.Add_TextElement();
        //            BooleanElement showTaudemOutputElem = delinstreamDiag.Add_BooleanElement();

        //            delinstreamDiag.Text = "Delineate TauDEM Stream Grids and Network";
        //            delinstreamDiag.HelpTitle = "Delineate TauDEM Stream Grids and Network";
        //            delinstreamDiag.HelpText = "This function will generate stream grids and network data from the given inputs.";
        //            delinstreamDiag.Height = 550;
        //            delinstreamDiag.HelpPanelVisible = false;

        //            demElem.Caption = "Digital Elevation Model Grid Path";
        //            demElem.HelpButtonVisible = false;

        //            fillElem.Caption = "Pit-Filled DEM Grid Path";
        //            fillElem.HelpButtonVisible = false;

        //            d8Elem.Caption = "D8 Flow Direction Grid Path";
        //            d8Elem.HelpButtonVisible = false;

        //            d8slpElem.Caption = "D8 Slope Grid Path";
        //            d8slpElem.HelpButtonVisible = false;

        //            aread8Elem.Caption = "D8 Contributing Area Grid Path";
        //            aread8Elem.HelpButtonVisible = false;

        //            usedinfElem.Caption = "Use D-Infinity Contributing Area Mask";
        //            usedinfElem.Text = "Use D-Infinity Mask";
        //            usedinfElem.HelpButtonVisible = false;

        //            areadinfElem.Caption = "Area D-Infinity Grid Path (optional)";
        //            areadinfElem.HelpButtonVisible = false;
        //            areadinfElem.HaltOnEmpty = false;

        //            useOutletsElem.Caption = "Use Outlets/Inlets Shapefile";
        //            useOutletsElem.Text = "Use Outlets/Inlets";
        //            useOutletsElem.HelpButtonVisible = false;

        //            outletsElem.Caption = "Outlets/Inlets Point Shapefile Path (optional)";
        //            outletsElem.HelpButtonVisible = false;
        //            outletsElem.HaltOnEmpty = false;

        //            useEdgeElem.Caption = "Check for Edge Contamination";
        //            useEdgeElem.Text = "Use Edge Contamination Check";
        //            useEdgeElem.HelpButtonVisible = false;

        //            threshElem.Caption = "# of Cells to Use for Accumulation threshold";
        //            threshElem.Text = "1000";
        //            threshElem.HelpButtonVisible = false;

        //            strahlResElem.Caption = "Strahler Network Order Grid Result Path";
        //            strahlResElem.HelpButtonVisible = false;

        //            longestResElem.Caption = "Longest Upslope Length Grid Result Path";
        //            longestResElem.HelpButtonVisible = false;

        //            totalResElem.Caption = "Total Upslope Length Grid Result Path";
        //            totalResElem.HelpButtonVisible = false;

        //            streamOrdResElem.Caption = "Stream Order Grid Result Path";
        //            streamOrdResElem.HelpButtonVisible = false;

        //            streamGridResElem.Caption = "Stream Raster Grid Result Path";
        //            streamGridResElem.HelpButtonVisible = false;

        //            wshedGridResElem.Caption = "Watershed Grid Result Path";
        //            wshedGridResElem.HelpButtonVisible = false;

        //            streamShapeResElem.Caption = "Stream Shapefile Result Path";
        //            streamShapeResElem.HelpButtonVisible = false;

        //            treeDatResElem.Caption = "Network Tree Result Data File Path";
        //            treeDatResElem.Filter = "Data Files (*.dat)|*.dat";
        //            treeDatResElem.HelpButtonVisible = false;

        //            coordDatResElem.Caption = "Network Coordinates Result Data File Path";
        //            coordDatResElem.Filter = "Data Files (*.dat)|*.dat";
        //            coordDatResElem.HelpButtonVisible = false;

        //            numProcessesElem.Caption = "Number of processes";
        //            numProcessesElem.Text = "8";
        //            numProcessesElem.HelpButtonVisible = false;

        //            showTaudemOutputElem.Caption = "Show Taudem output";
        //            showTaudemOutputElem.Text = "Show Taudem output";
        //            showTaudemOutputElem.HelpButtonVisible = false;

        //            if (delinstreamDiag.ShowDialog() == DialogResult.OK)
        //            {
        //                //TODO add stream shape and watershed grid
        //                return DelinStreamGrids(demElem.Filename, fillElem.Filename, d8Elem.Filename, d8slpElem.Filename, aread8Elem.Filename, areadinfElem.Filename, outletsElem.Filename, strahlResElem.Filename, longestResElem.Filename, totalResElem.Filename, streamGridResElem.Filename, streamOrdResElem.Filename, treeDatResElem.Filename, coordDatResElem.Filename, streamShapeResElem.Filename, wshedGridResElem.Filename, int.Parse(threshElem.Value), useOutletsElem.Value, useEdgeElem.Value, usedinfElem.Value, Int32.Parse(numProcessesElem.Value), showTaudemOutputElem.Value, callback);
        //            }
        //            return -2;
        //        }

        /// <summary>
        /// Runs GridNet.exe
        /// </summary>
        /// <param name="demGridPath">Input file</param>
        /// <param name="d8Path">Input file</param>
        /// <param name="longestUpslopeResultPath">Output file</param>
        /// <param name="totalUpslopeResultPath">Output file</param>
        /// <param name="strahlOrdResultPath">Output file</param>
        /// <param name="outletsPath">Input file</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>
        /// Taudem return number, 0 means OK
        /// </returns>
        public static int RunGridNetwork(
            string demGridPath,
            string d8Path,
            string longestUpslopeResultPath,
            string totalUpslopeResultPath,
            string strahlOrdResultPath,
            string outletsPath,
            bool useOutlets,
            int numProcesses,
            bool showTaudemOutput,
            IProgressHandler callback)
        {
            Trace.WriteLine("Grid Network");
            if (callback != null)
            {
                callback.Progress("Status", 0, "Grid Network");
            }

            //DataManagement.DeleteGrid(strahlOrdResultPath);
            //DataManagement.DeleteGrid(longestUpslopeResultPath);
            //DataManagement.DeleteGrid(totalUpslopeResultPath);

            var pars = "-p " + Quote(d8Path) + " -plen " + Quote(longestUpslopeResultPath) + " -tlen "
                       + Quote(totalUpslopeResultPath) + " -gord " + Quote(strahlOrdResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            var result = RunTaudem("GridNet.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                MessageBox.Show(
                    "TauDEM Error " + result,
                    "TauDEM Error " + result,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            //CopyProjectionFromGrid(demGridPath, strahlOrdResultPath);
            //CopyProjectionFromGrid(demGridPath, longestUpslopeResultPath);
            //CopyProjectionFromGrid(demGridPath, totalUpslopeResultPath);

            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            return result;
        }

        /// <summary>Runs threshold.exe</summary>
        /// <param name="areaD8Path">Input file</param>
        /// <param name="streamGridResultPath">Output file</param>
        /// <param name="threshold">The threshold</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>Taudem return number, 0 means OK</returns>
        /// <remarks>Created by Paul Meems, 23 Aug 2011</remarks>
        public static int RunAllStreamDelineation(
            string areaD8Path,
            string streamGridResultPath,
            int threshold,
            int numProcesses,
            bool showTaudemOutput,
            IProgressHandler callback)
        {
            Trace.WriteLine("All Stream Delineation");
            if (callback != null)
            {
                callback.Progress("Status", 0, "All Stream Delineation");
            }

            //DataManagement.DeleteGrid(streamGridResultPath);

            var areaGridPath = areaD8Path; // TODO CWG inf does not seems to work (useDinf)?areaDInfPath:areaD8Path;
            var pars = "-ssa " + Quote(areaGridPath) + " -src " + Quote(streamGridResultPath) + " -thresh "
                   + threshold.ToString(CultureInfo.InvariantCulture);
            var result = RunTaudem("threshold.exe", pars, numProcesses, showTaudemOutput);
            if (result != 0)
            {
                MessageBox.Show(
                    "TauDEM Error " + result,
                    "TauDEM Error " + result,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Call StreamNet.exe
        /// </summary>
        /// <param name="demGridPath">Input file</param>
        /// <param name="pitFillPath">Input file</param>
        /// <param name="d8Path">Input file</param>
        /// <param name="areaD8Path">Input file</param>
        /// <param name="outletsPath">Input file</param>
        /// <param name="streamGridResultPath">Output file</param>
        /// <param name="streamOrdResultPath">Output file</param>
        /// <param name="streamShapeResultPath">Output file</param>
        /// <param name="watershedGridResultPath">Output file</param>
        /// <param name="coordDatResultPath">Output file</param>
        /// <param name="treeDatResultPath">Output file</param>
        /// <param name="useOutlets">Use outlets</param>
        /// <param name="numProcesses">Number of processes</param>
        /// <param name="showTaudemOutput">Show taudem output</param>
        /// <param name="callback">Callback objet</param>
        /// <returns>
        /// Taudem return number, 0 means OK
        /// </returns>
        public static int RunStreamOrderGridRaster(
            string demGridPath,
            string pitFillPath,
            string d8Path,
            string areaD8Path,
            string outletsPath,
            string streamGridResultPath,
            string streamOrdResultPath,
            string streamShapeResultPath,
            string watershedGridResultPath,
            string coordDatResultPath,
            string treeDatResultPath,
            bool useOutlets,
            int numProcesses,
            bool showTaudemOutput,
            IProgressHandler callback)
        {
            Trace.WriteLine("Stream Order Grid and Raster");
            if (callback != null)
            {
                callback.Progress("Status", 0, "Stream Order Grid and Raster");
            }

            //DataManagement.DeleteGrid(streamOrdResultPath);
            //DataManagement.DeleteGrid(streamShapeResultPath);
            //DataManagement.DeleteGrid(watershedGridResultPath);

            File.Delete(coordDatResultPath);
            File.Delete(treeDatResultPath);

            var pars = "-fel " + Quote(pitFillPath) + " -p " + Quote(d8Path) + " -ad8 " + Quote(areaD8Path) + " -src "
                   + Quote(streamGridResultPath) + " -ord " + Quote(streamOrdResultPath) + " -tree "
                   + Quote(treeDatResultPath) + " -coord " + Quote(coordDatResultPath) + " -net "
                   + Quote(streamShapeResultPath) + " -w " + Quote(watershedGridResultPath);
            if (useOutlets)
            {
                pars += " -o " + Quote(outletsPath);
            }

            var result = RunTaudem("StreamNet.exe", pars, numProcesses, showTaudemOutput);
            if (result == -1)
            {
                if (!File.Exists(streamOrdResultPath))
                {
                    MessageBox.Show(
                        "Automatic Watershed Delineation encountered an error which generally occurs from too many outlets/inlets being delineated, especially ones close together. Please lower the snap threshold and rerun to minimize outlets/inlets not direclty on the streams.",
                        "Application Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
            }
            else
            {
                if (result != 0)
                {
                    MessageBox.Show(
                        "TauDEM Error " + result,
                        "TauDEM Error " + result,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
            }

            //CopyProjectionFromGrid(demGridPath, streamGridResultPath);
            //CopyProjectionFromGrid(demGridPath, streamOrdResultPath);
            //CopyProjectionFromGrid(demGridPath, watershedGridResultPath);

            DataManagement.TryCopy(
                Path.ChangeExtension(demGridPath, ".prj"), Path.ChangeExtension(streamShapeResultPath, ".prj"));

            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            return result;
        }

        private static int RunDelinStreamGrids(
            string demGridPath,
            string pitFillPath,
            string d8Path,
            string d8SlopePath,
            string areaD8Path,
            string areaDInfPath,
            string outletsPath,
            string strahlOrdResultPath,
            string longestUpslopeResultPath,
            string totalUpslopeResultPath,
            string streamGridResultPath,
            string streamOrdResultPath,
            string treeDatResultPath,
            string coordDatResultPath,
            string streamShapeResultPath,
            string watershedGridResultPath,
            int threshold,
            bool useOutlets,
            bool useEdgeContamCheck,
            bool useDinf,
            int numProcesses,
            bool showTaudemOutput,
            IProgressHandler callback)
        {
            Trace.WriteLine("RunDelinStreamGrids(demGridPath: " + demGridPath + "\n " +
                                      "pitFillPath: " + pitFillPath + "\n" +
                                      "d8Path: " + d8Path + "\n" +
                                      "d8SlopePath: " + d8SlopePath + "\n" +
                                      "areaD8Path: " + areaD8Path + "\n" +
                                      "areaDInfPath: " + areaDInfPath + "\n" +
                                      "outletsPath: " + outletsPath + "\n" +
                                      "strahlOrdResultPath: " + strahlOrdResultPath + "\n" +
                                      "longestUpslopeResultPath: " + longestUpslopeResultPath + "\n" +
                                      "totalUpslopeResultPath: " + totalUpslopeResultPath + "\n" +
                                      "streamGridResultPath: " + streamGridResultPath + "\n" +
                                      "streamOrdResultPath: " + streamOrdResultPath + "\n" +
                                      "treeDatResultPath: " + treeDatResultPath + "\n" +
                                      "coordDatResultPath: " + coordDatResultPath + "\n" +
                                      "streamShapeResultPath: " + streamShapeResultPath + "\n" +
                                      "watershedGridResultPath: " + watershedGridResultPath + "\n" +
                                      "threshold: " + threshold.ToString() + "\n" +
                                      "useOutlets: " + useOutlets + "\n" +
                                      "useEdgeContamCheck: " + useEdgeContamCheck + "\n" +
                                      "useDinf: " + useDinf + "\n" +
                                      "NumProcesses: " + numProcesses.ToString() + "\n" +
                                      "ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
                                      "callback)");

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            var result = RunGridNetwork(
                demGridPath,
                d8Path,
                longestUpslopeResultPath,
                totalUpslopeResultPath,
                strahlOrdResultPath,
                outletsPath,
                useOutlets,
                numProcesses,
                showTaudemOutput,
                callback);
            if (result != 0)
            {
                return result;
            }

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            result = RunAllStreamDelineation(
                areaD8Path, streamGridResultPath, threshold, numProcesses, showTaudemOutput, callback);
            if (result != 0)
            {
                return result;
            }

            // Paul Meems, 23-Aug-2011, Moved to a separate method:
            result = RunStreamOrderGridRaster(
                demGridPath,
                pitFillPath,
                d8Path,
                areaD8Path,
                outletsPath,
                streamGridResultPath,
                streamOrdResultPath,
                streamShapeResultPath,
                watershedGridResultPath,
                coordDatResultPath,
                treeDatResultPath,
                useOutlets,
                numProcesses,
                showTaudemOutput,
                callback);

            return result;
        }

        //        /// <summary>
        //        /// A function to generation only the path length grids using taudem GridNet
        //        /// </summary>
        //        /// <param name="D8Path"></param>
        //        /// <param name="StrahlOrdResultPath"></param>
        //        /// <param name="LongestUpslopeResultPath"></param>
        //        /// <param name="TotalUpslopeResultPath"></param>
        //        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        //        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int PathLength(string D8Path, string StrahlOrdResultPath, string LongestUpslopeResultPath, string TotalUpslopeResultPath, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("PathLength: " + D8Path + "\n" +
        //                                     "strahlOrdResultPath: " + StrahlOrdResultPath + "\n" +
        //                                     "longestUpslopeResultPath: " + LongestUpslopeResultPath + "\n" +
        //                                     "totalUpslopeResultPath: " + TotalUpslopeResultPath + "\n" +
        //                                     "NumProcesses: " + numProcesses.ToString() + "\n" +
        //                                     "ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
        //                                     "callback)");
        //            int result = -1;
        //            string pars =
        //                "-p " + Quote(D8Path) +
        //                " -plen " + Quote(LongestUpslopeResultPath) +
        //                " -tlen " + Quote(TotalUpslopeResultPath) +
        //                " -gord " + Quote(StrahlOrdResultPath);
        //            result = RunTaudem("GridNet.exe", pars, numProcesses, showTaudemOutput);

        //            if (result != 0)
        //            {
        //                MessageBox.Show("TauDEM Error " + result, "TauDEM Error " + result, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);

        //                return result;
        //            }

        //            CopyProjectionFromGrid(D8Path, StrahlOrdResultPath);
        //            CopyProjectionFromGrid(D8Path, LongestUpslopeResultPath);
        //            CopyProjectionFromGrid(D8Path, TotalUpslopeResultPath);
        //            if (callback != null) callback.Progress("Status", 0, String.Empty);
        //            return result;
        //        }

        //        #region "     Grid Net conversion"

        //        private static int GridNet(string pfile, string plenfile, string tlenfile, string gordfile, string afile, double[] x, double[] y, long nxy, int useMask, int useOutlets, int thresh, IProgressHandler callback)
        //        {
        //            int row, col;
        //            double dx, dy, nx, ny;
        //            int err = 0;
        //            int[] d1 = new int[9];
        //            int[] d2 = new int[9];
        //            double[] dist = new double[9];

        //            /* define directions */
        //            d1[1] = 0; d1[2] = -1; d1[3] = -1; d1[4] = -1; d1[5] = 0; d1[6] = 1; d1[7] = 1; d1[8] = 1;
        //            d2[1] = 1; d2[2] = 1; d2[3] = 0; d2[4] = -1; d2[5] = -1; d2[6] = -1; d2[7] = 0; d2[8] = 1;

        //            Raster sdir = new Raster();
        //            if (sdir.Open(pfile, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 1; //TD_FAILED_GRID_OPEN
        //            }
        //            if (err != 0) return err;
        //            dx = sdir.Header.dX;
        //            dy = sdir.Header.dY;
        //            nx = sdir.NumColumns;
        //            ny = sdir.NumRows;

        //            Raster laag = new Raster();
        //            if (useMask == 1)
        //            {
        //                /*   read mask  */
        //                if (laag.Open(afile, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //                {
        //                    err = 0;
        //                }
        //                else
        //                {
        //                    err = 1; //TD_FAILED_GRID_OPEN
        //                }
        //                if (err != 0) return err;
        //            }
        //            else
        //            {
        //                RasterHeader laagHead = new RasterHeader();
        //                laagHead.CopyFrom(sdir.Header);
        //                laag.CreateNew(afile, laagHead, RasterDataType.LongDataType, 0, true, RasterFileType.UseExtension, null);
        //            }

        //            RasterHeader sordgHead = new RasterHeader();
        //            sordgHead.CopyFrom(sdir.Header);
        //            sordgHead.NodataValue = -1;
        //            Raster sordg = new Raster();
        //            sordg.CreateNew(gordfile, sordgHead, RasterDataType.ShortDataType, sordgHead.NodataValue, true, RasterFileType.UseExtension, null);

        //            RasterHeader fltpgHead = new RasterHeader();
        //            fltpgHead.CopyFrom(sdir.Header);
        //            fltpgHead.NodataValue = -1;
        //            Raster fltpg = new Raster();
        //            fltpg.CreateNew(tlenfile, fltpgHead, RasterDataType.FloatDataType, fltpgHead.NodataValue, true, RasterFileType.UseExtension, null);

        //            RasterHeader flengHead = new RasterHeader();
        //            flengHead.CopyFrom(sdir.Header);
        //            flengHead.NodataValue = -1;
        //            Raster fleng = new Raster();
        //            fleng.CreateNew(plenfile, flengHead, RasterDataType.FloatDataType, flengHead.NodataValue, true, RasterFileType.UseExtension, null);

        //            /*  Calculate Distances  */
        //            for (int i = 1; i <= 8; i++)
        //            {
        //                dist[i] = Math.Sqrt(d1[i] * d1[i] * dy * dy + d2[i] * d2[i] * dx * dx);
        //            }

        //            if (useOutlets == 1)  /*  Only compute area's for designated locations  */
        //            {
        //                for (int curXY = 0; curXY < nxy; curXY++)
        //                {
        //                    sdir.ProjToCell(x[curXY], y[curXY], out col, out row);
        //                    if (row < 0 || row > ny || col < 0 || col > nx)
        //                    {
        //                        row = 0; col = 0;
        //                    }
        //                    d2area(row, col, nx, ny, d1, d2, dist, thresh, sdir, laag, sordg, fltpg, fleng);
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i < ny; i++)
        //                    for (int j = 0; j < nx; j++)
        //                        d2area(i, j, nx, ny, d1, d2, dist, thresh, sdir, laag, sordg, fltpg, fleng);
        //            }

        //            if (fleng.Save(plenfile, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 2; //TD_FAILED_GRID_SAVE
        //                return err;
        //            }

        //            if (fltpg.Save(tlenfile, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 2; //TD_FAILED_GRID_SAVE
        //                return err;
        //            }

        //            if (sordg.Save(gordfile, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 2; //TD_FAILED_GRID_SAVE
        //                return err;
        //            }

        //            sordg.Close();
        //            laag.Close();
        //            fltpg.Close();
        //            fleng.Close();

        //            return err;
        //        }

        //        private static void d2area(int i, int j, double nx, double ny, int[] d1, int[] d2, double[] dist, int thresh, Raster sdir, Raster laag, Raster sordg, Raster fltpg, Raster fleng)
        //        {
        //            int ni, nj;
        //            short a1, a2;
        //            double ld;

        //            if ((short)sordg.get_Value(j, i) <= 0)
        //            {
        //                if (i != 0 && i != ny - 1 && j != nx - 1 && (short)sdir.get_Value(j, i) != -32767)
        //                {
        //                    sordg.set_Value(j, i, 1);
        //                    fltpg.set_Value(j, i, 0);
        //                    fleng.set_Value(j, i, 0);
        //                    a1 = 0;
        //                    a2 = 0;

        //                    for (int k = 1; k <= 8; k++)
        //                    {
        //                        ni = i + d1[k];
        //                        nj = j + d2[k];

        //                        /* test if neighbor drains towards cell excluding boundaries */
        //                        if ((short)sdir.get_Value(nj, ni) >= 0)
        //                        {
        //                            if ((int)laag.get_Value(nj, ni) >= thresh && ((short)sdir.get_Value(nj, ni) - k == 4 || (short)sdir.get_Value(nj, ni) - k == -4))
        //                            {
        //                                d2area(ni, nj, nx, ny, d1, d2, dist, thresh, sdir, laag, sordg, fltpg, fleng);
        //                                if ((short)sordg.get_Value(nj, ni) >= a1)
        //                                {
        //                                    a2 = a1;
        //                                    a1 = (short)sordg.get_Value(nj, ni);
        //                                }
        //                                else if ((short)sordg.get_Value(nj, ni) > a2)
        //                                {
        //                                    a2 = (short)sordg.get_Value(nj, ni);
        //                                }

        //                                ld = (float)fleng.get_Value(nj, ni) + dist[(short)sdir.get_Value(nj, ni)];
        //                                fltpg.set_Value(j, i, (float)fltpg.get_Value(j, i) + (float)fltpg.get_Value(nj, ni) + dist[(short)sdir.get_Value(nj, ni)]);
        //                                if (ld > (float)fleng.get_Value(j, i))
        //                                {
        //                                    fleng.set_Value(j, i, ld);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (a2 + 1 > a1)
        //                    {
        //                        sordg.set_Value(j, i, a2 + 1);
        //                    }
        //                    else
        //                    {
        //                        sordg.set_Value(j, i, a1);
        //                    }
        //                }
        //            }
        //        }

        //        #endregion //GridNet conversion

        //        #region "     Source Def conversion"

        //        private static int SourceDef(string areafile, string slopefile, string plenfile, string dirfile, string srcfile, string elvfile, string gordfile, string scafile, string fdrfile, int ipar, float[] p, int nxy, double[] x, double[] y, int contcheck, int dropan, int masksca, IProgressHandler callback)
        //        {
        //            int err = 0;

        //            //    float ndvs,ndvp,ndvd,emax,ndve,ndvo,wsum,val;

        //            //    float **selev ;

        //            //    /**********Grid Declarations*************/
        //            //    fgrid faagrid;
        //            //    fgrid fplengrid;
        //            //    sgrid sgordgrid;
        //            //    //=============================
        //            //    int row, col, i,j,iomax,jomax,bound,ik,jk,k,itresh;
        //            //    err = TD_NO_ERROR;
        //            //    int rcgood=1;
        //            //    ccheck=contcheck;

        //            //    /* define directions */
        //            //    d1[1]=0; d1[2]= -1; d1[3]= -1; d1[4]= -1; d1[5]=0; d1[6]=1; d1[7]=1; d1[8]=1;
        //            //    d2[1]=1; d2[2]=1; d2[3]=0; d2[4]= -1; d2[5]= -1; d2[6]= -1; d2[7]=0; d2[8]=1;

        //            //    /* read grid files */
        //            //    if(ipar == 1)
        //            //    {
        //            //        if(gridread(areafile,&faagrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //        {
        //            //            err=TD_FAILED_GRID_OPEN;
        //            //        }

        //            //        nx = faagrid.head.nx;
        //            //        ny = faagrid.head.ny;
        //            //        dx = faagrid.head.dx;
        //            //        dy = faagrid.head.dy;
        //            //        csize = dx;
        //            //        ndva = faagrid.nodata;
        //            //        for(i=0;i<4;i++) bndbox[i]=faagrid.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }

        //            //    if(ipar == 2)
        //            //    {
        //            //        if ( gridread(scafile,&faagrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        nx =faagrid.head.nx;
        //            //        ny = faagrid.head.ny;
        //            //        dx = faagrid.head.dx;
        //            //        dy = faagrid.head.dy;
        //            //        csize = dx;
        //            //        ndva = faagrid.nodata;
        //            //        for(i=0;i<4;i++) bndbox[i]=faagrid.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;

        //            //        if ( gridread(slopefile,&fslopeg,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        ndvs = fslopeg.nodata;

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }
        //            //    if(ipar == 3)
        //            //    {
        //            //        if ( gridread(scafile,&faagrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        nx = faagrid.head.nx;
        //            //        ny = faagrid.head.ny;
        //            //        dx = faagrid.head.dx;
        //            //        dy = faagrid.head.dy;
        //            //        csize = dx;
        //            //        ndva = faagrid.nodata;
        //            //        for(i=0;i<4;i++) bndbox[i]=faagrid.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;

        //            //        if ( gridread(plenfile,&fplengrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        ndvp = fplengrid.nodata;

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }
        //            //    if(ipar == 4)
        //            //    {
        //            //        if( gridread(elvfile,&felevg,&filetype)==0)

        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        nx = felevg.head.nx;
        //            //        ny = felevg.head.ny;
        //            //        dx = felevg.head.dx;
        //            //        dy = felevg.head.dy;
        //            //        csize = dx;
        //            //        ndve = felevg.nodata;
        //            //        for(i=0;i<4;i++) bndbox[i]=felevg.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }
        //            //    if(ipar == 5)
        //            //    {
        //            //        if ( gridread(gordfile,&sgordgrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        nx = sgordgrid.head.nx;
        //            //        ny = sgordgrid.head.ny;
        //            //        dx = sgordgrid.head.dx;
        //            //        dy = sgordgrid.head.dy;
        //            //        csize = dx;
        //            //        ndvo = sgordgrid.nodata;
        //            //        for(i=0;i<4;i++)bndbox[i]=sgordgrid.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }
        //            //    if(ipar == 6)
        //            //    {
        //            //        if (gridread(fdrfile,&sgordgrid,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        nx = sgordgrid.head.nx;
        //            //        ny = sgordgrid.head.ny;
        //            //        dx = sgordgrid.head.dx;
        //            //        dy = sgordgrid.head.dy;
        //            //        csize = dx;
        //            //        ndvo = sgordgrid.nodata;
        //            //        for(i=0;i<4;i++)bndbox[i]=sgordgrid.head.bndbox[i];

        //            //        if(err != TD_NO_ERROR)goto ERROR1;
        //            //    }

        //            //    if((src = (short **)matalloc(nx,ny, RPSHRDTYPE)) == NULL)
        //            //    {
        //            //        err=TD_FAILED_MEMORY_ALLOC;
        //            //        //  printf("Could not allocate memory for src\n");
        //            //        goto ERROR1;
        //            //    }

        //            //    /*  Flag sources  */
        //            //    for(i=0; i < ny; i++)
        //            //        for(j=0; j< nx; j++)
        //            //        {
        //            //            src[j][i] = 0;
        //            //            if(ipar == 1)   /*  Area threshold   */
        //            //            {
        //            //                src[j][i] = (faagrid.d[j][i] >= p[0]) ? 1 : 0;
        //            //            }
        //            //            else if(ipar == 2)   /*  Slope and area combination   */
        //            //            {
        //            //                if( fslopeg.d[j][i] > 0.)
        //            //                {
        //            //                    val = (faagrid.d[j][i] * pow((double)fslopeg.d[j][i],(double)p[1])) ;
        //            //                    src[j][i] = (val >= p[0])	  ? 1: 0;
        //            //                }
        //            //            }else if(ipar == 3)  /*  Slope and Length combination   */
        //            //            {
        //            //                if(fplengrid.d[j][i] > 0.)
        //            //                {
        //            //                    src[j][i] = (faagrid.d[j][i] >= p[0]* pow((double)fplengrid.d[j][i],(double)p[1]))
        //            //                        ? 1: 0;
        //            //                }
        //            //            }
        //            //            else if(ipar == 5)  /*  Grid order threshold  */
        //            //                src[j][i] = (sgordgrid.d[j][i] >= p[0]) ? 1: 0;
        //            //            else if(ipar == 6)  /*  Given flow directions threshold  */
        //            //                src[j][i] = (sgordgrid.d[j][i] > 0) ? 1: 0;
        //            //        }
        //            //        if(ipar == 4)  /* Peuker and Douglas algorithm  */
        //            //        {
        //            //            /*  Initialize internal cells to 1 for Peuker and Douglas algorithm and smooth  */
        //            //            if((selev = (float **)matalloc(nx,ny, RPFLTDTYPE)) == NULL)
        //            //            {
        //            //                err=TD_FAILED_MEMORY_ALLOC;
        //            //                //  printf("Could not allocate memory for selev\n");
        //            //                goto ERROR1;
        //            //            }
        //            //            for(i=0; i <ny; i++)
        //            //                for(j=0; j<nx; j++)
        //            //                {
        //            //                    if (ndve > 0) //ARA 10/17/05 Fixed for possible positive nodata
        //            //              {
        //            //                  if(i == 0 || i == (ny-1) || j == 0 || j == (nx-1) || felevg.d[j][i] >= ndve)
        //            //                  {
        //            //                      selev[j][i]=felevg.d[j][i];
        //            //                  }
        //            //                  else
        //            //                  {
        //            //                      src[j][i] = 1;
        //            //                      selev[j][i]=p[1] * felevg.d[j][i];
        //            //                      wsum=p[1];
        //            //                      if(p[2] > 0.)
        //            //                          for(k=1; k<=7; k=k+2)
        //            //                          {
        //            //                              if(felevg.d[j+d2[k]][i+d1[k]] < ndve)
        //            //                              {
        //            //                                  selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[2];
        //            //                                  wsum += p[2];
        //            //                              }
        //            //                          }
        //            //                          if(p[3] > 0.)
        //            //                              for(k=2; k<=8; k=k+2)
        //            //                              {
        //            //                                  if(felevg.d[j+d2[k]][i+d1[k]] < ndve)
        //            //                                  {
        //            //                                      selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[3];
        //            //                                      wsum += p[3];
        //            //                                  }
        //            //                              }
        //            //                  }
        //            //              }
        //            //                    else
        //            //              {
        //            //                  if(i == 0 || i == (ny-1) || j == 0 || j == (nx-1) || felevg.d[j][i] <= ndve)
        //            //                  {
        //            //                      selev[j][i]=felevg.d[j][i];
        //            //                  }
        //            //                  else
        //            //                  {
        //            //                      src[j][i] = 1;
        //            //                      selev[j][i]=p[1] * felevg.d[j][i];
        //            //                      wsum=p[1];
        //            //                      if(p[2] > 0.)
        //            //                          for(k=1; k<=7; k=k+2)
        //            //                          {
        //            //                              if(felevg.d[j+d2[k]][i+d1[k]] > ndve)
        //            //                              {
        //            //                                  selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[2];
        //            //                                  wsum += p[2];
        //            //                              }
        //            //                          }
        //            //                          if(p[3] > 0.)
        //            //                              for(k=2; k<=8; k=k+2)
        //            //                              {
        //            //                                  if(felevg.d[j+d2[k]][i+d1[k]] > ndve)
        //            //                                  {
        //            //                                      selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[3];
        //            //                                      wsum += p[3];
        //            //                                  }
        //            //                              }
        //            //                  }
        //            //              }

        //            //                    if(i == 0 || i == (ny-1) || j == 0 || j == (nx-1) || felevg.d[j][i] <= ndve)
        //            //              {
        //            //                  selev[j][i]=felevg.d[j][i];
        //            //              }
        //            //                    else
        //            //              {
        //            //                  src[j][i] = 1;
        //            //                  selev[j][i]=p[1] * felevg.d[j][i];
        //            //                  wsum=p[1];
        //            //                  if(p[2] > 0.)
        //            //                      for(k=1; k<=7; k=k+2)
        //            //                      {
        //            //                          if(felevg.d[j+d2[k]][i+d1[k]] > ndve)
        //            //                          {
        //            //                              selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[2];
        //            //                              wsum += p[2];
        //            //                          }
        //            //                      }
        //            //                      if(p[3] > 0.)
        //            //                          for(k=2; k<=8; k=k+2)
        //            //                          {
        //            //                              if(felevg.d[j+d2[k]][i+d1[k]] > ndve)
        //            //                              {
        //            //                                  selev[j][i] += felevg.d[j+d2[k]][i+d1[k]] *p[3];
        //            //                                  wsum += p[3];
        //            //                              }
        //            //                          }
        //            //              }
        //            //                }

        //            //                for (int curcol = 0; curcol < nx; curcol++)
        //            //                    for (int currow = 0; currow < ny; currow++)

        //            //                        felevg.d[curcol][currow]=selev[curcol][currow];

        //            //                for(i=0; i <ny-1; i++)
        //            //                    for(j=0; j<nx-1; j++)
        //            //                    {
        //            //                        emax= felevg.d[j][i];
        //            //                        iomax=0;
        //            //                        jomax=0;
        //            //                        bound= 0;  /*  .false.  */
        //            //                        /*  --FIRST PASS FLAG MAX ELEVATION IN GROUP OF FOUR  */
        //            //                        for(ik=0; ik<2; ik++)
        //            //                            for(jk=1-ik; jk < 2; jk++)
        //            //                            {
        //            //                                if(felevg.d[j+jk][i+ik] > emax)
        //            //                                {
        //            //                                    emax=felevg.d[j+jk][i+ik];
        //            //                                    iomax=ik;
        //            //                                    jomax=jk;
        //            //                                }
        //            //                                if( felevg.d[j+jk][i+ik] <= ndve)
        //            //                                    bound= 1;  /*  .true.  */
        //            //                            }
        //            //                            /*  c---Unflag max pixel */
        //            //                            src[j+jomax][i+iomax] = 0;
        //            //                            /*  c---Unflag pixels where the group of 4 touches a boundary  */
        //            //                            if(bound == 1)
        //            //                            {
        //            //                                for(ik=0; ik < 2; ik++)
        //            //                                    for(jk=0; jk< 2; jk++)
        //            //                                    {
        //            //                                        src[j+jk][i+ik]=0;
        //            //                                    }
        //            //                            }
        //            //                            /* 		  i.e. unflag flats.  */
        //            //                            for(ik=0; ik < 2; ik++)
        //            //                                for(jk=0; jk< 2; jk++)
        //            //                                {
        //            //                                    if(felevg.d[j+jk][i+ik] == emax)src[j+jk][i+ik] = 0;
        //            //                                }
        //            //                    }
        //            //        }

        //            //        if(ipar == 2){
        //            //            free(fslopeg.d[0]); free(fslopeg.d);
        //            //            free(faagrid.d[0]); free(faagrid.d);
        //            //        }
        //            //        if(ipar == 3){
        //            //            free(fplengrid.d[0]); free(fplengrid.d);
        //            //            free(faagrid.d[0]); free(faagrid.d);
        //            //        }
        //            //        if(ipar == 4){
        //            //            free(felevg.d[0]); free(felevg.d);
        //            //        }
        //            //        if(ipar == 5 || ipar == 6){
        //            //            free(sgordgrid.d[0]); free(sgordgrid.d);
        //            //        }

        //            //        /*  Now get directions and compute area's  */

        //            //        if ( gridread(dirfile,&sdir,&filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else
        //            //            err=TD_FAILED_GRID_OPEN;

        //            //        ndvd = sdir.nodata;

        //            //        if(err != TD_NO_ERROR)goto ERROR1;

        //            //        //allocate memory and headers for larr
        //            //        larr.head.dx=dx;
        //            //        larr.head.dy=dy;
        //            //        larr.head.nx=nx;
        //            //        larr.head.ny=ny;
        //            //        larr.nodata=-2;
        //            //        for(i=0;i<4;i++) larr.head.bndbox[i]=bndbox[i];

        //            //        larr.nodata = -2;
        //            //        allocategrid(&larr,larr.head,larr.nodata);

        //            //        nout=0;
        //            //        itresh=1;
        //            //        if(ipar == 4)itresh = p[0];
        //            //        err=TD_CHANNEL_NETWORK_MISMATCH;   //This flag will indicate no outlet found  12/15/02  DGT moved to outside the if block
        //            //        // so that code works for at least one outlet found
        //            //        if(nxy >0)
        //            //        {
        //            //            for(i=0; i<nxy; i++)
        //            //            {
        //            //                col= (int)floor((x[i]-bndbox[0])/csize);
        //            //                row= (int)floor((bndbox[3]-y[i])/csize);
        //            //                if(row >0 && row < ny-1 && col > 0 && col < nx-1
        //            //                    && sdir.d[col][row]>0)  // DGT* this condition added 12/15/02 to not do outlets outside the domain
        //            //                {
        //            //                    /* call drainage area subroutine for pixel to zero on  */
        //            //                    srcarea(row,col);

        //            //                    if(larr.d[col][row] >= itresh)err=TD_NO_ERROR;  // an outlet found so no error
        //            //                }
        //            //            }
        //            //            if(err==TD_CHANNEL_NETWORK_MISMATCH)goto ERROR9;  //  no outlet error
        //            //        }
        //            //        else
        //            //        {
        //            //            //  Do all pixels
        //            //            for(i=1; i < ny-1; i++)
        //            //                for(j=1; j<nx-1; j++)srcarea(i,j);
        //            //            rcgood=0;  // no outlet coordinates found
        //            //        }

        //            //        //  Now threshold the src file
        //            //        if(dropan == 0)
        //            //        {
        //            //            for(i=1; i < ny-1; i++)
        //            //                for(j=1; j<nx-1; j++)
        //            //                {
        //            //                    if(larr.d[j][i] >= itresh && sdir.d[j][i]>0) larr.d[j][i]=1;
        //            //                    //  8/13/04  DGT added condition on sdir.d
        //            //                    else larr.d[j][i]=0;
        //            //                }
        //            //        }
        //            //        if(dropan == 1 && ipar == 1)  // overwrite accumulated source area with actual area
        //            //        {
        //            //            for(i=1; i < ny-1; i++)
        //            //                for(j=1; j<nx-1; j++)
        //            //                {
        //            //                    if(larr.d[j][i] >= itresh && sdir.d[j][i]>0)larr.d[j][i]=faagrid.d[j][i];
        //            //                    //  8/13/04  DGT added condition on sdir.d
        //            //                    else larr.d[j][i]=0;
        //            //                }
        //            //        }
        //            //        //free memory for sdir
        //            //        free(sdir.d[0]);free(sdir.d);

        //            //        if(ipar <= 3){free(faagrid.d[0]);free(faagrid.d);}  // Moved from below so that could reopen with sca file for sure
        //            //        // Exclude area with specific catchment area no data
        //            //        if(masksca == 1)
        //            //        {
        //            //            if(gridread(scafile,&faagrid,&filetype)==0)
        //            //                err=TD_NO_ERROR;
        //            //            else
        //            //            {
        //            //                err=TD_FAILED_GRID_OPEN;
        //            //                return err;
        //            //                //	AfxMessageBox( LPCTSTR(strcat( "Failed to open sca file for masking: ", scafile) ));
        //            //            }
        //            //            if(err != TD_NO_ERROR)goto ERROR9;
        //            //            for(i=1; i < ny-1; i++)
        //            //                for(j=1; j<nx-1; j++)
        //            //                {
        //            //                    if(faagrid.d[j][i] < 0)larr.d[j][i]=0;
        //            //                }
        //            //        }

        //            //        if ( gridwrite(srcfile,larr,filetype)==0)
        //            //            err=TD_NO_ERROR;
        //            //        else{
        //            //            err=TD_FAILED_GRID_SAVE;
        //            //            //if (srcfile)
        //            //            //	AfxMessageBox( LPCTSTR(strcat( "Failed to save file: ", srcfile) ));
        //            //        }

        //            //        free(src[0]); free(src);
        //            //        free(larr.d[0]); free(larr.d);
        //            //        return(err);
        //            //ERROR9:
        //            //        free(src[0]); free(src);
        //            //        //Kiran added the following statement to clean up.
        //            //        free(larr.d[0]); free(larr.d);
        //            //        if(faagrid.d != NULL) free(faagrid.d[0]); free(faagrid.d);
        //            //        if(sdir.d[0] != NULL) free(sdir.d[0]); free(sdir.d);
        //            //        return(err);

        //            //ERROR1:
        //            //        free(src[0]); free(src);
        //            //        free(larr.d[0]); free(larr.d);
        //            //        return(err);

        //            return err;
        //        }

        //        #endregion 'Sourcedef conversion

        //        #region "     Netsetup conversion"
        //        private static int NetSetup(string fnprefix, string pfile, string srcfile, string ordfile, string ad8file, string elevfile, string treefile, string coordfile, double[] xnode, double[] ynode, int nxy, long usetrace, int[] idnodes, IProgressHandler callback)
        //        {
        //            int err = 0;
        //            int itresh, icr, icend;

        //            /* define directions */
        //            int[] d1 = new int[9];
        //            int[] d2 = new int[9];
        //            d1[1] = 0; d1[2] = -1; d1[3] = -1; d1[4] = -1; d1[5] = 0; d1[6] = 1; d1[7] = 1; d1[8] = 1;
        //            d2[1] = 1; d2[2] = 1; d2[3] = 0; d2[4] = -1; d2[5] = -1; d2[6] = -1; d2[7] = 0; d2[8] = 1;

        //            itresh = 1;  // Thresholding to 1 done in source

        //            /*read dirfile   */
        //            Raster dirg = new Raster();
        //            if (dirg.Open(pfile, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 1; //TD_FAILED_GRID_OPEN
        //                return err;
        //            }

        //            double dx = dirg.Header.dX;
        //            double dy = dirg.Header.dY;
        //            int ny = dirg.NumRows;
        //            int nx = dirg.NumColumns;
        //            double[] bndbox = new double[4];
        //            bndbox[0] = dirg.Header.XllCenter - dx / 2;
        //            bndbox[1] = dirg.Header.YllCenter - dy / 2;
        //            bndbox[2] = dirg.Header.XllCenter + dx * nx - dx / 2;
        //            bndbox[3] = dirg.Header.YllCenter + dy * ny - dy / 2;

        //            float[] tmpRow;
        //            short[,] dir = new short[ny, nx];
        //            for (int i = 0; i < ny; i++)
        //            {
        //                tmpRow = new float[nx];
        //                dirg.GetRow(i, tmpRow[0]);
        //                for (int j = 0; j < nx; j++)
        //                {
        //                    dir[i, j] = Convert.ToInt16(tmpRow[j]);
        //                }
        //            }

        //            /*read srcfile   */
        //            Raster area = new Raster();
        //            if (area.Open(srcfile, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 1; //TD_FAILED_GRID_OPEN
        //                dirg.Close();
        //                return err;
        //            }
        //            if (area.NumColumns != nx || area.NumRows != ny)
        //            {
        //                err = 27; //TD_GRID_SIZE_MISMATCH
        //                dirg.Close();
        //                area.Close();
        //                return 1;
        //            }

        //            int[,] aread = new int[ny, nx];
        //            for (int i = 0; i < ny; i++)
        //            {
        //                tmpRow = new float[nx];
        //                area.GetRow(i, tmpRow[0]);
        //                for (int j = 0; j < nx; j++)
        //                {
        //                    aread[i, j] = Convert.ToInt32(tmpRow[j]);
        //                }
        //            }

        //            /*  check for source values >= threshold else fortran crashes*/
        //            int n = 0;
        //            short dirgNoData = Convert.ToInt16(dirg.Header.NodataValue);
        //            for (int i = 0; i < ny; i++)
        //            {
        //                for (int j = 0; j < nx; j++)
        //                {
        //                    if (dir[i, j] == dirgNoData)
        //                    {
        //                        dir[i, j] = 0;  // set direction no data to 0. netex needs this
        //                    }
        //                    if (aread[i, j] >= itresh)
        //                    {
        //                        n = n + 1;
        //                    }
        //                }
        //            }

        //            if (n <= 0)
        //            {
        //                err = 8; //TD_CHANNEL_NETWORK_MISMATCH
        //            }
        //            else
        //            {
        //                int[] inodes;
        //                int[] jnodes;

        //                //  If there are outlets set up inodes, jnodes arrays
        //                if (nxy > 0)
        //                {
        //                    inodes = new int[nxy];
        //                    jnodes = new int[nxy];
        //                }
        //                else
        //                {
        //                    inodes = new int[1];
        //                    jnodes = new int[1];
        //                }

        //                for (int inode = 0; inode < nxy; inode++)
        //                {
        //                    jnodes[inode] = (int)Math.Floor((xnode[inode] - bndbox[0]) / dx);
        //                    inodes[inode] = (int)Math.Floor((bndbox[3] - ynode[inode]) / dy);
        //                    //   Trace to raster if necessary but only for nodes that are inside the domain
        //                    if (usetrace == 1 && inodes[inode] > 0 && inodes[inode] < ny - 1 && jnodes[inode] > 0 && jnodes[inode] < nx - 1)
        //                    {
        //                        if ((int)aread[jnodes[inode], inodes[inode]] < itresh)  // Not on grid
        //                        {
        //                            //   Next downslope
        //                            short dirn = dir[jnodes[inode], inodes[inode]];
        //                            int nexti = inodes[inode] + d1[dirn];
        //                            int nextj = jnodes[inode] + d2[dirn];
        //                            int loopcount = 0;
        //                            while (nexti > 0 && nexti < ny - 1 && nextj > 0 && nextj < nx - 1 && (int)area.get_Value(jnodes[inode], inodes[inode]) < itresh)
        //                            {
        //                                inodes[inode] = nexti;
        //                                jnodes[inode] = nextj;
        //                                dirn = dir[jnodes[inode], inodes[inode]];
        //                                if (dirn < 1 || dirn > 8) break;   // Here have gone out of grid so terminate trace downwards
        //                                nexti = nexti + d1[dirn];
        //                                nextj = nextj + d2[dirn];
        //                                loopcount = loopcount + 1;
        //                                if (loopcount > nx && loopcount > ny) break;   // Here possible infinite loop so terminate trace downwards
        //                            }
        //                        }
        //                    }
        //                }

        //                Netex(dir, aread, treefile, coordfile, ordfile, nx, ny, itresh, out icr, out icend, dx, dy, 0, dx, 0, err, inodes, jnodes, nxy, idnodes);

        //                //Write any changes that were made to the area
        //                for (int i = 0; i < ny; i++)
        //                {
        //                    tmpRow = new float[nx];
        //                    for (int j = 0; j < nx; j++)
        //                    {
        //                        tmpRow[j] = aread[i, j];
        //                    }
        //                    area.PutRow(i, tmpRow[0]);
        //                }

        //                area.Header.NodataValue = -1;
        //                if (area.Save(ordfile, RasterFileType.UseExtension, null))
        //                {
        //                    err = 0;
        //                }
        //                else
        //                {
        //                    err = 2; //TD_FAILED_GRID_SAVE;
        //                    dirg.Close();
        //                    area.Close();
        //                    return err;
        //                }
        //                area.Close();

        //                //Fix negative markers
        //                for (int i = 0; i < ny; i++)
        //                {
        //                    for (int j = 0; j < nx; j++)
        //                    {
        //                        if (dir[i, j] < 0 && dir[i, j] > -9)
        //                        {
        //                            dir[i, j] = (short)-dir[i, j];
        //                        }
        //                    }
        //                }

        //                area = new Raster();
        //                if (area.Open(ad8file, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //                {
        //                    err = 0;
        //                }
        //                else
        //                {
        //                    err = 1; //TD_FAILED_GRID_OPEN
        //                    dirg.Close();
        //                    return err;
        //                }
        //                aread = new int[ny, nx];
        //                for (int i = 0; i < ny; i++)
        //                {
        //                    tmpRow = new float[nx];
        //                    area.GetRow(i, tmpRow[0]);
        //                    for (int j = 0; j < nx; j++)
        //                    {
        //                        aread[i, j] = Convert.ToInt32(tmpRow[j]);
        //                    }
        //                }

        //                /*****read elevfile   *****/
        //                Raster elevg = new Raster();
        //                if (elevg.Open(elevfile, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null))
        //                {
        //                    err = 0;
        //                }
        //                else
        //                {
        //                    err = 1; //TD_FAILED_GRID_OPEN
        //                    dirg.Close();
        //                    area.Close();
        //                    return err;
        //                }
        //                if (elevg.NumColumns != nx || elevg.NumRows != ny)
        //                {
        //                    dirg.Close();
        //                    area.Close();
        //                    elevg.Close();
        //                    return 1;
        //                }

        //                float[,] elevd = new float[ny, nx];
        //                for (int i = 0; i < ny; i++)
        //                {
        //                    tmpRow = new float[nx];
        //                    elevg.GetRow(i, tmpRow[0]);
        //                    for (int j = 0; j < nx; j++)
        //                    {
        //                        elevd[i, j] = tmpRow[j];
        //                    }
        //                }

        //                NetProp(dir, aread, elevd, coordfile, icr, icend, dx, dy, nx, ny, bndbox, err);

        //                elevg.Close();
        //            }  //  end if associated with err from source
        //            dirg.Close();
        //            area.Close();

        //            return err;
        //        }

        //        private static int Netex(short[,] dir, int[,] area, string treefile, string coordfile, string ordfile, int nx, int ny, int itresh, out int icr, out int icend, double dx, double dy, double bndbox, double csize, int iftype, int err, int[] inodes, int[] jnodes, int nnodes, int[] idnodes)
        //        {
        //            int i, j;
        //            int inodeid;
        //            int nodeno;
        //            int ics;
        //            int[] iordup = new int[8];
        //            int[] ipoint = new int[8];
        //            int[] d1 = new int[9];
        //            int[] d2 = new int[9];
        //            d1[1] = 0; d1[2] = -1; d1[3] = -1; d1[4] = -1; d1[5] = 0; d1[6] = 1; d1[7] = 1; d1[8] = 1;
        //            d2[1] = 1; d2[2] = 1; d2[3] = 0; d2[4] = -1; d2[5] = -1; d2[6] = -1; d2[7] = 0; d2[8] = 1;
        //            icr = 0;
        //            icend = 0;
        //            err = 0;

        //            int[] ist, jst, iord, istart, jstart, iend, jend, mag;

        //            //READ INPUT
        //            int igy = ny;
        //            int igx = nx;

        //            //
        //            //     MEANING OF POINTERS IS -------------
        //            //                            I 4 I 3 I 2 I
        //            //      0 = POINTS TO SELF    -------------
        //            //          I.E. UNRESOLVED   I 5 I 0 I 1 I
        //            //     -1 = BOUNDARY PIXEL    -------------
        //            //                            I 6 I 7 I 8 I
        //            //                            -------------
        //            //

        //            //-----FIRST FIND ALL START PIXELS
        //            int n = 0;
        //            for (i = 1; i < ny - 1; i++)
        //            {
        //                for (j = 1; j < nx - 1; j++)
        //                {
        //                    if (strt(i, j, area, dir, nx, ny, igx, igy, itresh))
        //                    {
        //                        n = n + 1;
        //                    }
        //                }
        //            }

        //            int nmax = n;
        //            int mnl = 2 * n + 1 + nnodes;   // 11/17/04  DGT added nnodes to avoid memory overflow

        //            //     when added nodes increase the number of links
        //            int[] nextl = new int[mnl + 1];
        //            int[] prevl1 = new int[mnl + 1];
        //            int[] prevl2 = new int[mnl + 1];
        //            ist = new int[nmax + 1];
        //            jst = new int[nmax + 1];
        //            iord = new int[mnl + 1];
        //            istart = new int[mnl + 1];
        //            jstart = new int[mnl + 1];
        //            iend = new int[mnl + 1];
        //            jend = new int[mnl + 1];
        //            mag = new int[mnl + 1];

        //            n = 0;
        //            for (i = 1; i < ny - 1; i++)
        //            {
        //                for (j = 1; j < nx - 1; j++)
        //                {
        //                    if (strt(i, j, area, dir, nx, ny, igx, igy, itresh))
        //                    {
        //                        n = n + 1;
        //                        if (n <= nmax)
        //                        {
        //                            ist[n] = i;
        //                            jst[n] = j;
        //                        }
        //                    }
        //                }
        //            }

        //            if (n > nmax)
        //            {
        //                err = 2;//stop too big
        //                return err;
        //            }

        //            //---ZERO AREA ARRAY
        //            for (i = 1; i < ny - 1; i++)
        //            {
        //                for (j = 1; j < nx - 1; j++)
        //                {
        //                    area[i, j] = 0;
        //                }
        //            }

        //            //----TRACE STREAMS DOWNWARDS ADDING 1 TO MAGNITUDE OF EACH PIXEL (MAGNITUDE STORED IN AREA ARRAY)
        //            int inext;
        //            int jnext;
        //            for (int si = 1; si <= n; si++)
        //            {
        //                i = ist[si];
        //                j = jst[si];
        //                while (dir[i, j] > 0)
        //                {
        //                    area[i, j] = area[i, j] + 1;
        //                    inext = i + d1[dir[i, j]];
        //                    jnext = j + d2[dir[i, j]];
        //                    i = inext;
        //                    j = jnext;
        //                }
        //            }

        //            //----IDENTIFY LINKS BY DIFFERENT MAGNITUDES
        //            bool runEndPath;
        //            int ilink = 1;
        //            int mnext, intemp, jntemp, msum, iconv, itemp;
        //            for (int si = 1; si <= n; si++)
        //            {
        //                istart[ilink] = ist[si];
        //                jstart[ilink] = jst[si];

        //                //---INITIALISE POINTERS
        //                prevl1[ilink] = 0;
        //                prevl2[ilink] = 0;
        //                i = ist[si];
        //                j = jst[si];
        //                mag[ilink] = area[i, j];
        //                iord[ilink] = 1;

        //                inext = i + d1[Math.Abs(dir[i, j])];
        //                jnext = j + d2[Math.Abs(dir[i, j])];
        //                runEndPath = true;
        //                while (dir[inext, jnext] != 0)
        //                {
        //                    mnext = area[inext, jnext];
        //                    i = inext;
        //                    j = jnext;
        //                    iend[ilink] = i;
        //                    jend[ilink] = j;

        //                    //mods allow insertion of nodes   DGT 7/17/02
        //                    if (isnode(mnext, mag[ilink], i, j, inodes, jnodes, nnodes))
        //                    {
        //                        //heck here that this is not the end of a path because then it will be a node anyway
        //                        intemp = i + d1[Math.Abs(dir[i, j])];
        //                        jntemp = j + d2[Math.Abs(dir[i, j])];
        //                        if (dir[intemp, jntemp] != 0)
        //                        {
        //                            ilink = ilink + 1;
        //                            istart[ilink] = i;
        //                            jstart[ilink] = j;
        //                            prevl1[ilink] = ilink - 1;
        //                            prevl2[ilink] = 0;
        //                            nextl[ilink - 1] = ilink;
        //                            mag[ilink] = mag[ilink - 1];
        //                            iord[ilink] = iord[ilink - 1];
        //                            iend[ilink] = i;
        //                            jend[ilink] = j;
        //                        }
        //                    }
        //                    //end mods to allow insertion of nodes  DGT
        //                    if (mnext != mag[ilink])
        //                    {
        //                        //----CONTINUE HERE FOR NEW LINK
        //                        //----CHECK IF JUNCTION ALREADY REACHED (FLAGGED BY NEGATIVE DIRECTION)
        //                        if (dir[i, j] < 0)
        //                        {
        //                            //----CHECK IF ALL LINKS CONVERGING HERE HAVE BEEN DONE BY SUMMING MAGNITUDE
        //                            msum = 0;
        //                            iconv = 0;
        //                            for (int il = 1; il <= ilink; il++)
        //                            {
        //                                if (iend[il] == i && jend[il] == j)
        //                                {
        //                                    iconv = iconv + 1;
        //                                    ipoint[iconv] = il;
        //                                    iordup[iconv] = iord[il];
        //                                    msum = msum + mag[il];
        //                                }
        //                            }

        //                            if (msum == mnext) //All links have been processed
        //                            {
        //                                //---SORT IORDUP,IPOINT INTO DECENDING STREAM ORDER
        //                                for (int ic = 1; ic <= iconv - 1; ic++)
        //                                {
        //                                    for (int iic = ic + 1; iic <= iconv; iic++)
        //                                    {
        //                                        if (iordup[iic] > iordup[ic]) //switch these
        //                                        {
        //                                            itemp = iordup[iic];
        //                                            iordup[iic] = iordup[ic];
        //                                            iordup[ic] = itemp;
        //                                            itemp = ipoint[iic];
        //                                            ipoint[iic] = ipoint[ic];
        //                                            ipoint[ic] = itemp;
        //                                        }
        //                                    }
        //                                }
        //                                for (int ic = 1; ic <= iconv - 1; ic++)
        //                                {
        //                                    ilink = ilink + 1;
        //                                    istart[ilink] = i;
        //                                    jstart[ilink] = j;
        //                                    prevl1[ilink] = ipoint[ic];
        //                                    prevl2[ilink] = ipoint[ic + 1];
        //                                    nextl[ipoint[ic]] = ilink;
        //                                    nextl[ipoint[ic + 1]] = ilink;
        //                                    mag[ilink] = mag[prevl1[ilink]] + mag[prevl2[ilink]];
        //                                    iord[ilink] = Math.Max(iordup[1], iordup[2] + 1);
        //                                    ipoint[ic + 1] = ilink;
        //                                    iend[ilink] = i;
        //                                    jend[ilink] = j;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ilink = ilink + 1;
        //                                runEndPath = false;
        //                                break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            dir[i, j] = (short)(-dir[i, j]);
        //                            ilink = ilink + 1;
        //                            runEndPath = false;
        //                            break;
        //                        }
        //                    } //end if mnext != mag(ilink)

        //                    inext = i + d1[Math.Abs(dir[i, j])];
        //                    jnext = j + d2[Math.Abs(dir[i, j])];
        //                } // end while dir != 0

        //                if (runEndPath)
        //                {
        //                    iend[ilink] = i;
        //                    jend[ilink] = j;
        //                    nextl[ilink] = -1;
        //                    if (si < n)
        //                    {
        //                        ilink = ilink + 1;
        //                    }
        //                }
        //            } //end for (int si=1; si <= n; si++)

        //            StreamWriter coord = new System.IO.StreamWriter(coordfile);
        //            StreamWriter tree = new System.IO.StreamWriter(treefile);

        //            //     reinitialize area array - for output it will contain order
        //            for (i = 0; i < ny; i++)
        //            {
        //                for (j = 0; j < nx; j++)
        //                {
        //                    area[i, j] = 0;
        //                }
        //            }

        //            int icord = 0;

        //            //---  WRITE ROOT LINK FIRST
        //            i = istart[ilink];
        //            j = jstart[ilink];
        //            ics = icord;

        //            if (i != 0 && j != 0)
        //            {
        //                coord.Write("{0,10:G} {1,10:G}\n", i + 1, j + 1);
        //                area[i, j] = Math.Max(iord[ilink], area[i, j]);
        //            }
        //            icend = icord;
        //            icord = icord + 1;
        //            while (i != iend[ilink] || j != jend[ilink] && i != 0 && j != 0)
        //            {
        //                inext = i + d1[Math.Abs(dir[i, j])];
        //                jnext = j + d2[Math.Abs(dir[i, j])];
        //                i = inext;
        //                j = jnext;

        //                if (i != 0 && j != 0)
        //                {
        //                    coord.Write("{0,10:G} {1,10:G}\n", i + 1, j + 1);
        //                    area[i, j] = Math.Max(iord[ilink], area[i, j]);
        //                    icend = icord;
        //                    icord = icord + 1;
        //                }
        //            }

        //            inodeid = 0; //This is the first one so it will be the most downstream
        //            if (isnode2(iend[ilink], jend[ilink], inodes, jnodes, nnodes, out nodeno))
        //            {
        //                if (idnodes[nodeno] >= 0)
        //                {
        //                    inodeid = idnodes[nodeno];
        //                    idnodes[nodeno] = -1; //This logic to pick only the first one if there are multiple at a junction
        //                }
        //            }
        //            tree.Write("{0,10:G} {1,10:G} {2,10:G} {3,10:G} {4,10:G} {5,10:G} {6,10:G} {7,10:G}\n", 0, ics, icend, -1, prevl1[ilink], prevl2[ilink], iord[ilink], inodeid);

        //            icr = icord;

        //            //---  WRITE REMAINDER OF LINKS
        //            for (int il = 1; il <= ilink - 1; il++)
        //            {
        //                i = istart[il];
        //                j = jstart[il];
        //                ics = icord;

        //                if (i != 0 && j != 0)
        //                {
        //                    coord.Write("{0,10:G} {1,10:G}\n", i + 1, j + 1);
        //                    area[i, j] = Math.Max(iord[il], area[i, j]);
        //                }
        //                icend = icord;
        //                icord = icord + 1;
        //                while (i != iend[il] || j != jend[il] && i != 0 && j != 0)
        //                {
        //                    inext = i + d1[Math.Abs(dir[i, j])];
        //                    jnext = j + d2[Math.Abs(dir[i, j])];
        //                    i = inext;
        //                    j = jnext;

        //                    if (i != 0 && j != 0)
        //                    {
        //                        coord.Write("{0,10:G} {1,10:G}\n", i + 1, j + 1);
        //                        area[i, j] = Math.Max(iord[il], area[i, j]);
        //                        icend = icord;
        //                        icord = icord + 1;
        //                    }
        //                }

        //                if (nextl[il] == ilink)
        //                {
        //                    nextl[il] = 0;
        //                }
        //                inodeid = -1;

        //                if (nextl[il] < 0)
        //                {
        //                    inodeid = 0;
        //                }

        //                if (isnode2(iend[il], jend[il], inodes, jnodes, nnodes, out nodeno))
        //                {
        //                    if (idnodes[nodeno] >= 0)
        //                    {
        //                        inodeid = idnodes[nodeno];
        //                        idnodes[nodeno] = -1; //This logic to pick only the first one if there are multiple at a junction
        //                    }
        //                }

        //                tree.Write("{0,10:G} {1,10:G} {2,10:G} {3,10:G} {4,10:G} {5,10:G} {6,10:G} {7,10:G}\n", il, ics, icend, nextl[il], prevl1[il], prevl2[il], iord[il], inodeid);
        //            }

        //            coord.Close();
        //            tree.Close();

        //            return err;
        //        }

        //        private static bool strt(int i, int j, int[,] area, short[,] dir, int nx, int ny, int igx, int igy, int itresh)
        //        {
        //            bool result = true;
        //            int[] d1 = new int[9];
        //            int[] d2 = new int[9];
        //            d1[1] = 0; d1[2] = -1; d1[3] = -1; d1[4] = -1; d1[5] = 0; d1[6] = 1; d1[7] = 1; d1[8] = 1;
        //            d2[1] = 1; d2[2] = 1; d2[3] = 0; d2[4] = -1; d2[5] = -1; d2[6] = -1; d2[7] = 0; d2[8] = 1;

        //            if (area[i, j] < itresh || dir[i, j] < 0)
        //            {
        //                result = false;
        //                if (area[i, j] <= 0)
        //                {
        //                    dir[i, j] = 0; //ZERO DIRECTIONS OUTSIDE AREA
        //                }
        //            }
        //            else //CHECK UPSTREAM PIXELS
        //            {
        //                int ni, nj, ind, jnd;
        //                for (int k = 1; k <= 8; k++)
        //                {
        //                    ni = i + d1[k]; //neighbor pixel
        //                    nj = j + d2[k];
        //                    if (dir[ni, nj] > 0)
        //                    {
        //                        ind = ni + d1[dir[ni, nj]]; //pixel downstream from neighbor
        //                        jnd = nj + d2[dir[ni, nj]];
        //                        if (ind == i && jnd == j) //Neighbor drains into i,j
        //                        {
        //                            if (area[ni, nj] >= itresh)
        //                            {
        //                                result = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                //Do not allow sources that drain off the raster set i.e. a link of 0 length
        //                ni = i + d1[dir[i, j]];
        //                nj = j + d2[dir[i, j]];
        //                if (area[ni, nj] < itresh)
        //                {
        //                    result = false;
        //                }
        //            }

        //            return result;
        //        }

        //        private static bool isnode(int mnext, int mag, int i, int j, int[] inodes, int[] jnodes, int nnodes)
        //        {
        //            bool result = false;
        //            for (int k = 0; k < nnodes; k++)
        //            {
        //                if ((inodes[k]) == i && (jnodes[k]) == j)
        //                {
        //                    result = true;
        //                    if (mnext != mag) //false alarm it is a junction
        //                    {
        //                        result = false;
        //                    }
        //                    return result;
        //                }
        //            }
        //            return result;
        //        }

        //        private static bool isnode2(int i, int j, int[] inodes, int[] jnodes, int nnodes, out int nodeno)
        //        {
        //            nodeno = -1;
        //            for (int k = 0; k < nnodes; k++)
        //            {
        //                if ((inodes[k]) == i && (jnodes[k]) == j)
        //                {
        //                    //+1 is because arrays came from C
        //                    nodeno = k;  // for return to use in indexing
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }

        //        private static int NetProp(short[,] dir, int[,] area, float[,] elev, string coordfile, int icr, int icmax, double dx, double dy, int nx, int ny, double[] bndbox, int err)
        //        {
        //            int[] d1 = new int[9];
        //            int[] d2 = new int[9];
        //            d1[1] = 0; d1[2] = -1; d1[3] = -1; d1[4] = -1; d1[5] = 0; d1[6] = 1; d1[7] = 1; d1[8] = 1;
        //            d2[1] = 1; d2[2] = 1; d2[3] = 0; d2[4] = -1; d2[5] = -1; d2[6] = -1; d2[7] = 0; d2[8] = 1;

        //            int mc = icmax + 1;
        //            double[] rarea = new double[mc];
        //            double[] length = new double[mc];
        //            double[] elv = new double[mc];
        //            int[] ia = new int[mc];
        //            int[] ja = new int[mc];

        //            StreamReader coordr = new StreamReader(coordfile);

        //            string bufferLine;
        //            int n;
        //            for (n = 0; n <= mc; n++)
        //            {
        //                try
        //                {
        //                    bufferLine = coordr.ReadLine();
        //                    if (bufferLine != String.Empty)
        //                    {
        //                        ia[n] = Convert.ToInt32(bufferLine.Substring(0, 10).Trim()) - 1;
        //                        ja[n] = Convert.ToInt32(bufferLine.Substring(11, 10).Trim()) - 1;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //                catch
        //                {
        //                    break;
        //                }
        //            }
        //            coordr.Close();

        //            n = n - 1;
        //            for (int ic = 0; ic <= n; ic++)
        //            {
        //                rarea[ic] = area[ia[ic], ja[ic]] * dx * dy;
        //                elv[ic] = elev[ia[ic], ja[ic]];
        //            }

        //            int iroot = ia[icr];
        //            int jroot = ja[icr];

        //            //----TRACE STREAMS DOWNWARDS
        //            int i, j, inext, jnext;
        //            double DXx, DYy;
        //            for (int ic = 0; ic <= n; ic++)
        //            {
        //                length[ic] = 0;
        //                i = ia[ic];
        //                j = ja[ic];
        //                inext = i + d1[dir[i, j]];
        //                jnext = j + d2[dir[i, j]];

        //                while (dir[inext, jnext] != 0) //not yet end of path
        //                {
        //                    DXx = dx * (double)(j - jnext);
        //                    DYy = dy * (double)(i - inext);
        //                    length[ic] = length[ic] + Math.Sqrt(DXx * DXx + DYy * DYy);
        //                    i = inext;
        //                    j = jnext;
        //                    inext = i + d1[dir[i, j]];
        //                    jnext = j + d2[dir[i, j]];
        //                }
        //            }

        //            //--WRITE OUTPUT
        //            StreamWriter coordw = new StreamWriter(coordfile);
        //            double x, y;
        //            for (int ic = 0; ic <= n; ic++)
        //            {
        //                x = (ja[ic]) * dx + bndbox[0] + dx * 0.5;
        //                y = dy * (ny - ia[ic] - 1) + bndbox[1] + dy * 0.5;
        //                coordw.Write("{0,15:F4} {1,15:F4} {2,15:F4} {3,15:F4} {4,15:F4}\n", x, y, length[ic], elv[ic], rarea[ic]);
        //            }
        //            coordw.Close();

        //            return 0;
        //        }
        //        #endregion

        //        #endregion

        //        #region "Delin Streams Shapefile And Subbasins Grid"
        //        // CWG 27/1/2011 In TauDEM V5 this functionality is included in DelinStreamGrids

        //        //		/// <summary>
        //        //		/// A function which makes calls to TauDEM to delineate streams shapefile and subbasin grid
        //        //		/// </summary>
        //        //		/// <param name="d8Path"></param>
        //        //		/// <param name="TreeDatPath"></param>
        //        //		/// <param name="CoordDatPath"></param>
        //        //		/// <param name="streamShapeResultPath"></param>
        //        //		/// <param name="watershedGridResultPath"></param>
        //        //		/// <param name="callback"></param>
        //        //		/// <returns></returns>
        //        //		public static int DelinStreamsAndSubBasins(string d8Path, string TreeDatPath, string CoordDatPath, string streamShapeResultPath, string watershedGridResultPath, IProgressHandler callback)
        //        //		{
        //        //			int result = -1;
        //        //			int ordert = 1;
        //        //			int subbno = 0;
        //        //
        //        //			TKTAUDEMLib.TauDEM TaudemLib = new TKTAUDEMLib.TauDEM();
        //        //			if (callback != null) TaudemLib.Callback = callback;
        //        //
        //        //			if (callback != null) callback.Progress("Status", 0, "Stream Shapefile and Watershed Grid");
        //        //			DataManagement.DeleteGrid(watershedGridResultPath);
        //        //			DataManagement.DeleteShapefile(streamShapeResultPath);
        //        //
        //        //			try
        //        //			{
        //        //				//result = TaudemLib.Subbasinsetup(d8Path, watershedGridResultPath, TreeDatPath, CoordDatPath, streamShapeResultPath, ordert, subbno);
        //        //				result = CreateSubbasinGridAndNetworkShape(d8Path, TreeDatPath, CoordDatPath, ordert, subbno, watershedGridResultPath, streamShapeResultPath, callback);
        //        //			}
        //        //			catch
        //        //			{
        //        //			}
        //        //
        //        //			if (result != 0)
        //        //			{
        //        //				MessageBox.Show(TaudemLib.getErrorMsg(result), "TauDEM Error " + result, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //        //			}
        //        //
        //        //			CopyProjectionFromGrid(d8Path, watershedGridResultPath);
        //        //			CopyProjectionFromGrid(d8Path, streamShapeResultPath);
        //        //			if (callback != null) callback.Progress("Status", 0, String.Empty);
        //        //			return result;
        //        //		}
        //        //
        //        //		/// <summary>
        //        //		/// An overload of the DelinStreamsAndSubBasins function which will generate a GeoprocDialog for the DelinStreamsAndSubBasins function
        //        //		/// </summary>
        //        //		/// <param name="callback"></param>
        //        //		/// <returns></returns>
        //        //		public static int DelinStreamsAndSubBasins(IProgressHandler callback)
        //        //		{
        //        //			return doDelinStreamsAndSubBasinsDiag(callback);
        //        //		}
        //        //
        //        //		/// <summary>
        //        //		/// An overload of the DelinStreamsAndSubBasins function which will generate a GeoprocDialog for the DelinStreamsAndSubBasins function
        //        //		/// </summary>
        //        //		/// <returns></returns>
        //        //		public static int DelinStreamsAndSubBasins()
        //        //		{
        //        //			return doDelinStreamsAndSubBasinsDiag(null);
        //        //		}
        //        //
        //        //		private static int doDelinStreamsAndSubBasinsDiag(IProgressHandler callback)
        //        //		{
        //        //			GeoProcDialog delinstreamshedDiag = new GeoProcDialog();
        //        //			FileElement d8Elem = delinstreamshedDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //        //			FileElement treeDatElem = delinstreamshedDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenFile);
        //        //			FileElement coordDatElem = delinstreamshedDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenFile);
        //        //			FileElement streamShapeResElem = delinstreamshedDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveShapefile);
        //        //			FileElement shedGridResElem = delinstreamshedDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //        //
        //        //			delinstreamshedDiag.Text = "TauDEM Stream Network Shapefile and Sub-basin Grid";
        //        //			delinstreamshedDiag.HelpTitle = "TauDEM Stream Network Shapefile and Sub-basin Grid";
        //        //			delinstreamshedDiag.HelpText = "This function will generate a stream network shapefile and sub-basin grid from the given inputs.";
        //        //			delinstreamshedDiag.Height = 350;
        //        //			delinstreamshedDiag.HelpPanelVisible = false;
        //        //
        //        //			d8Elem.Caption = "D8 Flow Direction Grid Path";
        //        //			d8Elem.HelpButtonVisible = false;
        //        //
        //        //			treeDatElem.Caption = "Network Tree Data File Path";
        //        //			treeDatElem.Filter = "Data Files (*.dat)|*.dat";
        //        //			treeDatElem.HelpButtonVisible = false;
        //        //
        //        //			coordDatElem.Caption = "Network Coordinates Data File Path";
        //        //			coordDatElem.Filter = "Data Files (*.dat)|*.dat";
        //        //			coordDatElem.HelpButtonVisible = false;
        //        //
        //        //			streamShapeResElem.Caption = "Stream Network Shapefile Result Path";
        //        //			streamShapeResElem.HelpButtonVisible = false;
        //        //
        //        //			shedGridResElem.Caption = "Sub-basins Grid Result Path";
        //        //			shedGridResElem.HelpButtonVisible = false;
        //        //
        //        //
        //        //			if (delinstreamshedDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        //			{
        //        //				return Hydrology.DelinStreamsAndSubBasins(d8Elem.Filename, treeDatElem.Filename, coordDatElem.Filename, streamShapeResElem.Filename, shedGridResElem.Filename, callback);
        //        //			}
        //        //			return -2;
        //        //		}

        //        #region "    Subbasinsetup conversion"

        //        private static int CreateSubbasinGridAndNetworkShape(string D8GridPath, string TreeDatPath, string CoordDatPath, int ordert, int subbno, string ResultBasinGridPath, string ResultNetShapePath, IProgressHandler callback)
        //        {
        //            int newperc = 0;
        //            int oldperc = 0;
        //            if (callback != null) callback.Progress("Status", 0, "Stream Shapefile and Watershed Grid");
        //            int err = 0;
        //            bool success;

        //            int numTreeNodes = -1;
        //            long[] dsNodeID;
        //            int[,] FlowNet;
        //            if (ReadTreeFile(TreeDatPath, out FlowNet, out dsNodeID, numTreeNodes) == 1)
        //            {
        //                return 1;
        //            }

        //            int numCoords = 0;
        //            float[,] CoordList;
        //            if (ReadCoordFile(CoordDatPath, out CoordList, numCoords) == 1)
        //            {
        //                return 1;
        //            }

        //            int numBasins = 0;
        //            int currReach = 2 * (numTreeNodes + 1) - 1; //Initialize current reach number
        //            int maxReaches = 5 * (numTreeNodes + 1) - 2; //The maximum number of reaches possible in binary tree
        //            int[,] ReachConnections = new int[maxReaches + 1, 3];
        //            float[,] ReachProperties = new float[maxReaches + 1, 5];
        //            int[] Magnitude = new int[numTreeNodes + 2];

        //            Raster d8Grid = new Raster();
        //            success = d8Grid.Open(D8GridPath, RasterDataType.UnknownDataType, true, RasterFileType.UseExtension, null);
        //            if (success)
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 1;
        //            }

        //            int numCols = d8Grid.NumColumns;
        //            int numRows = d8Grid.NumRows;
        //            RasterHeader BasinGridHead = new RasterHeader();
        //            BasinGridHead.CopyFrom(d8Grid.Header);
        //            BasinGridHead.NodataValue = -1;

        //            Raster BasinGrid = new Raster();
        //            BasinGrid.CreateNew(ResultBasinGridPath, BasinGridHead, RasterDataType.ShortDataType, BasinGridHead.NodataValue, true, RasterFileType.UseExtension, null);
        //            //TODO: May need this to be a temp path instead of result grid

        //            Shapefile NetSF = new Shapefile();
        //            NetSF.CreateNew(ResultNetShapePath, MapWinGIS.ShpfileType.SHP_POLYLINE);
        //            NetSF.StartEditingShapes(true, null);
        //            InitializeNetFields(NetSF);

        //            Queue<int> Links = new Queue<int>();

        //            for (int i = 0; i <= numTreeNodes; i++)
        //            {
        //                if (numTreeNodes > 0)
        //                {
        //                    newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(numTreeNodes)) * 100);
        //                    if (newperc > oldperc)
        //                    {
        //                        if (callback != null) callback.Progress("Status", newperc, "Stream Shapefile and Watershed Grid");
        //                        oldperc = newperc;
        //                    }
        //                }

        //                if (FlowNet[i, 3] == -1) //This is a root link
        //                {
        //                    if (ordert >= 0)
        //                    {
        //                        PopulateNetworkProperties(numBasins, i, FlowNet, CoordList, currReach, ordert, subbno, Magnitude, dsNodeID, numRows, numCols, ReachConnections, ReachProperties, d8Grid, BasinGrid, NetSF, callback);
        //                    }
        //                    else
        //                    {
        //                        subbno = subbno + 1;
        //                        PopulateNetworkProperties(numBasins, i, FlowNet, CoordList, currReach, ordert, subbno, Magnitude, dsNodeID, numRows, numCols, ReachConnections, ReachProperties, d8Grid, BasinGrid, NetSF, callback);
        //                    }
        //                }

        //                if (FlowNet[i, 4] == 0 && FlowNet[i, 5] == 0) //This is a branch
        //                {
        //                    Links.Enqueue(i);
        //                }
        //            }

        //            if (Links.Count > 0)
        //            {
        //                numBasins = 0;
        //                if (ordert >= 0)
        //                {
        //                    MarkBasinsAndNetworkStack(Links, numBasins, FlowNet, CoordList, currReach, ordert, subbno, Magnitude, dsNodeID, numRows, numCols, ReachConnections, ReachProperties, d8Grid, BasinGrid, NetSF, callback);
        //                }
        //                else
        //                {
        //                    subbno = subbno + 1;
        //                    MarkBasinsAndNetworkStack(Links, numBasins, FlowNet, CoordList, currReach, ordert, subbno, Magnitude, dsNodeID, numRows, numCols, ReachConnections, ReachProperties, d8Grid, BasinGrid, NetSF, callback);
        //                }
        //            }

        //            NetSF.Save();
        //            NetSF.Close();

        //            success = BasinGrid.Save(ResultBasinGridPath, RasterFileType.UseExtension, null);
        //            if (success)
        //            {
        //                err = 0;
        //            }
        //            else
        //            {
        //                err = 2;
        //            }
        //            BasinGrid.Close();
        //            if (callback != null) callback.Progress("Status", 0, "Stream Shapefile and Watershed Grid");

        //            return err;
        //        }

        //        private static int ReadTreeFile(string TreeDatPath, out int[,] FlowNet, out long[] dsNodeID, int numTreeNodes)
        //        {
        //            System.IO.StreamReader tree = null;
        //            try
        //            {
        //                tree = new System.IO.StreamReader(TreeDatPath);
        //                string line;
        //                while (!tree.EndOfStream)
        //                {
        //                    line = tree.ReadLine();
        //                    numTreeNodes = numTreeNodes + 1;
        //                }
        //                tree.Close();
        //                FlowNet = new int[numTreeNodes + 1, 7];
        //                dsNodeID = new long[numTreeNodes + 1];
        //                tree = new System.IO.StreamReader(TreeDatPath);
        //                for (int i = 0; i <= numTreeNodes; i++)
        //                {
        //                    line = tree.ReadLine();
        //                    int counter = -1;
        //                    string[] split = line.Split(' ');
        //                    for (int j = 0; j <= split.Length - 1; j++)
        //                    {
        //                        if (split[j] != String.Empty)
        //                        {
        //                            counter = counter + 1;
        //                            if (counter < 7)
        //                            {
        //                                FlowNet[i, counter] = Int32.Parse(split[j]);
        //                            }
        //                            else if (counter == 7)
        //                            {
        //                                dsNodeID[i] = Int32.Parse(split[j]);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                tree.Close();
        //            }
        //            catch
        //            {
        //                FlowNet = new int[0, 0];
        //                dsNodeID = new long[0];
        //                return 1;
        //            }
        //            finally
        //            {
        //                if (tree != null)
        //                {
        //                    tree.Close();
        //                }
        //            }
        //            return 0;
        //        }

        //        private static int ReadCoordFile(string CoordDatPath, out float[,] CoordList, int numCoords)
        //        {
        //            System.IO.StreamReader coordSR = null;
        //            try
        //            {
        //                string line;
        //                coordSR = new System.IO.StreamReader(CoordDatPath);
        //                while (!coordSR.EndOfStream)
        //                {
        //                    line = coordSR.ReadLine();
        //                    numCoords = numCoords + 1;
        //                }
        //                coordSR.Close();
        //                CoordList = new float[numCoords + 1, 5];
        //                numCoords = numCoords - 1;
        //                coordSR = new System.IO.StreamReader(CoordDatPath);
        //                for (int i = 0; i <= numCoords; i++)
        //                {
        //                    line = coordSR.ReadLine();
        //                    int counter = -1;
        //                    string[] split = line.Split(' ');
        //                    for (int j = 0; j <= split.Length - 1; j++)
        //                    {
        //                        if (split[j] != String.Empty)
        //                        {
        //                            counter = counter + 1;
        //                            if (counter < 4)
        //                            {
        //                                CoordList[i, counter] = float.Parse(split[j]);
        //                            }
        //                            else if (counter == 4)
        //                            {
        //                                CoordList[i, counter] = float.Parse(split[j]);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                coordSR.Close();
        //            }
        //            catch
        //            {
        //                CoordList = new float[0, 0];
        //                return 1;
        //            }
        //            finally
        //            {
        //                if (coordSR != null)
        //                {
        //                    coordSR.Close();
        //                }
        //            }
        //            return 0;
        //        }

        //        private static int PopulateNetworkProperties(int numBasins, int StartLink, int[,] FlowNet, float[,] CoordList, int currReach, int ordert, int subbno, int[] Magnitude, long[] dsNodeID, int numRows, int numCols, int[,] ReachConnections, float[,] ReachProperties, Raster d8Grid, Raster BasinGrid, Shapefile NetSF, IProgressHandler callback)
        //        {
        //            int currLink;
        //            int currDSLink;
        //            int currDSFrom;
        //            int row;
        //            int UpstreamLink1;
        //            int UpstreamLink2;
        //            int col;
        //            int thisreach;
        //            int BasinID;
        //            int flag = 0;
        //            int LinkEnd;
        //            int LinkBegin;
        //            int LinkEndArea = 0;
        //            //  variables for CoordList positions
        //            float x;
        //            float y;

        //            Stack<int> links = new Stack<int>();
        //            links.Push(-1);
        //            links.Push(-1);
        //            links.Push(StartLink);

        //            while (links.Count != 0)
        //            {
        //                if (callback != null) callback.Progress("Status", 0, "Stream Shapefile and Watershed Grid");
        //                flag = 0;
        //                LinkEndArea = 0;

        //                currLink = links.Pop();
        //                currDSFrom = links.Pop();
        //                currDSLink = links.Pop();

        //                LinkEnd = FlowNet[currLink, 3 - 1];//*  This is CoordList of end of link */
        //                LinkBegin = FlowNet[currLink, 2 - 1];//*  This is CoordList of beg of link */
        //                Magnitude[currLink] = 0;// Initiaize magnitude recursion

        //                if (LinkBegin < LinkEnd)
        //                {
        //                    //has physical length
        //                    numBasins = numBasins + 1;
        //                    if (ordert < 0)
        //                    {
        //                        numBasins = subbno;
        //                    }

        //                    if (FlowNet[currLink, 4 - 1] != -1) //For anything other than a downstream end area is defined one grid cell back
        //                    {
        //                        LinkEndArea = LinkEnd - 1;
        //                    }
        //                    else
        //                    {
        //                        LinkEndArea = LinkEnd;
        //                    }

        //                    x = CoordList[LinkEndArea, 1 - 1];
        //                    y = CoordList[LinkEndArea, 2 - 1];
        //                    BasinGrid.ProjToCell(x, y, out col, out row);
        //                    if (row < 0 | row > numRows | col < 0 | col > numCols)
        //                    {
        //                        if (currDSLink != -1 && currDSFrom != -1)
        //                        {
        //                            ReachConnections[currDSLink, currDSFrom] = -1;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    x = CoordList[LinkEndArea, 1 - 1];
        //                    y = CoordList[LinkEndArea, 2 - 1];
        //                    BasinGrid.ProjToCell(x, y, out col, out row);
        //                    if (row < 0 | row > numRows | col < 0 | col > numCols)
        //                    {
        //                        if (currDSLink != -1 && currDSFrom != -1)
        //                        {
        //                            ReachConnections[currDSLink, currDSFrom] = -2;
        //                        }
        //                    }
        //                    LinkEndArea = LinkEnd;
        //                    flag = 1;
        //                }

        //                if (callback != null) callback.Progress("Status", 50, "Stream Shapefile and Watershed Grid");

        //                //Search for upstream basins
        //                UpstreamLink1 = FlowNet[currLink, 5 - 1]; //pointers to upstream links
        //                UpstreamLink2 = FlowNet[currLink, 6 - 1];

        //                if (UpstreamLink1 > 0 | UpstreamLink2 > 0)
        //                {
        //                    if (flag == 1)
        //                    {
        //                        //dummy 0 length reach
        //                        currReach = currReach + 1;
        //                        thisreach = currReach;
        //                        if (ordert <= 0)
        //                        {
        //                            BasinID = subbno;
        //                        }
        //                        else
        //                        {
        //                            BasinID = 0;
        //                        }

        //                        ReachConnections[thisreach, 1 - 1] = thisreach;
        //                        if (UpstreamLink1 > 0)
        //                        {
        //                            links.Push(thisreach);
        //                            links.Push(2 - 1);
        //                            links.Push(UpstreamLink1);
        //                            Magnitude[currLink] = Magnitude[currLink] + Magnitude[UpstreamLink1];
        //                        }
        //                        else
        //                        {
        //                            ReachConnections[thisreach, 2 - 1] = 0;
        //                        }
        //                        // CWG 20/7/2010 missing code for second link added
        //                        if (UpstreamLink2 > 0)
        //                        {
        //                            links.Push(thisreach);
        //                            links.Push(3 - 1);
        //                            links.Push(UpstreamLink2);
        //                            Magnitude[currLink] = Magnitude[currLink] + Magnitude[UpstreamLink2];
        //                        }
        //                        else
        //                        {
        //                            ReachConnections[thisreach, 3 - 1] = 0;
        //                        }
        //                        //AddReachShape3(NetSF, FlowNet, CoordList, currLink, BasinID, Magnitude[currLink], dsNodeID[currLink]);

        //                        //'Assign properties to dummy reach
        //                        ReachProperties[thisreach, 1 - 1] = 0.01f; //slope
        //                        ReachProperties[thisreach, 2 - 1] = CoordList[LinkEndArea, 5 - 1]; //ontributing area
        //                        ReachProperties[thisreach, 3 - 1] = 0; //Length
        //                        ReachProperties[thisreach, 4 - 1] = CoordList[LinkEndArea, 0]; //end x CoordList
        //                        ReachProperties[thisreach, 5 - 1] = CoordList[LinkEndArea, 1]; //end x CoordList
        //                    }
        //                    else
        //                    {
        //                        //Lower half reach
        //                        currReach = currReach + 1;
        //                        thisreach = currReach;
        //                        ReachConnections[thisreach, 1 - 1] = thisreach;
        //                        ReachConnections[thisreach, 2 - 1] = numBasins;
        //                        ReachConnections[thisreach, 3 - 1] = currReach + 1;
        //                        ReachProperties[thisreach, 3 - 1] = (CoordList[LinkBegin, 3 - 1] - CoordList[LinkEnd, 3 - 1]) / 2;
        //                        ReachProperties[thisreach, 2 - 1] = CoordList[LinkEndArea, 5 - 1];
        //                        ReachProperties[thisreach, 1 - 1] = (CoordList[LinkBegin, 4 - 1] - CoordList[LinkEnd, 4 - 1]);
        //                        ReachProperties[thisreach, 4 - 1] = CoordList[LinkEndArea, 0]; //end x CoordList
        //                        ReachProperties[thisreach, 5 - 1] = CoordList[LinkEndArea, 1]; //end y coordinate
        //                        if (ordert <= 0)
        //                        {
        //                            BasinID = subbno;
        //                        }
        //                        else
        //                        {
        //                            BasinID = numBasins;
        //                        }

        //                        //Upper half reach
        //                        currReach = currReach + 1;
        //                        ReachConnections[thisreach + 1, 1 - 1] = currReach;
        //                        if (UpstreamLink1 > 0)
        //                        {
        //                            links.Push(thisreach + 1);
        //                            links.Push(2 - 1);
        //                            links.Push(UpstreamLink1);
        //                            Magnitude[currLink] = Magnitude[currLink] + Magnitude[UpstreamLink1];
        //                        }
        //                        else
        //                        {
        //                            ReachConnections[thisreach + 1, 2 - 1] = 0;
        //                        }
        //                        if (UpstreamLink2 > 0)
        //                        {
        //                            links.Push(thisreach + 1);
        //                            links.Push(3 - 1);
        //                            links.Push(UpstreamLink2);
        //                            Magnitude[currLink] = Magnitude[currLink] + Magnitude[UpstreamLink2];
        //                        }
        //                        else
        //                        {
        //                            ReachConnections[thisreach + 1, 3 - 1] = 0;
        //                        }
        //                        //AddReachShape3(NetSF, FlowNet, CoordList, currLink, BasinID, Magnitude[currLink], dsNodeID[currLink]);
        //                        ReachProperties[thisreach + 1, 3 - 1] = ReachProperties[thisreach, 3 - 1];
        //                        ReachProperties[thisreach + 1, 2 - 1] = ReachProperties[thisreach, 2 - 1];
        //                        ReachProperties[thisreach + 1, 1 - 1] = ReachProperties[thisreach, 1 - 1];
        //                        ReachProperties[thisreach + 1, 4 - 1] = CoordList[(LinkEndArea + LinkBegin) / 2, 0]; //approx midpoint
        //                        ReachProperties[thisreach + 1, 5 - 1] = CoordList[(LinkEndArea + LinkBegin) / 2, 1];
        //                    }
        //                }
        //                else
        //                {
        //                    //This is an external basin
        //                    currReach = currReach + 1;
        //                    thisreach = currReach;
        //                    ReachConnections[thisreach, 1 - 1] = currReach;
        //                    ReachConnections[thisreach, 2 - 1] = numBasins;
        //                    ReachConnections[thisreach, 3 - 1] = 0;
        //                    ReachProperties[thisreach, 3 - 1] = (CoordList[LinkBegin, 3 - 1] - CoordList[LinkEnd, 3 - 1]) / 2;
        //                    ReachProperties[thisreach, 2 - 1] = CoordList[LinkEndArea, 5 - 1];
        //                    if (ReachProperties[thisreach, 3 - 1] <= 0)
        //                    {
        //                        ReachProperties[thisreach, 1 - 1] = 0.01f;
        //                    }
        //                    else
        //                    {
        //                        ReachProperties[thisreach, 1 - 1] = (CoordList[LinkBegin, 4 - 1] - CoordList[LinkEnd, 4 - 1]) / (2 * ReachProperties[thisreach, 3 - 1]);
        //                    }
        //                    ReachProperties[thisreach, 4 - 1] = CoordList[LinkEndArea, 0]; //end x coordinate
        //                    ReachProperties[thisreach, 5 - 1] = CoordList[LinkEndArea, 1]; //end y coordinate

        //                    if (ordert <= 0)
        //                    {
        //                        BasinID = subbno;
        //                    }
        //                    else
        //                    {
        //                        BasinID = numBasins;
        //                    }
        //                    Magnitude[currLink] = 1;  //magnitude of external basin
        //                    //AddReachShape3(NetSF, FlowNet, CoordList, currLink, BasinID, Magnitude[currLink], dsNodeID[currLink]);
        //                }
        //                if (currDSLink != -1 && currDSFrom != -1)
        //                {
        //                    ReachConnections[currDSLink, currDSFrom] = thisreach;
        //                }

        //                if (callback != null) callback.Progress("Status", 100, "Stream Shapefile and Watershed Grid");
        //            }
        //            return 0;
        //        }

        //        private static int MarkBasinsAndNetworkStack(Queue<int> Links, int numBasins, int[,] FlowNet, float[,] CoordList, int currReach, int ordert, int subbno, int[] Magnitude, long[] dsNodeID, int numRows, int numCols, int[,] ReachConnections, float[,] ReachProperties, Raster d8Grid, Raster BasinGrid, Shapefile NetSF, IProgressHandler callback)
        //        {
        //            int newperc = 0;
        //            int oldperc = 0;
        //            bool alreadyMarked = false;
        //            int currLink, row, col, DownstreamLink, LinkEnd, LinkBegin, LinkEndArea = 0;
        //            //  variables for CoordList positions
        //            float x, y;

        //            List<int> marked = new List<int>();
        //            List<MapWinGIS.Point> markedPoint = new List<MapWinGIS.Point>();

        //            int totLinks;
        //            while (Links.Count != 0)
        //            {
        //                totLinks = Links.Count;
        //                if (totLinks > 0)
        //                {
        //                    newperc = Convert.ToInt32((Convert.ToDouble(Links.Count) / Convert.ToDouble(totLinks)) * 100);
        //                    if (newperc > oldperc)
        //                    {
        //                        if (callback != null) callback.Progress("Status", newperc, "Stream Shapefile and Watershed Grid");
        //                        oldperc = newperc;
        //                    }
        //                }

        //                LinkEndArea = 0;

        //                currLink = Links.Dequeue();

        //                if ((FlowNet[currLink, 4] == 0 || (FlowNet[currLink, 4] != 0 && marked.Contains(FlowNet[currLink, 4]))) && (FlowNet[currLink, 5] == 0 || (FlowNet[currLink, 5] != 0 && marked.Contains(FlowNet[currLink, 5]))))
        //                {
        //                    LinkEnd = FlowNet[currLink, 3 - 1];//*  This is CoordList of end of link */
        //                    LinkBegin = FlowNet[currLink, 2 - 1];//*  This is CoordList of beg of link */
        //                    if (LinkBegin < LinkEnd)
        //                    {
        //                        //has physical length
        //                        numBasins = numBasins + 1;
        //                        if (ordert < 0)
        //                        {
        //                            numBasins = subbno;
        //                        }

        //                        if (FlowNet[currLink, 4 - 1] != -1) //For anything other than a downstream end area is defined one grid cell back
        //                        {
        //                            LinkEndArea = LinkEnd - 1;
        //                        }
        //                        else
        //                        {
        //                            LinkEndArea = LinkEnd;
        //                        }

        //                        x = CoordList[LinkEndArea, 1 - 1];
        //                        y = CoordList[LinkEndArea, 2 - 1];
        //                        BasinGrid.ProjToCell(x, y, out col, out row);
        //                        alreadyMarked = false;
        //                        for (int i = 0; i < markedPoint.Count; i++)
        //                        {
        //                            if ((double)col == markedPoint[i].x && (double)row == markedPoint[i].y)
        //                            {
        //                                alreadyMarked = true;
        //                                break;
        //                            }
        //                        }
        //                        if (!alreadyMarked)
        //                        {
        //                            marked.Add(currLink);
        //                            MapWinGIS.Point pt = new MapWinGIS.Point();
        //                            pt.x = (double)col;
        //                            pt.y = (double)row;
        //                            markedPoint.Add(pt);
        //                            MarkBasinAreaStack(d8Grid, row, col, numBasins, numCols, numRows, BasinGrid, callback); //Label the region that drains to this pixel
        //                            AddReachShape(NetSF, FlowNet, CoordList, currLink, numBasins, Magnitude[currLink], dsNodeID[currLink]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        LinkEndArea = LinkEnd;
        //                        marked.Add(currLink); // CWG 20/7/2010 added to prevent loop
        //                        // CWG 20/7/2010 include zero length links in network shapefile
        //                        AddReachShape(NetSF, FlowNet, CoordList, currLink, 0, Magnitude[currLink], dsNodeID[currLink]);
        //                    }

        //                    DownstreamLink = FlowNet[currLink, 4 - 1];
        //                    if (DownstreamLink >= 0 && !Links.Contains(DownstreamLink) && !marked.Contains(DownstreamLink))
        //                    {
        //                        Links.Enqueue(DownstreamLink);
        //                    }
        //                }
        //                else
        //                {
        //                    Links.Enqueue(currLink);
        //                }
        //            }

        //            return 0;
        //        }

        //        private static void MarkBasinAreaStack(Raster d8Grid, int StartRow, int StartCol, int BasinID, int numCols, int numRows, Raster BasinGrid, IProgressHandler callback)
        //        {
        //            int row;
        //            int col;
        //            int newRow;
        //            int newCol;
        //            int[] rowMod = new int[9];
        //            int[] colMod = new int[9];
        //            rowMod[1] = 0; rowMod[2] = -1; rowMod[3] = -1; rowMod[4] = -1; rowMod[5] = 0; rowMod[6] = 1; rowMod[7] = 1; rowMod[8] = 1;
        //            colMod[1] = 1; colMod[2] = 1; colMod[3] = 0; colMod[4] = -1; colMod[5] = -1; colMod[6] = -1; colMod[7] = 0; colMod[8] = 1;

        //            Stack<int> cells = new Stack<int>();
        //            cells.Push(StartCol);
        //            cells.Push(StartRow);

        //            int totCells = 0, newperc = 0, oldperc = 0;
        //            while (cells.Count != 0)
        //            {
        //                if (totCells > 0)
        //                {
        //                    newperc = Convert.ToInt32((Convert.ToDouble(cells.Count) / Convert.ToDouble(totCells)) * 100);
        //                    if (newperc > oldperc)
        //                    {
        //                        if (callback != null) callback.Progress("Status", newperc, "Stream Shapefile and Watershed Grid");
        //                        oldperc = newperc;
        //                    }
        //                }

        //                row = cells.Pop();
        //                col = cells.Pop();

        //                if ((short)BasinGrid.get_Value(col, row) == -1)
        //                {
        //                    if (row != 0 & row != numRows - 1 & col != 0 & col != numCols - 1 & (short)d8Grid.get_Value(col, row) != -1)
        //                    {
        //                        //Not on boundary
        //                        BasinGrid.set_Value(col, row, BasinID);
        //                        for (int k = 1; k <= 8; k++)
        //                        {
        //                            newRow = row + rowMod[k];
        //                            newCol = col + colMod[k];

        //                            //test if neighbor drains towards cell excluding boundaries
        //                            if ((short)d8Grid.get_Value(newCol, newRow) >= 0 & (((short)d8Grid.get_Value(newCol, newRow) - k) == 4 | ((short)d8Grid.get_Value(newCol, newRow) - k) == -4))
        //                            {
        //                                cells.Push(newCol);
        //                                cells.Push(newRow);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        private static void InitializeNetFields(Shapefile netSF)
        //        {
        //            int zero = 0;
        //            MapWinGIS.Field field = new MapWinGIS.Field();
        //            field.Name = "DOUT_MID";
        //            field.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field.Width = 16;
        //            field.Precision = 1;
        //            netSF.EditInsertField(field, zero, null);

        //            MapWinGIS.Field field2 = new MapWinGIS.Field();
        //            field2.Name = "DOUT_START";
        //            field2.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field2.Width = 16;
        //            field2.Precision = 1;
        //            netSF.EditInsertField(field2, zero, null);

        //            MapWinGIS.Field field3 = new MapWinGIS.Field();
        //            field3.Name = "DOUT_END";
        //            field3.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field3.Width = 16;
        //            field3.Precision = 1;
        //            netSF.EditInsertField(field3, zero, null);

        //            MapWinGIS.Field field4 = new MapWinGIS.Field();
        //            field4.Name = "WSNO";
        //            field4.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field4.Width = 6;
        //            field4.Precision = 0;
        //            netSF.EditInsertField(field4, zero, null);

        //            MapWinGIS.Field field5 = new MapWinGIS.Field();
        //            field5.Name = "US_Cont_Area";
        //            field5.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field5.Width = 16;
        //            field5.Precision = 1;
        //            netSF.EditInsertField(field5, zero, null);

        //            MapWinGIS.Field field6 = new MapWinGIS.Field();
        //            field6.Name = "Straight_Length";
        //            field6.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field6.Width = 16;
        //            field6.Precision = 0;
        //            netSF.EditInsertField(field6, zero, null);

        //            MapWinGIS.Field field7 = new MapWinGIS.Field();
        //            field7.Name = "Slope";
        //            field7.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field7.Width = 16;
        //            field7.Precision = 12;
        //            netSF.EditInsertField(field7, zero, null);

        //            MapWinGIS.Field field8 = new MapWinGIS.Field();
        //            field8.Name = "Drop";
        //            field8.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field8.Width = 16;
        //            field8.Precision = 2;
        //            netSF.EditInsertField(field8, zero, null);

        //            MapWinGIS.Field field9 = new MapWinGIS.Field();
        //            field9.Name = "DS_Cont_Area";
        //            field9.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field9.Width = 16;
        //            field9.Precision = 1;
        //            netSF.EditInsertField(field9, zero, null);

        //            MapWinGIS.Field field10 = new MapWinGIS.Field();
        //            field10.Name = "Magnitude";
        //            field10.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field10.Width = 6;
        //            field10.Precision = 0;
        //            netSF.EditInsertField(field10, zero, null);

        //            MapWinGIS.Field field11 = new MapWinGIS.Field();
        //            field11.Name = "Length";
        //            field11.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field11.Width = 16;
        //            field11.Precision = 1;
        //            netSF.EditInsertField(field11, zero, null);

        //            MapWinGIS.Field field12 = new MapWinGIS.Field();
        //            field12.Name = "Order";
        //            field12.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field12.Width = 6;
        //            field12.Precision = 0;
        //            netSF.EditInsertField(field12, zero, null);

        //            MapWinGIS.Field field13 = new MapWinGIS.Field();
        //            field13.Name = "dsNodeID";
        //            field13.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
        //            field13.Width = 12;
        //            field13.Precision = 1;
        //            netSF.EditInsertField(field13, zero, null);

        //            MapWinGIS.Field field14 = new MapWinGIS.Field();
        //            field14.Name = "USLINKNO2";
        //            field14.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field14.Width = 6;
        //            field14.Precision = 0;
        //            netSF.EditInsertField(field14, zero, null);

        //            MapWinGIS.Field field15 = new MapWinGIS.Field();
        //            field15.Name = "USLINKNO1";
        //            field15.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field15.Width = 6;
        //            field15.Precision = 0;
        //            netSF.EditInsertField(field15, zero, null);

        //            MapWinGIS.Field field16 = new MapWinGIS.Field();
        //            field16.Name = "DSLINKNO";
        //            field16.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field16.Width = 6;
        //            field16.Precision = 0;
        //            netSF.EditInsertField(field16, zero, null);

        //            MapWinGIS.Field field17 = new MapWinGIS.Field();
        //            field17.Name = "LINKNO";
        //            field17.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            field17.Width = 6;
        //            field17.Precision = 0;
        //            netSF.EditInsertField(field17, zero, null);
        //        }

        //        private static int AddReachShape(Shapefile NetSF, int[,] FlowNet, float[,] CoordList, int currLink, int BasinID, int Magnitude, long dsNodeID)
        //        {
        //            double x = 0;
        //            double y = 0;
        //            double length;
        //            double glength;
        //            double x1;
        //            double y1;
        //            double xlast;
        //            double ylast;
        //            double usarea;
        //            double dsarea;
        //            double dslast;
        //            double dl;
        //            double drop;
        //            double slope;
        //            int istart;
        //            int iend;
        //            int zero;

        //            istart = FlowNet[currLink, 1];  //start index for reach
        //            iend = FlowNet[currLink, 2]; //end index for reach
        //            x1 = CoordList[istart, 0]; //start x CoordList for reach
        //            y1 = CoordList[istart, 1]; //start y CoordList for reach
        //            length = 0;
        //            xlast = x1;
        //            ylast = y1;
        //            usarea = CoordList[istart, 4];
        //            dslast = usarea;
        //            dsarea = usarea;

        //            IFeature shp = new IFeature();
        //            shp.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
        //            for (int j = 0; j <= (iend - istart); j++)
        //            {
        //                x = CoordList[j + istart, 0];
        //                y = CoordList[j + istart, 1];
        //                dl = Math.Sqrt((x - xlast) * (x - xlast) + (y - ylast) * (y - ylast));
        //                if (dl > 0)
        //                {
        //                    length = length + dl;
        //                    xlast = x;
        //                    ylast = y;
        //                    dsarea = dslast; //keeps track of last ds area
        //                    dslast = CoordList[j + istart, 4];
        //                }
        //                MapWinGIS.Point p = new MapWinGIS.Point();
        //                p.x = x;
        //                p.y = y;
        //                zero = 0;
        //                shp.InsertPoint(p, zero);
        //            }
        //            if (iend == istart)
        //            {
        //                MapWinGIS.Point p = new MapWinGIS.Point();
        //                p.x = x;
        //                p.y = y;
        //                int numpts = shp.numPoints;
        //                shp.InsertPoint(p, numpts);
        //            }
        //            drop = CoordList[istart, 3] - CoordList[iend, 3];
        //            slope = 0;
        //            float dsdist = CoordList[iend, 2];
        //            float usdist = CoordList[istart, 2];
        //            float middist = (dsdist + usdist) * 0.5f;
        //            if (length > 0)
        //            {
        //                slope = drop / length;
        //            }
        //            glength = Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));
        //            zero = 0;
        //            NetSF.EditInsertShape(shp, zero);
        //            NetSF.EditCellValue(0, 0, currLink);
        //            NetSF.EditCellValue(1, 0, FlowNet[currLink, 3]);
        //            NetSF.EditCellValue(2, 0, FlowNet[currLink, 4]);
        //            NetSF.EditCellValue(3, 0, FlowNet[currLink, 5]);
        //            NetSF.EditCellValue(4, 0, dsNodeID);
        //            NetSF.EditCellValue(5, 0, FlowNet[currLink, 6]);
        //            NetSF.EditCellValue(6, 0, length);
        //            NetSF.EditCellValue(7, 0, Magnitude);
        //            NetSF.EditCellValue(8, 0, dsarea);
        //            NetSF.EditCellValue(9, 0, drop);
        //            NetSF.EditCellValue(10, 0, slope);
        //            NetSF.EditCellValue(11, 0, glength);
        //            NetSF.EditCellValue(12, 0, usarea);
        //            NetSF.EditCellValue(13, 0, BasinID);
        //            NetSF.EditCellValue(14, 0, dsdist);
        //            NetSF.EditCellValue(15, 0, usdist);
        //            NetSF.EditCellValue(16, 0, middist);

        //            return 0;
        //        }

        //        #endregion //converted Subbasinsetup

        //        #endregion

        //        #region "Create Network Outlets"
        //        /// <summary>
        //        /// A function to generate a network outlets shapefile from the tree.dat and coords.dat files.
        //        /// </summary>
        //        /// <param name="TreeDatPath">The path to the tree.dat file</param>
        //        /// <param name="CoordDatPath">The path to the coords.dat file</param>
        //        /// <param name="OutletsShapeResultPath">The output path for the network outlets shapefile</param>
        //        /// <param name="callback">A callback for progress messages</param>
        //        /// <returns></returns>
        //        public static bool CreateNetworkOutlets(string TreeDatPath, string CoordDatPath, string OutletsShapeResultPath, IProgressHandler callback)
        //        {
        //            int newperc = 0;
        //            int oldperc = 0;
        //            int numShps = 0, LinkEnd, LinkBegin, LinkEndArea = 0;
        //            float x, y;

        //            if (callback != null) callback.Progress("Status", 0, "Create Network Outlets");
        //            DataManagement.DeleteShapefile(OutletsShapeResultPath);

        //            Shapefile sf = new Shapefile();
        //            sf.CreateNew(OutletsShapeResultPath, MapWinGIS.ShpfileType.SHP_POINT);
        //            sf.StartEditingShapes(true, null);

        //            int zero = 0;
        //            MapWinGIS.Field field = new MapWinGIS.Field();
        //            field.Name = "MWShapeID";
        //            field.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //            sf.EditInsertField(field, zero, null);

        //            int numTreeNodes = 0;
        //            long[] dsNodeID;
        //            int[,] FlowNet;
        //            if (ReadTreeFile(TreeDatPath, out FlowNet, out dsNodeID, numTreeNodes) == 1)
        //            {
        //                MessageBox.Show("An error occured in creating the network outlets while reading the tree.dat file.", "Creating Network Outlets Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //                return false;
        //            }

        //            int numCoords = 0;
        //            float[,] CoordList;
        //            if (ReadCoordFile(CoordDatPath, out CoordList, numCoords) == 1)
        //            {
        //                MessageBox.Show("An error occured in creating the network outlets while reading the coords.dat file.", "Creating Network Outlets Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //                return false;
        //            }

        //            for (int i = 0; i <= numTreeNodes; i++)
        //            {
        //                if (numTreeNodes > 0)
        //                {
        //                    newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(numTreeNodes)) * 100);
        //                    if (newperc > oldperc)
        //                    {
        //                        if (callback != null) callback.Progress("Status", newperc, "Create Network Outlets");
        //                        oldperc = newperc;
        //                    }
        //                }

        //                LinkEnd = FlowNet[i, 3 - 1];//*  This is CoordList of end of link */
        //                LinkBegin = FlowNet[i, 2 - 1];//*  This is CoordList of beg of link */
        //                if (LinkBegin < LinkEnd)
        //                {
        //                    //has physical length
        //                    if (FlowNet[i, 4 - 1] != -1) //For anything other than a downstream end area is defined one grid cell back
        //                    {
        //                        LinkEndArea = LinkEnd - 1;
        //                    }
        //                    else
        //                    {
        //                        LinkEndArea = LinkEnd;
        //                    }

        //                    x = CoordList[LinkEndArea, 1 - 1];
        //                    y = CoordList[LinkEndArea, 2 - 1];

        //                    MapWinGIS.Point pt = new MapWinGIS.Point();
        //                    pt.x = x;
        //                    pt.y = y;
        //                    IFeature shp = new IFeature();
        //                    shp.Create(MapWinGIS.ShpfileType.SHP_POINT);
        //                    shp.InsertPoint(pt, numShps);
        //                    numShps = sf.NumRows();
        //                    sf.EditInsertShape(shp, numShps);
        //                    sf.EditCellValue(0, numShps, numShps);
        //                }
        //            }
        //            sf.Save();
        //            sf.Close();
        //            if (callback != null) callback.Progress("Status", 0, String.Empty);
        //            return true;
        //        }

        //        #endregion

        //        #region "Subbasins to Shapes"
        ///// <summary>
        ///// A function which uses the mapwingis utils to convert the subbasin grid to a polygon shapefile
        ///// </summary>
        ///// <param name="d8Path"></param>
        ///// <param name="watershedGridPath"></param>
        ///// <param name="watershedShapeResultPath"></param>
        ///// <param name="callback">A MapWinGIS.IProgressHandler used to return error messages etc.</param>
        ///// <returns>0 on success, -1 otherwise </returns>
        //public static int SubbasinsToShape(string d8Path, string watershedGridPath, string watershedShapeResultPath, IProgressHandler callback)
        //{
        //    var result = -1;
        //    var gridD8 = new Grid();
        //    var gridWatershed = new Grid();

        //    var u = new MapWinGIS.Utils();

        //    if (callback != null)
        //    {
        //        callback.Progress("Status", 0, "Watershed Grid to Shapefile");
        //    }

        //    //DataManagement.DeleteShapefile(watershedShapeResultPath);
        //    gridD8.Open(d8Path, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);
        //    gridWatershed.Open(watershedGridPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callback);
        //    var sf = u.GridToShapefile(gridWatershed, gridD8, callback);
        //    if (sf.SaveAs(watershedShapeResultPath, callback))
        //    {
        //        result = 0;
        //    }

        //    sf.Projection = gridD8.Projection.ToString();
        //    gridD8.Close();

        //    gridWatershed.Close();
        //    sf.Close();

        //    if (callback != null)
        //    {
        //        callback.Progress("Status", 0, string.Empty);
        //    }

        //    return result;
        //}

        //        /// <summary>
        //        /// An overload of the SubbasinsToShape function which will generate a GeoprocDialog for the SubbasinsToShape function
        //        /// </summary>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static int SubbasinsToShape(IProgressHandler callback)
        //        {
        //            return doSubbasinsToShapeDiag(callback);
        //        }

        //        /// <summary>
        //        /// An overload of the SubbasinsToShape function which will generate a GeoprocDialog for the SubbasinsToShape function
        //        /// </summary>
        //        /// <returns></returns>
        //        public static int SubbasinsToShape()
        //        {
        //            return doSubbasinsToShapeDiag(null);
        //        }

        //        private static int doSubbasinsToShapeDiag(IProgressHandler callback)
        //        {
        //            GeoProcDialog subtoshapeDiag = new GeoProcDialog();
        //            FileElement d8Elem = subtoshapeDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement shedGridElem = subtoshapeDiag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //            FileElement shedShapeResElem = subtoshapeDiag.Add_FileElement(GeoProcDialog.ElementTypes.SaveShapefile);

        //            subtoshapeDiag.Text = "Sub-basins to Shapefile Conversion";
        //            subtoshapeDiag.HelpTitle = "Sub-basins to Shapefile Conversion";
        //            subtoshapeDiag.HelpText = "This function will generate a polygon shapefile of sub-basins from a sub-basin grid and D8 grid.";
        //            subtoshapeDiag.Height = 250;
        //            subtoshapeDiag.HelpPanelVisible = false;

        //            d8Elem.Caption = "D8 Flow Direction Grid Path";
        //            d8Elem.HelpButtonVisible = false;

        //            shedGridElem.Caption = "Sub-basins Grid Result Path";
        //            shedGridElem.HelpButtonVisible = false;

        //            shedShapeResElem.Caption = "Sub-basins Shapefile Result Path";
        //            shedShapeResElem.HelpButtonVisible = false;

        //            if (subtoshapeDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                return Hydrology.SubbasinsToShape(d8Elem.Filename, shedGridElem.Filename, shedShapeResElem.Filename, callback);
        //            }
        //            return -2;
        //        }

        //        #endregion

        //        #region Move outlets to streams
        //        /// <summary>
        //        /// Move outlets down slope in d8Path to streams in StreamGridPath
        //        /// </summary>
        //        /// <param name="D8Path">D8 slope grid</param>
        //        /// <param name="StreamGridPath">grid of streams</param>
        //        /// <param name="OutletsPath">Point shape file of outlets and inlets</param>
        //        /// <param name="MovedOutletsPath">Result point shape file of moved outlets and inlets</param>
        //        /// <param name="Thresh">threshold</param>
        //        /// <param name="numProcesses">Limit on number of threads</param>
        //        /// <param name="showTaudemOutput">taudem output is shown if this is true</param>
        //        /// <param name="callback"></param>
        //        /// <returns>0 if success</returns>
        //        public static int MoveOutletsToStreams(string D8Path, string StreamGridPath, string OutletsPath, string MovedOutletsPath, int Thresh, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        //        {
        //           Trace.WriteLine("MoveOutletsToStreams(d8Path: " + D8Path + ",\n" +
        //                                     "       StreamGridPath: " + StreamGridPath + ",\n" +
        //                                     "       outletsPath: " + OutletsPath + ",\n" +
        //                                     "       MovedOutletsPath: " + MovedOutletsPath + ",\n" +
        //                                     "       threshold: " + Thresh + ",\n" +
        //                                     "       NumProcesses: " + numProcesses.ToString() + "\n" +
        //                                     "       ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
        //                                     "       callback)");
        //            int result = -1;
        //            DataManagement.DeleteShapefile(MovedOutletsPath);

        //            if (callback != null) callback.Progress("Status", 0, "Move Outlets to Streams");

        //            string pars =
        //                "-p " + D8Path +
        //                " -src " + StreamGridPath +
        //                " -o " + OutletsPath +
        //                " -om " + MovedOutletsPath +
        //                " -md " + Thresh.ToString();
        //            result = RunTaudem("MoveOutletsToStreams.exe", pars, numProcesses, showTaudemOutput);
        //            if (result != 0)
        //            {
        //                MessageBox.Show("TauDEM Error " + result, "TauDEM Error " + result, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //                return result;
        //            }

        //            DataManagement.TryCopy(System.IO.Path.ChangeExtension(OutletsPath, ".prj"),
        //                                   System.IO.Path.ChangeExtension(MovedOutletsPath, ".prj"));
        //            if (callback != null) callback.Progress("Status", 0, String.Empty);
        //           Trace.WriteLine("Finished Move Outlets to Streams");
        //            return result;
        //        }
        //        #endregion

        //        #region "Apply Stream Attributes"

        /// <summary>
        ///   Hydrology function used to add to the stream shapefile attributes
        /// </summary>
        /// <param name = "streamNetworkShapePath"></param>
        /// <param name = "demPath"></param>
        /// <param name = "subBasinShapePath"></param>
        /// <param name = "elevUnits"></param>
        /// <param name = "callback"></param>
        /// <returns></returns>
        public static bool ApplyStreamAttributes(
            string streamNetworkShapePath,
            string demPath,
            string subBasinShapePath,
            ElevationUnits elevUnits,
            IProgressHandler callback)
        {
            int sindx;
            int oldperc = 0;
            const int IDField = 0;
            const int DsField = 1;
            const int Us1Field = 2;
            const int Us2Field = 3;
            const int DsAreaField = 8;
            const int SlopeField = 10;
            const int UsAreaField = 12;

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating Stream Parameters");
            }

            if (!File.Exists(streamNetworkShapePath))
            {
                MessageBox.Show(string.Format(@"The file {0} does not exists! ", streamNetworkShapePath));
                return false;
            }

            IFeatureSet streamShape = FeatureSet.Open(streamNetworkShapePath);
            if (streamShape == null)
            {
                throw new ApplicationException(String.Format("Error in opening {0}", streamNetworkShapePath));
            }
            int streamShapeNumShapes = streamShape.NumRows();

            // Add some fields:
            int lowFieldNum = AddField(streamShape, "ElevLow", typeof(double));
            int highFieldNum = AddField(streamShape, "Elevhigh", typeof(double));
            int mwidthFieldNum = AddField(streamShape, "MeanWidth", typeof(double));
            int mdepthFieldNum = AddField(streamShape, "MeanDepth", typeof(double));
            int dsareaAcreFieldNum = AddField(streamShape, "DSAreaAcre", typeof(double));
            int dsareaSqMiFieldNum = AddField(streamShape, "USAreaAcre", typeof(double));
            int usareaAcreFieldNum = AddField(streamShape, "DSAreaSqMi", typeof(double));
            int usareaSqMiFieldNum = AddField(streamShape, "USAreaSqMi", typeof(double));

            DotSpatial.Projections.ProjectionInfo projStr = streamShape.Projection;
            IRaster demGrid = Raster.Open(demPath);
            IFeatureSet shedShape = FeatureSet.Open(subBasinShapePath);
            int shedShapeNumShapes = shedShape.NumRows();

            for (sindx = 0; sindx < streamShapeNumShapes; sindx++)
            {
                if (callback != null && streamShapeNumShapes > 1)
                {
                    int newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShapeNumShapes)) * 100);
                    if (newperc > oldperc)
                    {
                        callback.Progress("Status", newperc, "Calculating Stream Parameters");
                        oldperc = newperc;
                    }
                }

                int OriginalStreamID = Convert.ToInt32(streamShape.get_CellValue(IDField, sindx));

                double elevlow;
                double elevhigh;
                GetStreamElevationPoints(sindx, streamShape, demGrid, out elevlow, out elevhigh);

                switch (elevUnits)
                {
                    case ElevationUnits.centimeters:
                        elevlow /= 100;
                        elevhigh /= 100;
                        break;
                    case ElevationUnits.feet:
                        elevlow /= 3.280839895;
                        elevhigh /= 3.280839895;
                        break;
                }

                streamShape.EditCellValue(lowFieldNum, sindx, elevlow);
                streamShape.EditCellValue(highFieldNum, sindx, elevhigh);

                // Assume the first field in the watershed shape file is an ID field that corresponds to the IDs in streamShape IDField
                // GridToShapeManhattan creates this field for watershed shapes
                const int WShedIDField = 0;
                for (int shdindx = 0; shdindx < shedShapeNumShapes; shdindx++)
                {
                    if (Convert.ToInt32(shedShape.get_CellValue(WShedIDField, shdindx)) == OriginalStreamID)
                    {
                        var currShp = shedShape.get_Shape(shdindx);
                        double currArea = Utils.AreaOfPart(currShp, 0);
                        double meanWidth = 1.29 * Math.Pow(currArea / 1000000, 0.6);
                        double meanDepth = 0.13 * Math.Pow(currArea / 1000000, 0.4);
                        streamShape.EditCellValue(mwidthFieldNum, sindx, meanWidth);
                        streamShape.EditCellValue(mdepthFieldNum, sindx, meanDepth);
                        break;
                    }
                }

                // Change shape ID (and down/upstream IDs) from zero-based to one-based
                streamShape.EditCellValue(IDField, sindx, OriginalStreamID+1);

                int tmpID = Convert.ToInt32(streamShape.get_CellValue(DsField, sindx));
                if (tmpID > -1)
                {
                    tmpID++;
                }
                streamShape.EditCellValue(DsField, sindx, tmpID);

                tmpID = Convert.ToInt32(streamShape.get_CellValue(Us1Field, sindx));
                if (tmpID > 0)
                {
                    tmpID++;
                }
                else
                {
                    tmpID = -1;
                }
                streamShape.EditCellValue(Us1Field, sindx, tmpID);

                tmpID = Convert.ToInt32(streamShape.get_CellValue(Us2Field, sindx));
                if (tmpID > 0)
                {
                    tmpID++;
                }
                else
                {
                    tmpID = -1;
                }
                streamShape.EditCellValue(Us2Field, sindx, tmpID);

                double tmpSlope = Convert.ToDouble(streamShape.get_CellValue(SlopeField, sindx));
                double tmpDsArea = Convert.ToDouble(streamShape.get_CellValue(DsAreaField, sindx));
                double tmpUsArea = Convert.ToDouble(streamShape.get_CellValue(UsAreaField, sindx));

                if (projStr != null)
                {
                    if (projStr.Unit.Name == "Meter")
                    {
                        double dsAreaAcre = tmpDsArea * 0.000247105;
                        double dsAreaSqMi = dsAreaAcre * 0.0015625;
                        double usAreaAcre = tmpUsArea * 0.000247105;
                        double usAreaSqMi = usAreaAcre * 0.0015625;
                        streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dsAreaAcre);
                        streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                        streamShape.EditCellValue(usareaAcreFieldNum, sindx, usAreaAcre);
                        streamShape.EditCellValue(usareaSqMiFieldNum, sindx, usAreaSqMi);
                        switch (elevUnits)
                        {
                            case ElevationUnits.meters:
                                tmpSlope = tmpSlope * 100;
                                break;
                            case ElevationUnits.centimeters:
                                break;
                            case ElevationUnits.feet:
                                tmpSlope = (tmpSlope / 3.280839895) * 100;
                                break;
                        }

                        streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                    }
                    else if (projStr.Unit.Name == "Foot")
                    {
                        double dsAreaAcre = tmpDsArea * 2.2957E-05;
                        double dsAreaSqMi = dsAreaAcre * 0.0015625;
                        double usAreaAcre = tmpUsArea * 2.2957E-05;
                        double usAreaSqMi = usAreaAcre * 0.0015625;
                        streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dsAreaAcre);
                        streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                        streamShape.EditCellValue(usareaAcreFieldNum, sindx, usAreaAcre);
                        streamShape.EditCellValue(usareaSqMiFieldNum, sindx, usAreaSqMi);
                        switch (elevUnits)
                        {
                            case ElevationUnits.meters:
                                tmpSlope = (tmpSlope * 3.280839895) * 100;
                                break;
                            case ElevationUnits.centimeters:
                                tmpSlope = (tmpSlope / 30.48) * 100;
                                break;
                            case ElevationUnits.feet:
                                tmpSlope = tmpSlope * 100;
                                break;
                        }

                        streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                    }
                }
                else
                {
                    double dSAreaAcre = tmpDsArea * 0.000247105;
                    double dsAreaSqMi = dSAreaAcre * 0.0015625;
                    double uSAreaAcre = tmpUsArea * 0.000247105;
                    double uSAreaSqMi = uSAreaAcre * 0.0015625;
                    streamShape.EditCellValue(dsareaAcreFieldNum, sindx, dSAreaAcre);
                    streamShape.EditCellValue(dsareaSqMiFieldNum, sindx, dsAreaSqMi);
                    streamShape.EditCellValue(usareaAcreFieldNum, sindx, uSAreaAcre);
                    streamShape.EditCellValue(usareaSqMiFieldNum, sindx, uSAreaSqMi);
                    switch (elevUnits)
                    {
                        case ElevationUnits.meters:
                            tmpSlope = tmpSlope * 100;
                            break;
                        case ElevationUnits.centimeters:
                            break;
                        case ElevationUnits.feet:
                            tmpSlope = (tmpSlope / 3.280839895) * 100;
                            break;
                    }

                    streamShape.EditCellValue(SlopeField, sindx, tmpSlope);
                }
            }

            shedShape.Close();
            demGrid.Close();

            streamShape.Save();
            streamShape.Close();

            if (callback != null)
            {
                callback.Progress("Status", 0, String.Empty);
            }

            return true;
        }

        /// <summary>
        /// Adds a field to the shapefile
        /// </summary>
        /// <param name="sf">The shapefile</param>
        /// <param name="fieldname">The fieldname</param>
        /// <param name="fieldType">The field type</param>
        /// <param name="width">The width.</param>
        /// <param name="precision">The precision</param>
        /// <returns>
        /// The index of the inserted field
        /// </returns>
        private static int AddField(IFeatureSet sf, string fieldname, Type fieldType, int width, int precision)
        {
            var col = sf.DataTable.Columns.Add(fieldname, fieldType);

            return sf.DataTable.Columns.Count - 1;

            // doesn't look like DotSpatial has width or precision.
            //var newField = new Field { Name = fieldname, Type = fieldType, Width = width };
            //if (precision > -1)
            //{
            //    newField.Precision = precision;
            //}

            //var fieldNum = sf.NumFields;
            //if (!sf.EditInsertField(newField, fieldNum, null))
            //{
            //    throw new Exception("Error in adding field: " + sf.get_ErrorMsg(sf.LastErrorCode));
            //}
        }

        /// <summary>
        /// Overloaded method to add a field to the shapefile
        /// </summary>
        /// <param name="sf">The shapefile</param>
        /// <param name="fieldname">The fieldname.</param>
        /// <param name="fieldType">The field type.</param>
        /// <returns></returns>
        private static int AddField(IFeatureSet sf, string fieldname, Type fieldType)
        {
            return AddField(sf, fieldname, fieldType, -1, -1);
        }

        //        #endregion

        //        #region "Build Joined Basins"
        /// <summary>
        /// A function to create the joined basins from a watershed shapefile that has had the basic apply attributes set on it
        /// </summary>
        /// <param name="subBasinShapePath"></param>
        /// <param name="joinBasinShapeResultPath"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool BuildJoinedBasins(string subBasinShapePath, string joinBasinShapeResultPath, IProgressHandler callback)
        {
            return BuildJoinedBasins(subBasinShapePath, String.Empty, joinBasinShapeResultPath, callback);
        }

        /// <summary>
        /// Overload of BuildJoinedBasins that takes an outlets shape path used for Inlets resolution. If no outlets/inlets path given, it will treat all points as outlets
        /// </summary>
        // 2/11/09 rewritten by Chris George to dramatically improve speed:
        //  (a) use utils.ClipPolygon instead of SpatialOperations.MergeShapes
        //  (b) create a binary tree modeling the drainage pattern of the subbasins
        //  (b) merge "upstream-first" using the drainage tree so that each merge combines abutting polygons
        public static bool BuildJoinedBasins(string subBasinShapePath, string outletsShapePath, string joinBasinShapeResultPath, IProgressHandler callback)
        {
            var shapeIdxList = new ArrayList();
            BinTree drainage;

            IFeatureSet outlets = null;
            if (outletsShapePath != string.Empty)
            {
                outlets = FeatureSet.Open(outletsShapePath);
            }

            var shed = FeatureSet.Open(subBasinShapePath);

            var dsNodeFieldNum = -1;
            var dsShedFieldNum = -1;
            var us1FieldNum = -1;
            var us2FieldNum = -1;

            for (var i = 0; i < shed.DataTable.Columns.Count; i++)
            {
                switch (shed.DataTable.Columns[i].ColumnName.ToUpper())
                {
                    case "DSNODEID":
                        dsNodeFieldNum = i;
                        break;
                    case "DSWSID":
                        dsShedFieldNum = i;
                        break;
                    case "US1WSID":
                        us1FieldNum = i;
                        break;
                    case "US2WSID":
                        us2FieldNum = i;
                        break;
                }
            }

            //DataManagement.DeleteShapefile(joinBasinShapeResultPath);

            var newShed = new FeatureSet();
            newShed.FeatureType = FeatureType.Polygon;
            newShed.Filename = joinBasinShapeResultPath;

            var idfieldnum = AddField(newShed, "MWShapeID", typeof(int));
            var linkfieldnum = AddField(newShed, "LinkIDs", typeof(string));
            var outletfieldnum = AddField(newShed, "OutletID", typeof(int));
            var dswsfieldnum = AddField(newShed, "DSWSID", typeof(int));
            var uswsfieldnum1 = AddField(newShed, "USWSID1", typeof(int));
            var uswsfieldnum2 = AddField(newShed, "USWSID2", typeof(int));
            var reservoirfieldnum = AddField(newShed, "Reservoir", typeof(int));

            var oldperc = 0;
            for (var sindx = 0; sindx < shed.NumRows(); sindx++)
            {
                if (shed.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shed.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Merging Watersheds to Outlets/Inlets");
                        }

                        oldperc = newperc;
                    }
                }

                DSNode dsNodeType = DSNode.Outlet;
                var dsNodeVal = int.Parse(shed.get_CellValue(dsNodeFieldNum, sindx).ToString());

                if (dsNodeVal > -1)
                {
                    // an outlet, inlet, reservoir or point source
                    if (outletsShapePath == string.Empty)
                    {
                        // assume this is an outlet
                        drainage = getDrainageFromSubbasin(shed, outlets, false, true, sindx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                    }
                    else
                    {
                        dsNodeType = getDSNodeType(outlets, dsNodeVal);
                        if ((dsNodeType == DSNode.Outlet) || (dsNodeType == DSNode.Reservoir))
                        {
                            if (isUpstreamOfInlet(shed, outlets, sindx))
                            {
                                drainage = null;
                            }
                            else
                            {
                                drainage = getDrainageFromSubbasin(shed, outlets, true, true, sindx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                            }
                        }
                        else
                        {
                            // ignore inlets and point sources
                            drainage = null;
                        }
                    }

                    shapeIdxList.Clear();
                    if (drainage != null)
                    {
                        char[] sep = { ',' };
                        var idxs = drainage.ToString().Split(sep);
                        for (var i = 0; i < idxs.Length; i++)
                        {
                            shapeIdxList.Add(idxs[i]);
                        }
                    }

                    if (shapeIdxList.Count != 0)
                    {
                        IFeature mergeShape;
                        string strLinks;
                        if (shapeIdxList.Count == 1)
                        {
                            mergeShape = shed.get_Shape(int.Parse(shapeIdxList[0].ToString()));
                            strLinks = shed.get_CellValue(0, int.Parse(shapeIdxList[0].ToString())).ToString();
                        }
                        else
                        {
                            strLinks = shed.get_CellValue(0, int.Parse(shapeIdxList[0].ToString())).ToString();
                            for (int i = 1; i <= shapeIdxList.Count - 1; i++)
                            {
                                strLinks = strLinks + ", " + shed.get_CellValue(0, int.Parse(shapeIdxList[i].ToString()));
                            }

                            DateTime time = DateTime.Now;
                            mergeShape = mergeBasinsByDrainage(shed, drainage);
                            var elapsed = DateTime.Now.Subtract(time);
                            Trace.WriteLine("Made merged watershed of " + shapeIdxList.Count +
                                                      " subbasins in " + elapsed.TotalSeconds.ToString("F1") + " seconds");
                        }

                        // Check merged shape for single part and clockwise
                        if (mergeShape.NumGeometries > 1)
                        {
                            Trace.WriteLine("Merged polygon has " + mergeShape.NumGeometries + " parts");
                        }
                        else
                        {
                            var area = SignedArea(mergeShape);
                            if (area < 0)
                            {
                                Trace.WriteLine("Needed to reverse merged polygon");
                                mergeShape = ReverseShape(mergeShape);
                            }
                        }

                        var currshpidx = newShed.NumRows();
                        newShed.AddFeature(mergeShape);
                        newShed.EditCellValue(idfieldnum, currshpidx, currshpidx);
                        newShed.EditCellValue(linkfieldnum, currshpidx, strLinks);
                        newShed.EditCellValue(outletfieldnum, currshpidx, dsNodeVal);
                        if (int.Parse(shed.get_CellValue(dsShedFieldNum, sindx).ToString()) != -1)
                        {
                            newShed.EditCellValue(dswsfieldnum, currshpidx, shed.get_CellValue(dsShedFieldNum, sindx));
                        }
                        else
                        {
                            newShed.EditCellValue(dswsfieldnum, currshpidx, -1);
                        }

                        newShed.EditCellValue(uswsfieldnum1, currshpidx, -1);
                        newShed.EditCellValue(uswsfieldnum2, currshpidx, -1);
                        if (dsNodeType == DSNode.Reservoir)
                        {
                            newShed.EditCellValue(reservoirfieldnum, currshpidx, 1);
                        }
                        else
                        {
                            newShed.EditCellValue(reservoirfieldnum, currshpidx, 0);
                        }
                    }
                }
            }

            buildMergeDownstreamUpStream(newShed, idfieldnum, linkfieldnum, dswsfieldnum, uswsfieldnum1, uswsfieldnum2);

            newShed.Projection = shed.Projection;

            shed.Close();

            newShed.Save();
            newShed.Close();

            if (outletsShapePath != string.Empty)
            {
                outlets.Close();
            }

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        private static void buildMergeDownstreamUpStream(IFeatureSet newshed, int IDFieldNum, int LinksFieldNum, int DSFieldNum, int USFieldNum1, int USFieldNum2)
        {
            for (int i = 0; i <= newshed.NumRows() - 1; i++)
            {
                string currDSField = newshed.get_CellValue(DSFieldNum, i).ToString();
                if (currDSField != "-1")
                {
                    for (int j = 0; j <= newshed.NumRows() - 1; j++)
                    {
                        string links = newshed.get_CellValue(LinksFieldNum, j).ToString();
                        string[] split = links.Split(',');
                        for (int k = 0; k <= split.Length - 1; k++)
                        {
                            if (split[k].Trim() == currDSField)
                            {
                                newshed.EditCellValue(DSFieldNum, i, newshed.get_CellValue(IDFieldNum, j));
                                string upstream1 = newshed.get_CellValue(USFieldNum1, j).ToString();
                                if (upstream1 == "-1")
                                {
                                    newshed.EditCellValue(USFieldNum1, j, newshed.get_CellValue(IDFieldNum, i));
                                }
                                else
                                {
                                    newshed.EditCellValue(USFieldNum2, j, newshed.get_CellValue(IDFieldNum, i));
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get type of inlet/outlet for MWSHAPEID equal to ID in outlets shapefile
        /// </summary>
        /// <param name="outlets"></param>
        /// <param name="id"></param>
        /// <returns>0 for outlet, 1 for inlet, 2 for reservoir, 3 for point source</returns>
        private static DSNode getDSNodeType(IFeatureSet outlets, int id)
        {
            string idFieldColumnName = null;

            int inletFieldNum = -1;
            int resFieldNum = -1;
            int srcFieldNum = -1;

            //search for 4 particular columns
            for (int i = 0; i < outlets.DataTable.Columns.Count; i++)
            {
                string currname = outlets.DataTable.Columns[i].ColumnName.ToUpper();
                if (currname == "MWSHAPEID" || currname == "ID")
                {
                    idFieldColumnName = currname;
                }
                else if (currname == "INLET")
                {
                    inletFieldNum = i;
                }
                else if (currname == "RES")
                {
                    resFieldNum = i;
                }
                else if (currname == "PTSOURCE")
                {
                    srcFieldNum = i;
                }
            }

            int currInlet = 0;
            int currRes = 0;
            int currSrc = 0;

            // The id may represent the row number
            DataRow theRow = outlets.DataTable.Rows[id - 1];

            if (idFieldColumnName != null)
            {
                // The id represents a primary key an may not correspond directly to the row number.
                var query = from row in outlets.DataTable.AsEnumerable()
                            where row.Field<int>(idFieldColumnName) == id
                            select row;

                theRow = query.First();
            }

            currInlet = (inletFieldNum == -1) ? 0 : (int)theRow[inletFieldNum];
            currRes = (resFieldNum == -1) ? 0 : (int)theRow[resFieldNum];
            currSrc = (srcFieldNum == -1) ? 0 : (int)theRow[srcFieldNum];

            if (currInlet == 1)
            {
                if (currSrc == 1)
                    return DSNode.PointSource;

                return DSNode.Inlet;
            }
            else if (currRes == 1)
            {
                return DSNode.Reservoir;
            }
            else
            {
                return DSNode.Outlet;
            }
        }

        private static bool isUpstreamOfInlet(IFeatureSet shed, IFeatureSet outlets, int sindx)
        {
            const int dsNodeFieldNum = 3;
            const int dsShedFieldNum = 4;
            int currDSID = int.Parse(shed.DataTable.Rows[sindx].ItemArray[dsShedFieldNum].ToString());

            while (currDSID != -1)
            {
                for (int i = 0; i <= shed.DataTable.Rows.Count - 1; i++)
                {
                    if (int.Parse(shed.DataTable.Rows[i].ItemArray[0].ToString()) == currDSID)
                    {
                        currDSID = int.Parse(shed.DataTable.Rows[i].ItemArray[dsShedFieldNum].ToString());

                        int dsNodeVal = int.Parse(shed.DataTable.Rows[i].ItemArray[dsNodeFieldNum].ToString());
                        if (dsNodeVal != -1)
                        {
                            if (getDSNodeType(outlets, dsNodeVal) == DSNode.Inlet)
                            {
                                return true;
                            }
                        }

                        break;
                    }
                }
            }
            return false;
        }

        private static void getJoinShapesFromSubBasin(Shapefile shed, int sindx, ArrayList shapeIdxList)
        {
            int dsNodeFieldNum = 3;
            int USlink1FieldNum = 5;
            int USlink2FieldNum = 6;
            Stack currStack = new Stack();
            int currLink1ID;
            int currLink2ID;
            int currIdx;
            int currLinkIdx;
            shapeIdxList.Clear();

            shapeIdxList.Add(sindx);
            currLink1ID = int.Parse(shed.get_CellValue(USlink1FieldNum, sindx).ToString());
            currLink2ID = int.Parse(shed.get_CellValue(USlink2FieldNum, sindx).ToString());
            if (currLink1ID != -1)
            {
                currLinkIdx = GetBasinIndexByID(shed, currLink1ID);
                if (currLinkIdx != -1)
                {
                    currStack.Push(currLinkIdx);
                }
            }
            if (currLink2ID != -1)
            {
                currLinkIdx = GetBasinIndexByID(shed, currLink2ID);
                if (currLinkIdx != -1)
                {
                    currStack.Push(currLinkIdx);
                }
            }
            while (currStack.Count > 0)
            {
                currIdx = int.Parse(currStack.Pop().ToString());
                if (int.Parse(shed.get_CellValue(dsNodeFieldNum, currIdx).ToString()) == -1)
                {
                    shapeIdxList.Add(currIdx);
                    currLink1ID = int.Parse(shed.get_CellValue(USlink1FieldNum, currIdx).ToString());
                    currLink2ID = int.Parse(shed.get_CellValue(USlink2FieldNum, currIdx).ToString());
                    if (currLink1ID != -1)
                    {
                        currLinkIdx = GetBasinIndexByID(shed, currLink1ID);
                        if (currLinkIdx != -1)
                        {
                            currStack.Push(currLinkIdx);
                        }
                    }
                    if (currLink2ID != -1)
                    {
                        currLinkIdx = GetBasinIndexByID(shed, currLink2ID);
                        if (currLinkIdx != -1)
                        {
                            currStack.Push(currLinkIdx);
                        }
                    }
                }
            }
        }

        private static int GetBasinIndexByID(IFeatureSet shed, int id)
        {
            for (var i = 0; i < shed.NumRows(); i++)
            {
                if (int.Parse(shed.DataTable.Rows[i].ItemArray[0].ToString()) == id)
                {
                    return i;
                }
            }

            return -1;
        }

        //        #endregion

        //        #region "Apply Joined Basin Attributes"
        /// <summary>
        /// A function to apply attributes to a joined basin shapefile
        /// </summary>
        /// <param name="JoinBasinShapePath"></param>
        /// <param name="elevUnits"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ApplyJoinBasinAreaAttributes(string JoinBasinShapePath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            if (callback != null) callback.Progress("Status", 0, "Calculating Merge Shed Area Attributes");
            double currarea;
            double aream;
            double areasqmi;
            double areaacre;

            int oldperc = 0;
            int newperc = 0;
            var mergeshed = FeatureSet.Open(JoinBasinShapePath);

            int areamfieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_M", typeof(double));

            int areaacfieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_Acre", typeof(double));

            int areasqmifieldnum = mergeshed.DataTable.Columns.Count;
            mergeshed.DataTable.Columns.Add("Area_SqMi", typeof(double));

            var projection = mergeshed.Projection;
            for (int i = 0; i <= mergeshed.NumRows() - 1; i++)
            {
                if (mergeshed.NumRows() > 1)
                {
                    newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(mergeshed.NumRows())) * 100);
                    if ((newperc > oldperc))
                    {
                        if (callback != null) callback.Progress("Status", newperc, "Calculating Merge Shed Area Attributes");
                        oldperc = newperc;
                    }
                }

                IFeature currShape = mergeshed.get_Shape(i);
                currarea = Utils.AreaOfPart(currShape, 0);
                aream = 0;
                if (projection != null)
                {
                    if ((projection.Unit.Name == "Meter"))
                    {
                        aream = currarea;
                    }
                    else if ((projection.Unit.Name == "Foot"))
                    {
                        aream = currarea * 0.09290304;
                    }
                }
                else
                {
                    aream = currarea;
                }
                areaacre = aream * 0.000247105;
                areasqmi = areaacre * 0.0015625;
                mergeshed.EditCellValue(areamfieldnum, i, aream);
                mergeshed.EditCellValue(areaacfieldnum, i, areaacre);
                mergeshed.EditCellValue(areasqmifieldnum, i, areasqmi);
            }
            mergeshed.Save();
            mergeshed.Close();

            if (callback != null) callback.Progress(String.Empty, 0, String.Empty);
            return true;
        }

        /// <summary>
        /// A function that gets the mean width, mean height, length, and slope of the primary stream in the basin
        /// </summary>
        /// <param name="networkShapePath">The path to the streams network shapefile</param>
        /// <param name="basinShapePath">The path to the unjoined watershed shapefile</param>
        /// <param name="joinBasinShapePath">The path to the Joined Basins shapefile</param>
        /// <param name="callback">A mapwindow callback</param>
        /// <returns></returns>
        public static bool ApplyJoinBasinStreamAttributes(string networkShapePath, string basinShapePath, string joinBasinShapePath, IProgressHandler callback)
        {
            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating Merge Shed Area Attributes");
            }

            var shedShapefile = FeatureSet.Open(basinShapePath);

            var streamLinkFieldNum = -1;
            for (var i = 0; i < shedShapefile.DataTable.Columns.Count; i++)
            {
                if (shedShapefile.DataTable.Columns[i].ColumnName == "StreamLink")
                {
                    streamLinkFieldNum = i;
                    break;
                }
            }

            var netShapefile = FeatureSet.Open(networkShapePath);

            var streamIdfn = -1;
            var streamLengthFn = -1;
            var streamSlopeFn = -1;
            var streamMeanWidthFn = -1;
            var streamMeanDepthFn = -1;
            for (var i = 0; i < netShapefile.DataTable.Columns.Count; i++)
            {
                switch (netShapefile.DataTable.Columns[i].ColumnName)
                {
                    case "LINKNO":
                        streamIdfn = i;
                        break;
                    case "Length":
                        streamLengthFn = i;
                        break;
                    case "Slope":
                        streamSlopeFn = i;
                        break;
                    case "MeanWidth":
                        streamMeanWidthFn = i;
                        break;
                    case "MeanDepth":
                        streamMeanDepthFn = i;
                        break;
                }
            }

            var mergeshedShapefile = FeatureSet.Open(joinBasinShapePath);

            var linkIDsFieldNum = -1;
            for (var i = 0; i < mergeshedShapefile.DataTable.Columns.Count; i++)
            {
                if (mergeshedShapefile.DataTable.Columns[i].ColumnName == "LinkIDs")
                {
                    linkIDsFieldNum = i;
                    break;
                }
            }

            //mergeshedShapefile.StartEditingShapes(true, null);
            var streamwidthfieldnum = AddField(mergeshedShapefile, "CH_W2", typeof(double));
            var streamdepthfieldnum = AddField(mergeshedShapefile, "CH_D", typeof(double));
            var streamlengthfieldnum = AddField(mergeshedShapefile, "CH_L", typeof(double));
            var streamslopefieldnum = AddField(mergeshedShapefile, "CH_S", typeof(double));

            var oldperc = 0;
            for (var i = 0; i < mergeshedShapefile.NumRows(); i++)
            {
                if (mergeshedShapefile.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(mergeshedShapefile.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating Merge Shed Area Attributes");
                        }

                        oldperc = newperc;
                    }
                }

                var currLinkIDs = mergeshedShapefile.get_CellValue(linkIDsFieldNum, i).ToString();
                var links = currLinkIDs.Split(',');
                var shedID = int.Parse(links[0]);
                var shedIndex = GetBasinIndexByID(shedShapefile, shedID);
                var streamLink = shedShapefile.get_CellValue(streamLinkFieldNum, shedIndex).ToString();

                for (var j = 0; j < netShapefile.NumRows(); j++)
                {
                    if (netShapefile.get_CellValue(streamIdfn, j).ToString() != streamLink)
                    {
                        continue;
                    }

                    mergeshedShapefile.EditCellValue(streamlengthfieldnum, i, netShapefile.get_CellValue(streamLengthFn, j));
                    mergeshedShapefile.EditCellValue(streamslopefieldnum, i, netShapefile.get_CellValue(streamSlopeFn, j));
                    mergeshedShapefile.EditCellValue(streamdepthfieldnum, i, netShapefile.get_CellValue(streamMeanDepthFn, j));
                    mergeshedShapefile.EditCellValue(streamwidthfieldnum, i, netShapefile.get_CellValue(streamMeanWidthFn, j));
                }
            }

            mergeshedShapefile.Save();
            mergeshedShapefile.Close();

            netShapefile.Close();
            shedShapefile.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average elevation attribute
        /// </summary>
        /// <param name="SubBasinShapePath"></param>
        /// <param name="ElevGridPath"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ApplyWatershedElevationAttribute(string SubBasinShapePath, string ElevGridPath, IProgressHandler callback)
        {
            int sindx;
            int col, row;
            int[] countElev;
            double[] sumElev;
            double[] avgElev;

            int newperc = 0, oldperc = 0;
            IFeature tmpPoly;
            // CWG 23/1/2011 changed to GeoTiff for Taudem V5
            string tmpClipPath = Path.GetDirectoryName(ElevGridPath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(ElevGridPath) + "_clip.tif";

            double currVal, nodataVal;
            int nr, nc;
            if (callback != null) callback.Progress("Status", 0, "Calculating WS Elevation Parameters");
            var shedShape = FeatureSet.Open(SubBasinShapePath);
            var elevGrid = Raster.Open(ElevGridPath);

            countElev = new int[shedShape.NumRows()];
            sumElev = new double[shedShape.NumRows()];
            avgElev = new double[shedShape.NumRows()];

            DataManagement.DeleteGrid(tmpClipPath);
            for (sindx = 0; sindx <= shedShape.NumRows() - 1; sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows() - 1)) * 100);
                    if ((newperc > oldperc))
                    {
                        if (callback != null) callback.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        oldperc = newperc;
                    }
                }

                tmpPoly = shedShape.get_Shape(sindx);
                IRaster tmpClipGrid = ClipRaster.ClipRasterWithPolygon(ElevGridPath, tmpPoly, tmpClipPath);
                if (tmpClipGrid != null)
                {
                    nr = tmpClipGrid.NumRows;
                    nc = tmpClipGrid.NumColumns;
                    nodataVal = tmpClipGrid.NoDataValue;
                    countElev[sindx] = 0;
                    sumElev[sindx] = 0;
                    avgElev[sindx] = 0;
                    for (row = 0; row <= nr - 1; row += 2)
                    {
                        for (col = 0; col <= nc - 1; col += 2)
                        {
                            currVal = tmpClipGrid.Value[row, col];
                            if (currVal != nodataVal)
                            {
                                countElev[sindx] = countElev[sindx] + 1;
                                sumElev[sindx] = sumElev[sindx] + currVal;
                            }
                        }
                    }
                    tmpClipGrid.Close();
                    DataManagement.DeleteGrid(tmpClipPath);
                }
            }
            if (callback != null) callback.Progress("Status", 0, "Calculating WS Elevation Parameters");

            int slopeFieldNum = AddField(shedShape, "AveElev", typeof(double));

            string slopeProj = elevGrid.Projection.ToString();
            newperc = 0;
            oldperc = 0;
            for (sindx = 0; sindx <= shedShape.NumRows() - 1; sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if ((newperc > oldperc))
                    {
                        if (callback != null) callback.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        oldperc = newperc;
                    }
                }
                if (countElev[sindx] > 0)
                {
                    avgElev[sindx] = (sumElev[sindx] / countElev[sindx]);

                    shedShape.EditCellValue(slopeFieldNum, sindx, avgElev[sindx]);
                }
            }

            shedShape.Save();
            shedShape.Close();
            elevGrid.Close();

            if (callback != null) callback.Progress(String.Empty, 0, String.Empty);
            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average elevation attribute
        /// </summary>
        /// <param name="subBasinGridPath"></param>
        /// <param name="subBasinShapePath"></param>
        /// <param name="elevGridPath"></param>
        /// <param name="callback"></param>
        /// <returns>True on success</returns>
        public static bool ApplyWatershedElevationAttribute(string subBasinGridPath, string subBasinShapePath, string elevGridPath, IProgressHandler callback)
        {
            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Elevation Parameters");
            }

            var shedShape = FeatureSet.Open(subBasinShapePath);
            var countElev = new int[shedShape.NumRows()];
            var sumElev = new double[shedShape.NumRows()];
            var avgElev = new double[shedShape.NumRows()];

            var currPolyIDIdx = -1;
            var currLinkIDIdx = -1;
            for (var sindx = 0; sindx < shedShape.DataTable.Columns.Count; sindx++)
            {
                if (shedShape.DataTable.Columns[sindx].ColumnName == "PolygonID" || shedShape.DataTable.Columns[sindx].ColumnName == "MWShapeID")
                {
                    currPolyIDIdx = sindx;
                }

                if (shedShape.DataTable.Columns[sindx].ColumnName == "LinkIDs")
                {
                    currLinkIDIdx = sindx;
                }

                // Paul Meems, 24-Aug-2011: Added:
                if (currPolyIDIdx != -1 && currLinkIDIdx != -1)
                {
                    // Found the values so stop searching:
                    break;
                }
            }

            var linkIDVals = new List<int>();
            var linkIDMerged = new List<int>();
            if (currLinkIDIdx != -1 && currPolyIDIdx != -1)
            {
                for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    var tmpLinkIDs = shedShape.get_CellValue(currLinkIDIdx, sindx).ToString();
                    var tmpLinks = tmpLinkIDs.Split(',');
                    foreach (var tmpLink in tmpLinks)
                    {
                        linkIDMerged.Add(sindx);
                        linkIDVals.Add(int.Parse(tmpLink.Trim()));
                    }
                }
            }
            else
            {
                for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    linkIDMerged.Add(sindx);
                    linkIDVals.Add(int.Parse(shedShape.get_CellValue(currPolyIDIdx, sindx).ToString()));
                }
            }

            var subBasinGrid = Raster.Open(subBasinGridPath);

            var elevGrid = Raster.Open(elevGridPath);
            var nodataVal = elevGrid.NoDataValue;
            var numberRows = elevGrid.NumRows;
            var numberCols = elevGrid.NumColumns;

            var oldperc = -1;
            for (var row = 0; row < numberRows; row++)
            {
                var newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(numberRows - 1)) * 100);
                if (newperc > oldperc)
                {
                    if (callback != null)
                    {
                        callback.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                    }

                    oldperc = newperc;
                }

                for (var col = 0; col < numberCols; col++)
                {
                    var currVal = double.Parse(elevGrid.Rows[row][col].ToString());

                    if (currVal == nodataVal)
                    {
                        continue;
                    }

                    var currBasinID = int.Parse(subBasinGrid.Rows[row][col].ToString());

                    // Paul Meems, 24-Aug-2011: Changed:
                    // if (currBasinID != -1)
                    // TODO: Check if the result is still the same
                    if (currBasinID > -1)
                    {
                        var tmp = linkIDVals.IndexOf(currBasinID);
                        if (tmp > -1)
                        {
                            var currID = linkIDMerged[tmp];
                            countElev[currID] = countElev[currID] + 1;
                            sumElev[currID] = sumElev[currID] + currVal;
                        }
                    }
                }
            }

            elevGrid.Close();

            subBasinGrid.Close();

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Elevation Parameters");
            }

            var slopeFieldNum = AddField(shedShape, "AveElev", typeof(double));

            oldperc = 0;
            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating WS Elevation Parameters");
                        }

                        oldperc = newperc;
                    }
                }

                if (countElev[sindx] <= 0)
                {
                    continue;
                }

                avgElev[sindx] = sumElev[sindx] / countElev[sindx];

                shedShape.EditCellValue(slopeFieldNum, sindx, avgElev[sindx]);
            }

            shedShape.Save();
            shedShape.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        //        #endregion

        //        #region "Create SWAT *.Fig"
        //        /// <summary>
        //        /// A function to generate a *.fig file from joined basins for use in SWAT.
        //        /// </summary>
        //        /// <param name="JoinBasinShapePath"></param>
        //        /// <param name="ResultFigPath"></param>
        //        /// <param name="callback"></param>
        //        /// <returns></returns>
        //        public static bool CreateSWATFig(string JoinBasinShapePath, string ResultFigPath, IProgressHandler callback)
        //        {
        //            int OutletIDFieldNum = -1, DSWSIDFieldNum = -1, USWSIDFieldNum1 = -1, USWSIDFieldNum2 = -1, ReservoirFieldNum = -1;
        //            if (callback != null) callback.Progress("Status", 0, "Create SWAT *.fig");
        //            Shapefile sf = new Shapefile();

        //            sf.Open(JoinBasinShapePath, null);
        //            for (int i = 0; i < sf.NumFields; i++)
        //            {
        //                switch (sf.DataTable.Columns[i].ColumnName)
        //                {
        //                    case "OutletID":
        //                        OutletIDFieldNum = i;
        //                        break;
        //                    case "DSWSID":
        //                        DSWSIDFieldNum = i;
        //                        break;
        //                    case "USWSID1":
        //                        USWSIDFieldNum1 = i;
        //                        break;
        //                    case "USWSID2":
        //                        USWSIDFieldNum2 = i;
        //                        break;
        //                    case "Reservoir":
        //                        ReservoirFieldNum = i;
        //                        break;
        //                }
        //            }
        //            if (OutletIDFieldNum == -1 || DSWSIDFieldNum == -1 || USWSIDFieldNum1 == -1 || USWSIDFieldNum2 == -1)
        //            {
        //                return false;
        //            }

        //            StreamWriter fig = new StreamWriter(ResultFigPath);

        //            Stack<int> substack = new Stack<int>();
        //            List<int> subIDs = new List<int>();
        //            int Hyd_Stor_Num = 0;
        //            int Res_Num = 0;
        //            int InFlow_Num1 = 0;
        //            int InFlow_Num2 = 0;
        //            int InFlow_ID = 0;
        //            int UpstreamCount, UpstreamFinishedCount;

        //            //Write subbasins
        //            for (int i = 0; i < sf.NumRows(); i++)
        //            {
        //                Hyd_Stor_Num++;
        //                fig.Write("subbasin       1{0,6:G6}{0,6:G6}                              Subbasin: {0:G}\n          {0,5:D5}0000.sub\n", Hyd_Stor_Num);
        //                if (sf.get_CellValue(DSWSIDFieldNum, i).ToString() == "-1")
        //                {
        //                    substack.Push(i);
        //                }
        //                subIDs.Add(-1);
        //            }

        //            //Write the rest
        //            int curridx;
        //            string currUS1, currUS2;
        //            int currUS1idx, currUS2idx, currUS1ID, currUS2ID;
        //            while (substack.Count > 0)
        //            {
        //                curridx = substack.Pop();

        //                currUS1 = sf.get_CellValue(USWSIDFieldNum1, curridx).ToString();
        //                currUS2 = sf.get_CellValue(USWSIDFieldNum2, curridx).ToString();
        //                if (currUS1 == "-1" && currUS2 == "-1") //then we're on an outer reach.
        //                {
        //                    if (subIDs[curridx] == -1) //then it hasn't been added yet. add a route
        //                    {
        //                        Hyd_Stor_Num++;
        //                        InFlow_Num1 = curridx + 1;
        //                        fig.Write("route          2{0,6:G6}{1,6:G6}{2,6:G6}\n          {1,5:D5}0000.rte{1,5:D5}0000.swq\n", Hyd_Stor_Num, curridx + 1, InFlow_Num1);
        //                        subIDs[curridx] = Hyd_Stor_Num;

        //                        if (sf.get_CellValue(ReservoirFieldNum, curridx).ToString() == "1") //it's a reservoir
        //                        {
        //                            Hyd_Stor_Num++;
        //                            Res_Num++;
        //                            InFlow_Num1 = Hyd_Stor_Num - 1;
        //                            InFlow_ID = curridx + 1;
        //                            fig.Write("routres        3{0,6:G6}{1,6:G6}{2,6:G6}{3,6:G6}\n          {3,5:D5}0000.res{3,5:D5}0000.lwq\n", Hyd_Stor_Num, Res_Num, InFlow_Num1, InFlow_ID);
        //                            subIDs[curridx] = Hyd_Stor_Num;
        //                        }
        //                    }
        //                }
        //                else //we're on a middle or final reach
        //                {
        //                    UpstreamCount = 0;
        //                    UpstreamFinishedCount = 0;

        //                    //Get the hydro IDs and indexes of the upstream links
        //                    currUS1ID = -2;
        //                    currUS2ID = -2;
        //                    currUS1idx = -1;
        //                    currUS2idx = -1;
        //                    if (currUS1 != "-1")
        //                    {
        //                        UpstreamCount++;
        //                        currUS1idx = GetBasinIndexByID(sf, Int32.Parse(currUS1));
        //                        if (currUS1idx >= 0)
        //                        {
        //                            currUS1ID = subIDs[currUS1idx];
        //                            if (currUS1ID != -1)
        //                            {
        //                                UpstreamFinishedCount++;
        //                            }
        //                        }
        //                    }
        //                    if (currUS2 != "-1")
        //                    {
        //                        UpstreamCount++;
        //                        currUS2idx = GetBasinIndexByID(sf, Int32.Parse(currUS2));
        //                        if (currUS2idx >= 0)
        //                        {
        //                            currUS2ID = subIDs[currUS2idx];
        //                            if (currUS2ID != -1)
        //                            {
        //                                UpstreamFinishedCount++;
        //                            }
        //                        }
        //                    }

        //                    if (UpstreamCount == UpstreamFinishedCount) //all upstreams finished
        //                    {
        //                        if (currUS1ID != -2 && currUS2ID != -2) //It has two upstream, have to do a double sum
        //                        {
        //                            Hyd_Stor_Num++;
        //                            InFlow_Num1 = currUS1ID;
        //                            InFlow_Num2 = curridx + 1;
        //                            fig.Write("add            5{0,6:G6}{1,6:G6}{2,6:G6}\n", Hyd_Stor_Num, InFlow_Num1, InFlow_Num2);

        //                            Hyd_Stor_Num++;
        //                            InFlow_Num1 = currUS2ID;
        //                            InFlow_Num2 = Hyd_Stor_Num - 1;
        //                            fig.Write("add            5{0,6:G6}{1,6:G6}{2,6:G6}\n", Hyd_Stor_Num, InFlow_Num1, InFlow_Num2);
        //                        }
        //                        else if (currUS1ID != -2) //It only has one upstream, check if it's 1
        //                        {
        //                            Hyd_Stor_Num++;
        //                            InFlow_Num1 = currUS1ID;
        //                            InFlow_Num2 = curridx + 1;
        //                            fig.Write("add            5{0,6:G6}{1,6:G6}{2,6:G6}\n", Hyd_Stor_Num, InFlow_Num1, InFlow_Num2);
        //                        }
        //                        else if (currUS2ID != -2) //It only has one upstream, check if it's 2
        //                        {
        //                            Hyd_Stor_Num++;
        //                            InFlow_Num1 = currUS2ID;
        //                            InFlow_Num2 = curridx + 1;
        //                            fig.Write("add            5{0,6:G6}{1,6:G6}{2,6:G6}\n", Hyd_Stor_Num, InFlow_Num1, InFlow_Num2);
        //                        }

        //                        //After summing, create the route and possibly reservoir
        //                        Hyd_Stor_Num++;
        //                        InFlow_Num1 = Hyd_Stor_Num - 1;
        //                        fig.Write("route          2{0,6:G6}{1,6:G6}{2,6:G6}\n          {1,5:D5}0000.rte{1,5:D5}0000.swq\n", Hyd_Stor_Num, curridx + 1, InFlow_Num1);
        //                        subIDs[curridx] = Hyd_Stor_Num;

        //                        if (sf.get_CellValue(ReservoirFieldNum, curridx).ToString() == "1")
        //                        {
        //                            Hyd_Stor_Num++;
        //                            Res_Num++;
        //                            InFlow_Num1 = Hyd_Stor_Num - 1;
        //                            InFlow_ID = curridx + 1;
        //                            fig.Write("routres        3{0,6:G6}{1,6:G6}{2,6:G6}{3,6:G6}\n          {3,5:D5}0000.res{3,5:D5}0000.lwq\n", Hyd_Stor_Num, Res_Num, InFlow_Num1, InFlow_ID);
        //                            subIDs[curridx] = Hyd_Stor_Num;
        //                        }
        //                    }
        //                    else //There are upstream items that need to still be processed before this one
        //                    {
        //                        substack.Push(curridx);
        //                        if (currUS1idx != -1 && currUS1ID == -1)
        //                        {
        //                            substack.Push(currUS1idx);
        //                        }
        //                        if (currUS2idx != -1 && currUS2ID == -1)
        //                        {
        //                            substack.Push(currUS2idx);
        //                        }
        //                    }
        //                }

        //            }

        //            //Write out the saveconc and finish commands
        //            int SaveFile_Num = 1;
        //            int Print_Freq = 0; //0 for daily, 1 for hourly
        //            fig.Write("saveconc      14{0,6:G6}{1,6:G6}{2,6:G6}\n          watout.dat\n", Hyd_Stor_Num, SaveFile_Num, Print_Freq);
        //            fig.WriteLine("finish         0");

        //            fig.Close();
        //            sf.Close();
        //            return true;
        //        }
        //        #endregion

        //        #region "Hydrology Private Helper Functions"

        private static void GetStreamElevationPoints(int sindx, IFeatureSet streamShape, IRaster demGrid, out double elevLow, out double elevHigh)
        {
            var shapePoints = streamShape.get_Shape(sindx).NumPoints;
            elevLow = 10000000;
            elevHigh = -1000000;
            for (var i = 0; i < shapePoints; i += 2)
            {
                var pt = streamShape.get_Shape(sindx).Coordinates[i];

                RcIndex position = demGrid.ProjToCell(pt.X, pt.Y);
                if (position.IsEmpty()) continue;

                double currVal = demGrid.Value[position.Row, position.Column];
                if (currVal < elevLow)
                {
                    elevLow = currVal;
                }

                if (currVal > elevHigh)
                {
                    elevHigh = currVal;
                }
            }
        }

        //private static int GetWshedFromStreamLink(int streamLink, IFeatureSet streamShape, IFeatureSet shedShape)
        //{
        //    int streamindx;
        //    const int LinkIDField = 0;
        //    const int WaterShedIDField = 13;
        //    for (streamindx = 0; streamindx <= streamShape.NumRows() - 1; streamindx++)
        //    {
        //        if (int.Parse(streamShape.get_CellValue(LinkIDField, streamindx).ToString()) == streamLink)
        //        {
        //            return int.Parse(streamShape.get_CellValue(WaterShedIDField, streamindx).ToString());
        //        }
        //    }

        //    return -1;
        //}

        //        private static bool ShapefileToArrays(string shpfileName, double[] X, double[] Y, int PntCount)
        //        {
        //            Shapefile shpfile = new Shapefile();
        //            int numShapes;
        //            int NumPoints;
        //            System.Collections.ArrayList xPnts = new System.Collections.ArrayList();
        //            System.Collections.ArrayList yPnts = new System.Collections.ArrayList();
        //            MapWinGIS.ShpfileType sft;
        //            bool result = false;
        //            if (shpfile.Open(shpfileName, null) == false)
        //            {
        //                return false;
        //            }
        //            sft = shpfile.ShapefileType;
        //            if (sft != MapWinGIS.ShpfileType.SHP_MULTIPOINT & sft != MapWinGIS.ShpfileType.SHP_POINT & sft != MapWinGIS.ShpfileType.SHP_POINTZ & sft != MapWinGIS.ShpfileType.SHP_POINTZ & sft != MapWinGIS.ShpfileType.SHP_POINTM)
        //            {
        //                MessageBox.Show("Error: Invalid shapefile type selected. Must be a point shapefile", "Argument Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //                return false;
        //            }
        //            numShapes = shpfile.NumRows();
        //            IFeature shp;
        //            MapWinGIS.Point pnt;
        //            for (int curShp = 0; curShp <= numShapes - 1; curShp++)
        //            {
        //                shp = shpfile.get_Shape(curShp);
        //                NumPoints = shp.numPoints;
        //                for (int curPnt = 0; curPnt <= NumPoints - 1; curPnt++)
        //                {
        //                    pnt = shp.get_Point(curPnt);
        //                    xPnts.Add(pnt.x);
        //                    yPnts.Add(pnt.y);
        //                }
        //            }
        //            NumPoints = xPnts.Count;
        //            PntCount = NumPoints;
        //            if (NumPoints <= 0)
        //            {
        //                return false;
        //            }
        //            //This is what it used to do. I have NO clue why. ARA
        //            //X = new double[NumPoints-1];
        //            //Y = new double[NumPoints-1];
        //            //for (int curPnt = 1; curPnt < NumPoints; curPnt++)
        //            //{
        //            //    X[curPnt - 1] = System.Convert.ToDouble(xPnts[curPnt]);
        //            //    Y[curPnt - 1] = System.Convert.ToDouble(yPnts[curPnt]);
        //            //}
        //            X = new double[NumPoints];
        //            Y = new double[NumPoints];
        //            for (int curPnt = 0; curPnt < NumPoints; curPnt++)
        //            {
        //                X[curPnt] = System.Convert.ToDouble(xPnts[curPnt]);
        //                Y[curPnt] = System.Convert.ToDouble(yPnts[curPnt]);
        //            }
        //            result = true;
        //            return result;
        //        }

        //        private static bool ShapefileToArraysWithIDs(string shpfileName, double[] X, double[] Y, int PntCount, int[] idNodes)
        //        {
        //            Shapefile shpfile = new Shapefile();
        //            int numShapes;
        //            int NumPoints;
        //            int numFields;
        //            System.Collections.ArrayList xPnts = new System.Collections.ArrayList();
        //            System.Collections.ArrayList yPnts = new System.Collections.ArrayList();
        //            System.Collections.ArrayList idNodesLocal = new System.Collections.ArrayList();
        //            MapWinGIS.ShpfileType sft;

        //            if (shpfile.Open(shpfileName, null) == false)
        //            {
        //                return false;
        //            }
        //            sft = shpfile.ShapefileType;
        //            if (sft != MapWinGIS.ShpfileType.SHP_MULTIPOINT & sft != MapWinGIS.ShpfileType.SHP_POINT & sft != MapWinGIS.ShpfileType.SHP_POINTZ & sft != MapWinGIS.ShpfileType.SHP_POINTZ & sft != MapWinGIS.ShpfileType.SHP_POINTM)
        //            {
        //                MessageBox.Show("Error: Invalid shapefile type selected. Must be a point shapefile", "Argument Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
        //                return false;
        //            }
        //            numShapes = shpfile.NumRows();
        //            IFeature shp;
        //            MapWinGIS.Point pnt;
        //            MapWinGIS.Field Field;
        //            string FieldName;
        //            int theFieldIndex;
        //            numFields = shpfile.NumFields;
        //            theFieldIndex = -1;
        //            for (int theField = 0; theField <= numFields - 1; theField++)
        //            {
        //                Field = shpfile.get_Field(theField);
        //                FieldName = Field.Name;
        //                if (FieldName.ToLower() == "id" | FieldName.ToLower() == "mwshapeid")
        //                {
        //                    theFieldIndex = theField;
        //                    break;
        //                }
        //            }

        //            if (theFieldIndex < 0)
        //            {
        //                MapWinGIS.Field f;
        //                f = new MapWinGIS.Field();
        //                f.Type = MapWinGIS.FieldType.INTEGER_FIELD;
        //                f.Name = "MWShapeID";
        //                shpfile.StartEditingTable(null);
        //                int tmpNumFields = shpfile.NumFields;
        //                shpfile.EditInsertField(f, tmpNumFields, null);
        //                int IDIndex;
        //                IDIndex = shpfile.NumFields - 1;
        //                for (int i = 0; i <= shpfile.NumRows() - 1; i++)
        //                {
        //                    shpfile.EditCellValue(IDIndex, i, i + 1);
        //                }
        //                shpfile.StopEditingTable(true, null);
        //                numFields = shpfile.NumFields;
        //                theFieldIndex = -1;
        //                for (int theField = 0; theField <= numFields - 1; theField++)
        //                {
        //                    Field = shpfile.get_Field(theField);
        //                    FieldName = Field.Name;
        //                    if (FieldName.ToLower() == "id" || FieldName.ToLower() == "mwshapeid")
        //                    {
        //                        theFieldIndex = theField;
        //                        break;
        //                    }
        //                }
        //            }
        //            for (int curShp = 0; curShp <= numShapes - 1; curShp++)
        //            {
        //                NumPoints = shpfile.get_Shape(curShp).numPoints;
        //                shp = shpfile.get_Shape(curShp);
        //                for (int curPnt = 0; curPnt <= NumPoints - 1; curPnt++)
        //                {
        //                    pnt = shp.get_Point(curPnt);
        //                    xPnts.Add(pnt.x);
        //                    yPnts.Add(pnt.y);
        //                }
        //                idNodesLocal.Add(shpfile.get_CellValue(theFieldIndex, curShp));
        //            }

        //            NumPoints = xPnts.Count;
        //            PntCount = NumPoints;
        //            if (NumPoints <= 0)
        //            {
        //                return false;
        //            }

        //            X = new double[NumPoints];
        //            Y = new double[NumPoints];
        //            idNodes = new int[NumPoints];
        //            for (int curPnt = 0; curPnt < NumPoints; curPnt++)
        //            {
        //                X[curPnt] = System.Convert.ToDouble(xPnts[curPnt]);
        //                Y[curPnt] = System.Convert.ToDouble(yPnts[curPnt]);
        //                idNodes[curPnt] = System.Convert.ToInt32(idNodesLocal[curPnt]);
        //            }
        //            return true;
        //        }

        //        /// <summary>Copies the projection of the grid to the other file</summary>
        //        /// <param name="gridFromPath">The path of the grid </param>
        //        /// <param name="fileToPath">The path to the grid or shapefile</param>
        //        private static void CopyProjectionFromGrid(string gridFromPath, string fileToPath)
        //        {
        //            var gridFrom = new Grid();
        //            gridFrom.Open(gridFromPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);
        //            var projection = gridFrom.Projection.ToString();
        //            gridFrom.Close();

        //            if (fileToPath.Contains(".shp"))
        //            {
        //                var shpTo = new Shapefile();
        //                shpTo.Open(fileToPath, null);
        //                shpTo.Projection = projection;
        //                shpTo.Close();
        //            }
        //            else
        //            {
        //                var gridTo = new Grid();
        //                gridTo.Open(fileToPath, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);
        //                gridTo.AssignNewProjection(projection);
        //                gridTo.Close();

        //            }
        //        }

        //        private static IFeature getShapeFromExtents(MapWinGIS.Extents ext)
        //        {
        //            int numPoints;
        //            IFeature shp = new IFeature();
        //            MapWinGIS.Point pt;
        //            shp.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMin;
        //            pt.y = ext.yMax;
        //            numPoints = shp.numPoints;
        //            shp.InsertPoint(pt, numPoints);
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMax;
        //            pt.y = ext.yMax;
        //            numPoints = shp.numPoints;
        //            shp.InsertPoint(pt, numPoints);
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMax;
        //            pt.y = ext.yMin;
        //            numPoints = shp.numPoints;
        //            shp.InsertPoint(pt, numPoints);
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMin;
        //            pt.y = ext.yMin;
        //            numPoints = shp.numPoints;
        //            shp.InsertPoint(pt, numPoints);
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMin;
        //            pt.y = ext.yMax;
        //            // CWG 24/4/11 add final point to close shape
        //            pt = new MapWinGIS.Point();
        //            pt.x = ext.xMin;
        //            pt.y = ext.yMax;
        //            numPoints = shp.numPoints;
        //            shp.InsertPoint(pt, numPoints);
        //            return shp;
        //        }

        //        private static MapWinGIS.Extents getGridExtents(Raster g)
        //        {
        //            MapWinGIS.Extents ext = new MapWinGIS.Extents();
        //            RasterHeader head;
        //            double centerX;
        //            double centerY;
        //            double yHalf;
        //            double xHalf;
        //            double xMin;
        //            double xMax;
        //            double yMin;
        //            double yMax;

        //            // This is fine for speed, but remember this is a reference-copied header
        //            head = g.Header;
        //            xHalf = head.dX / 2;
        //            yHalf = head.dY / 2;
        //            g.CellToProj(0, 0, out centerX, out centerY);
        //            xMin = centerX - xHalf;
        //            yMax = centerY + yHalf;
        //            g.CellToProj(head.NumberCols - 1, head.NumberRows - 1, out centerX, out centerY);
        //            xMax = centerX + xHalf;
        //            yMin = centerY - yHalf;
        //            ext.SetBounds(xMin, yMin, 0, xMax, yMax, 0);
        //            return ext;
        //        }
        //        #endregion

        //        #region Binary trees for modelling drainage

        /// <summary>
        /// Build drainage tree recursively upstream from outlet or reservoir,
        /// stopping at inlet, outlet, or reservoir (ignoring point sources)
        /// </summary>
        /// <param name="shed"></param>
        /// <param name="outlets"></param>
        /// <param name="haveOutletsShapefile">if this is false any dsnode not -1 stops the tree </param>
        /// <param name="isRoot">Flag to avoid stopping on root node, which is an outlet or reservoir</param>
        /// <param name="sindx"></param>
        /// <param name="dsNodeFieldNum"></param>
        /// <param name="us1FieldNum"></param>
        /// <param name="us2FieldNum"></param>
        /// <returns></returns>
        private static BinTree getDrainageFromSubbasin(IFeatureSet shed, IFeatureSet outlets,
                                                       bool haveOutletsShapefile, bool isRoot, int sindx,
                                                       int dsNodeFieldNum, int us1FieldNum, int us2FieldNum)
        {
            int nodeID;
            int leftID;
            int rightID;
            int Idx;
            BinTree left = null;
            BinTree right = null;

            nodeID = int.Parse(shed.get_CellValue(dsNodeFieldNum, sindx).ToString());
            if ((!isRoot) && (nodeID != -1))
            {
                if (!haveOutletsShapefile)
                {
                    return null;
                }
                if (getDSNodeType(outlets, nodeID) != DSNode.PointSource)
                {
                    return null;
                }
            }
            leftID = int.Parse(shed.get_CellValue(us1FieldNum, sindx).ToString());
            rightID = int.Parse(shed.get_CellValue(us2FieldNum, sindx).ToString());
            if (leftID != -1)
            {
                Idx = GetBasinIndexByID(shed, leftID);
                if (Idx != -1)
                {
                    left = getDrainageFromSubbasin(shed, outlets, haveOutletsShapefile, false, Idx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                }
            }
            if (rightID != -1)
            {
                Idx = GetBasinIndexByID(shed, rightID);
                if (Idx != -1)
                {
                    right = getDrainageFromSubbasin(shed, outlets, haveOutletsShapefile, false, Idx, dsNodeFieldNum, us1FieldNum, us2FieldNum);
                }
            }
            return new BinTree(sindx, left, right);
        }

        /// <summary>
        /// Merge basins using IGeometry, and only converting each shape once
        /// </summary>
        /// <param name="shed"></param>
        /// <param name="drainage"></param>
        /// <returns></returns>
        private static IGeometry mergeBasinsByDrainageI(Shapefile shed, BinTree drainage)
        {
            if (drainage == null) return null;
            IGeometry left = mergeBasinsByDrainageI(shed, drainage.left);
            IGeometry right = mergeBasinsByDrainageI(shed, drainage.right);
            IFeature outlet = shed.get_Shape(drainage.val);
            // will this work?
            IGeometry outg = outlet.BasicGeometry as IGeometry;
            if (left == null)
            {
                if (right == null)
                    return outg;
                else
                    return right.Union(outg);
            }
            else
            {
                if (right == null)
                    return left.Union(outg);
                else
                    return left.Union(right.Union(outg));
            }
        }

        /// <summary>
        /// Merge basins using using MapWinGIS.Utils
        /// </summary>
        /// <param name="shed"></param>
        /// <param name="drainage"></param>
        /// <returns></returns>
        private static IFeature mergeBasinsByDrainage(IFeatureSet shed, BinTree drainage)
        {
            if (drainage == null) return null;
            IFeature left = mergeBasinsByDrainage(shed, drainage.left);
            IFeature right = mergeBasinsByDrainage(shed, drainage.right);
            IFeature lr = mergeAbuttingPolygons(left, right);
            IFeature outlet = shed.get_Shape(drainage.val);
            // check for multipart shape
            if (outlet.NumGeometries > 1)
            {
                Trace.WriteLine("Subbasin " + drainage.val.ToString() + " has " +
                                          outlet.NumGeometries.ToString() + " parts");
            }
            else
            {
                // check for anticlockwise polygon
                double area = SignedArea(outlet);
                if (area < 0)
                {
                    Trace.WriteLine("Needed to reverse subbasin " + drainage.val.ToString());
                    outlet = ReverseShape(outlet);
                }
            }
            return mergeAbuttingPolygons(lr, outlet);
        }

        private static IFeature mergeAbuttingPolygons(IFeature shape1, IFeature shape2)
        {
            if (shape1 == null) return shape2;
            if (shape2 == null) return shape1;
            IFeature mergeShape = null;
            // check both shapes are single parts
            bool areSingleGeometryFeatures = (shape1.NumGeometries == 1) && (shape2.NumGeometries == 1);
            if (areSingleGeometryFeatures)
            {
                mergeShape = shape1.Union(shape2);
            }
            else
            {
                Trace.WriteLine("Error - there were multiple parts.");
                //mergeShape = new IFeatureClass();
                //mergeShape.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
                //SpatialOperations.MergeShapes(shape1, shape2, out mergeShape);
            }

            return mergeShape;
        }

        /// <summary>
        /// Reverses a single part shape.  Shape must be already checked to have only one part
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private static IFeature ReverseShape(IFeature shape)
        {
            IFeature result = new Feature(FeatureType.Polygon, shape.Coordinates.Reverse());

            return result;
        }

        /// <summary>
        /// Returns the signed area for a polygon shape.
        /// The area is positive if the polygon runs clockwise.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static double SignedArea(IFeature shape)
        {
            if (shape.NumPoints < 4)
                return 0.0;

            double sum = 0.0;
            Coordinate ptb;
            Coordinate ptc = shape.Coordinates[0];
            // If this is a close polygon, the starting point will
            // be the ssame as the end point, so we don't look at the last point.
            for (int i = 0; i < shape.NumPoints - 1; i++)
            {
                ptb = ptc;
                ptc = shape.Coordinates[i + 1];
                double bx = ptb.X;
                double by = ptb.Y;
                double cx = ptc.X;
                double cy = ptc.Y;
                sum += (bx + cx) * (cy - by);
            }
            return -sum / 2.0;
        }

        //        #endregion

        //        #endregion

        private static string TaudemExeDir()
        {
            return
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)
                + @"\Taudem5Exe\";
        }

        #region "Apply Watershed Attributes"

        /// <summary>
        /// Hydrology function to apply the watershed link attributes copied or interpretted from the stream network
        /// </summary>
        /// <param name="subBasinShapePath"></param>
        /// <param name="streamNetworkShapePath"></param>
        /// <param name="callback"></param>
        /// <returns>0 on success</returns>
        public static int ApplyWatershedLinkAttributes(
            string subBasinShapePath, string streamNetworkShapePath, IProgressHandler callback)
        {
            int shedIndex;

            if (callback != null) callback.Progress("Status", 0, "Assigning WS Link");

            //Stream fields
            const int IDField = 0;
            const int DsidField = 1;
            const int Us1IDField = 2;
            const int Us2IDField = 3;
            const int DsNodeIDField = 4;
            const int StreamLenID = 6;

            IFeatureSet streamShape = FeatureSet.Open(streamNetworkShapePath);
            if (streamShape == null)
            {
                throw new ApplicationException(String.Format("Error in opening {0}", streamNetworkShapePath));
            }
            int streamShapeNumShapes = streamShape.NumRows();

            IFeatureSet shedShape = FeatureSet.Open(subBasinShapePath);
            if (shedShape == null)
            {
                throw new ApplicationException(String.Format("Error in opening {0}", subBasinShapePath));
            }
            int shedShapeNumShapes = shedShape.NumRows();

            const int WShedIDField = 0;
            int streamLinkFieldNum = AddField(shedShape, "StreamLinkNo", typeof(int));
            int streamLenFieldNum = AddField(shedShape, "StreamLen", typeof(int));
            int dsnodeidFieldNum = AddField(shedShape, "DSNodeID", typeof(int));
            int dslinkFieldNum = AddField(shedShape, "DSWSID", typeof(int));
            int us1LinkFieldNum = AddField(shedShape, "US1WSID", typeof(int));
            int us2LinkFieldNum = AddField(shedShape, "US2WSID", typeof(int));

            int oldperc = 0;
            int uS1WSID;
            int dSWSID;
            int us2Wsid;

            for (shedIndex = 0; shedIndex < shedShapeNumShapes; shedIndex++)
            {
                if (callback != null && shedShapeNumShapes > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(shedIndex) / Convert.ToDouble(shedShapeNumShapes)) * 100);
                    if (newperc > oldperc)
                    {
                        callback.Progress("Status", newperc, "Assigning WS Link");
                        oldperc = newperc;
                    }
                }

                // Change watershed ID from zero-based to one-based to match change to stream ID in ApplyStreamAttributes
                int WatershedID = Convert.ToInt32(shedShape.get_CellValue(WShedIDField, shedIndex)) + 1; 
                shedShape.EditCellValue(WShedIDField, shedIndex, WatershedID);
                for (int streamIndex = 0; streamIndex < streamShapeNumShapes; streamIndex++)
                {   //Stream IDs have already been incremented to start at 1 in ApplyStreamAttributes, so match with incremented Watershed ID
                    if (Convert.ToInt32(streamShape.get_CellValue(IDField, streamIndex)) == WatershedID)
                    {
                        shedShape.EditCellValue(dsnodeidFieldNum, shedIndex, streamShape.get_CellValue(DsNodeIDField, streamIndex));
                        shedShape.EditCellValue(streamLinkFieldNum, shedIndex, streamShape.get_CellValue(IDField, streamIndex));
                        shedShape.EditCellValue(streamLenFieldNum, shedIndex, streamShape.get_CellValue(StreamLenID, streamIndex));
                        int currDsStreamLink = Convert.ToInt32(streamShape.get_CellValue(DsidField, streamIndex));
                        int currUs1StreamLink = Convert.ToInt32(streamShape.get_CellValue(Us1IDField, streamIndex));
                        int currUs2StreamLink = Convert.ToInt32(streamShape.get_CellValue(Us2IDField, streamIndex));

                        if (currDsStreamLink == -1)
                        {
                            dSWSID = -1;
                        }
                        else
                        {
                            dSWSID = currDsStreamLink; // GetWshedFromStreamLink(currDsStreamLink, streamShape, shedShape);
                        }

                        if (currUs1StreamLink <= 0)
                        {
                            uS1WSID = -1;
                        }
                        else
                        {
                            uS1WSID = currUs1StreamLink; // GetWshedFromStreamLink(currUs1StreamLink, streamShape, shedShape);
                        }

                        if (currUs2StreamLink <= 0)
                        {
                            us2Wsid = -1;
                        }
                        else
                        {
                            us2Wsid = currUs2StreamLink; // GetWshedFromStreamLink(currUs2StreamLink, streamShape, shedShape);
                        }

                        shedShape.EditCellValue(dslinkFieldNum, shedIndex, dSWSID);
                        shedShape.EditCellValue(us1LinkFieldNum, shedIndex, uS1WSID);
                        shedShape.EditCellValue(us2LinkFieldNum, shedIndex, us2Wsid);
                        break;
                    }
                }
            }

            shedShape.Save();
            shedShape.Close();

            streamShape.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return 0;
        }

        /// <summary>
        /// A function to apply area attributes to a watershed polygon shapefile
        /// </summary>
        /// <param name="subBasinShapePath">Subbasin shapefile</param>
        /// <param name="callback">Callback object</param>
        /// <returns>True on success</returns>
        public static bool ApplyWatershedAreaAttributes(string subBasinShapePath, IProgressHandler callback)
        {
            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Area Parameters");
            }

            var shedShape = FeatureSet.Open(subBasinShapePath);

            var areaMFieldNum = AddField(shedShape, "Area_M", typeof(double));
            var areaAcreFieldNum = AddField(shedShape, "Area_Acre", typeof(double));
            var areaMileFieldNum = AddField(shedShape, "Area_SqMi", typeof(double));

            double areaM = 0;
            var oldperc = 0;

            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating WS Area Parameters");
                        }

                        oldperc = newperc;
                    }
                }
                var tmpShp = shedShape.get_Shape(sindx);

                var tmpArea = Utils.AreaOfPart(tmpShp, 0);
                if (shedShape.Projection != null || String.IsNullOrWhiteSpace(shedShape.Projection.ToString()))
                {
                    if (shedShape.Projection.Unit.Name == "Meter")
                    {
                        areaM = tmpArea;
                    }
                    else if (shedShape.Projection.Unit.Name == "Foot")
                    {
                        areaM = tmpArea * 0.09290304;
                    }
                }
                else
                {
                    areaM = tmpArea;
                }

                var areaAcre = areaM * 0.000247105;
                var areaSqMi = areaAcre * 0.0015625;
                shedShape.EditCellValue(areaMFieldNum, sindx, areaM);
                shedShape.EditCellValue(areaAcreFieldNum, sindx, areaAcre);
                shedShape.EditCellValue(areaMileFieldNum, sindx, areaSqMi);
            }

            shedShape.Save();
            shedShape.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average slope attribute
        /// </summary>
        /// <param name="subBasinShapePath"></param>
        /// <param name="slopeGridPath"></param>
        /// <param name="elevUnits"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ApplyWatershedSlopeAttribute(string subBasinShapePath, string slopeGridPath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            // CWG 23/1/2011 changed to GeoTiff for Taudem V5
            var tmpClipPath = Path.Combine(
                Path.GetDirectoryName(slopeGridPath), Path.GetFileNameWithoutExtension(slopeGridPath) + "_clip.tif");

            //DataManagement.DeleteGrid(tmpClipPath);

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Slope Parameters");
            }

            var shedShape = FeatureSet.Open(subBasinShapePath);

            var slopeGrid = Raster.Open(slopeGridPath);
            var slopeProj = slopeGrid.Projection;
            slopeGrid.Close();

            var countSlope = new int[shedShape.NumRows()];
            var sumSlope = new double[shedShape.NumRows()];
            var avgSlope = new double[shedShape.NumRows()];

            var oldperc = 0;
            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows() - 1)) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating WS Slope Parameters");
                        }

                        oldperc = newperc;
                    }
                }
                var tmpPoly = shedShape.get_Shape(sindx);

                if (ClipRaster.ClipRasterWithPolygon(slopeGridPath, tmpPoly, tmpClipPath) != null)
                {
                    continue;
                }

                var tmpClipGrid = Raster.Open(tmpClipPath);

                var numberRows = tmpClipGrid.NumRows;
                var numberCols = tmpClipGrid.NumColumns;
                var nodataVal = tmpClipGrid.NoDataValue;
                countSlope[sindx] = 0;
                sumSlope[sindx] = 0;
                avgSlope[sindx] = 0;
                int row;
                for (row = 0; row < numberRows; row += 2)
                {
                    int col;
                    for (col = 0; col < numberCols; col += 2)
                    {
                        var currVal = double.Parse(tmpClipGrid.Rows[col][row].ToString());
                        if (currVal == nodataVal)
                        {
                            continue;
                        }

                        countSlope[sindx] = countSlope[sindx] + 1;
                        sumSlope[sindx] = sumSlope[sindx] + currVal;
                    }
                }

                tmpClipGrid.Close();

                //DataManagement.DeleteGrid(tmpClipPath);
            }

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Slope Parameters");
            }

            var slopeFieldNum = AddField(shedShape, "AveSlope", typeof(double));

            oldperc = 0;
            for (var sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating WS Slope Parameters");
                        }

                        oldperc = newperc;
                    }
                }

                if (countSlope[sindx] <= 0)
                {
                    continue;
                }

                //if (!string.IsNullOrEmpty(slopeProj))
                //{
                if (slopeProj.Unit.Name == "Meter")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.meters:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                        case ElevationUnits.centimeters:
                            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                            break;
                        case ElevationUnits.feet:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                            break;
                    }
                }
                else if (slopeProj.Unit.Name == "Foot")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.meters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) * 3.280839895) * 100;
                            break;
                        case ElevationUnits.centimeters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 30.48) * 100;
                            break;
                        case ElevationUnits.feet:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                    }
                }
                //}
                //else
                //{
                //    switch (elevUnits)
                //    {
                //        case ElevationUnits.meters:
                //            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                //            break;
                //        case ElevationUnits.centimeters:
                //            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                //            break;
                //        case ElevationUnits.feet:
                //            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                //            break;
                //    }
                //}

                shedShape.EditCellValue(slopeFieldNum, sindx, avgSlope[sindx]);
            }

            shedShape.Save();
            shedShape.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        /// <summary>
        /// Hydrology function used to add to the subbasin shapefile average slope attribute
        /// </summary>
        /// <param name="subBasinGridPath"></param>
        /// <param name="subBasinShapePath"></param>
        /// <param name="slopeGridPath"></param>
        /// <param name="elevUnits"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ApplyWatershedSlopeAttribute(string subBasinGridPath, string subBasinShapePath, string slopeGridPath, ElevationUnits elevUnits, IProgressHandler callback)
        {
            int sindx;
            int row;

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Slope Parameters");
            }

            var shedShape = FeatureSet.Open(subBasinShapePath);
            var subBasinGrid = Raster.Open(subBasinGridPath);
            var slopeGrid = Raster.Open(slopeGridPath);

            var numberRows = slopeGrid.NumRows;
            var numberCols = slopeGrid.NumColumns;
            var nodataVal = slopeGrid.NoDataValue;
            var slopeProj = slopeGrid.Projection;

            var countSlope = new int[shedShape.NumRows()];
            var sumSlope = new double[shedShape.NumRows()];
            var avgSlope = new double[shedShape.NumRows()];

            var currPolyIDIdx = -1;
            var currLinkIDIdx = -1;
            for (sindx = 0; sindx < shedShape.DataTable.Columns.Count; sindx++)
            {
                if (shedShape.DataTable.Columns[sindx].ColumnName == "PolygonID" || shedShape.DataTable.Columns[sindx].ColumnName == "MWShapeID")
                {
                    currPolyIDIdx = sindx;
                }

                if (shedShape.DataTable.Columns[sindx].ColumnName == "LinkIDs")
                {
                    currLinkIDIdx = sindx;
                }

                // Paul Meems, 24-Aug-2011: Added:
                if (currPolyIDIdx != -1 && currLinkIDIdx != -1)
                {
                    // Found the values so stop searching:
                    break;
                }
            }

            string tmpLinkIDs;
            var linkIDVals = new List<int>();
            var linkIDMerged = new List<int>();

            if (currLinkIDIdx != -1 && currPolyIDIdx != -1)
            {
                for (sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    tmpLinkIDs = shedShape.get_CellValue(currLinkIDIdx, sindx).ToString();
                    var tmpLinks = tmpLinkIDs.Split(',');
                    foreach (var tmpLink in tmpLinks)
                    {
                        linkIDMerged.Add(sindx);
                        linkIDVals.Add(int.Parse(tmpLink.Trim()));
                    }
                }
            }
            else
            {
                for (sindx = 0; sindx < shedShape.NumRows(); sindx++)
                {
                    linkIDMerged.Add(sindx);
                    linkIDVals.Add(int.Parse(shedShape.get_CellValue(currPolyIDIdx, sindx).ToString()));
                }
            }

            var oldperc = 0;
            for (row = 0; row < numberRows; row++)
            {
                var newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(numberRows - 1)) * 100);
                if (newperc > oldperc)
                {
                    if (callback != null)
                    {
                        callback.Progress("Status", newperc, "Calculating WS Slope Parameters");
                    }

                    oldperc = newperc;
                }

                int col;
                for (col = 0; col < numberCols; col++)
                {
                    var currVal = double.Parse(slopeGrid.Rows[col][row].ToString());

                    if (currVal == nodataVal)
                    {
                        continue;
                    }

                    var currBasinID = int.Parse(subBasinGrid.Rows[col][row].ToString());

                    // Paul Meems, 24-Aug-2011: Changed:
                    // if (currBasinID != -1)
                    // TODO: Check if the result is still the same
                    if (currBasinID > -1)
                    {
                        // Paul Meems, 24-Aug-2011: Added extra check:
                        var tmp = linkIDVals.IndexOf(currBasinID);
                        if (tmp != -1)
                        {
                            var currID = linkIDMerged[tmp];

                            countSlope[currID] = countSlope[currID] + 1;
                            sumSlope[currID] = sumSlope[currID] + currVal;
                        }
                    }
                }
            }

            slopeGrid.Close();

            if (callback != null)
            {
                callback.Progress("Status", 0, "Calculating WS Slope Parameters");
            }

            var slopeFieldNum = AddField(shedShape, "AveSlope", typeof(double));

            oldperc = 0;
            for (sindx = 0; sindx < shedShape.NumRows(); sindx++)
            {
                // TODO: Why > 1 instead of > 0?
                if (shedShape.NumRows() > 1)
                {
                    var newperc = Convert.ToInt32((Convert.ToDouble(sindx) / Convert.ToDouble(shedShape.NumRows())) * 100);
                    if (newperc > oldperc)
                    {
                        if (callback != null)
                        {
                            callback.Progress("Status", newperc, "Calculating WS Slope Parameters");
                        }

                        oldperc = newperc;
                    }
                }

                if (countSlope[sindx] <= 0)
                {
                    continue;
                }

                //if (!string.IsNullOrEmpty(slopeProj))
                //{
                if (slopeProj.Unit.Name == "Meter")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.meters:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                        case ElevationUnits.centimeters:
                            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                            break;
                        case ElevationUnits.feet:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                            break;
                    }
                }
                else if (slopeProj.Unit.Name == "Foot")
                {
                    switch (elevUnits)
                    {
                        case ElevationUnits.meters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) * 3.280839895) * 100;
                            break;
                        case ElevationUnits.centimeters:
                            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 30.48) * 100;
                            break;
                        case ElevationUnits.feet:
                            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                            break;
                    }
                }
                //}
                //else
                //{
                //    switch (elevUnits)
                //    {
                //        case ElevationUnits.meters:
                //            avgSlope[sindx] = (sumSlope[sindx] / countSlope[sindx]) * 100;
                //            break;
                //        case ElevationUnits.centimeters:
                //            avgSlope[sindx] = sumSlope[sindx] / countSlope[sindx];
                //            break;
                //        case ElevationUnits.feet:
                //            avgSlope[sindx] = ((sumSlope[sindx] / countSlope[sindx]) / 3.280839895) * 100;
                //            break;
                //    }
                //}

                shedShape.EditCellValue(slopeFieldNum, sindx, avgSlope[sindx]);
            }

            shedShape.Save();
            shedShape.Close();

            subBasinGrid.Close();

            if (callback != null)
            {
                callback.Progress(string.Empty, 0, string.Empty);
            }

            return true;
        }

        #endregion

        #region Run Taudem executable

        /// <summary>
        /// Run a Taudem V5 executable
        /// </summary>
        /// <param name="command">Taudem executable file name</param>
        /// <param name="parameters">Parameters for Taudem executable.  Parameters containing spaces must be quoted.</param>
        /// <param name="numProc">Maximum number of threads (ignored if MPICH2 not running)</param>
        /// <param name="showOutput">Standard output and standard error from Taudem executable are shown iff this flag is true or there is an error in the Taudem run.</param>
        /// <returns>
        /// Return code from Taudem executable
        /// </returns>
        public static int RunTaudem(string command, string parameters, int numProc, bool showOutput)
        {
            var tfc = new TempFileCollection();
            var tempFile = tfc.AddExtension("txt");
            var taudemProcess = new Process
                                    {
                                        StartInfo =
                                            {
                                                CreateNoWindow = true,
                                                WindowStyle = ProcessWindowStyle.Minimized,
                                                WorkingDirectory = TaudemExeDir(),
                                                FileName = "runtaudem.bat",
                                                Arguments =
                                                    Quote(tempFile) + " mpiexec -n "
                                                    + numProc.ToString(CultureInfo.InvariantCulture) + " " + command
                                                    + " " + parameters
                                            }
                                    };
            taudemProcess.Start();
            taudemProcess.WaitForExit();

            var needOutput = showOutput
                             ||
                             ((taudemProcess.ExitCode != 0)
                              &&
                              (MessageBox.Show(
                                  @"Taudem error.  Do you want to see the details?", string.Empty, MessageBoxButtons.YesNo)
                               == DialogResult.Yes));
            if (needOutput)
            {
                var proc = new Process
                               {
                                   StartInfo =
                                       {
                                           FileName = Path.GetFileName(tempFile),
                                           WorkingDirectory = Path.GetDirectoryName(tempFile)
                                       }
                               };
                proc.Start();

                // Paul Meems, 23-aug-2011: Don't wait for exit but continue the proces:
                // cwg 13/9/2011 do wait - else get a confusing stack of outputs
                proc.WaitForExit();
            }

            // Paul Meems, 23-aug-2011
            // tfc.Delete();
            return taudemProcess.ExitCode;
        }

        #endregion

        #region "D8"

        /// <summary>
        /// Uses TauDEM V5
        /// Generates a D8 directional grid by assigning a number from 1 to 8 (0 to 7 in some algorithms) based on a direction to the lowest elevation cell surrounding that cell.
        /// </summary>
        /// <param name="pitFillPath">Path of a pit-filled DEM.</param>
        /// <param name="d8ResultPath">Output result file of a D8 directional grid.</param>
        /// <param name="d8SlopeResultPath">Path to an output grid containing the slope from the cell to the lowest elevation surrounding cell.</param>
        /// <param name="numProcesses">Number of threads to be used by Taudem</param>
        /// <param name="showTaudemOutput">Show Taudem output if true</param>
        /// <param name="callback"> A callback object for internal status messages</param>
        /// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
        public static bool D8(string pitFillPath, string d8ResultPath, string d8SlopeResultPath, int numProcesses, bool showTaudemOutput, IProgressHandler callback)
        {
            Trace.WriteLine("D8(pitFillPath: " + pitFillPath + "\n" +
                            "   D8ResultPath: " + d8ResultPath + "\n" +
                            "   D8SlopeResultPath: " + d8SlopeResultPath + "\n" +
                            "   NumProcesses: " + numProcesses.ToString() + "\n" +
                            "   ShowTaudemOutput: " + showTaudemOutput.ToString() + "\n" +
                            "   callback)");
            if (callback != null)
            {
                callback.Progress("Status", 0, "D8 Flow Directions");
            }

            //DataManagement.DeleteGrid(d8ResultPath);
            //DataManagement.DeleteGrid(d8SlopeResultPath);

            var pars = "-p " + Quote(d8ResultPath) + " -sd8 " + Quote(d8SlopeResultPath) + " -fel " + Quote(pitFillPath);
            var result = RunTaudem("D8FlowDir.exe", pars, numProcesses, showTaudemOutput);

            if (result != 0)
            {
                MessageBox.Show(
                    "TauDEM Error " + result,
                    "TauDEM Error " + result,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }

            //CopyProjectionFromGrid(pitFillPath, d8ResultPath);
            //CopyProjectionFromGrid(pitFillPath, d8SlopeResultPath);
            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            Trace.WriteLine("Finished D8");
            return result == 0;
        }

        ///// <summary>
        ///// An overload of D8 which will generate a GeoprocDialog and execute the d8 from that.
        ///// </summary>
        ///// <param name="callback"></param>
        ///// <returns></returns>
        //public static int D8(IProgressHandler callback)
        //{
        //    return DoD8Diag(callback);
        //}

        ///// <summary>
        ///// An overload of D8 which will generate a GeoprocDialog and execute the d8 from that.
        ///// </summary>
        ///// <returns></returns>
        //public static int D8()
        //{
        //    return DoD8Diag(null);
        //}

        //private static int DoD8Diag(IProgressHandler callback)
        //{
        //    Trace.WriteLine("DoD8Diag(callback)");
        //    var d8Diag = new GeoProcDialog();
        //    var pitFillElem = d8Diag.Add_FileElement(GeoProcDialog.ElementTypes.OpenGridFile);
        //    var d8ResElem = d8Diag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //    var d8SlopeResElem = d8Diag.Add_FileElement(GeoProcDialog.ElementTypes.SaveGridFile);
        //    var numProcessesElem = d8Diag.Add_TextElement();
        //    var showTaudemOutputElem = d8Diag.Add_BooleanElement();

        //    d8Diag.Text = @"D8 Flow Directions and Slopes";
        //    d8Diag.HelpTitle = "D8 Flow Directions and Slopes";
        //    d8Diag.HelpText = "This function will generate a grid of D8 flow directions and a grid of slopes, given a pit-filled DEM.";
        //    d8Diag.Height = 250;
        //    d8Diag.HelpPanelVisible = false;

        //    pitFillElem.Caption = "Pit Filled DEM Path";
        //    pitFillElem.HelpButtonVisible = false;

        //    d8ResElem.Caption = "D8 Flow Direction Grid Result Path";
        //    d8ResElem.HelpButtonVisible = false;

        //    d8SlopeResElem.Caption = "D8 Slope Grid Result Path";
        //    d8SlopeResElem.HelpButtonVisible = false;

        //    numProcessesElem.Caption = "Number of processes";
        //    numProcessesElem.Text = "8";
        //    numProcessesElem.HelpButtonVisible = false;

        //    showTaudemOutputElem.Caption = "Show Taudem output";
        //    showTaudemOutputElem.Text = @"Show Taudem output";
        //    showTaudemOutputElem.HelpButtonVisible = false;

        //    if (d8Diag.ShowDialog() == DialogResult.OK)
        //    {
        //        return D8(
        //            pitFillElem.Filename,
        //            d8ResElem.Filename,
        //            d8SlopeResElem.Filename,
        //            int.Parse(numProcessesElem.Text),
        //            showTaudemOutputElem.Value,
        //            callback);
        //    }

        //    Trace.WriteLine("Finished DoD8Diag");
        //    return -2;
        //}

        #endregion

        #region Nested type: BinTree

        /// <summary>
        /// Binary (integer) trees
        /// </summary>
        public class BinTree
        {
            /// <summary>
            /// Subtrees
            /// </summary>
            public BinTree left, right;

            /// <summary>
            /// value at node
            /// </summary>
            public int val;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="v">node value</param>
            /// <param name="l">left subtree</param>
            /// <param name="r">right subtree</param>
            public BinTree(int v, BinTree l, BinTree r)
            {
                val = v;
                left = l;
                right = r;
            }

            /// <summary>
            /// Make a string correponding to (reverse) depth-first traverse
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string l = (left == null) ? String.Empty : ", " + left;
                string r = (right == null) ? String.Empty : ", " + right;
                string v = val.ToString();
                return v + r + l;
            }
        }

        #endregion
    }//End Of Hydrology
}