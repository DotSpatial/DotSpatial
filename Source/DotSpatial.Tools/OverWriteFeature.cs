// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Overwrite feature in a featureset.
    /// </summary>
    public class OverWriteFeature : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OverWriteFeature"/> class.
        /// </summary>
        public OverWriteFeature()
        {
            Name = TextStrings.OverWriteFeature;
            Category = TextStrings.VectorOverlay;
            UpdateToolResources();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the help text to be displayed when no input field is selected.
        /// </summary>
        public string HelpText => TextStrings.OverWriteFeaturebyindex;

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
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            int index = (int)_inputParam[1].Value;

            IFeatureSet input2 = _inputParam[2].Value as IFeatureSet;
            input2?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, index, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the overwrite feature Opaeration tool programmatically.
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="index">The Index to overwrite</param>
        /// <param name="input2">The input2 featureSet which has the new feature to overwrite.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input1, int index, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            if (cancelProgressHandler.Cancel)
            {
                return false;
            }

            IFeature newFeature = input2.Features[0];
            output.FeatureType = input1.FeatureType;
            foreach (IFeature f in input1.Features)
            {
                output.Features.Add(f);
            }

            if (index > -1)
            {
                if (index < output.Features.Count)
                {
                    output.Features.RemoveAt(index);
                    output.Features.Insert(index, newFeature);
                }
            }

            output.SaveAs(output.Filename, true);
            cancelProgressHandler.Progress(string.Empty, 100, 100 + TextStrings.progresscompleted);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettodelete
                                 };

            _inputParam[1] = new IntParam(TextStrings.Index, 0)
                                 {
                                     HelpText = TextStrings.IndexNotoOverwrite
                                 };

            _inputParam[2] = new FeatureSetParam(TextStrings.input2FeatureSettoOverwrite)
                                 {
                                     HelpText = TextStrings.InputFeatureSettobeoverwrite
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Attempt to update the tool's resources.
        /// </summary>
        public override void UpdateToolResources()
        {
            NameLabel = TextStrings.OverWriteFeature_Label;
            CategoryLabel = TextStrings.VectorOverlay_Label;
            CategoryToolTip = TextStrings.VectorOverlay_ToolTip;
            Description = TextStrings.OverWriteFeatureDescription;
            ToolTip = TextStrings.OverWriteFeatureintheFeatureSet;
        }

        #endregion
    }
}