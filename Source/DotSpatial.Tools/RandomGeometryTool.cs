// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool provides access to the random geometry functionality in DotSpatial.Analysis.RandomGeometry.
    /// DotSpatial tools are intended to be used through the DotSpatial toolbox or modeler.
    /// To perform buffer analysis through code, consider using DotSpatial.Analysis.RandomGeometry directly.
    /// </summary>
    public class RandomGeometryTool : Tool
    {
        #region Fields

        // Declare input and output parameter arrays
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomGeometryTool"/> class.
        /// Create a new instance of the RandomGeometry tool
        /// </summary>
        public RandomGeometryTool()
        {
            Name = TextStrings.RandomGeometry;
            Category = TextStrings.Analysis;
            Description = TextStrings.RandomGeometryDescription;
            ToolTip = TextStrings.RandomGeometryToolTip;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array.
        /// Number of parameter and parameter types are defined during initialize.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array.
        /// Number of parameter and parameter types are defined during initialize.
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Executes the random geometry tool, returning true when it has completed.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            // Get the needed input and output parameters
            IFeatureSet inputFeatures = _inputParam[0].Value as IFeatureSet;
            IFeatureSet outputFeatures = _outputParam[0].Value as IFeatureSet;
            IntParam intInput = _inputParam[1] as IntParam;

            int numPoints = 1;
            if (intInput != null)
            {
                numPoints = intInput.Value;
            }

            RandomGeometry.RandomPoints(inputFeatures, numPoints, outputFeatures, cancelProgressHandler);

            if (cancelProgressHandler.Cancel)
            {
                // Set output param to null so that ToolManager does not attempt to open file.
                _outputParam = null;
                return false;
            }

            if (outputFeatures == null) return false;

            outputFeatures.Save();
            return true;
        }

        /// <summary>
        /// Inititalize input and output arrays with parameter types and default values.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new IntParam(TextStrings.RandomGeometryNumPoint, 10);
            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}