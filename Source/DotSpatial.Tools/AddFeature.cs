﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// An add feature tool
    /// </summary>
    public class AddFeature : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFeature"/> class.
        /// </summary>
        public AddFeature()
        {
            Name = TextStrings.AddFeature;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.AddFeatureDescription;
            ToolTip = TextStrings.AddFeatureintheFeatureSet;
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
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True if executed successfully</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            IFeatureSet input2 = _inputParam[1].Value as IFeatureSet;
            input2?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the add features Opaeration tool programmatically.
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="input2">The input2 featureSet which has the new features to add.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input1, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            int previous = 0;
            int i = 0;
            int maxFeature = input2.Features.Count;
            output.FeatureType = input1.FeatureType;
            foreach (IFeature f1 in input1.Features)
            {
                output.Features.Add(f1);
            }

            // go through new featureset that wanted to add.
            foreach (IFeature f in input2.Features)
            {
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                if (input1.FeatureType == input2.FeatureType)
                {
                    output.Features.Add(f);
                }

                int current = Convert.ToInt32(Math.Round(i * 100D / maxFeature));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
                i++;
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettodelete
                                 };

            _inputParam[1] = new FeatureSetParam(TextStrings.input2FeatureSettoAdd)
                                 {
                                     HelpText = TextStrings.InputFeatureSetaddnewfeatures
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}