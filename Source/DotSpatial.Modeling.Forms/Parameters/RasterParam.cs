// ********************************************************************************************************
// Product Name: DotSpatial.Tools.RasterParam
// Description:  Raster parameter allows ITools to specify that they require a Raster data set as input
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Raster parameter allows ITools to specify that they require a Raster data set as input
    /// </summary>
    public class RasterParam : Parameter
    {
        /// <summary>
        /// Creates a new Raster parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public RasterParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial Raster Param";
        }

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new IRaster Value
        {
            get { return (IRaster)base.Value; }
            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new RasterElement(this, dataSets));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new RasterElementOut(this, dataSets));
        }

        /// <summary>
        /// Creates a new blank raster
        /// </summary>
        public override void GenerateDefaultOutput(string path)
        {
            Value = new Raster { Filename = path };
        }
    }
}