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
    /// This class is used to enable linking of tools that work with File parameters.
    /// </summary>
    public class TextFile : DataSet
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        public TextFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <param name="fileName">the associated file name</param>
        public TextFile(string fileName)
        {
            Filename = fileName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the file name.
        /// </summary>
        /// <returns>The file name</returns>
        public override string ToString()
        {
            return Filename;
        }

        #endregion
    }
}