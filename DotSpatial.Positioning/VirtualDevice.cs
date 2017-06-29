// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.IO;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a simulated GPS device.
    /// </summary>
    public sealed class VirtualDevice : Device
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Emulator _emulator;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified emulator.
        /// </summary>
        /// <param name="emulator">The emulator.</param>
        public VirtualDevice(Emulator emulator)
        {
            if (emulator == null)
                throw new ArgumentNullException("emulator", "The emulator of a virtual device cannot be null.");

            _emulator = emulator;

            // These devices are always GPS devices
            SetIsGpsDevice(true);
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Gets the Name of the Virtual Device
        /// </summary>
        public override string Name
        {
            get
            {
                return _emulator.Name;
            }
        }

        /// <summary>
        /// Overrides OnChaceRemove
        /// Since emulators don't qualify as detected devices, this
        /// method has no behavior.
        /// </summary>
        protected override void OnCacheRemove()
        {
        }

        /// <summary>
        /// Records information about this device to the registry.
        /// </summary>
        protected override void OnCacheWrite()
        {
            /* Since emulators don't qualify as detected devices, this
             * method has no behavior.
             */
        }

        /// <summary>
        /// Reads information about this device from the registry.
        /// </summary>
        protected override void OnCacheRead()
        {
            /* Since emulators don't qualify as detected devices, this
             * method has no behavior.
             */
        }

        /// <summary>
        /// Creates a new Stream object for the device.
        /// </summary>
        /// <param name="access">The access.</param>
        /// <param name="sharing">The sharing.</param>
        /// <returns>A <strong>Stream</strong> object.</returns>
        protected override Stream OpenStream(FileAccess access, FileShare sharing)
        {
            // Start the emulation thread
            _emulator.Open();

            // Return the emulator
            return _emulator;
        }

        #endregion Overrides
    }
}