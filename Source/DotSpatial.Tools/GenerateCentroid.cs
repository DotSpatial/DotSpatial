// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Generate a centroid featureset from an input featureset.
    /// </summary>
    public class GenerateCentroid : Tool
    {
        #region Fields
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateCentroid"/> class.
        /// </summary>
        public GenerateCentroid()
        {
            Name = TextStrings.GenerateCentroid;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.GenerateCentroidDescription;
            ToolTip = TextStrings.GenerateCentroidfrominputFeatureSet;
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
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            input1?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the generate centroid FeatureSet Opaeration tool programmatically.
        /// Ping deleted static for external testing 01/2010.
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input1, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || output == null) return false;

            bool multPoint = false;
            foreach (IFeature f1 in input1.Features)
            {
                if (f1.Geometry.NumGeometries > 1)
                {
                    multPoint = true;
                }
            }

            output.FeatureType = multPoint == false ? FeatureType.Point : FeatureType.MultiPoint;

            int previous = 0;
            int i = 0;
            int maxFeature = input1.Features.Count;
            output.CopyTableSchema(input1);
            foreach (IFeature f in input1.Features)
            {
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                IFeature fnew = new Feature(f.Geometry.Centroid);

                // Add the centroid to output
                output.Features.Add(fnew);

                fnew.CopyAttributes(f);

                int current = Convert.ToInt32(Math.Round(i * 100D / maxFeature));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
                i++;
            }

            output.AttributesPopulated = true;
            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
            {
                HelpText = TextStrings.InputFeatureSettogenerate
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