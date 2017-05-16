// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Project file provider with capability to open files.
    /// </summary>
    [InheritedExport]
    public interface IOpenProjectFileProvider : IProjectFileProvider
    {
        /// <summary>
        /// Opens the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True, if the file was opened.</returns>
        bool Open(string fileName);
    }
}