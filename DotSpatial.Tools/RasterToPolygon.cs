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
            var input = _inputParam[0].Value as IRaster;
            var output = _outputParam[0].Value as IFeatureSet;
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

            var featureHash = new Dictionary<double, LineSegmentList>();
            var previous = 0.0;

            var height = input.CellHeight;
            var width = input.CellWidth;

            var xMin = input.Xllcenter - width/2.0;
            var yMin = input.Yllcenter - height/2.0;
            var xMax = xMin + width*input.NumColumns;
            var yMax = yMin + height*input.NumRows;

            var numRows = input.NumRows;
            var numColumns = input.NumColumns;
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
                    var value = input.Value[y, x];
                    if (value == input.NoDataValue)
                    {
                        continue;
                    }

                    LineSegmentList lineList;
                    if (!featureHash.TryGetValue(value, out lineList))
                    {
                        lineList = new LineSegmentList();
                        featureHash.Add(value, lineList);
                    }

                    if (y == 0)
                    {
                        lineList.AddSegment(xMin + x*width, yMax, xMin + (x + 1)*width, yMax);
                        if (input.Value[y + 1, x] != value)
                        {
                            lineList.AddSegment(xMin + x*width, yMax - height, xMin + (x + 1)*width, yMax - height);
                        }
                    }
                    else if (y == (numRows - 1))
                    {
                        lineList.AddSegment(xMin + x*width, yMin, xMin + (x + 1)*width, yMin);
                        if (input.Value[y - 1, x] != value)
                        {
                            lineList.AddSegment(xMin + x*width, yMin + height, xMin + (x + 1)*width, yMin + height);
                        }
                    }
                    else
                    {
                        if (input.Value[y + 1, x] != value)
                        {
                            lineList.AddSegment(xMin + x * width, yMax - (y + 1) * height, xMin + (x + 1) * width, yMax - (y + 1) * height);
                        }
                        if (input.Value[y - 1, x] != value)
                        {
                            lineList.AddSegment(xMin + x * width, yMax - y * height, xMin + (x + 1) * width, yMax - y * height);
                        }
                    }

                    if (x == 0)
                    {
                        lineList.AddSegment(xMin, yMax - y*height, xMin, yMax - (y + 1)*height);
                        if (input.Value[y, x + 1] != value)
                        {
                            lineList.AddSegment(xMin + width, yMax - y*height, xMin + width, yMax - (y + 1)*height);
                        }
                    }
                    else if (x == (numColumns - 1))
                    {
                        lineList.AddSegment(xMax, yMax - y*height, xMax, yMax - (y + 1)*height);
                        if (input.Value[y, x - 1] != value)
                        {
                            lineList.AddSegment(xMax - width, yMax - y*height, xMax - width, yMax - (y + 1)*height);
                        }
                    }
                    else
                    {
                        if (input.Value[y, x + 1] != value)
                        {
                            lineList.AddSegment(xMin + (x + 1)*width, yMax - y*height, xMin + (x + 1)*width, yMax - (y + 1)*height);
                        }
                        if (input.Value[y, x - 1] != value)
                        {
                            lineList.AddSegment(xMin + x*width, yMax - y*height, xMin + x*width, yMax - (y + 1)*height);
                        }
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }
            }

            var sw = new Stopwatch();
            foreach (var pair in featureHash)
            {
                sw.Restart();

                var key = pair.Key;
                var lineSegList = pair.Value.List;

                var polyList = new List<Polygon>();
                while (lineSegList.Count != 0)
                {
                    var polyShell = new List<Coordinate>();
                    var start = lineSegList[0];
                    polyShell.Add(start.P0);
                    polyShell.Add(start.P1);
                    lineSegList.Remove(start);

                    while (!polyShell[0].Equals2D(polyShell[polyShell.Count - 1]))
                    {
                        var last = polyShell[polyShell.Count - 1];
                        var segment = lineSegList.Find(o => o.P0.Equals2D(last) || o.P1.Equals2D(last));
                        polyShell.Add(segment.P0.Equals2D(last) ? segment.P1 : segment.P0);
                        lineSegList.Remove(segment);
                    }

                    polyList.Add(new Polygon(polyShell));
                }

                var geometry = polyList.Count == 1
                                   ? (IBasicGeometry) polyList[0]
                                   : new MultiPolygon(polyList.ToArray());
                var f = output.AddFeature(geometry);
                f.DataRow["Value"] = key;

                sw.Stop();
                Debug.WriteLine(sw.ElapsedMilliseconds);
            }
            
            output.AttributesPopulated = true;
            output.Save();
            return true;
        }

        private class LineSegmentList
        {
            private readonly List<LineSegment> _list = new List<LineSegment>();

            public void AddSegment(double x1, double y1, double x2, double y2)
            {
                _list.Add(new LineSegment(new Coordinate(x1, y1), new Coordinate(x2, y2)));
            }

            public List<LineSegment> List { get { return _list; } }
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