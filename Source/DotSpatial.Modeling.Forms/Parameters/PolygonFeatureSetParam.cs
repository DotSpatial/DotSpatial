// ********************************************************************************************************
// Product Name: DotSpatial.Tools.PolygonFeatureSetParam
// Description:  Parameters past back from a ITool to the toolbox manager
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
using System.IO;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Polygon Feature Set Parameters past back from a ITool to the toolbox manager
    /// </summary>
    public class PolygonFeatureSetParam : Parameter
    {
        /// <summary>
        /// Creates a new Polygon Feature Set parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public PolygonFeatureSetParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial PolygonFeatureSet Param";
        }

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new IFeatureSet Value
        {
            get { return (IFeatureSet)base.Value; }
            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        /// <summary>
        /// Generates a default instance of the data type so that tools have something to write too
        /// </summary>
        /// <param name="path"></param>
        public override void GenerateDefaultOutput(string path)
        {
            FeatureSet addedFeatureSet = new PolygonShapefile
                                             {
                                                 Filename =
                                                     Path.GetDirectoryName(path) +
                                                     Path.DirectorySeparatorChar + ModelName + ".shp"
                                             };
            Value = addedFeatureSet;
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new PolygonElement(this, dataSets));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new PolygonElementOut(this, dataSets));
        }
    }
}