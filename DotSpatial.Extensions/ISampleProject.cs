// -----------------------------------------------------------------------
// <copyright file="ISampleProject.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Used to specify a sample .dspx project that the user can download as a package and open.
    /// </summary>
    public interface ISampleProject
    {
        /// <summary>
        /// Gets the absolute path to project file. This includes the directory, filename, and extension
        /// </summary>
        public string AbsolutePathToProjectFile { get; }

        /// <summary>
        /// Gets the name to display to the user when they are selecting a project.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the project
        /// </summary>
        public string Description { get; }
    }
}
