// *******************************************************************************************************
// Product: DotSpatial.Tools.NoDataValueChange.cs
// Description:  Tool that changes the no data value of a raster file.
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//---------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|-------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for NoDataValueChange
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// No Data Value Change
    /// </summary>
    public class NoDataValueChange : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the NoDataValueChange class.
        /// </summary>
        public NoDataValueChange()
        {
            this.Name = TextStrings.NoDataValueChange;
            this.Category = TextStrings.RasterOverlay;
            this.Description = TextStrings.NoDataValueChangeDescription;

            // this.ToolTip = TextStrings.NoDataValueChange;
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
            IRaster input1 = _inputParam[0].Value as IRaster;
            double value1 = Convert.ToDouble(_inputParam[1].Value);

            double value2 = Convert.ToDouble(_inputParam[2].Value);

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, value1, value2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Erase Opaeration tool programaticaly
        /// Ping Yang deleted static for external testing 01/2010
        /// </summary>
        /// <param name="input">The input raster</param>
        /// <param name="oldValue">The original double value representing no-data</param>
        /// <param name="newValue">The new double value representing no-data</param>
        /// <param name="output">The output raster</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(
            IRaster input,
            double oldValue,
            double newValue,
            IRaster output,
            ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || newValue == 0 || output == null)
            {
                return false;
            }

            Extent envelope = input.Bounds.Extent;

            int noOfCol = input.NumColumns;
            int noOfRow = input.NumRows;
            int previous = 0;
            Type dataType = input.DataType;

            // create output raster
            output = Raster.CreateRaster(
                output.Filename, string.Empty, noOfCol, noOfRow, 1, dataType, new[] { string.Empty });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = newValue;

            // Loop throug every cell
            int max = output.Bounds.NumRows + 1;
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    if (input.Value[i, j] == oldValue)
                    {
                        output.Value[i, j] = newValue;
                    }
                    else
                    {
                        output.Value[i, j] = input.Value[i, j];
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }

                int current = Convert.ToInt32(Math.Round(i * 100D / max));

                // only update when increment in persentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

            // output = Temp;
            output.Save();
            return true;
        }                                           

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster) { HelpText = TextStrings.InputRasternoValue };
            _inputParam[1] = new StringParam(TextStrings.Optional) { HelpText = TextStrings.Optionaltochange };
            _inputParam[2] = new StringParam(TextStrings.UserNewValues) { HelpText = TextStrings.UserinputNewValue };

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster) { HelpText = TextStrings.newrastername };
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        public override void ParameterChanged(Parameter sender)
        {
            // This will Diplay NoDataValue(Already exisit) in the Optional input box;
            if (sender != _inputParam[0])
            {
                return;
            }

            IRaster inputTemp = _inputParam[0].Value as IRaster;
            StringParam string1 = _inputParam[1] as StringParam;
            if (string1 != null && inputTemp != null)
            {
                string1.Value = inputTemp.NoDataValue.ToString();
            }
        }

        #endregion
    }
}