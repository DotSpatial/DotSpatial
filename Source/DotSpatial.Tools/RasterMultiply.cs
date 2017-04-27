// -----------------------------------------------------------------------
// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterMultiply.cs
// Description:  Multiply two raster layers cell by cell.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date             |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for RasterMultiply
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This will Multiply two rasters values cell by cell
    /// </summary>
    public class RasterMultiply : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMultiply"/> class.
        /// </summary>
        public RasterMultiply()
        {
            Name = TextStrings.MultiplyRasterLayers;
            Category = TextStrings.RasterOverlay;
            Description = TextStrings.RasterMultiplyDescription;
            ToolTip = TextStrings.RasterMultiplyDescription;
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
        /// <returns>Boolean, true if the execute was successful.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            IRaster input2 = _inputParam[1].Value as IRaster;

            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes RastersMultiply Opaeration tool programmatically
        /// Ping Yang deleted static property for external testing 01/2010.
        /// </summary>
        /// <param name="input1">The original input input raster for raster multiplication.</param>
        /// <param name="input2">The second input raster with values to multiply by.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the execute was successful.</returns>
        public bool Execute(IRaster input1, IRaster input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            RasterMagic magic = new RasterMagic(Operation);
            return magic.RasterMath(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.input1Raster)
                                 {
                                     HelpText = TextStrings.InputFirstRaster
                                 };
            _inputParam[1] = new RasterParam(TextStrings.input2Raster)
                                 {
                                     HelpText = TextStrings.InputSecondRasterforMultiply
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
                                  {
                                      HelpText = TextStrings.ResultRasterDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        private static double Operation(double val1, double val2)
        {
            return val1 * val2;
        }

        #endregion
    }
}