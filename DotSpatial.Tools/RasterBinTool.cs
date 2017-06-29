// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterBinTool.cs
// Description:  Generate slope raster from given altitude raster.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.Windows.Forms;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster Slope
    /// </summary>
    public class RasterBinTool : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the RasterBinTool class.
        /// </summary>
        public RasterBinTool()
        {
            this.Name = "Raster Bin";
            this.Category = TextStrings.TerrainAnalysis;
            this.Description =
                "This bins the values of a raster so that there will be large regions of equal values. This will replace the raster value with the middle value of the bin range that it falls with.  The baseValue represents the starting point of the first boundary, while the bin size represents the total distance between raster values.  So for instance, an interval of every 20, starting at 10 would have a bin size of 20, and a base value of 10.  The cells ranging from 10 to 30 would all get the half value of 20.";
            this.ToolTip = "Bins the continuous values into histogram like categories.";
            this.Author = "Ted Dunsford?";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input parameter array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output parameter array
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
            IRaster input1 = _inputParam[0].Value as IRaster;
            double baseValue = (double)_inputParam[1].Value;
            double binSize = (double)_inputParam[2].Value;
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, baseValue, binSize, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="source">The input altitude raster.</param>
        /// <param name="baseValue">The double precision base value.</param>
        /// <param name="binSize">The double size of the output bin.</param>
        /// <param name="result">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if the method worked.</returns>
        public bool Execute(
            IRaster source,
            double baseValue,
            double binSize,
            IRaster result,
            ICancelProgressHandler cancelProgressHandler)
        {
            RasterBin rasterBin = new RasterBin { BaseValue = baseValue, BinSize = binSize };

            try
            {
                return rasterBin.BinRaster(source, result.Filename, cancelProgressHandler);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("The Execute method failed for RasterBin tool: " + ex.Message);
                MessageBox.Show("The RasterBin tool failed to complete successfully.");
                return false;
            }
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
            _inputParam[1] = new DoubleParam("Base value", 0)
                                 {
                                     HelpText =
                                         "The starting point for bin calculations.  A base value of 10 with a bin size of 20 would give all values from 10 to 30 a value of 20."
                                 };
            _inputParam[2] = new DoubleParam("Bin size", 10)
                                 {
                                     HelpText =
                                         "The size of the bins.  A base value of 10 with a bin size of 20 would give all the values from 10 to 30 a value of 20."
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputslopeRaster)
                                  {
                                      HelpText = TextStrings.Resultofaverageslope
                                  };
        }

        /// <summary>
        /// Fires when one of the parameters value has been changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        public override void ParameterChanged(Parameter sender)
        {
            return;
        }

        #endregion
    }
}