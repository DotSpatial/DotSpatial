// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel.Composition;
using DotSpatial.Extensions;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A provider is the base class that enables plug-ins to work with the Application Manager.
    /// This is true whether it is a plug-in or a data provider or some other extension.
    /// </summary>
    [Serializable]
    public abstract class Extension : AssemblyInformation, IExtension
    {
        #region Fields

        private bool _deactivationAllowed = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        public AppManager App { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [deactivation is allowed].
        /// </summary>
        /// <value><c>true</c> if [deactivation is allowed]; otherwise, <c>false</c>.</value>
        public bool DeactivationAllowed
        {
            get
            {
                // Assemblies in the Application Extensions folder cannot be deactivated.
                const string StrApplicationExtensionsDirectoryName = @"\Application Extensions\";
                if (ReferenceAssembly.Location != null && ReferenceAssembly.Location.IndexOf(StrApplicationExtensionsDirectoryName, StringComparison.OrdinalIgnoreCase) >= 0)
                    _deactivationAllowed = false;
                return _deactivationAllowed;
            }

            set
            {
                _deactivationAllowed = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extension is active and running.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the activation priority order.
        /// </summary>
        public virtual int Priority => 0;

        #endregion

        #region Methods

        /// <summary>
        /// Activates this provider.
        /// </summary>
        public virtual void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this provider.
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false;
        }

        #endregion
    }
}