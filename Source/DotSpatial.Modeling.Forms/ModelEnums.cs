// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// The tool finished running successfully
        /// </summary>
        Done,

        /// <summary>
        /// The tool returned an error when executing
        /// </summary>
        Error
    }
}