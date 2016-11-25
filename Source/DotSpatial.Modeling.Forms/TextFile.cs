// ********************************************************************************************************
// Product Name: DotSpatial.Tools.OpenFileParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
//
// ********************************************************************************************************
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