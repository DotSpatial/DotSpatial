// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Generate slope raster from given altitude raster.
    /// </summary>
    public class RasterBinTool : Tool
    {
        #region Fields
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterBinTool"/> class.
        /// </summary>
        public RasterBinTool()
        {
            Name = TextStrings.RasterBinTool_Name;
            Category = TextStrings.RasterOverlay;
            UpdateToolResources();
            Author = "Ted Dunsford?";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input parameter array
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output parameter array
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
        public bool Execute(IRaster source, double baseValue, double binSize, IRaster result, ICancelProgressHandler cancelProgressHandler)
        {
            RasterBin rasterBin = new RasterBin
            {
                BaseValue = baseValue,
                BinSize = binSize
            };

            try
            {
                return rasterBin.BinRaster(source, result.Filename, cancelProgressHandler);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("The Execute method failed for RasterBin tool: " + ex.Message);
                MessageBox.Show(TextStrings.RasterBinTool_Execute_FailedToCompleteSuccessfully);
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
                HelpText = TextStrings.RasterBinTool_Initialize_BaseValueHelText
            };
            _inputParam[2] = new DoubleParam("Bin size", 10)
            {
                HelpText = TextStrings.RasterBinTool_Initialize_BinSizeHelpText
            };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputslopeRaster)
            {
                HelpText = TextStrings.Resultofaverageslope
            };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the parameters value has been changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
        }

        /// <summary>
        /// Attempt to update the tool's resources.
        /// </summary>
        public override void UpdateToolResources()
        {
            NameLabel = TextStrings.RasterBinTool_Label;
            CategoryLabel = TextStrings.RasterOverlay_Label;
            CategoryToolTip = TextStrings.RasterOverlay_ToolTip;
            ToolTip = TextStrings.RasterBinTool_ToolTip;
        }

        #endregion
    }
}