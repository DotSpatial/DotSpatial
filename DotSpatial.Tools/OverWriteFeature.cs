// *******************************************************************************************************
// Product: DotSpatial.Tools.OverWriteFeature.cs
// Description:  Overwrite feature in a featureset.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for OverWriteFeature
// Ping                   |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Overwrite feature
    /// </summary>
    public class OverWriteFeature : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the OverWriteFeature class.
        /// </summary>
        public OverWriteFeature()
        {
            this.Name = TextStrings.OverWriteFeature;
            this.Category = TextStrings.VectorOverlay;
            this.Description = TextStrings.OverWriteFeatureDescription;
            this.ToolTip = TextStrings.OverWriteFeatureintheFeatureSet;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Help text to be displayed when no input field is selected
        /// </summary>
        public string HelpText
        {
            get
            {
                return TextStrings.OverWriteFeaturebyindex;
            }
        }

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

            int index = (int)_inputParam[1].Value;

            IFeatureSet input2 = _inputParam[2].Value as IFeatureSet;
            if (input2 != null)
            {
                input2.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input1, index, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the overwrite feature Opaeration tool programaticaly.
        /// </summary>
        /// <param name="input1">The input FeatureSet.</param>
        /// <param name="index">The Index to overwrite</param>
        /// <param name="input2">The input2 featureSet which has the new feature to overwrite.</param>
        /// <param name="output">The output FeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns></returns>
        public bool Execute(
            IFeatureSet input1,
            int index,
            IFeatureSet input2,
            IFeatureSet output,
            ICancelProgressHandler cancelProgressHandler)
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
        /// The parameters array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new FeatureSetParam(TextStrings.input1FeatureSet)
                                 {
                                     HelpText = TextStrings.InputFeatureSettodelete
                                 };

            _inputParam[1] = new IntParam(TextStrings.Index, 0) { HelpText = TextStrings.IndexNotoOverwrite };

            _inputParam[2] = new FeatureSetParam(TextStrings.input2FeatureSettoOverwrite)
                                 {
                                     HelpText = TextStrings.InputFeatureSettobeoverwrite
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