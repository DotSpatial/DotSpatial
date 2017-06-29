// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterFromLAS.cs
// Description:  Converts the point elevations in a LAS file to a Raster.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster Add
    /// </summary>
    public class RasterFromLAS : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the RasterFromLAS class.
        /// </summary>
        public RasterFromLAS()
        {
            this.Name = TextStrings.RasterFromLAS;
            this.Category = TextStrings.Conversion;
            this.Description = TextStrings.HelpLASTool;
            this.ToolTip = TextStrings.ToolFromLasToRaster;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input parameter array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output parameter array
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
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            string input1 = _inputParam[0].Value as string;
            Extent input2 = _inputParam[1].Value as Extent;
            int numRows = (int)_inputParam[2].Value;
            int numCols = (int)_inputParam[3].Value;
            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, input2, numRows, numCols, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the RasterFromLAS tool.
        /// </summary>
        /// <param name="filenameLAS">The string filename of the LAS file to convert.</param>
        /// <param name="outputExtent">The extent of the output raster.</param>
        /// <param name="numRows">The integer number of rows of the output raster.</param>
        /// <param name="numColumns">The integer number of columns.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        public bool Execute(
            string filenameLAS,
            Extent outputExtent,
            int numRows,
            int numColumns,
            IRaster output,
            ICancelProgressHandler cancelProgressHandler)
        {
            // create output raster
            output = Raster.CreateRaster(
                output.Filename, string.Empty, numColumns, numRows, 1, typeof(int), new[] { string.Empty });
            RasterBounds bound = new RasterBounds(numRows, numColumns, outputExtent);
            output.Bounds = bound;

            output.NoDataValue = int.MinValue;

            ProgressMeter pm = new ProgressMeter(
                cancelProgressHandler,
                TextStrings.ConvertingLAS + filenameLAS + TextStrings.Progresstoraster + "...",
                numRows);

            for (int row = 0; row < numRows; row++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    // TO DO: PING CAN ADD LAS READING AND CELL ASSIGNMENT HERE
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }

                pm.CurrentValue = row;
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
            _inputParam = new Parameter[4];
            _inputParam[0] = new FileParam(TextStrings.lasFilename, "LAS Files(*.las)|*.las")
                                 {
                                     HelpText = TextStrings.LasFullpath
                                 };
            ExtentParam p = new ExtentParam(TextStrings.RasterExtent);
            p.HelpText = TextStrings.GeographicExtent;
            p.DefaultToMapExtent = true;

            _inputParam[1] = p;

            _inputParam[2] = new IntParam(TextStrings.NumRows) { HelpText = TextStrings.numberofrows };

            _inputParam[3] = new IntParam(TextStrings.NumColumns) { HelpText = TextStrings.numberofcolums };

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster) { HelpText = TextStrings.ResultRasterDirectory };
        }

        /// <summary>
        /// Fires when one of the parameters value has been changed, usually when a user changes a input or
        /// output Parameter value, this can be used to populate input2 Parameter default values.
        /// </summary>
        public override void ParameterChanged(Parameter sender)
        {
            return;
        }

        #endregion
    }
}