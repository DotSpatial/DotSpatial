﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// A tool for deleting features from a feature set.
    /// </summary>
    public class DeleteFeatures : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFeatures"/> class.
        /// </summary>
        public DeleteFeatures()
        {
            Name = TextStrings.DeleteFeatures;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.DeleteFeaturesDescription;
            ToolTip = TextStrings.DeleteFeaturesfromFeatureSet;
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
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            IndexParam ip = _inputParam[1] as IndexParam;
            string input2 = string.Empty;
            if (ip != null)
            {
                input2 = ip.Value;
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Erase Opaeration tool programmatically.
        /// Ping deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="input2">The input Expression string to select features to Delete.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input1, string input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null) return false;

            if (cancelProgressHandler.Cancel)
            {
                return false;
            }

            int previous = 0;
            List<IFeature> fetList = input1.SelectByAttribute(input2);
            int noOfFeaturesToDelete = fetList.Count;
            int noOfFeatures = input1.Features.Count;
            output.FeatureType = input1.FeatureType;
            foreach (IFeature f in input1.Features)
            {
                output.Features.Add(f);
            }

            // Go through every item in the list
            for (int i = 0; i < noOfFeaturesToDelete; i++)
            {
                int current = Convert.ToInt32(Math.Round(i * 100D / noOfFeaturesToDelete));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;

                // loop through every item in the list
                for (int j = 0; j < noOfFeatures; j++)
                {
                    // Select the Feature from Feature set to detlete
                    if (fetList[i] == input1.Features[j])
                    {
                        output.Features.Remove(input1.Features[j]);
                    }
                }
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettodelete
                                 };

            _inputParam[1] = new IndexParam(TextStrings.SelectFeaturestoDelete)
                                 {
                                     HelpText = TextStrings.ExpressionSelectFeatures
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate input2 parameters default values.
        /// </summary>
        /// <param name="sender">Sender that fired the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            // This will give the Featureset values to second parameter
            if (sender != _inputParam[0])
            {
                return;
            }

            FeatureSetParam fsp = _inputParam[0] as FeatureSetParam;
            IndexParam ip = _inputParam[1] as IndexParam;
            if (fsp?.Value != null && ip != null)
            {
                ip.Fs = fsp.Value as FeatureSet;
            }
        }

        #endregion
    }
}