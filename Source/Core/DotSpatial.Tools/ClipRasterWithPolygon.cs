// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Tool that clips a raster layer with a polygon.
    /// </summary>
    public class ClipRasterWithPolygon : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipRasterWithPolygon"/> class.
        /// </summary>
        public ClipRasterWithPolygon()
        {
            Name = TextStrings.ClipRasterLayer;
            Category = TextStrings.Analysis;
            Description = TextStrings.ClipGridswithPolygon;
            ToolTip = TextStrings.ClipRasterLayerwithPolygon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array.
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (_inputParam[0].Value is not IRaster raster || _inputParam[1].Value is not IFeatureSet polygon || _outputParam[0].Value is not IRaster output) return false;

            ClipRaster.ClipRasterWithPolygon(polygon.Features[0], raster, output.Filename, cancelProgressHandler);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.input1Raster)
                                 {
                                     HelpText = TextStrings.InputRasterforCliping
                                 };
            _inputParam[1] = new PolygonFeatureSetParam(TextStrings.input2PolygonforCliping)
                                 {
                                     HelpText = TextStrings.InputPolygonforclipingtoRaster
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
                                  {
                                      HelpText = TextStrings.ResultRasterDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}