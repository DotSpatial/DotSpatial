// ********************************************************************************************************
// Product Name: MapWindow.Tools.Buffer
// Description:  Inverse Distance Weighting point to raster interpolation
//
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Kandasamy Prasanna. Created in 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for Buffer
// Ping  Yang         |  12/2009           |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Buffer
    /// </summary>
    public class Buffer : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the buffer tool
        /// </summary>
        public Buffer()
        {
            this.Name = TextStrings.Buffer;
            this.Category = TextStrings.Analysis;
            this.Description = TextStrings.BufferDescription;
            this.ToolTip = TextStrings.Bufferwithdistance;
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
        /// Buffer test
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bufferDistance"></param>
        /// <param name="output"></param>
        /// <param name="cancelProgressHandler"></param>
        /// <returns></returns>
        public static bool GetBuffer(
            IFeatureSet input, double bufferDistance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            int previous = 0;
            int maxNo = input.Features.Count;
            for (int i = 0; i < maxNo; i++)
            {
                input.Features[i].Buffer(bufferDistance, output);

                // Here we update the progress
                int current = Convert.ToInt32(i * 100 / maxNo);
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                    previous = current;
                }

                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            DoubleParam dp = _inputParam[1] as DoubleParam;
            double bufferDistance = 1;
            if (dp != null)
            {
                bufferDistance = dp.Value;
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, bufferDistance, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Buffer tool programaticaly
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="bufferDistance">The included radius from current features to use when Buffering</param>
        /// <param name="output">The output polygon feature set</param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input, double bufferDistance, IFeatureSet output)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            if (input.FeatureType == FeatureType.Point)
            {
                return GetBuffer(input, bufferDistance, output);
            }

            if (input.FeatureType == FeatureType.MultiPoint)
            {
                return GetBuffer(input, bufferDistance, output);
            }

            if (input.FeatureType == FeatureType.Line)
            {
                return GetBuffer(input, bufferDistance, output);
            }

            if (input.FeatureType == FeatureType.Polygon)
            {
                return GetBuffer(input, bufferDistance, output);
            }

            return false;
        }

        // Ping Yang Added it for external testing 01/10

        /// <summary>
        /// Executes the buffer calculation
        /// </summary>
        /// <param name="input">The IFeatureSet</param>
        /// <param name="bufferDistance">The double buffer distance</param>
        /// <param name="output">The IFeatureSet output</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns>True if the method was successful</returns>
        public bool Execute(
            IFeatureSet input, double bufferDistance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            if (input.FeatureType == FeatureType.Point)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }

            if (input.FeatureType == FeatureType.MultiPoint)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }

            if (input.FeatureType == FeatureType.Line)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }

            if (input.FeatureType == FeatureType.Polygon)
            {
                return GetBuffer(input, bufferDistance, output, cancelProgressHandler);
            }

            return false;
        }

        /// <summary>
        /// Get Buffer test method
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bufferDistance"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool GetBuffer(IFeatureSet input, double bufferDistance, IFeatureSet output)
        {
            int previous = 0;
            int maxNo = input.Features.Count;
            for (int i = 0; i < maxNo; i++)
            {
                input.Features[i].Buffer(bufferDistance, output);

                // Here we update the progress
                int current = Convert.ToInt32(i * 100 / maxNo);
                if (current > previous)
                {
                    previous = current;
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new FeatureSetParam(TextStrings.InputFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.BufferDistance, 10.0);

            // _inputParam[1].Value = 10.0;
            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.OutputPolygonFeatureSet);
        }

        #endregion
    }
}