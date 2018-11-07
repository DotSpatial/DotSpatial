// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using DotSpatial.Projections;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Reprojects a given featureset to a new coordinate system.
    /// </summary>
    public class ReprojectFeatureSet : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReprojectFeatureSet"/> class.
        /// </summary>
        public ReprojectFeatureSet()
        {
            Name = TextStrings.ReprojectFeatures;
            Category = TextStrings.SpatialReference;
            UpdateToolResources();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input parameter array
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output parameter array
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            ProjectionParam source = _inputParam[1] as ProjectionParam;
            ProjectionParam dest = _inputParam[2] as ProjectionParam;
            ProjectionInfo pSource = source?.Value;

            if (dest == null) return false;

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, pSource, dest.Value, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ReprojectFeatureSet Operation tool programmatically.
        /// </summary>
        /// <param name="featureSet">The input FeatureSet.</param>
        /// <param name="sourceProjection">The input Expression string to select features to Delete.</param>
        /// <param name="destProjection">The target projected coordinate system to reproject the featureset to</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet featureSet, ProjectionInfo sourceProjection, ProjectionInfo destProjection, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            string filename = output.Filename;
            output = featureSet.CopySubset(string.Empty);

            if (sourceProjection != null)
            {
                output.Projection = sourceProjection;
            }

            output.Reproject(destProjection);
            output.SaveAs(filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettoreproject
                                 };

            _inputParam[1] = new ProjectionParam(TextStrings.SourceProjection)
                                 {
                                     HelpText = TextStrings.sourceprojectiondifferent
                                 };

            _inputParam[2] = new ProjectionParam(TextStrings.DesiredOutputProjection)
                                 {
                                     HelpText = TextStrings.Thedestinationprojection
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the parameters value has been changed, usually when a user changes a input or output
        /// parameters value, this can be used to populate input2 parameters default values.
        /// </summary>
        /// <param name="sender">Sender that fired the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            // This will give the Featureset values to second parameter
            if (sender != _inputParam[0]) return;

            FeatureSetParam fsp = _inputParam[0] as FeatureSetParam;
            if (fsp?.Value == null) return;

            _inputParam[1].Value = fsp.Value.Projection;
        }

        /// <summary>
        /// Attempt to update the tool's resources.
        /// </summary>
        public override void UpdateToolResources()
        {
            NameLabel = TextStrings.ReprojectFeatures_Label;
            CategoryLabel = TextStrings.SpatialReference_Label;
            CategoryToolTip = TextStrings.SpatialReference_ToolTip;
            Description = TextStrings.ReprojectFeatureSetDescription;
            ToolTip = TextStrings.Reprojectsallcoordinates;
        }

        #endregion
    }
}