// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Project file provider with capability to save files.
    /// </summary>
    [InheritedExport]
    public interface ISaveProjectFileProvider : IProjectFileProvider
    {
        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="graph">The control graph.</param>
        void Save(string fileName, string graph);
    }
}