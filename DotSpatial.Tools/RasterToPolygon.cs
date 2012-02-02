// ********************************************************************************************************
// Product Name: MapWindow.Tools.mwRasterToPolygon
// Description:  Converts a raster data set to a polygon feature set
//
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initial developer of this Original Code is Brian Marchionni. Created in Aug. 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name               |   Date             |         Comments
//------------------------|------------------------|-------------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some unnecessary references using re-sharper
// KP                     |  9/2009                |  Used IDW as model for RasterToPolygon
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster to Polyogn tool
    /// </summary>
    public class RasterToPolygon : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the RasterToPolygon class.
        /// </summary>
        public RasterToPolygon()
        {
            this.Name = TextStrings.RasterToPolygon;
            this.Category = TextStrings.Conversion;
            this.Description = TextStrings.RasterToPolygonDescription;
            this.ToolTip = TextStrings.RasterToPolygonDescription;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters
        {
            get
            {
                return _outputParam;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input = _inputParam[0].Value as IRaster;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;
            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// Ping delete static for external testing
        /// </summary>
        /// <param name="input">The Polygon Raster(Grid file).</param>
        /// <param name="output">The Polygon shapefile path.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(IRaster input, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            if ((input == null) || (output == null))
            {
                return false;
            }

            output.DataTable.Columns.Add("Value", typeof(double));

            Hashtable featureHash = new Hashtable();
            double previous = 0.0;
            double height = input.CellHeight;
            double width = input.CellWidth;
            int numRows = input.NumRows;
            int numColumns = input.NumColumns;
            double xMin = input.Xllcenter - (input.CellWidth / 2.0);
            double yMin = input.Yllcenter - (input.CellHeight / 2.0);
            double xMax = xMin + (height * input.NumColumns);
            double yMax = yMin + (width * input.NumRows);
            for (int y = 0; y < numRows; y++)
            {
                int current = Convert.ToInt32((y * 100.0) / input.NumRows);
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                    previous = current;
                }

                Debug.WriteLine("Row : " + y + " done.");
                for (int x = 0; x < numColumns; x++)
                {
                    double value = input.Value[y, x];
                    List<LineSegment> lineList = new List<LineSegment>();
                    if (!featureHash.Contains(value))
                    {
                        featureHash.Add(value, lineList);
                    }
                    else
                    {
                        lineList = featureHash[value] as List<LineSegment>;
                    }

                    if (y == 0)
                    {
                        if (lineList != null)
                        {
                            lineList.Add(
                                new LineSegment(
                                    new Coordinate((x * width) + xMin, yMax),
                                    new Coordinate(((x + 1) * width) + xMin, yMax)));
                            if (input.Value[y + 1, x] != value)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate((x * width) + xMin, yMax - height),
                                        new Coordinate(((x + 1) * width) + xMin, yMax - height)));
                            }
                        }
                    }
                    else if (y == (numRows - 1))
                    {
                        if (lineList != null)
                        {
                            lineList.Add(
                                new LineSegment(
                                    new Coordinate((x * width) + xMin, yMin),
                                    new Coordinate(((x + 1) * width) + xMin, yMin)));
                            if (input.Value[y - 1, x] != value)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate((x * width) + xMin, yMin + height),
                                        new Coordinate(((x + 1) * width) + xMin, yMin + height)));
                            }
                        }
                    }
                    else
                    {
                        if (input.Value[y + 1, x] != value)
                        {
                            if (lineList != null)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate((x * width) + xMin, yMax - ((y + 1) * height)),
                                        new Coordinate(((x + 1) * width) + xMin, yMax - ((y + 1) * height))));
                            }
                        }

                        if (input.Value[y - 1, x] != value)
                        {
                            if (lineList != null)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate((x * width) + xMin, yMax - (y * height)),
                                        new Coordinate(((x + 1) * width) + xMin, yMax - (y * height))));
                            }
                        }
                    }

                    if (x == 0)
                    {
                        if (lineList != null)
                        {
                            lineList.Add(
                                new LineSegment(
                                    new Coordinate(xMin, yMax - (y * height)),
                                    new Coordinate(xMin, yMax - ((y + 1) * height))));
                            if (input.Value[y, x + 1] != value)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate(xMin + width, yMax - (y * height)),
                                        new Coordinate(xMin + width, yMax - ((y + 1) * height))));
                            }
                        }
                    }
                    else if (x == (numColumns - 1))
                    {
                        if (lineList != null)
                        {
                            lineList.Add(
                                new LineSegment(
                                    new Coordinate(xMax, yMax - (y * height)),
                                    new Coordinate(xMax, yMax - ((y + 1) * height))));
                            if (input.Value[y, x - 1] != value)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate(xMax - width, yMax - (y * height)),
                                        new Coordinate(xMax - width, yMax - ((y + 1) * height))));
                            }
                        }
                    }
                    else
                    {
                        if (input.Value[y, x + 1] != value)
                        {
                            if (lineList != null)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate(xMin + ((x + 1) * width), yMax - (y * height)),
                                        new Coordinate(xMin + ((x + 1) * width), yMax - ((y + 1) * height))));
                            }
                        }

                        if (input.Value[y, x - 1] != value)
                        {
                            if (lineList != null)
                            {
                                lineList.Add(
                                    new LineSegment(
                                        new Coordinate(xMin + (x * width), yMax - (y * height)),
                                        new Coordinate(xMin + (x * width), yMax - ((y + 1) * height))));
                            }
                        }
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            foreach (double key in featureHash.Keys)
            {
                sw.Reset();
                sw.Start();
                List<LineSegment> lineSegList = featureHash[key] as List<LineSegment>;
                if (lineSegList == null)
                {
                    break;
                }

                List<Polygon> polyList = new List<Polygon>();
                while (lineSegList.Count != 0)
                {
                    List<Coordinate> polyShell = new List<Coordinate>();
                    LineSegment start = lineSegList[0];
                    polyShell.Add(start.P0);
                    polyShell.Add(start.P1);
                    lineSegList.Remove(start);
                    while (!polyShell[0].Equals2D(polyShell[polyShell.Count - 1]))
                    {
                        LineSegment segment =
                            lineSegList.Find(
                                o =>
                                o.P0.Equals2D(polyShell[polyShell.Count - 1])
                                || o.P1.Equals2D(polyShell[polyShell.Count - 1]));
                        polyShell.Add(segment.P0.Equals2D(polyShell[polyShell.Count - 1]) ? segment.P1 : segment.P0);
                        lineSegList.Remove(segment);
                    }

                    polyList.Add(new Polygon(polyShell));
                }

                if (polyList.Count == 1)
                {
                    Feature feat = new Feature(polyList[0], output);
                    feat.DataRow["Value"] = key;
                }

                sw.Stop();
                Debug.WriteLine(sw.ElapsedMilliseconds);
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster) { HelpText = TextStrings.inputrastetoconvert };

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.Convertedfeatureset)
                                  {
                                      HelpText = TextStrings.featuresetcreated
                                  };
        }

        #endregion
    }
}