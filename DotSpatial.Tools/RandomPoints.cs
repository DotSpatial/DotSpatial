// *******************************************************************************************************
// Product: DotSpatial.Tools.RandomPoints.cs
// Description:  Wraps DotSpatial.Analysis.RandomGeometry in a tool wrapper for use through the DotSptial toolbox.
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// *******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Modeling.Forms;
using DotSpatial.Data;
using DotSpatial.Analysis;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool provides access to the random geometry functionality in DotSpatial.Analysis.RandomGeometry.
    /// DotSpatial tools are intended to be used through the DotSpatial toolbox or modeler.
    /// To perform buffer analysis through code, consider using DotSpatial.Analysis.RandomGeometry directly.
    /// </summary>
    class RandomGeometryTool : Tool
    {
        #region Constants and Fields

        // Declare input and output parameter arrays
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the RandomGeometry tool
        /// </summary>
        public RandomGeometryTool()
        {
            this.Name = TextStrings.RandomGeometry;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.RandomGeometryDescription;
            this.ToolTip = TextStrings.RandomGeometryToolTip;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input paramater array. 
        /// Number of parameter and parameter types are defined during initialize. 
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array. 
        /// Number of parameter and parameter types are defined during initialize. 
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
        /// Executes the random geometry tool, returning true when it has completed.
        /// </summary>
        /// <param name="cancelProgressHandler"></param>
        /// <returns></returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            //Get the needed input and output parameters
            IFeatureSet inputFeatures = _inputParam[0].Value as IFeatureSet;          
            IFeatureSet outputFeatures = _outputParam[0].Value as IFeatureSet;
            IntParam intInput = _inputParam[1] as IntParam;

            int numPoints = 1;
            if (intInput != null){ numPoints = intInput.Value; }

            RandomGeometry.RandomPoints(inputFeatures, numPoints, outputFeatures, cancelProgressHandler);
            
            if (cancelProgressHandler.Cancel)
            {
                //Set output param to null so that ToolManager does not attempt to open file.
                _outputParam = null;
                return false;
            }
            else
            {
                outputFeatures.Save();
                return true;
            }
        }


        /// <summary>
        /// Inititalize input and output arrays with parameter types and default values.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new IntParam(TextStrings.RandomGeometryNumPoint, 10);
            _outputParam = new Parameter[1];
            _outputParam[0] = new FeatureSetParam(TextStrings.OutputFeatureSet);
        }

        #endregion
    }
}
