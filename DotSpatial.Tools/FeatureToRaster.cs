// *******************************************************************************************************
// Product: DotSpatial.Tools.FeatureToRaster.cs
// Description:  Generate a new raster from a given polygon.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  built the genuine functionality to work with the newly created Analysis methods
// KP                     |  9/2009                |  Used IDW as model for FeatureToRaster
// Ping  Yang             |  12/2009               |  Cleaning code and fixing bugs.
// ***********************************************************************************************************************************

using System.Collections.Generic;
using System.Data;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool creates a raster by drawing the specified feature onto the raster.
    /// </summary>
    public class FeatureToRaster : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the FeatureToRaster class.
        /// </summary>
        public FeatureToRaster()
        {
            this.Name = TextStrings.FeatureToRaster;
            this.Category = TextStrings.Conversion;
            this.Description = TextStrings.FeatureToRasterDescription;
            this.ToolTip = TextStrings.newrasteronspecifiedfeatureset;
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
            IFeatureSet poly = _inputParam[0].Value as IFeatureSet;
            double cellSize = (double)_inputParam[2].Value;
            if (poly == null)
            {
                return false;
            }

            int indx = (int)_inputParam[1].Value - 1;
            string field = indx < 0 ? "FID" : poly.DataTable.Columns[indx].ColumnName;

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(poly, cellSize, field, output, cancelProgressHandler);
        }

        /// <summary>
        /// Generates a new raster given the specified featureset.  Values will be given to
        /// each cell that coincide with the values in the specified field of the attribute
        /// table.  If the cellSize is 0, then it will be automatically calculated so that
        /// the smaller dimension (between width and height) is 256 cells.
        /// Ping Yang delete static for external testing 01/2010
        /// </summary>
        /// <param name="source">The featureset to convert into a vector format</param>
        /// <param name="cellSize">A double giving the geographic cell size.</param>
        /// <param name="fieldName">The string fieldName to use</param>
        /// <param name="output">The raster that will be created</param>
        /// <param name="cancelProgressHandler">A progress handler for handling progress messages</param>
        /// <returns></returns>
        public bool Execute(
            IFeatureSet source,
            double cellSize,
            string fieldName,
            IRaster output,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (source == null || output == null)
            {
                return false;
            }

            output = VectorToRaster.ToRaster(source, cellSize, fieldName, output.Filename, string.Empty, new string[] { }, cancelProgressHandler);
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[4];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1polygontoRaster)
                                 {
                                     HelpText = TextStrings.InputPolygontochange
                                 };
            _inputParam[2] = new DoubleParam(TextStrings.DesiredCellSize) { HelpText = TextStrings.Themaximumnumber };
            _inputParam[1] = new ListParam(TextStrings.stringnameoffield) { HelpText = TextStrings.Thevalueofeachcell };
            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster) { HelpText = TextStrings.Resultofaverageslope };
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        public override void ParameterChanged(Parameter sender)
        {
            if (sender != _inputParam[0])
            {
                return;
            }

            List<string> fields = new List<string>();
            IFeatureSet fs = _inputParam[0].Value as IFeatureSet;
            if (fs == null)
            {
                return;
            }

            DataTable dt = fs.DataTable;
            if (dt == null)
            {
                return;
            }

            fields.Add("FID [Integer]");
            foreach (DataColumn column in dt.Columns)
            {
                fields.Add(column.ColumnName + " [" + column.DataType.Name + "]");
            }

            ListParam lp = _inputParam[1] as ListParam;
            if (lp == null)
            {
                return;
            }

            lp.ValueList = fields;
            lp.Value = 0;
        }

        #endregion
    }
}