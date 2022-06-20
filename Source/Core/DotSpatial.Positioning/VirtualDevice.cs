// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
            {
                throw new ArgumentNullException(nameof(emulator), "The emulator of a virtual device cannot be null.");
            }

            _emulator = emulator;

            // These devices are always GPS devices
            SetIsGpsDevice(true);
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Gets the Name of the Virtual Device
        /// </summary>
        public override string Name => _emulator.Name;

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