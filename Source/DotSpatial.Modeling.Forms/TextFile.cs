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

using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// This class is used to enable linking of tools
    /// that work with File parameters
    /// </summary>
    public class TextFile : DataSet
    {
        /// <summary>
        /// Text File constructor
        /// </summary>
        public TextFile()
        {
        }

        /// <summary>
        /// Creates a new instance of the TextFile class
        /// </summary>
        /// <param name="fileName">the associated file name</param>
        public TextFile(string fileName)
        {
            Filename = fileName;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Filename;
        }
    }
}