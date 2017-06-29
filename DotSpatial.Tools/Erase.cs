// *******************************************************************************************************
// Product: DotSpatial.Tools.Erase
// Description:  Erase a portion of a data set.
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Erase
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
// Troy Shields       |  03/2013           |  Updated License and cleaned up code.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Analysis;

namespace DotSpatial.Tools
{
    /// <summary>
    /// An Erase tool
    /// </summary>
    public class Erase : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the Erase class.
        /// </summary>
        public Erase()
        {
            this.Name = TextStrings.Erase;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.RraseDescription;
            this.ToolTip = TextStrings.EliminateSecondFeatureset;
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
            else
            {
                result.Filename = ((IFeatureSet)_outputParam[0].Value).Filename;
                result.Save();
                _outputParam[0].Value = result;
                return true;
            }
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.BaseFeatureSet);
            _inputParam[1] = new FeatureSetParam(TextStrings.RemoveFeatureSet);
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.ErasedResultFeatureSet);
        }

        #endregion
    }
}