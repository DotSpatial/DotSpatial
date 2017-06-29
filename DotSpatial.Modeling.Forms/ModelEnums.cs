// ********************************************************************************************************
// Product Name: DotSpatial.Tools Enumerations for the Model
// Description:  Contains enumerations used in the model
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Definitions for the shapes that components can have in the modeler
    /// </summary>
    public enum ModelShape
    {
        /// <summary>
        /// Defines the Model Component as a Rectangle
        /// </summary>
        Rectangle,
        /// <summary>
        /// Defines the Model Component as a Triangle
        /// </summary>
        Triangle,
        /// <summary>
        /// Defines the Model Component as a Ellipse
        /// </summary>
        Ellipse,
        /// <summary>
        /// Defines an Arrow
        /// </summary>
        Arrow
    }

    /// <summary>
    /// Used internally to decided if a tool has executed, is done, or finished in error
    /// </summary>
    public enum ToolExecuteStatus
    {
        /// <summary>
        /// The tool has not been run yet
        /// </summary>
        NotRun,

        /// <summary>
        /// The tool is currently executing
        /// </summary>
        Running,

        /// <summary>
        /// The tool finished running succesfully
        /// </summary>
        Done,

        /// <summary>
        /// The tool returned an error when executing
        /// </summary>
        Error
    }
}