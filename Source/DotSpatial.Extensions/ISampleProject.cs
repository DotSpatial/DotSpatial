namespace DotSpatial.Extensions
{
    /// <summary>
    /// Used to specify a sample .dspx project that the user can download as a package and open.
    /// </summary>
    public interface ISampleProject
    {
        /// <summary>
        /// Gets the absolute path to project file. This includes the directory, filename, and extension
        /// </summary>
        string AbsolutePathToProjectFile { get; }

        /// <summary>
        /// Gets the name to display to the user when they are selecting a project.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the project
        /// </summary>
        string Description { get; }
    }
}
