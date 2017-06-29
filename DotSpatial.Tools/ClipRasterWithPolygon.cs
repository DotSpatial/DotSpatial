// *******************************************************************************************************
// Product: DotSpatial.Tools.RandomGeometry
// Description:  Tool that clips a raster layer with a polygon.
// Copyright & License: See www.DotSpatial.org.
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

namespace DotSpatial.Tools
{
    /// <summary>
    /// Clip With Polygon
    /// </summary>
    public class ClipRasterWithPolygon : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ClipRasterWithPolygon class.
        /// </summary>
        public ClipRasterWithPolygon()
        {
            this.Name = TextStrings.ClipRasterLayer;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.ClipGridswithPolygon;
            this.ToolTip = TextStrings.ClipRasterLayerwithPolygon;
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
            IRaster raster = _inputParam[0].Value as IRaster;
            IFeatureSet polygon = _inputParam[1].Value as IFeatureSet;

            IRaster output = _outputParam[0].Value as IRaster;

            // Validates the input and output data
            if (raster == null || polygon == null || output == null)
            {
                return false;
            }

            ClipRaster.ClipRasterWithPolygon(
                polygon.Features[0], raster, output.Filename, cancelProgressHandler);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.input1Raster) { HelpText = TextStrings.InputRasterforCliping };
            _inputParam[1] = new PolygonFeatureSetParam(TextStrings.input2PolygonforCliping)
                                 {
                                     HelpText = TextStrings.InputPolygonforclipingtoRaster
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster) { HelpText = TextStrings.ResultRasterDirectory };
        }

        #endregion
    }
}