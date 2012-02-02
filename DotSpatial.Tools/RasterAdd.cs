// ********************************************************************************************************
// Product Name: MapWindow.Tools.mwRasterAdd
// Description:  Add Two Raster Layer cell by cell
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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -----------------------|------------------------|--------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for RasterAdd
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Raster Add
    /// </summary>
    public class RasterAdd : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterAdd"/> class.
        /// </summary>
        public RasterAdd()
        {
            this.Name = TextStrings.AddRasterLayers;
            this.Category = TextStrings.RasterOverlay;
            this.Description = TextStrings.AddTwoRasteLayer;
            this.ToolTip = TextStrings.AddTwoRasteLayer;
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
            IRaster input2 = _inputParam[1].Value as IRaster;

            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        private static double Operation(double val1, double val2)
        {
            return val1 + val2;
        }

        /// <summary>
        /// Executes the Erase Operation tool programaticaly, 01/10 Ping Yang changed it for external testing.
        /// </summary>
        /// <param name="input1">The first input raster.</param>
        /// <param name="input2">The input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
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
            _inputParam[0] = new RasterParam(TextStrings.input1Raster) { HelpText = TextStrings.InputFirstRaster };
            _inputParam[1] = new RasterParam(TextStrings.input2Raster)
                                 {
                                     HelpText = TextStrings.InputSecondRasterforAdding
                                 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster) { HelpText = TextStrings.ResultRasterDirectory };
        }

        #endregion
    }
}