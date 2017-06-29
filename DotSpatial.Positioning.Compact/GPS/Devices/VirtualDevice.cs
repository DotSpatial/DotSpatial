using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DotSpatial.Positioning.Gps.Emulators;
using DotSpatial.Positioning.Gps.IO;

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Represents a simulated GPS device.
    /// </summary>
    public sealed class VirtualDevice : Device
    {
        private Emulator _Emulator;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified emulator.
        /// </summary>
        /// <param name="emulator"></param>
        public VirtualDevice(Emulator emulator)
        {
            if(emulator == null)
                throw new ArgumentNullException("emulator", "The emulator of a virtual device cannot be null.");

            _Emulator = emulator;
            
            // These devices are always GPS devices
            SetIsGpsDevice(true);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets the Name of the Virtual Device
        /// </summary>
        public override string Name
        {
            get
            {
                return _Emulator.Name;
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

        protected override void OnCacheWrite()
        {
            /* Since emulators don't qualify as detected devices, this
             * method has no behavior.
             */
        }

        protected override void OnCacheRead()
        {
            /* Since emulators don't qualify as detected devices, this
             * method has no behavior.
             */
        }

        protected override Stream OpenStream(FileAccess access, FileShare sharing)
        {
            // Start the emulation thread
            _Emulator.Open();

            // Return the emulator
            return _Emulator;
        }

        #endregion
    }
}