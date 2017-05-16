// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// The IExtension interface represents the shared content between all providers and plugins.  This simply acts like
    /// an on-off switch for enabling or disabling the extension.
    /// </summary>
    [InheritedExport]
    public interface IExtension
    {
        #region Properties

        /// <summary>
        /// Gets the Assembly Qualified FullName.
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the build date.
        /// </summary>
        string BuildDate { get; }

        /// <summary>
        /// Gets a value indicating whether [deactivation is allowed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deactivation is allowed]; otherwise, <c>false</c>.
        /// </value>
        bool DeactivationAllowed { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the extension is active and running.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the activation priority order.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        string Version { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Activates this extension
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivates this extension
        /// </summary>
        void Deactivate();

        #endregion
    }
}