// *******************************************************************************************************
// Product: DotSpatial.Tools.Buffer.cs
// Description:  Wraps DotSpatial.Feature.Buffer in a tool wrapper for use through the DotSptial toolbox.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Buffer
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
// Dan Ames           |  3/2013            |  Cleaning up header and interface as model for other tools. 
// *******************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool provides access to buffer functionality native to DotSpatial.Features. 
    /// DotSpatial tools are intended to be used through the DotSpatial toolbox or modeler.
    /// To perform buffer analysis through code, consider using DotSpatial.Feature.Buffer directly.
    /// </summary>
    public class Buffer : Tool
    {
        // Declare input and output parameter arrays
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        /// <summary>
        /// Create a new instance of the buffer tool
        /// </summary>
        public Buffer()
        {
            this.Name = TextStrings.Buffer;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.BufferDescription;
            this.ToolTip = TextStrings.Bufferwithdistance;
        }

        /// <summary>
        /// Inititalize input and output arrays with parameter types and default values.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.BufferDistance, 10.0);
            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.OutputPolygonFeatureSet);
        }

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
        /// Gets or Sets the input paramater array. 
        /// Number of parameter and parameter types are defined during initialize. 
        /// </summary>
        public override Parameter[] OutputParameters
        {
            get
            {
                return _outputParam;
            }
        }

        /// <summary>
        /// Once the parameters have been configured, the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            //Get the needed input and output parameters
            IFeatureSet inputFeatures = _inputParam[0].Value as IFeatureSet;
            DoubleParam dp = _inputParam[1] as DoubleParam;
            double bufferDistance = 1;
            if (dp != null)
            {
                bufferDistance = dp.Value;
            }
            IFeatureSet outputFeatures = _outputParam[0].Value as IFeatureSet;
            return GetBuffer(inputFeatures, bufferDistance, outputFeatures, cancelProgressHandler);
        }

        
        /// <summary>
        /// Private function to compute the buffer and return the result to the Execute function.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bufferDistance"></param>
        /// <param name="outputFeatures"></param>
        /// <returns></returns>
        private bool GetBuffer(IFeatureSet inputFeatures, double bufferDistance, IFeatureSet outputFeatures, ICancelProgressHandler cancelProgressHandler)
        {
            int numFeatures = inputFeatures.Features.Count;
            for (int i = 0; i < numFeatures; i++)
            {
                inputFeatures.Features[i].Buffer(bufferDistance, outputFeatures);
                // Here we update the progress
                int progress = Convert.ToInt32(i * 100 / numFeatures);
                cancelProgressHandler.Progress("buffer_tool", progress, "Buffering features.");
            }
            outputFeatures.Save();
            return true;
        }
    }
}