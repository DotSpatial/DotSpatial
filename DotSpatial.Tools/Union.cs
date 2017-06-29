// -----------------------------------------------------------------------
// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterSlope.cs
// Description:  Add all features of input with attributes.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for Union
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Union the features
    /// </summary>
    public class Union : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the Union tool
        /// </summary>
        public Union()
        {
            this.Name = TextStrings.Union;
            this.Category = TextStrings.VectorOverlay;
            this.Description = TextStrings.UnionDescription;
            this.ToolTip = TextStrings.Unionofinputs;
            this.HelpImage = BitmapResources.Union;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters
        {
            get
            {
                return _outputParam;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet self = _inputParam[0].Value as IFeatureSet;
            if (self != null)
            {
                self.FillAttributes();
            }

            IFeatureSet other = _inputParam[1].Value as IFeatureSet;
            if (other != null)
            {
                other.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            if (self == null)
            {
                return false;
            }

            if (other == null)
            {
                return false;
            }

            return self.Features.Count < other.Features.Count
                       ? Execute(self, other, output, cancelProgressHandler)
                       : Execute(other, self, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Union Opaeration tool programaticaly
        /// </summary>
        /// <param name="self">The input are feature set</param>
        /// <param name="other">The second input feature set</param>
        /// <param name="output">The output feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(
            IFeatureSet self, IFeatureSet other, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
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
                intersectList = other.Select(self.Features[i].Envelope.ToExtent());
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
                cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
            }

            max = other.Features.Count;

            // Take (Other-Intersect Featureset)
            for (int i = 0; i < other.Features.Count; i++)
            {
                intersectList = self.Select(other.Features[i].Envelope.ToExtent());
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
                cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
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

                cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
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
                cancelProgressHandler.Progress(string.Empty, previous, previous + TextStrings.progresscompleted);
            }

            output.SaveAs(output.Filename, true);
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet) { HelpText = TextStrings.MainFeatureset };
            _inputParam[1] = new FeatureSetParam(TextStrings.ChildFeatureSet)
                                 {
                                     HelpText = TextStrings.SecondFeatureset
                                 };
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.UnionFeatureSet);
        }

        #endregion
    }
}