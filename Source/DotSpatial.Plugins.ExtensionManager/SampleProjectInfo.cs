namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Contains informations about DotSpatial sample projects.
    /// </summary>
    internal class SampleProjectInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the absolute path to the project file.
        /// </summary>
        public string AbsolutePathToProjectFile { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        #endregion
    }
}