// *******************************************************************************************************
// Product: DotSpatial.Tools.AddFeature.cs
// Description:  A tool for adding a feature to a featureset.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for AddFeature
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// An add feature tool
    /// </summary>
    public class AddFeature : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the AddFeature class.
        /// </summary>
        public AddFeature()
        {
            this.Name = TextStrings.AddFeature;
            this.Category = TextStrings.VectorOverlay;
            this.Description = TextStrings.AddFeatureDescription;
            this.ToolTip = TextStrings.AddFeatureintheFeatureSet;
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
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input1 = _inputParam[0].Value as IFeatureSet;
            if (input1 != null)
            {
                input1.FillAttributes();
            }

            IFeatureSet input2 = _inputParam[1].Value as IFeatureSet;
            if (input2 != null)
            {
                input2.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the add features Opaeration tool programaticaly.
        /// Ping deleted "static" for external Testing
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="input2">The input2 featureSet which has the new features to add.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(
            IFeatureSet input1, IFeatureSet input2, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
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

            // cancelProgressHandler.Progress("", 100, 100.ToString() + TextStrings.progresscompleted);
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

            _inputParam[1] = new FeatureSetParam(TextStrings.input2FeatureSettoAdd)
                                 {
                                     HelpText = TextStrings.InputFeatureSetaddnewfeatures
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet)
                                  {
                                      HelpText = TextStrings.SelectResultFeatureSetDirectory
                                  };
        }

        #endregion
    }
}