// *******************************************************************************************************
// Product: DotSpatial.Tools.Voronoi.cs
// Description:  Create thiessen polygons within a raster layer.
// Copyright & License: See www.DotSpatial.org.
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

namespace DotSpatial.Tools
{
    /// <summary>
    /// Class for creating voronoi (Thiessen) polygons
    /// </summary>
    public class Voronoi : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the voronoi polygon tool
        /// </summary>
        public Voronoi()
        {
            this.Name = TextStrings.ThiessenPolygons;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.VoronoiDescription;
            this.ToolTip = TextStrings.CreateVoronoiPolygons;
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
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            if (input != null)
            {
                input.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

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

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);
        }

        #endregion
    }
}