﻿namespace DotSpatial.Extensions
{
    /// <summary>
    /// Generic interface for project file providers.
    /// </summary>
    public interface IProjectFileProvider
    {
        /// <summary>
        /// Gets the file type description.
        /// </summary>
        string FileTypeDescription { get; }

        /// <summary>
        /// Gets the extension, which by convention will be lower case.
        /// </summary>
        string Extension { get; }
    }
}