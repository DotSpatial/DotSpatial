// *******************************************************************************************************
// Product: DotSpatial.Tools.FindAverageSlopeExtended.cs
// Description:  Calculate Average Slope for given polygons with more user preferences
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for FindAverageSlopeExtented
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools.Terrain_Analysis
{
    /// <summary>
    /// Find the slope
    /// </summary>
    public class FindAverageSlopeExtented : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the FindAverageSlopeExtented class.
        /// </summary>
        public FindAverageSlopeExtented()
        {
            this.Name = TextStrings.FindAverageSlope;
            this.Category = TextStrings.TerrainAnalysis;
            this.Description = TextStrings.FindAverageSlopeExtentedDescription;
            this.ToolTip = TextStrings.FindAvrageSlopeExtented;
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
            bool slopeInPercent = (bool)_inputParam[2].Value;
            IFeatureSet poly = _inputParam[3].Value as IFeatureSet;
            IFeatureSet outerShpFile = _inputParam[4].Value as IFeatureSet;
            int outerShpIndex = (int)_inputParam[5].Value;
            string fldInPolyToStoreSlope = (string)_inputParam[6].Value;

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(
                grid,
                inZFactor,
                slopeInPercent,
                poly,
                fldInPolyToStoreSlope,
                outerShpFile,
                outerShpIndex,
                output,
                cancelProgressHandler);
        }

        /// <summary>
        /// Finds the average slope in the given polygons with more user preferences.
        /// </summary>
        /// <param name="ras">The dem Raster(Grid file).</param>
        /// <param name="inZFactor">The scaler factor</param>
        /// <param name="slopeInPercent">The slope in percentage.</param>
        /// <param name="poly">The flow poly shapefile path.</param>
        /// <param name="fldInPolyToStoreSlope">The field name to store average slope in the attribute.</param>
        /// <param name="outerShpFile">The Featureset where we have the area of interest</param>
        /// <param name="outerShpIndex">The index of featureset which give paticular area of interest.</param>
        /// <param name="output">The path to save created slope Feature set.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(
            IRaster ras,
            double inZFactor,
            bool slopeInPercent,
            IFeatureSet poly,
            string fldInPolyToStoreSlope,
            IFeatureSet outerShpFile,
            int outerShpIndex,
            IFeatureSet output,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (ras == null || poly == null || outerShpFile == null || output == null)
            {
                return false;
            }

            if (poly.FeatureType != FeatureType.Polygon || outerShpFile.FeatureType != FeatureType.Polygon)
            {
                return false;
            }

            int previous = 0;
            IRaster slopegrid = new Raster();

            int[] areaCount = new int[poly.Features.Count];
            double[] areaTotal = new double[poly.Features.Count];
            double[] areaAve = new double[poly.Features.Count];

            Slope(ras, inZFactor, slopeInPercent, slopegrid, cancelProgressHandler);
            if (slopegrid == null)
            {
                throw new SystemException(TextStrings.Slopegridfileisnull);
            }

            foreach (IFeature f in poly.Features)
            {
                output.Features.Add(f);
            }

            for (int i = 0; i < slopegrid.NumRows; i++)
            {
                int current = Convert.ToInt32(Math.Round(i * 100D / slopegrid.NumRows));

                // only update when increment in percentage
                if (current > previous + 5)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                    previous = current;
                }

                for (int j = 0; j < slopegrid.NumColumns; j++)
                {
                    Coordinate coordin = slopegrid.CellToProj(i, j);
                    IPoint pt = new Point(coordin);
                    IFeature point = new Feature(pt);
                    if (!outerShpFile.Features[outerShpIndex].Covers(point))
                    {
                        continue; // not found the point inside.
                    }

                    for (int c = 0; c < poly.Features.Count; c++)
                    {
                        if (output.Features[c].Covers(point))
                        {
                            areaCount[c]++;
                            areaTotal[c] += slopegrid.Value[i, j] / 100;
                        }

                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            // Add the column
            output.DataTable.Columns.Add("FID", typeof(int));
            output.DataTable.Columns.Add(fldInPolyToStoreSlope, typeof(Double));
            for (int c = 0; c < output.Features.Count; c++)
            {
                if (areaCount[c] == 0)
                {
                    areaAve[c] = 0.0;
                }
                else
                {
                    areaAve[c] = areaTotal[c] / areaCount[c];
                }

                // Add the field values
                output.Features[c].DataRow["FID"] = c;
                output.Features[c].DataRow[fldInPolyToStoreSlope] = areaAve[c];
            }

            output.SaveAs(output.Filename, true);
            slopegrid.Close();
            ras.Close();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[7];
            _inputParam[0] = new RasterParam(TextStrings.input1altitudeRaster)
                                 {
                                     HelpText = TextStrings.InputRasterforaverageslopecalculation
                                 };
            _inputParam[1] = new DoubleParam(TextStrings.inputZfactor, 1.0)
                                 {
                                     HelpText = TextStrings.InputZfactorforslopedisplay
                                 };
            _inputParam[2] = new BooleanParam(TextStrings.slopeinpercentage, TextStrings.boxSlopeInPercentage, false)
                                 {
                                     HelpText = TextStrings.slopeinpercentage
                                 };
            _inputParam[3] = new FeatureSetParam(TextStrings.input1polygonfeatureset)
                                 {
                                     HelpText = TextStrings.averageslopeinarribute
                                 };
            _inputParam[4] = new FeatureSetParam(TextStrings.inputtheareaofinterest)
                                 {
                                     HelpText = TextStrings.featuresetcontainareainterest
                                 };
            _inputParam[5] = new IntParam(TextStrings.Indexofareaofinterestfeature, 0)
                                 {
                                     HelpText = TextStrings.indexspecificarea
                                 };
            _inputParam[6] = new StringParam(TextStrings.Fieldnameforavrageslope, TextStrings.AveSlope)
                                 {
                                     HelpText = TextStrings.Fieldnamecolomavrageslope
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.Outputwithaverageslope)
                                  {
                                      HelpText = TextStrings.SelecttheResultofOutput
                                  };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="ras">The input altitude raster.</param>
        /// <param name="inZFactor">A multiplicative scaling factor to be applied to the elevation values before calculating the slope.</param>
        /// <param name="slopeInPercent">If this is true, the slope is returned as a percentage.</param>
        /// <param name="result">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        private static void Slope(
            IRaster ras,
            double inZFactor,
            bool slopeInPercent,
            IRaster result,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (ras == null || result == null)
            {
                return;
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
                                return;
                            }
                        }
                        else
                        {
                            temp.Value[i, j] = temp.NoDataValue;
                        }

                        if (cancelProgressHandler.Cancel)
                        {
                            return;
                        }
                    }
                }

                result = temp;
                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return;
                }

                return;
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