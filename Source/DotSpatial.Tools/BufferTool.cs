// *******************************************************************************************************
// Product: DotSpatial.Tools.Buffer.cs
// Description:  Wraps DotSpatial.Feature.Buffer in a tool wrapper for use through the DotSptial toolbox.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Buffer
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
// Dan Ames           |  3/2013            |  Cleaning up header and interface as model for other tools.
// *******************************************************************************************************

using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool provides access to buffer functionality native to DotSpatial.Features.
    /// DotSpatial tools are intended to be used through the DotSpatial toolbox or modeler.
    /// To perform buffer analysis through code, consider using DotSpatial.Feature.Buffer directly.
    /// </summary>
    public class BufferTool : Tool
    {
        #region Fields

        // Declare input and output parameter arrays
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferTool"/> class.
        /// </summary>
        public BufferTool()
        {
            Name = TextStrings.Buffer;
            Category = TextStrings.Analysis;
            Description = TextStrings.BufferDescription;
            ToolTip = TextStrings.Bufferwithdistance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array.
        /// Number of parameter and parameter types are defined during initialize.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the input paramater array.
        /// Number of parameter and parameter types are defined during initialize.
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the parameters have been configured, the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            // Get the needed input and output parameters
            IFeatureSet inputFeatures = _inputParam[0].Value as IFeatureSet;
            DoubleParam dp = _inputParam[1] as DoubleParam;
            double bufferDistance = 1;
            if (dp != null)
            {
                bufferDistance = dp.Value;
            }

            IFeatureSet outputFeatures = _outputParam[0].Value as IFeatureSet;

            if (Buffer.AddBuffer(inputFeatures, bufferDistance, outputFeatures, cancelProgressHandler))
            {
                if (outputFeatures == null) return false;
                outputFeatures.Save();
                return true;
            }

            _outputParam = null;
            return false;
        }

        /// <summary>
        /// Inititalize input and output arrays with parameter types and default values.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.BufferDistance, 10.0);
            _outputParam = new Parameter[2];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.OutputPolygonFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}