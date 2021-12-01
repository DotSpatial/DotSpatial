// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Union the features.
    /// </summary>
    public class Union : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Union"/> class.
        /// </summary>
        public Union()
        {
            Name = TextStrings.Union;
            Category = TextStrings.VectorOverlay;
            Description = TextStrings.UnionDescription;
            ToolTip = TextStrings.Unionofinputs;
            HelpImage = BitmapResources.Union;
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
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            if (!(_inputParam[0].Value is IFeatureSet self) || !(_inputParam[1].Value is IFeatureSet other)) return false;

            self.FillAttributes();
            other.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return self.Features.Count < other.Features.Count ? Execute(self, other, output, cancelProgressHandler) : Execute(other, self, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Union Opaeration tool programmatically.
        /// </summary>
        /// <param name="self">The input are feature set.</param>
        /// <param name="other">The second input feature set.</param>
        /// <param name="output">The output feature set.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet self, IFeatureSet other, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (self == null || other == null || output == null)
            {
                return false;
            }

            IFeatureSet tempOutput = self.Intersection(other, FieldJoinType.All, null);
            IFeatureSet tempFeatureSet = self.CombinedFields(other);

            int previous = 0;
            int max = self.Features.Count;

            // Take (Self-Intersect Featureset)
            List<IFeature> intersectList;
            for (int i = 0; i < self.Features.Count; i++)
            {
                intersectList = other.Select(self.Features[i].Geometry.EnvelopeInternal.ToExtent());
                foreach (IFeature feat in intersectList)
                {
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }

                    self.Features[i].Difference(feat, tempFeatureSet, FieldJoinType.LocalOnly);
                }

                if (Math.Round(i * 40D / max) <= previous)
                {
                    continue;
                }

                previous = Convert.ToInt32(Math.Round(i * 40D / max));
                cancelProgressHandler.Progress(previous, previous + TextStrings.progresscompleted);
            }

            max = other.Features.Count;

            // Take (Other-Intersect Featureset)
            for (int i = 0; i < other.Features.Count; i++)
            {
                intersectList = self.Select(other.Features[i].Geometry.EnvelopeInternal.ToExtent());
                foreach (IFeature feat in intersectList)
                {
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }

                    other.Features[i].Difference(feat, tempFeatureSet, FieldJoinType.LocalOnly);
                }

                if (Math.Round((i * 40D / max) + 40D) <= previous)
                {
                    continue;
                }

                previous = Convert.ToInt32(Math.Round((i * 40D / max) + 40D));
                cancelProgressHandler.Progress(previous, previous + TextStrings.progresscompleted);
            }

            max = tempFeatureSet.Features.Count;
            output.CopyTableSchema(tempFeatureSet);

            // Add the individual feature to output
            for (int i = 0; i < tempFeatureSet.Features.Count; i++)
            {
                output.Features.Add(tempFeatureSet.Features[i]);
                if (Math.Round((i * 10D / max) + 80D) <= previous)
                {
                    continue;
                }

                previous = Convert.ToInt32(Math.Round((i * 10D / max) + 80D));
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                cancelProgressHandler.Progress(previous, previous + TextStrings.progresscompleted);
            }

            max = tempOutput.Features.Count;

            // Add the Intersect feature to output
            for (int i = 0; i < tempOutput.Features.Count; i++)
            {
                output.Features.Add(tempOutput.Features[i]);
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }

                if (Math.Round((i * 10D / max) + 90D) <= previous)
                {
                    continue;
                }

                previous = Convert.ToInt32(Math.Round((i * 10D / max) + 90D));
                cancelProgressHandler.Progress(previous, previous + TextStrings.progresscompleted);
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet)
            {
                HelpText = TextStrings.MainFeatureset
            };
            _inputParam[1] = new FeatureSetParam(TextStrings.ChildFeatureSet)
            {
                HelpText = TextStrings.SecondFeatureset
            };
            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.UnionFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}