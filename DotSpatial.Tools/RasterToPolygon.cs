// -----------------------------------------------------------------------
// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterToPolygon.cs
// Description:  Convert a raster dataset to a polygon featureset.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
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
            Func<int, int, double> get = (y, x) => y >= 0 && y < numRows && x >= 0 && x < numColumns
                                                       ? input.Value[y, x]
                                                       : input.NoDataValue;
            Func<int, int, int, int, bool> eqValues = (y1, x1, y2, x2) => get(y1, x1) == get(y2, x2);

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

                    var con_8l = enableCon &&
                                same(y + 1, x + 1, value) && !con_7 &&
                                (curCon == 8 || connection(y + 1, x + 1) == 4);   
                    var con_8r = enableCon &&
                                same(y + 1, x + 1, value) && !con_1 &&
                                (curCon == 8 || connection(y + 1, x + 1) == 4);

                    var con_6l = enableCon &&
                                same(y + 1, x - 1, value) && !con_5 &&
                                (curCon == 6 || connection(y + 1, x - 1) == 2);
                    var con_6r = enableCon &&
                                same(y + 1, x - 1, value) && !con_7 &&
                                (curCon == 6 || connection(y + 1, x - 1) == 2);

                    var con_4l = enableCon &&
                                same(y - 1, x - 1, value) && !con_5 &&
                                (curCon == 4 || connection(y - 1, x - 1) == 8);
                    var con_4r = enableCon &&
                                same(y - 1, x - 1, value) && !con_3 &&
                                (curCon == 4 || connection(y - 1, x - 1) == 8);

                    var con_2l = enableCon &&
                                same(y - 1, x + 1, value) && !con_3 &&
                                (curCon == 2 || connection(y - 1, x + 1) == 6);
                    var con_2r = enableCon &&
                                same(y - 1, x + 1, value) && !con_1 &&
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

                    /*
                     Cell edges:
                    e_tl   e_tr
                     ----|----
               e_lt  |       | e_rt
                     -       -
               e_lb  |       | e_rb 
                     ----|---- 
                    e_bl   e_br
                     */

                    bool e_tr = false,
                         e_tl = false,
                         e_lt = false,
                         e_lb = false,
                         e_rt = false,
                         e_rb = false,
                         e_br = false,
                         e_bl = false;

                    if (!con_7)
                    {
                        #region Check Cell 7

                        // top: right half
                        if (!con_8l)
                        {
                            var a1 = con_6r && con_1;
                            var a2 = !con_8r && !con_6l && con_4l && !con_2r;
                            e_tr = a1 || a2;
                        }

                        // top: left half
                        if (!con_6r)
                        {
                            var a1 = con_8l && con_5;
                            var a2 = !con_8r && !con_6l && !con_4l && con_2r;
                            e_tl = a1 || a2;
                        }

                        // top: full
                        if (!con_8l && !con_6r && !con_4l && !con_2r)
                        {
                            e_tr = e_tl = true;
                        }
                       
                        #endregion
                    }

                    if (!con_3)
                    {
                        #region Check Cell 3

                        // bottom: left half
                        if (!con_4r)
                        {
                            var a1 = con_2l && con_5;
                            var a2 = !con_2r && !con_4l && !con_6l && con_8r;
                            e_bl = a1 || a2;
                        }

                        // bottom: right half
                        if (!con_2l)
                        {
                            var a1 = con_4r && con_1;
                            var a2 = !con_2r && !con_4l && !con_8r && con_6l;
                            e_br = a1 || a2;
                        }

                        // bottom: full
                        if (!con_4r && !con_2l && !con_8r && !con_6l)
                        {
                            e_bl = e_br = true;
                        }

                        #endregion
                    }

                    if (!con_1)
                    {
                        #region Check Cell 1

                        // right: bottom half
                        if (!con_2r)
                        {
                            var a1 = con_8r && con_3;
                            var a2 = !con_2l && !con_8l && !con_4r && con_6r;
                            e_rb = a1 || a2;
                        }

                        // right: top half
                        if (!con_8r)
                        {
                            var a1 = con_2r && con_7;
                            var a2 = !con_2l && !con_8l && !con_6r && con_4r;
                            e_rt = a1 || a2;
                        }

                        // right: full
                        if (!con_2r && !con_8r && !con_4r && !con_6r)
                        {
                            e_rb = e_rt = true;
                        }
                        
                        #endregion
                    }

                    if (!con_5)
                    {
                        #region Check Cell 5

                        // left: bottom half
                        if (!con_4l)
                        {
                            var a1 = con_3 && con_6l;
                            var a2 = !con_6r && !con_4r && con_8l && !con_2l;
                            e_lb = a1 || a2;
                        }

                        // left: top half
                        if (!con_6l)
                        {
                            var a1 = con_4l && con_7;
                            var a2 = !con_6r && !con_4r && !con_8l && con_2l;
                            e_lt = a1 || a2;
                        }

                        // left: full
                        if (!con_4l && !con_6l && !con_8l && !con_2l)
                        {
                            e_lb = e_lt = true;
                        }

                        #endregion
                    }

                    // Smooth boundaries regarding to outer cells
                    // Right top corner
                    if (e_tr && e_rt)
                    {
                        if (eqValues(y + 1, x, y, x + 1))
                        {
                            var a1 = connection(y + 1, x);
                            var a2 = connection(y, x + 1);
                            if ((a1 == 6 || a1 == 2 || a2 == 6 || a2 == 2) && !(a1 == 6 && a2 == 2))
                            {
                                lineList.AddSegment(tc, cr);
                                e_tr = e_rt = false;
                            }
                        }
                    }

                    // Left top corner
                    if (e_tl && e_lt)
                    {
                        if (eqValues(y, x - 1, y + 1, x))
                        {
                            var a1 = connection(y, x - 1);
                            var a2 = connection(y + 1, x);
                            if ((a1 == 8 || a1 == 4 || a2 == 8 || a2 == 4) && !(a1 == 8 && a2 == 4))
                            {
                                lineList.AddSegment(tc, cl);
                                e_tl = e_lt = false;
                            }
                        }
                    }

                    // Left bottom corner
                    if (e_bl && e_lb)
                    {
                        if (eqValues(y - 1, x, y, x - 1))
                        {
                            var a1 = connection(y - 1, x);
                            var a2 = connection(y, x - 1);
                            if ((a1 == 6 || a1 == 2 || a2 == 6 || a2 == 2) && !(a1 == 6 && a2 == 2))
                            {
                                lineList.AddSegment(cl, bc);
                                e_bl = e_lb = false;
                            }
                        }
                    }

                    // Right bottom corner
                    if (e_br && e_rb)
                    {
                        if (eqValues(y, x + 1, y - 1, x))
                        {
                            var a1 = connection(y, x + 1);
                            var a2 = connection(y - 1, x);
                            if ((a1 == 8 || a1 == 4 || a2 == 8 || a2 == 4) && !(a1 == 8 && a2 == 4))
                            {
                                lineList.AddSegment(bc, cr);
                                e_br = e_rb = false;
                            }
                        }
                    }

                    // Smooth boundaries regarding direction of current cell
                    switch (curCon)
                    {
                        case 2:
                        case 6:
                            if (e_tr && e_rt)
                            {
                                lineList.AddSegment(tc, cr);
                                e_tr = e_rt = false;
                            }
                            if (e_bl && e_lb)
                            {
                                lineList.AddSegment(bc, cl);
                                e_bl = e_lb = false;
                            }
                            break;
                        case 4:
                        case 8:
                            if (e_tl && e_lt)
                            {
                                lineList.AddSegment(cl, tc);
                                e_tl = e_lt = false;
                            }
                            if (e_br && e_rb)
                            {
                                lineList.AddSegment(cr, bc);
                                e_br = e_rb = false;
                            }
                            break;
                    }

                    // Add remaining edges
                    // Top
                    if (e_tl && e_tr)
                    {
                        lineList.AddSegment(tl, tr);
                    }
                    else if (e_tl)
                    {
                        lineList.AddSegment(tl, tc);
                    }
                    else if(e_tr)
                    {
                        lineList.AddSegment(tc, tr);
                    }

                    //Left
                    if (e_lt && e_lb)
                    {
                        lineList.AddSegment(tl, bl);
                    }
                    else if (e_lt)
                    {
                        lineList.AddSegment(tl, cl);
                    }
                    else if (e_lb)
                    {
                        lineList.AddSegment(cl, bl);
                    }

                    // Bottom
                    if (e_bl && e_br)
                    {
                        lineList.AddSegment(bl, br);
                    }
                    else if (e_bl)
                    {
                        lineList.AddSegment(bl, bc);
                    }
                    else if (e_br)
                    {
                        lineList.AddSegment(bc, br);
                    }

                    // Right
                    if (e_rt && e_rb)
                    {
                        lineList.AddSegment(tr, br);
                    }
                    else if (e_rt)
                    {
                        lineList.AddSegment(tr, cr);
                    }
                    else if (e_rb)
                    {
                        lineList.AddSegment(cr, br);
                    }

                    // Right top out diagonals
                    if (con_8l)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + (x + 1)*width, yMax - (y + 1 + 0.5)*height));
                    }
                    if (con_8r)
                    {
                        lineList.AddSegment(cr, new Coordinate(xMin + (x + 1 + 0.5) * width, yMax - (y + 1) * height));
                    }

                    // Right in diagonals
                    if (con_4l || con_8l)
                    {
                        if (!con_6r && !con_6l && !con_7 && !con_5)
                        {
                            lineList.AddSegment(tc, cl);
                        }
                    }
                    if (con_4r || con_8r)
                    {
                        if (!con_2r && !con_2l && !con_1 && !con_3)
                        {
                            lineList.AddSegment(cr, bc);
                        }
                    }

                    // Left Top out diagonals
                    if (con_6r)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + x*width, yMax - (y + 1 + 0.5)*height));
                    }
                    if (con_6l)
                    {
                        lineList.AddSegment(cl, new Coordinate(xMin + (x - 0.5) * width, yMax - (y + 1) * height));
                    }

                    // Left In diagonals
                    if (con_6r || con_2r)
                    {
                        if (!con_8r && !con_8l && !con_7 && !con_1)
                        {
                            lineList.AddSegment(tc, cr);
                        }
                    }
                    if (con_6l || con_2l)
                    {
                        if (!con_4r && !con_4l && !con_5 && !con_3)
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