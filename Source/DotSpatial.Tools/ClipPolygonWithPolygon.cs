// *******************************************************************************************************
// Product: DotSpatial.Tools.ClipPolygonWithPolygon.cs
// Description:  Clip a polygon with another polygon.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// KP                     |  9/2009                |  Used IDW as model for ClipPolygonWithPolygon
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Clip Polygon with Polygon
    /// </summary>
    public class ClipPolygonWithPolygon : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipPolygonWithPolygon"/> class.
        /// </summary>
        public ClipPolygonWithPolygon()
        {
            Name = TextStrings.ClipFeatureSetWithPolygon;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.tooltakesafeatureset;
            ToolTip = TextStrings.Clipslayerwithlayer;
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
        /// <param name="cancelProgressHandler">The progress handler for progress message updates</param>
        /// <returns>True if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            input?.FillAttributes();

            IFeatureSet input2 = _inputParam[1].Value as IFeatureSet;
            input2?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ClipPolygonWithPolygon tool with programatic input
        /// </summary>
        /// <param name="input">The input feature set to clip</param>
        /// <param name="input2">The input polygon feature set to clip with</param>
        /// <param name="output">The output feature set</param>
        /// <param name="cancelProgressHandler">The progress handler for progress message updates</param>
        /// <returns>True if executed successfully.</returns>
        public bool Execute(IFeatureSet input, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || input2 == null || output == null)
            {
                cancelProgressHandler.Progress(string.Empty, 100, TextStrings.Oneparameterinnull);
                return false;
            }

            if (input2.FeatureType != FeatureType.Polygon)
            {
                cancelProgressHandler.Progress(string.Empty, 100, TextStrings.secondinputlayer);
                return false;
            }

            output.FeatureType = input.FeatureType;

            // we add all the old features to output
            IFeatureSet tempoutput = input.Intersection(input2, FieldJoinType.LocalOnly, cancelProgressHandler);

            // We add all the fields
            foreach (DataColumn inputColumn in tempoutput.DataTable.Columns)
            {
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));
            }

            foreach (var fe in tempoutput.Features)
            {
                output.Features.Add(fe);
            }

            // Setting the AttributesPopulated to true here means the output shapefile will get attribute columns copied from
            // the source file. This problem occurs when using the ClipPolygonWithPolygon tool due to how the input/output files
            // are loaded. https://github.com/DotSpatial/DotSpatial/issues/892
            output.AttributesPopulated = true;

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.Featuresettoclip);
            _inputParam[1] = new PolygonFeatureSetParam(TextStrings.Clipbounds);

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.Clippedfeatureset);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}