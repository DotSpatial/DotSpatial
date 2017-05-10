// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterSlope.cs
// Description:  Generate slope raster from given altitude raster.
//
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// KP                     |  9/2009                |  Used IDW as model for RasterSlope
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster Slope
    /// </summary>
    public class RasterSlope : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSlope"/> class.
        /// </summary>
        public RasterSlope()
        {
            Name = TextStrings.SlopeRasterLayer;
            Category = TextStrings.TerrainAnalysis;
            Description = TextStrings.RasterSlopeDescription;
            ToolTip = TextStrings.GenerateslopeRasterLayer;
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
        /// <returns>True if the method worked.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            double inZFactor = (double)_inputParam[1].Value;

            bool slopeInPercent = (bool)_inputParam[2].Value;

            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, inZFactor, slopeInPercent, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="ras">The input altitude raster.</param>
        /// <param name="inZFactor">A multiplicative scaling factor to be applied to the elevation values before calculating the slope.</param>
        /// <param name="slopeInPercent">If this is true, the resulting slopes are returned as percentages.</param>
        /// <param name="output">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if the method worked.</returns>
        public bool Execute(IRaster ras, double inZFactor, bool slopeInPercent, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (ras == null || output == null)
            {
                return false;
            }

            try
            {
                int noOfCol = ras.NumColumns;
                int noOfRow = ras.NumRows;
                output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, typeof(double), new[] { string.Empty });
                output.NoDataValue = ras.NoDataValue;
                output.Bounds = ras.Bounds;

                int previous = 0;
                for (int i = 0; i < output.NumRows; i++)
                {
                    int current = Convert.ToInt32(Math.Round(i * 100D / output.NumRows));

                    // only update when increment in percentage
                    if (current > previous)
                    {
                        cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                    }

                    previous = current;

                    for (int j = 0; j < output.NumColumns; j++)
                    {
                        if (i > 0 && i < output.NumRows - 1 && j > 0 && j < output.NumColumns - 1)
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
                            double dZdX = inZFactor * ((z3 - z1) + (2 * (z5 - z4)) + (z8 - z6)) / (8 * ras.CellWidth);
                            double dZdY = inZFactor * ((z1 - z6) + (2 * (z2 - z7)) + (z3 - z8)) / (8 * ras.CellHeight);

                            double slope = Math.Atan(Math.Sqrt((dZdX * dZdX) + (dZdY * dZdY))) * (180 / Math.PI);

                            // change to radious and in persentage
                            if (slopeInPercent)
                            {
                                slope = Math.Tan(slope * Math.PI / 180) * 100;
                            }

                            output.Value[i, j] = slope;

                            if (cancelProgressHandler.Cancel)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            output.Value[i, j] = output.NoDataValue;
                        }

                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }

                if (output.IsFullyWindowed())
                {
                    output.Save();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.ToString());
            }
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
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
            _inputParam[2] = new BooleanParam(TextStrings.slopeinpercentage, TextStrings.boxSlopeInPercentage, false)
                                 {
                                     HelpText = TextStrings.slopeinpercentageindegree
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputslopeRaster)
                                  {
                                      HelpText = TextStrings.Resultofaverageslope
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}