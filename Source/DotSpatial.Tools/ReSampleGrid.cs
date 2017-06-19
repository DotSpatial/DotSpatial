// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using GeoAPI.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Change the cell size of a grid.
    /// </summary>
    public class ReSampleGrid : Tool
    {
        #region Fields

        private Parameter[] _inputParam;
        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReSampleGrid"/> class.
        /// </summary>
        public ReSampleGrid()
        {
            Name = TextStrings.ChangeCellSize;
            Category = TextStrings.RasterOverlay;
            Description = TextStrings.ReSampleGridDescription;
            ToolTip = TextStrings.ChangeCellSize;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            DoubleParam dp1 = _inputParam[1] as DoubleParam;
            DoubleParam dp2 = _inputParam[2] as DoubleParam;
            double value1 = dp1?.Value ?? 0;
            double value2 = dp2?.Value ?? 0;

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, value1, value2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ReSample Opaeration tool programmatically
        /// Ping deleted the static property for external testing.
        /// </summary>
        /// <param name="input1">The input raster.</param>
        /// <param name="newCellHeight">The size of the cell's hight.</param>
        /// <param name="newCellWidth">The size of the cell's width.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        public bool Execute(IRaster input1, double newCellHeight, double newCellWidth, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || newCellWidth == 0 || output == null)
            {
                return false;
            }

            Extent envelope = input1.Bounds.Extent;

            // Calculate new number of columns and rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / newCellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / newCellHeight));

            int previous = 0;

            // create output raster
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, input1.DataType, new[] { string.Empty });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            // Loop throug every cell for new value
            int max = output.Bounds.NumRows + 1;
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    // Projet the cell position to Map
                    Coordinate cellCenter = output.CellToProj(i, j);
                    var index1 = input1.ProjToCell(cellCenter);

                    double val;
                    if (index1.Row <= input1.EndRow && index1.Column <= input1.EndColumn && index1.Row > -1 && index1.Column > -1)
                    {
                        val = input1.Value[index1.Row, index1.Column] == input1.NoDataValue ? output.NoDataValue : input1.Value[index1.Row, index1.Column];
                    }
                    else
                    {
                        val = output.NoDataValue;
                    }

                    output.Value[i, j] = val;

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

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[3];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster)
                                 {
                                     HelpText = TextStrings.InputtheRasterforCellsizeChange
                                 };
            _inputParam[1] = new DoubleParam(TextStrings.InputnewcellHight)
                                 {
                                     HelpText = TextStrings.DisplayingistheOldCellHight
                                 };
            _inputParam[2] = new DoubleParam(TextStrings.InputnewcellWidth)
                                 {
                                     HelpText = TextStrings.DisplayingistheOldCellHight
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
        /// <param name="sender">Sender that fired this event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            // This will Diplay NoDataValue(Already exisit) in the Optional input box;
            if (sender != _inputParam[0])
            {
                return;
            }

            IRaster inputTemp = _inputParam[0].Value as IRaster;
            if (inputTemp == null)
            {
                return;
            }

            DoubleParam inParam1 = _inputParam[1] as DoubleParam;
            if (inParam1 != null)
            {
                inParam1.Value = inputTemp.CellHeight;
            }

            DoubleParam inParam2 = _inputParam[2] as DoubleParam;
            if (inParam2 != null)
            {
                inParam2.Value = inputTemp.CellWidth;
            }
        }

        #endregion
    }
}