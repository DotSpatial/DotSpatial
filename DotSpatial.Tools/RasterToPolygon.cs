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
using System.Collections.Generic;
using System.Diagnostics;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster to Polygon tool
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
        /// Create polygons from raster.
        /// </summary>
        /// <param name="input">The Polygon Raster(Grid file).</param>
        /// <param name="output">The Polygon shapefile path.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(IRaster input, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            return Execute(input, null, output, cancelProgressHandler);
        }

        /// <summary>
        /// Create polygons from raster.
        /// </summary>
        /// <param name="input">The Polygon Raster(Grid file).</param>
        /// <param name="connectionGrid">Connection Grid.</param>
        /// <param name="output">The Polygon shapefile path.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(IRaster input, IRaster connectionGrid, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            if (input == null || output == null)
            {
                return false;
            }

            output.DataTable.Columns.Add("Value", typeof(double));

            var featureHash = new Dictionary<double, LineSegmentList>();
            var previous = 0.0;

            var height = input.CellHeight;
            var width = input.CellWidth;

            var xMin = input.Xllcenter - width / 2.0;
            var yMin = input.Yllcenter - height / 2.0;
            var xMax = xMin + width * input.NumColumns;
            var yMax = yMin + height * input.NumRows;

            var numRows = input.NumRows;
            var numColumns = input.NumColumns;

            Func<int, int, double, bool> same = (y, x, value) => y >= 0 && y < numRows &&
                                                                 x >= 0 && x < numColumns &&
                                                                 input.Value[y, x] == value;

            var enableCon = connectionGrid != null;
            Func<int, int, int> connection = (y, x) => enableCon ? (int) connectionGrid.Value[y, x] : 0;
            
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

                    var curCon = connection(y, x);

                    /*  
                     6 7 8
                     5 0 1
                     4 3 2  
                 current cell (x,y)=0                             
                     */

                    var con_7 = same(y + 1, x, value);
                    var con_5 = same(y, x - 1, value);
                    var con_3 = same(y - 1, x, value);
                    var con_1 = same(y, x + 1, value);

                    var con_8 = enableCon &&
                                same(y + 1, x + 1, value) && !con_7 && !con_1 &&
                                (curCon == 8 || connection(y + 1, x + 1) == 4);
                    var con_6 = enableCon &&
                                same(y + 1, x - 1, value) && !con_7 && !con_5 &&
                                (curCon == 6 || connection(y + 1, x - 1) == 2);
                    var con_4 = enableCon &&
                                same(y - 1, x - 1, value) && !con_3 && !con_5 &&
                                (curCon == 4 || connection(y - 1, x - 1) == 8);
                    var con_2 = enableCon &&
                                same(y - 1, x + 1, value) && !con_3 && !con_1 &&
                                (curCon == 2 || connection(y - 1, x + 1) == 6);

                    /* Cell points:
                     tl tc tr
                     cl    cr 
                     bl bc br   
                     */
                    var tl = new Coordinate(xMin + x*width, yMax - (y + 1)*height);
                    var tc = new Coordinate(xMin + (x + 0.5)*width, yMax - (y + 1)*height);
                    var tr = new Coordinate(xMin + (x + 1)*width, yMax - (y + 1)*height);
                    var cl = new Coordinate(xMin + x*width, yMax - (y + 0.5)*height);
                    var cr = new Coordinate(xMin + (x + 1)*width, yMax - (y + 0.5)*height);
                    var bl = new Coordinate(xMin + x * width, yMax - y * height);
                    var bc = new Coordinate(xMin + (x + 0.5)*width, yMax - y*height);
                    var br = new Coordinate(xMin + (x + 1)*width, yMax - y*height);

                    bool topFull = false, rightFull = false, leftFull = false, bottomFull = false;

                    if (!con_7)
                    {
                        #region Check Cell 7

                        if (con_8 && con_5)
                        {
                            lineList.AddSegment(tl, tc); // top: left half
                        }
                        if (con_6 && con_1)
                        {
                            lineList.AddSegment(tc, tr); // top: right half
                        }

                        if (!con_8 && !con_6)
                        {
                            if (con_4 && !con_2)
                            {
                                lineList.AddSegment(tc, tr); // top: right half
                            }
                            else if (!con_4 && con_2)
                            {
                                lineList.AddSegment(tl, tc); // top: left half
                            }
                            else if (!con_4 && !con_2)
                            {
                                topFull = true; // add later
                            }
                        }

                        #endregion
                    }

                    if (!con_3)
                    {
                        #region Check Cell 3

                        if (con_2 && con_5)
                        {
                            lineList.AddSegment(bl, bc); // bottom: left half
                        }

                        if (con_4 && con_1)
                        {
                            lineList.AddSegment(bc, br); // bottom: right half
                        }

                        if (!con_2 && !con_4)
                        {
                            if (con_6 && !con_8)
                            {
                                lineList.AddSegment(bc, br); // bottom: right half
                            }
                            else if (!con_6 && con_8)
                            {
                                lineList.AddSegment(bl, bc); // bottom: left half
                            }
                            else if (!con_6 && !con_8)
                            {
                                bottomFull = true; // add later
                            }
                        }

                        #endregion
                    }

                    if (!con_1)
                    {
                        #region Check Cell 1

                        if (con_8 && con_3)
                        {
                            lineList.AddSegment(cr, br); //right: bottom half
                        }

                        if (con_2 && con_7)
                        {
                            lineList.AddSegment(tr, cr); // right: top half
                        }

                        if (!con_2 && !con_8)
                        {
                            if (con_4 && !con_6)
                            {
                                lineList.AddSegment(tr, cr); // right: top half
                            }
                            else if (!con_4 && con_6)
                            {
                                lineList.AddSegment(cr, br); //right: bottom half
                            }
                            else if (!con_4 && !con_6)
                            {
                                rightFull = true; // add later
                            }
                        }

                        #endregion
                    }

                    if (!con_5)
                    {
                        #region Check Cell 5

                        if (con_6 && con_3)
                        {
                            lineList.AddSegment(cl, bl); // left: bottom half
                        }
                        if (con_4 && con_7)
                        {
                            lineList.AddSegment(tl, cl); //left: top half
                        }
                        if (!con_6 && !con_4)
                        {
                            if (con_8 && !con_2)
                            {
                                lineList.AddSegment(cl, bl); // left: bottom half
                            }
                            else if (!con_8 && con_2)
                            {
                                lineList.AddSegment(tl, cl); //left: top half
                            }
                            else if (!con_8 && !con_2)
                            {
                                leftFull = true; // add later
                            }
                        }

                        #endregion
                    }

                    // Smooth borders
                    switch (curCon)
                    {
                   /*
                   tl tc tr
                   cl    cr 
                   bl bc br 
                   */
                        case 2:
                        case 6:
                            goto default; // todo: test this
                            if (topFull && rightFull && leftFull && bottomFull)
                            {
                                lineList.AddSegment(tl, tc);
                                lineList.AddSegment(tc, cr);
                                lineList.AddSegment(cr, br);
                                lineList.AddSegment(br, bc);
                                lineList.AddSegment(bc, cl);
                                lineList.AddSegment(cl, tl);
                                topFull = rightFull = leftFull = bottomFull = false;
                            }
                            else if (topFull && rightFull)
                            {
                                lineList.AddSegment(tl, tc);
                                lineList.AddSegment(tc, cr);
                                lineList.AddSegment(cr, br);
                                topFull = rightFull = false;
                            }
                            else if (bottomFull && leftFull)
                            {
                                lineList.AddSegment(br, bc);
                                lineList.AddSegment(bc, cl);
                                lineList.AddSegment(cl, tl);
                                bottomFull = leftFull = false;
                            }
                            goto default;
                        case 4:
                        case 8:
                            goto default; // todo: test this
                            if (topFull && rightFull && leftFull && bottomFull)
                            {
                                lineList.AddSegment(tr, cr);
                                lineList.AddSegment(cr, bc);
                                lineList.AddSegment(bc, bl);
                                lineList.AddSegment(bl, cl);
                                lineList.AddSegment(cl, tc);
                                lineList.AddSegment(tc, tr);
                                topFull = rightFull = leftFull = bottomFull = false;
                            }
                            else if (topFull && leftFull)
                            {
                                lineList.AddSegment(bl, cl);
                                lineList.AddSegment(cl, tc);
                                lineList.AddSegment(tc, tr);
                                topFull = leftFull = false;
                            }
                            else if (bottomFull && rightFull)
                            {
                                lineList.AddSegment(tr, cr);
                                lineList.AddSegment(cr, bc);
                                lineList.AddSegment(bc, bl);
                                bottomFull = rightFull = false;
                            }
                            goto default;
                        default:
                            if (topFull) lineList.AddSegment(tl, tr);
                            if (bottomFull) lineList.AddSegment(bl, br);
                            if (rightFull) lineList.AddSegment(tr, br);
                            if (leftFull) lineList.AddSegment(tl, bl);
                            break;
                    }
                    
                    // Right top out diagonals
                    if (con_8)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + (x + 1)*width, yMax - (y + 1 + 0.5)*height));
                        lineList.AddSegment(cr, new Coordinate(xMin + (x + 1 + 0.5)*width, yMax - (y + 1)*height));
                    }

                    // Right in diagonals
                    if (con_8 || con_4)
                    {
                        if (!con_6 && !con_7 && !con_5)
                        {
                            lineList.AddSegment(tc, cl);
                        }
                        if (!con_2 && !con_1 && !con_3)
                        {
                            lineList.AddSegment(cr, bc);
                        }
                    }

                    // Left Top out diagonals
                    if (con_6)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + x*width, yMax - (y + 1 + 0.5)*height));
                        lineList.AddSegment(cl, new Coordinate(xMin + (x - 0.5)*width, yMax - (y + 1)*height));
                    }

                    // Left In diagonals
                    if (con_6 || con_2)
                    {
                        if (!con_8 && !con_7 && !con_1)
                        {
                            lineList.AddSegment(tc, cr);
                        }
                        if (!con_4 && !con_5 && !con_3)
                        {
                            lineList.AddSegment(cl, bc);
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
                var ind = 0;
                while (ind != lineSegList.Count)
                {
                    var polyShell = new List<Coordinate>();

                    var start = lineSegList[ind++];
                    polyShell.Add(start.P0);
                    polyShell.Add(start.P1);

                    while (!polyShell[0].Equals2D(polyShell[polyShell.Count - 1]))
                    {
                        var last = polyShell[polyShell.Count - 1];
                        LineSegment segment = null;
                        for (int i = ind; i < lineSegList.Count; i++)
                        {
                            var cur = lineSegList[i];
                            if (cur.P0.Equals2D(last) || cur.P1.Equals2D(last))
                            {
                                segment = cur;
                                if (ind != i)
                                {
                                    var swap = lineSegList[ind];
                                    lineSegList[ind] = cur;
                                    lineSegList[i] = swap;
                                }

                                ind++;
                                break;
                            }
                        }
                        Debug.Assert(segment != null);
                        polyShell.Add(segment.P0.Equals2D(last) ? segment.P1 : segment.P0);
                    }

                    polyList.Add(new Polygon(polyShell));
                }

                var geometry = polyList.Count == 1
                                   ? (IBasicGeometry)polyList[0]
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

            public void AddSegment(Coordinate p0, Coordinate p1)
            {
                _list.Add(new LineSegment(p0, p1));
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