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
// The Original Code is from DotSpatial.Positioning.dll
//
// Created on 11/1/2010 By Ted Dunsford to try to deal with Security exceptions by a broken LinkDemand.
//
// ********************************************************************************************************
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace DotSpatial.Positioning
{
    //
    // Safe Bluetooth Enumeration Handle
    //
    //[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    /// <summary>
    ///
    /// </summary>
    [SecurityCritical]
    public sealed class SafeBluetoothRadioFindHandle
        : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Runtime.ConstrainedExecution.CriticalFinalizerObject"/> class.
        /// </summary>
        private SafeBluetoothRadioFindHandle() : base(true) { }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.</returns>
        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            return BluetoothFindRadioClose(handle);
        }

        /// <summary>
        /// Bluetoothes the find radio close.
        /// </summary>
        /// <param name="hFind">The h find.</param>
        /// <returns></returns>
        [DllImport("Irprops.cpl")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecurityCritical]
        private static extern bool BluetoothFindRadioClose(IntPtr hFind);
    }
}