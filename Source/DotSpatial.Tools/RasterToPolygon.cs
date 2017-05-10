// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterToPolygon.cs
// Description:  Convert a raster dataset to a polygon featureset.
//
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
// -------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
// -----------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some unnecessary references using re-sharper
// KP                     |  9/2009                |  Used IDW as model for RasterToPolygon
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster to Polygon tool
    /// </summary>
    public class RasterToPolygon : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterToPolygon"/> class.
        /// </summary>
        public RasterToPolygon()
        {
            Name = TextStrings.RasterToPolygon;
            Category = TextStrings.Conversion;
            Description = TextStrings.RasterToPolygonDescription;
            ToolTip = TextStrings.RasterToPolygonDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
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
        /// <returns>True, if executed successfully.</returns>
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
        /// <returns>True, if executed successfully.</returns>
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

            var xMin = input.Xllcenter - (width / 2.0);
            var yMin = input.Yllcenter - (height / 2.0);
            var xMax = xMin + (width * input.NumColumns);
            var yMax = yMin + (height * input.NumRows);

            var numRows = input.NumRows;
            var numColumns = input.NumColumns;

            Func<int, int, double, bool> same = (y, x, value) => y >= 0 && y < numRows && x >= 0 && x < numColumns && input.Value[y, x] == value;
            Func<int, int, double> get = (y, x) => y >= 0 && y < numRows && x >= 0 && x < numColumns ? input.Value[y, x] : input.NoDataValue;
            Func<int, int, int, int, bool> eqValues = (y1, x1, y2, x2) => get(y1, x1) == get(y2, x2);

            var enableCon = connectionGrid != null;
            Func<int, int, int> connection = (y, x) => connectionGrid != null ? (int)connectionGrid.Value[y, x] : 0;

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
                    var con7 = same(y + 1, x, value);
                    var con5 = same(y, x - 1, value);
                    var con3 = same(y - 1, x, value);
                    var con1 = same(y, x + 1, value);

                    var con8L = enableCon && same(y + 1, x + 1, value) && !con7 && (curCon == 8 || connection(y + 1, x + 1) == 4);
                    var con8R = enableCon && same(y + 1, x + 1, value) && !con1 && (curCon == 8 || connection(y + 1, x + 1) == 4);

                    var con6L = enableCon && same(y + 1, x - 1, value) && !con5 && (curCon == 6 || connection(y + 1, x - 1) == 2);
                    var con6R = enableCon && same(y + 1, x - 1, value) && !con7 && (curCon == 6 || connection(y + 1, x - 1) == 2);

                    var con4L = enableCon && same(y - 1, x - 1, value) && !con5 && (curCon == 4 || connection(y - 1, x - 1) == 8);
                    var con4R = enableCon && same(y - 1, x - 1, value) && !con3 && (curCon == 4 || connection(y - 1, x - 1) == 8);

                    var con2L = enableCon && same(y - 1, x + 1, value) && !con3 && (curCon == 2 || connection(y - 1, x + 1) == 6);
                    var con2R = enableCon && same(y - 1, x + 1, value) && !con1 && (curCon == 2 || connection(y - 1, x + 1) == 6);

                    /* Cell points:
                     tl tc tr
                     cl    cr
                     bl bc br
                     */
                    var tl = new Coordinate(xMin + (x * width), yMax - ((y + 1) * height));
                    var tc = new Coordinate(xMin + ((x + 0.5) * width), yMax - ((y + 1) * height));
                    var tr = new Coordinate(xMin + ((x + 1) * width), yMax - ((y + 1) * height));
                    var cl = new Coordinate(xMin + (x * width), yMax - ((y + 0.5) * height));
                    var cr = new Coordinate(xMin + ((x + 1) * width), yMax - ((y + 0.5) * height));
                    var bl = new Coordinate(xMin + (x * width), yMax - (y * height));
                    var bc = new Coordinate(xMin + ((x + 0.5) * width), yMax - (y * height));
                    var br = new Coordinate(xMin + ((x + 1) * width), yMax - (y * height));

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
                    bool eTr = false, eTl = false, eLt = false, eLb = false, eRt = false, eRb = false, eBr = false, eBl = false;

                    if (!con7)
                    {
                        // top: right half
                        if (!con8L)
                        {
                            var a1 = con6R && con1;
                            var a2 = !con8R && !con6L && con4L && !con2R;
                            eTr = a1 || a2;
                        }

                        // top: left half
                        if (!con6R)
                        {
                            var a1 = con8L && con5;
                            var a2 = !con8R && !con6L && !con4L && con2R;
                            eTl = a1 || a2;
                        }

                        // top: full
                        if (!con8L && !con6R && !con4L && !con2R)
                        {
                            eTr = eTl = true;
                        }
                    }

                    if (!con3)
                    {
                        // bottom: left half
                        if (!con4R)
                        {
                            var a1 = con2L && con5;
                            var a2 = !con2R && !con4L && !con6L && con8R;
                            eBl = a1 || a2;
                        }

                        // bottom: right half
                        if (!con2L)
                        {
                            var a1 = con4R && con1;
                            var a2 = !con2R && !con4L && !con8R && con6L;
                            eBr = a1 || a2;
                        }

                        // bottom: full
                        if (!con4R && !con2L && !con8R && !con6L)
                        {
                            eBl = eBr = true;
                        }
                    }

                    if (!con1)
                    {
                        // right: bottom half
                        if (!con2R)
                        {
                            var a1 = con8R && con3;
                            var a2 = !con2L && !con8L && !con4R && con6R;
                            eRb = a1 || a2;
                        }

                        // right: top half
                        if (!con8R)
                        {
                            var a1 = con2R && con7;
                            var a2 = !con2L && !con8L && !con6R && con4R;
                            eRt = a1 || a2;
                        }

                        // right: full
                        if (!con2R && !con8R && !con4R && !con6R)
                        {
                            eRb = eRt = true;
                        }
                    }

                    if (!con5)
                    {
                        // left: bottom half
                        if (!con4L)
                        {
                            var a1 = con3 && con6L;
                            var a2 = !con6R && !con4R && con8L && !con2L;
                            eLb = a1 || a2;
                        }

                        // left: top half
                        if (!con6L)
                        {
                            var a1 = con4L && con7;
                            var a2 = !con6R && !con4R && !con8L && con2L;
                            eLt = a1 || a2;
                        }

                        // left: full
                        if (!con4L && !con6L && !con8L && !con2L)
                        {
                            eLb = eLt = true;
                        }
                    }

                    // Smooth boundaries regarding to outer cells
                    // Right top corner
                    if (eTr && eRt)
                    {
                        if (eqValues(y + 1, x, y, x + 1))
                        {
                            var a1 = connection(y + 1, x);
                            var a2 = connection(y, x + 1);
                            if ((a1 == 6 || a1 == 2 || a2 == 6 || a2 == 2) && !(a1 == 6 && a2 == 2))
                            {
                                lineList.AddSegment(tc, cr);
                                eTr = eRt = false;
                            }
                        }
                    }

                    // Left top corner
                    if (eTl && eLt)
                    {
                        if (eqValues(y, x - 1, y + 1, x))
                        {
                            var a1 = connection(y, x - 1);
                            var a2 = connection(y + 1, x);
                            if ((a1 == 8 || a1 == 4 || a2 == 8 || a2 == 4) && !(a1 == 8 && a2 == 4))
                            {
                                lineList.AddSegment(tc, cl);
                                eTl = eLt = false;
                            }
                        }
                    }

                    // Left bottom corner
                    if (eBl && eLb)
                    {
                        if (eqValues(y - 1, x, y, x - 1))
                        {
                            var a1 = connection(y - 1, x);
                            var a2 = connection(y, x - 1);
                            if ((a1 == 6 || a1 == 2 || a2 == 6 || a2 == 2) && !(a1 == 6 && a2 == 2))
                            {
                                lineList.AddSegment(cl, bc);
                                eBl = eLb = false;
                            }
                        }
                    }

                    // Right bottom corner
                    if (eBr && eRb)
                    {
                        if (eqValues(y, x + 1, y - 1, x))
                        {
                            var a1 = connection(y, x + 1);
                            var a2 = connection(y - 1, x);
                            if ((a1 == 8 || a1 == 4 || a2 == 8 || a2 == 4) && !(a1 == 8 && a2 == 4))
                            {
                                lineList.AddSegment(bc, cr);
                                eBr = eRb = false;
                            }
                        }
                    }

                    // Smooth boundaries regarding direction of current cell
                    switch (curCon)
                    {
                        case 2:
                        case 6:
                            if (eTr && eRt)
                            {
                                lineList.AddSegment(tc, cr);
                                eTr = eRt = false;
                            }

                            if (eBl && eLb)
                            {
                                lineList.AddSegment(bc, cl);
                                eBl = eLb = false;
                            }

                            break;
                        case 4:
                        case 8:
                            if (eTl && eLt)
                            {
                                lineList.AddSegment(cl, tc);
                                eTl = eLt = false;
                            }

                            if (eBr && eRb)
                            {
                                lineList.AddSegment(cr, bc);
                                eBr = eRb = false;
                            }

                            break;
                    }

                    // Add remaining edges
                    // Top
                    if (eTl && eTr)
                    {
                        lineList.AddSegment(tl, tr);
                    }
                    else if (eTl)
                    {
                        lineList.AddSegment(tl, tc);
                    }
                    else if (eTr)
                    {
                        lineList.AddSegment(tc, tr);
                    }

                    // Left
                    if (eLt && eLb)
                    {
                        lineList.AddSegment(tl, bl);
                    }
                    else if (eLt)
                    {
                        lineList.AddSegment(tl, cl);
                    }
                    else if (eLb)
                    {
                        lineList.AddSegment(cl, bl);
                    }

                    // Bottom
                    if (eBl && eBr)
                    {
                        lineList.AddSegment(bl, br);
                    }
                    else if (eBl)
                    {
                        lineList.AddSegment(bl, bc);
                    }
                    else if (eBr)
                    {
                        lineList.AddSegment(bc, br);
                    }

                    // Right
                    if (eRt && eRb)
                    {
                        lineList.AddSegment(tr, br);
                    }
                    else if (eRt)
                    {
                        lineList.AddSegment(tr, cr);
                    }
                    else if (eRb)
                    {
                        lineList.AddSegment(cr, br);
                    }

                    // Right top out diagonals
                    if (con8L)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + ((x + 1) * width), yMax - ((y + 1 + 0.5) * height)));
                    }

                    if (con8R)
                    {
                        lineList.AddSegment(cr, new Coordinate(xMin + ((x + 1 + 0.5) * width), yMax - ((y + 1) * height)));
                    }

                    // Right in diagonals
                    if (con4L || con8L)
                    {
                        if (!con6R && !con6L && !con7 && !con5)
                        {
                            lineList.AddSegment(tc, cl);
                        }
                    }

                    if (con4R || con8R)
                    {
                        if (!con2R && !con2L && !con1 && !con3)
                        {
                            lineList.AddSegment(cr, bc);
                        }
                    }

                    // Left Top out diagonals
                    if (con6R)
                    {
                        lineList.AddSegment(tc, new Coordinate(xMin + (x * width), yMax - ((y + 1 + 0.5) * height)));
                    }

                    if (con6L)
                    {
                        lineList.AddSegment(cl, new Coordinate(xMin + ((x - 0.5) * width), yMax - ((y + 1) * height)));
                    }

                    // Left In diagonals
                    if (con6R || con2R)
                    {
                        if (!con8R && !con8L && !con7 && !con1)
                        {
                            lineList.AddSegment(tc, cr);
                        }
                    }

                    if (con6L || con2L)
                    {
                        if (!con4R && !con4L && !con5 && !con3)
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

            foreach (var pair in featureHash)
            {
#if DEBUG
                var sw = new Stopwatch();
                sw.Start();
#endif
                var key = pair.Key;
                var lineSegList = pair.Value.List;

                var polyList = new List<IPolygon>();
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

                        Debug.Assert(segment != null, "Segment may not be null");
                        polyShell.Add(segment.P0.Equals2D(last) ? segment.P1 : segment.P0);
                    }

                    polyList.Add(new Polygon(new LinearRing(polyShell.ToArray())));
                }

                IGeometry geometry = polyList.Count == 1 ? (IGeometry)polyList[0] : new MultiPolygon(polyList.ToArray());
                var f = output.AddFeature(geometry);
                f.DataRow["Value"] = key;

#if DEBUG
                sw.Stop();
                Debug.WriteLine(sw.ElapsedMilliseconds);
#endif
            }

            output.AttributesPopulated = true;
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster)
                                 {
                                     HelpText = TextStrings.inputrastetoconvert
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.Convertedfeatureset)
                                  {
                                      HelpText = TextStrings.featuresetcreated
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion

        #region Classes

        private class LineSegmentList
        {
            #region Properties

            public List<LineSegment> List { get; } = new List<LineSegment>();

            #endregion

            #region Methods

            public void AddSegment(Coordinate p0, Coordinate p1)
            {
                List.Add(new LineSegment(p0, p1));
            }

            #endregion
        }

        #endregion
    }
}