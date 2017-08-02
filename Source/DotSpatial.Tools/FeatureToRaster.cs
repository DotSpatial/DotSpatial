// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Data;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool creates a raster by drawing the specified feature onto the raster.
    /// </summary>
    public class FeatureToRaster : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureToRaster"/> class.
        /// </summary>
        public FeatureToRaster()
        {
            Name = TextStrings.FeatureToRaster;
            Category = TextStrings.Conversion;
            Description = TextStrings.FeatureToRasterDescription;
            ToolTip = TextStrings.newrasteronspecifiedfeatureset;
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
        /// <param name="cancelProgressHandler">A progress handler for handling progress messages</param>
        /// <returns>True, if executed successfully.</returns>
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
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet source, double cellSize, string fieldName, IRaster output, ICancelProgressHandler cancelProgressHandler)
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
            _inputParam[2] = new DoubleParam(TextStrings.DesiredCellSize)
            {
                HelpText = TextStrings.Themaximumnumber
            };
            _inputParam[1] = new ListParam(TextStrings.stringnameoffield)
            {
                HelpText = TextStrings.Thevalueofeachcell
            };
            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
            {
                HelpText = TextStrings.Resultofaverageslope
            };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        /// <param name="sender">Sender that fired the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            if (sender != _inputParam[0])
            {
                return;
            }

            List<string> fields = new List<string>();
            IFeatureSet fs = _inputParam[0].Value as IFeatureSet;
            IDataTable dt = fs?.DataTable;
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