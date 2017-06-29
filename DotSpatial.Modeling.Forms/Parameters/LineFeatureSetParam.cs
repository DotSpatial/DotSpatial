// ********************************************************************************************************
// Product Name: DotSpatial.Tools.LineFeatureSetParam
// Description:  Parameters past back from a ITool to the toolbox manager
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Line Feature Parameter past back from a ITool to the toolbox manager
    /// </summary>
    public class LineFeatureSetParam : Parameter
    {
        /// <summary>
        /// Creates a new Line Feature Set parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public LineFeatureSetParam(string name)
        {
            Name = name;
            Value = new FeatureSet();
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial LineFeatureSet Param";
            DefaultSpecified = false;
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
            FeatureSet addedFeatureSet = new LineShapefile
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
            return (new LineElement(this, dataSets));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new LineElementOut(this, dataSets));
        }
    }
}