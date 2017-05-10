// *******************************************************************************************************
// Product: DotSpatial.Tools.RandomGeometry
// Description:  Tool that clips a raster layer with a polygon.
//
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for ClipwithPolygon
// Ping               |  12/2009           |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Clip With Polygon
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
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster raster = _inputParam[0].Value as IRaster;
            IFeatureSet polygon = _inputParam[1].Value as IFeatureSet;
            IRaster output = _outputParam[0].Value as IRaster;

            // Validates the input and output data
            if (raster == null || polygon == null || output == null) return false;

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