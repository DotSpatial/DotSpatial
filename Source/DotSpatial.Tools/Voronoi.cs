// *******************************************************************************************************
// Product: DotSpatial.Tools.Voronoi.cs
// Description:  Create thiessen polygons within a raster layer.
//
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//---------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|-------------------------------------------------------
// KP                     |  9/2009                |  Used IDW as model for Voronoi
// Ping  Yang             |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Class for creating voronoi (Thiessen) polygons.
    /// </summary>
    public class Voronoi : Tool
    {
        #region Fields

        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Voronoi"/> class.
        /// </summary>
        public Voronoi()
        {
            Name = TextStrings.ThiessenPolygons;
            Category = TextStrings.Analysis;
            Description = TextStrings.VoronoiDescription;
            ToolTip = TextStrings.CreateVoronoiPolygons;
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
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            input?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            if (output == null) return false;

            Analysis.Voronoi.VoronoiPolygons(input, output, true);
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new PointFeatureSetParam(TextStrings.PointFeatureSet);

            _outputParam = new Parameter[2];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}