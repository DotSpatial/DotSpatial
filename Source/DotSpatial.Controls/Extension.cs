// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 1:28:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Methods

        /// <summary>
        /// Activates this provider
        /// </summary>
        public virtual void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this provider
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that is true if the extension is active and running.
        /// </summary>
        public bool IsActive { get; set; }

        private bool deactivationAllowed = true;
        /// <summary>
        /// Gets a value indicating whether [deactivation is allowed].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [deactivation is allowed]; otherwise, <c>false</c>.
        /// </value>
        public bool DeactivationAllowed
        {
            get
            {
                // Assemblies in the Application Extensions folder cannot be deactivated.
                const string STR_ApplicationExtensionsDirectoryName = @"\Application Extensions\";
                if(!(ReferenceAssembly.Location.IndexOf(STR_ApplicationExtensionsDirectoryName, StringComparison.OrdinalIgnoreCase) < 0))
                    deactivationAllowed = false;
                return deactivationAllowed;
            }
            set
            {
                deactivationAllowed = value;
            }
        }

        /// <summary>
        /// Specifies the activation priority order
        /// </summary>
        public virtual int Priority { get { return 0; } }

        #endregion

        /// <summary>
        /// Gets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        public AppManager App { get; set; }
    }
}