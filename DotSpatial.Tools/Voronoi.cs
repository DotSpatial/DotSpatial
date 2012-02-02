// ********************************************************************************************************
// Product Name: MapWindow.Tools.mwVoronoi
// Description:  computes voronoi polygons around each of the points,
//               defining the regions that are closer to that point than any other points
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Ted Dunsford. Created in 8/26/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                   |   Date                 |   Comments
//------------------------|------------------------|---------------------------------
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

            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        /// computes voronoi polygons around each of the points,
        /// defining the regions that are closer to that point than any other points
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
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