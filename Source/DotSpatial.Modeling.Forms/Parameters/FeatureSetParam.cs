// ********************************************************************************************************
// Product Name: DotSpatial.Tools.FeatureSetParam
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
using System.IO;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Raster parameter allows ITools to specify that they require a Raster data set as input
    /// </summary>
    public class FeatureSetParam : Parameter
    {
        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public FeatureSetParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial FeatureSet Param";
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
            FeatureSet addedFeatureSet = new Shapefile
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
            return (new FeatureSetElement(this, dataSets));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new FeatureSetElementOut(this, dataSets));
        }
    }
}