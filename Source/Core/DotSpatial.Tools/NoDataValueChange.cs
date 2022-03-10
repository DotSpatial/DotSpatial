// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Tool that changes the no data value of a raster file.
    /// </summary>
    public class NoDataValueChange : Tool
    {
        #region Fields
        private Parameter[] _inputParam;
        private Parameter[] _outputParam;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataValueChange"/> class.
        /// </summary>
        public NoDataValueChange()
        {
            Name = TextStrings.NoDataValueChange;
            Category = TextStrings.RasterOverlay;
            Description = TextStrings.NoDataValueChangeDescription;
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
            IRaster input1 = _inputParam[0].Value as IRaster;
            double value1 = Convert.ToDouble(_inputParam[1].Value);

            double value2 = Convert.ToDouble(_inputParam[2].Value);

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, value1, value2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Erase Opaeration tool programmatically.
        /// Ping Yang deleted static for external testing 01/2010.
        /// </summary>
        /// <param name="input">The input raster.</param>
        /// <param name="oldValue">The original double value representing no-data.</param>
        /// <param name="newValue">The new double value representing no-data.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IRaster input, double oldValue, double newValue, IRaster output, ICancelProgressHandler cancelProgressHandler)
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
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, dataType, new[] { string.Empty });
            RasterBounds bound = new(noOfRow, noOfCol, envelope);
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
                    cancelProgressHandler.Progress(current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

            // output = Temp;
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster)
                                 {
                                     HelpText = TextStrings.InputRasternoValue
                                 };
            _inputParam[1] = new StringParam(TextStrings.Optional)
                                 {
                                     HelpText = TextStrings.Optionaltochange
                                 };
            _inputParam[2] = new StringParam(TextStrings.UserNewValues)
                                 {
                                     HelpText = TextStrings.UserinputNewValue
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
                                  {
                                      HelpText = TextStrings.newrastername
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        /// <param name="sender">Sender that fired the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            // This will Diplay NoDataValue(Already exisit) in the Optional input box;
            if (sender != _inputParam[0])
            {
                return;
            }

            if (_inputParam[1] is StringParam string1 && _inputParam[0].Value is IRaster inputTemp)
            {
                string1.Value = inputTemp.NoDataValue.ToString();
            }
        }

        #endregion
    }
}