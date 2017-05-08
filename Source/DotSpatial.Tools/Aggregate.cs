// *******************************************************************************************************
// Product: DotSpatial.Tools.Aggregate.cs
// Description:  A tool for aggregating multiple polygons into a single polygon.
//
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
// -------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
// -----------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for Aggregate
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Union all of the features from one polygon feature set into a single polygon.
    /// </summary>
    public class Aggregate : Tool
    {
        #region Fields

        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregate"/> class.
        /// </summary>
        public Aggregate()
        {
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.Givenafeatureset;
            Name = TextStrings.Aggregate;
            ToolTip = TextStrings.AggregateToollip;
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
        /// Once the Parameters have been configured, the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if successful.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self = _inputParam[0].Value as IFeatureSet;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;
            return Execute(self, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Union Operation tool programmatically.
        /// </summary>
        /// <param name="sourceData">The source FeatureSet to aggregate the features for.</param>
        /// <param name="resultData">The result FeatureSet of aggregated features.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if successful</returns>
        public bool Execute(IFeatureSet sourceData, IFeatureSet resultData, ICancelProgressHandler cancelProgressHandler)
        {
            // removed "static" dpa 12/2009 so that this can be run from an external call directly.
            // Validates the input and output data
            if (sourceData?.Features == null || sourceData.Features.Count == 0 || resultData == null)
            {
                return false;
            }

            IFeature oneFeature = sourceData.Features[0];

            for (int i = 1; i < sourceData.Features.Count; i++)
            {
                if (sourceData.Features[i] == null) continue;

                oneFeature = oneFeature.Union(sourceData.Features[i].Geometry);
            }

            resultData.Features.Add(oneFeature);
            resultData.SaveAs(resultData.Filename, true);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new PolygonFeatureSetParam(TextStrings.BaseFeatureSet);

            _outputParam = new Parameter[2];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.UnionFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}