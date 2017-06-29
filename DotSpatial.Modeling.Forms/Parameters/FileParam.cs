// ********************************************************************************************************
// Product Name: DotSpatial.Tools.OpenFileParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
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
// The Initial Developer of this Original Code is Teva Veluppillai. Created in Feb, 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A File access parameter
    /// </summary>
    public class FileParam : Parameter
    {
        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="filter">The string dialog filter to use.</param>
        public FileParam(string name, string filter)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
            DialogFilter = filter;
        }

        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public FileParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
        }

        /// <summary>
        /// Creates a new Feature Set Parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">A TextFile</param>
        public FileParam(string name, TextFile value)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial File Param";
            Value = value;
        }

        /// <summary>
        /// Gets or sets the filter expression to limit extensions that can be used for this FileParam.
        /// </summary>
        public string DialogFilter { get; set; }

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new TextFile Value
        {
            get { return (TextFile)base.Value; }
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
            TextFile addedTextFile = new TextFile
                                     {
                                         FileName =
                                             Path.GetDirectoryName(path) +
                                             Path.DirectorySeparatorChar + ModelName
                                     };
            Value = addedTextFile;
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualize INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new OpenFileElement(this, dataSets));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualize OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new SaveFileElement(this, dataSets));
        }
    }
}