// *******************************************************************************************************
// Product: DotSpatial.Tools.FindAverageSlope.cs
// Description:  Find the average slope in the given polygon.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for FindAverageSlope
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// A tool for finding the average slope
    /// </summary>
    public class FindAverageSlope : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the FindAverageSlope class.
        /// </summary>
        public FindAverageSlope()
        {
            this.Name = TextStrings.FindAverageSlope;
            this.Category = TextStrings.TerrainAnalysis;
            this.Description = TextStrings.FindAverageSlopeDescription;
            this.ToolTip = TextStrings.CalculateSlopegivenpolygons;
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
            IRaster grid = _inputParam[0].Value as IRaster;
            double inZFactor = (double)_inputParam[1].Value;
            IFeatureSet poly = _inputParam[2].Value as IFeatureSet;

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(grid, inZFactor, poly, output, cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons.
        /// </summary>
        /// <param name="ras">The dem Raster(Grid file).</param>
        /// <param name="zFactor">The scaler factor</param>
        /// <param name="poly">The flow poly shapefile path.</param>
        /// <param name="output">The resulting DEM of slopes</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        public bool Execute(
            IRaster ras,
            double zFactor,
            IFeatureSet poly,
            IFeatureSet output,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (ras == null || poly == null || output == null)
            {
                return false;
            }

            output.FeatureType = poly.FeatureType;
            foreach (IFeature f in poly.Features)
            {
                output.Features.Add(f);
            }

            output.DataTable.Columns.Add("FID", typeof(int));
            output.DataTable.Columns.Add(TextStrings.AveSlope, typeof(Double));

            IRaster slopeGrid = new Raster { DataType = ras.DataType, Bounds = ras.Bounds };

            // FeatureSet polyShape = new FeatureSet();
            int previous = 0;

            if (Slope(ref ras, zFactor, false, ref slopeGrid, cancelProgressHandler) == false)
            {
                return false;
            }

            int shapeCount = output.Features.Count;
            int[] areaCount = new int[shapeCount];
            double[] areaTotal = new double[shapeCount];
            double[] areaAve = new double[shapeCount];
            double dxHalf = slopeGrid.CellWidth / 2;
            double dyHalf = slopeGrid.CellHeight / 2;

            // check whether those two envelope are intersect
            if (ras.Extent.Intersects(output.Extent) == false)
            {
                return false;
            }

            RcIndex start = slopeGrid.ProjToCell(output.Extent.MinX, output.Extent.MaxY);
            RcIndex stop = slopeGrid.ProjToCell(output.Extent.MaxX, output.Extent.MinY);

            int rowStart = start.Row;
            int colStart = start.Column;
            int rowStop = stop.Row;
            int colStop = stop.Column;
            for (int row = rowStart - 1; row < rowStop + 1; row++)
            {
                int current = Convert.ToInt32((row - rowStart + 1) * 100.0 / (rowStop + 1 - rowStart + 1));

                // only update when increment in percentage
                if (current > previous + 5)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                    previous = current;
                }

                for (int col = colStart - 1; col < colStop + 1; col++)
                {
                    Coordinate cent = slopeGrid.CellToProj(row, col);
                    double xCent = cent.X;
                    double yCent = cent.Y;
                    for (int shpindx = 0; shpindx < output.Features.Count; shpindx++)
                    {
                        IFeature tempFeat = output.Features[shpindx];
                        Point pt1 = new Point(xCent, yCent);
                        Point pt2 = new Point(xCent - dxHalf, yCent - dyHalf);
                        Point pt3 = new Point(xCent + dxHalf, yCent - dyHalf);
                        Point pt4 = new Point(xCent + dxHalf, yCent + dyHalf);
                        Point pt5 = new Point(xCent - dxHalf, yCent + dyHalf);
                        if ((((!tempFeat.Covers(pt1) && !tempFeat.Covers(pt2)) && !tempFeat.Covers(pt3))
                             && !tempFeat.Covers(pt4)) && !tempFeat.Covers(pt5))
                        {
                            continue;
                        }

                        areaCount[shpindx]++;
                        areaTotal[shpindx] += slopeGrid.Value[row, col] / 100;

                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            for (int shpindx = 0; shpindx < output.Features.Count; shpindx++)
            {
                if (areaCount[shpindx] == 0)
                {
                    areaAve[shpindx] = 0;
                }
                else
                {
                    areaAve[shpindx] = areaTotal[shpindx] / areaCount[shpindx];
                }

                output.Features[shpindx].DataRow["FID"] = shpindx;
                output.Features[shpindx].DataRow[TextStrings.AveSlope] = areaAve[shpindx];
            }

            poly.Close();
            slopeGrid.Close();
            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.input1altitudeRaster)
                                 {
                                     HelpText = TextStrings.InputRasterforaverageslopecalculation
                                 };
            _inputParam[1] = new DoubleParam(TextStrings.inputZfactor, 1.0)
                                 {
                                     HelpText = TextStrings.InputZfactorforslopedisplay
                                 };
            _inputParam[2] = new PolygonFeatureSetParam(TextStrings.input1polygonfeatureset)
                                 {
                                     HelpText = TextStrings.FindAverageSlopeDescription
                                 };

            // _inputParam[2] = new FeatureSetParam(TextStrings."input1 polygon feature set");
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.Outputfeaturesetwithaverageslope)
                                  {
                                      HelpText = TextStrings.Resultofaverageslope
                                  };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="ras">The input altitude raster.</param>
        /// <param name="inZFactor">The multiplicitive scaling factor for elveation.</param>
        /// <param name="slopeInPercent">Boolean that is true if the slope values should be returned as percentages.</param>
        /// <param name="result">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        private static bool Slope(
            ref IRaster ras,
            double inZFactor,
            bool slopeInPercent,
            ref IRaster result,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (ras == null || result == null)
            {
                return false;
            }

            try
            {
                int noOfCol = ras.NumColumns;
                int noOfRow = ras.NumRows;

                // Create the new raster with the appropriate dimensions
                IRaster temp = Raster.CreateRaster(
                    "SlopeRaster.bgd", string.Empty, noOfCol, noOfRow, 1, typeof(double), new[] { string.Empty });
                temp.NoDataValue = ras.NoDataValue;
                temp.Bounds = ras.Bounds;

                for (int i = 0; i < temp.NumRows; i++)
                {
                    for (int j = 0; j < temp.NumColumns; j++)
                    {
                        if (i > 0 && i < temp.NumRows - 1 && j > 0 && j < temp.NumColumns - 1)
                        {
                            double z1 = ras.Value[i - 1, j - 1];
                            double z2 = ras.Value[i - 1, j];
                            double z3 = ras.Value[i - 1, j + 1];
                            double z4 = ras.Value[i, j - 1];
                            double z5 = ras.Value[i, j + 1];
                            double z6 = ras.Value[i + 1, j - 1];
                            double z7 = ras.Value[i + 1, j];
                            double z8 = ras.Value[i + 1, j + 1];

                            // 3rd Order Finite Difference slope algorithm
                            double dZdX = inZFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6)) / (8 * ras.CellWidth);
                            double dZdY = inZFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8)) / (8 * ras.CellHeight);

                            double slope = Math.Atan(Math.Sqrt((dZdX * dZdX) + (dZdY * dZdY))) * (180 / Math.PI);

                            // change to radious and in persentage
                            if (slopeInPercent)
                            {
                                slope = Math.Tan(slope * Math.PI / 180) * 100;
                            }

                            temp.Value[i, j] = slope;

                            if (cancelProgressHandler.Cancel)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            temp.Value[i, j] = temp.NoDataValue;
                        }

                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }

                result = temp;
                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // throw new SystemException("Error in Slope: ", ex);
                throw new SystemException(ex.ToString());
            }
        }

        #endregion
    }
}