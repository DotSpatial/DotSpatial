// -----------------------------------------------------------------------
// *******************************************************************************************************
// Product: DotSpatial.Tools.ReSampleGrid.cs
// Description:  Change the cell size.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for ReSampleGrid
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Grid Resample
    /// </summary>
    public class ReSampleGrid : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the resample grid tool
        /// </summary>
        public ReSampleGrid()
        {
            this.Name = TextStrings.ChangeCellSize;
            this.Category = TextStrings.RasterOverlay;
            this.Description = TextStrings.ReSampleGridDescription;
            this.ToolTip = TextStrings.ChangeCellSize;
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
            DoubleParam dp1 = _inputParam[1] as DoubleParam;
            DoubleParam dp2 = _inputParam[2] as DoubleParam;
            double value1 = 0;
            double value2 = 0;
            if (dp1 != null)
            {
                value1 = dp1.Value;
            }

            if (dp2 != null)
            {
                value2 = dp2.Value;
            }

            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input1, value1, value2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the ReSample Opaeration tool programaticaly
        /// Ping deleted the static property for external testing.
        /// </summary>
        /// <param name="input1">The input raster.</param>
        /// <param name="newCellHeight">The size of the cell's hight.</param>
        /// <param name="newCellWidth">The size of the cell's width.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        public bool Execute(
            IRaster input1,
            double newCellHeight,
            double newCellWidth,
            IRaster output,
            ICancelProgressHandler cancelProgressHandler)
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

            // ************OLD Method
            ////Create the new raster with the appropriate dimensions
            // Raster Temp = new Raster();
            ////Temp.CreateNew(output.Filename, noOfRow, noOfCol, input1.DataType);
            // Temp.CreateNew(output.Filename, "", noOfCol, noOfRow, 1, input1.DataType, new string[] { "" });
            // Temp.CellWidth = newCellSize;
            // Temp.CellHeight = oldCellSize;
            // Temp.Xllcenter = input1.Bounds.Envelope.Minimum.X + (Temp.CellWidth / 2);
            // Temp.Yllcenter = input1.Bounds.Envelope.Minimum.Y + (Temp.CellHeight / 2);
            // ***************

            // create output raster
            output = Raster.CreateRaster(
                output.Filename, string.Empty, noOfCol, noOfRow, 1, input1.DataType, new[] { string.Empty });
            RasterBounds bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            RcIndex index1;

            // Loop throug every cell for new value
            int max = output.Bounds.NumRows + 1;
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    // Projet the cell position to Map
                    Coordinate cellCenter = output.CellToProj(i, j);
                    index1 = input1.ProjToCell(cellCenter);

                    double val;
                    if (index1.Row <= input1.EndRow && index1.Column <= input1.EndColumn && index1.Row > -1
                        && index1.Column > -1)
                    {
                        val = input1.Value[index1.Row, index1.Column] == input1.NoDataValue
                                  ? output.NoDataValue
                                  : input1.Value[index1.Row, index1.Column];
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