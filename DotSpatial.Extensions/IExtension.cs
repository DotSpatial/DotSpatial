// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 1:35:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that is true if the extension is active and running.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether [deactivation is allowed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deactivation is allowed]; otherwise, <c>false</c>.
        /// </value>
        bool DeactivationAllowed { get; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the build date.
        /// </summary>
        string BuildDate { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the Assembly Qualified FullName.
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// Specifies the activation priority order
        /// </summary>
        int Priority { get; }

        #endregion
    }
}