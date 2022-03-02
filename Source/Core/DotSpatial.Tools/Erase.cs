// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Erase a portion of a data set.
    /// </summary>
    public class Erase : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Erase"/> class.
        /// </summary>
        public Erase()
        {
            Name = TextStrings.Erase;
            Category = TextStrings.Analysis;
            Description = TextStrings.RraseDescription;
            ToolTip = TextStrings.EliminateSecondFeatureset;
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
            IFeatureSet self = _inputParam[0].Value as IFeatureSet;
            IFeatureSet other = _inputParam[1].Value as IFeatureSet;

            if (self != null && other != null)
            {
                self.FillAttributes();
                other.FillAttributes();
            }

            IFeatureSet result = Overlay.EraseFeatures(self, other, cancelProgressHandler);
            if (cancelProgressHandler.Cancel)
            {
                _outputParam = null;
                return false;
            }

            result.Filename = ((IFeatureSet)_outputParam[0].Value).Filename;
            result.Save();
            _outputParam[0].Value = result;
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet);
            _inputParam[1] = new FeatureSetParam(TextStrings.RemoveFeatureSet);
            _outputParam = new Parameter[2];
            _outputParam[0] = new FeatureSetParam(TextStrings.ErasedResultFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}