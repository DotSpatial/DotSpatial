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
using System.Runtime.InteropServices;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// An internal class for hosting windows members.
    /// </summary>
    internal static class NativeMethods2
    {
        #region Cross-Platform Members

        /*
         * Many API calls are identical on either framework. The only thing different is
         * the dll name in the DllImport atribute, which can be a string constant. The
         * API calls that are used by both frameworks can be moved here to lessen the
         * code footprint for the sake of maintainability. Others, like the GPSID and
         * Serial Comm can be kept seprarated.
         */
        /// <summary>
        ///
        /// </summary>
        private const string KERNEL32 = "kernel32.dll";

        #region Kernel32

        /// <summary>
        ///
        /// </summary>
        public static readonly IntPtr InvalidHandle = new IntPtr(-1);

        //http://msdn.microsoft.com/en-us/library/ms724211%28VS.85%29.aspx
        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        #region IOControl

        //http://msdn.microsoft.com/en-us/library/aa363216%28VS.85%29.aspx
        /// <summary>
        /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
        /// </summary>
        /// <param name="hDevice">A handle to the device on which the operation is to be performed. The device is typically a volume,
        /// directory, file, or stream. To retrieve a device handle, use the CreateFile function. For more information, see Remarks.</param>
        /// <param name="dwIoControlCode">The control code for the operation. This value identifies the specific operation to be
        /// performed and the type of device on which to perform it.  For a list of the control codes, see Remarks.
        /// The documentation for each control code provides usage details for the lpInBuffer, nInBufferSize, lpOutBuffer,
        /// and nOutBufferSize parameters.</param>
        /// <param name="lpInBuffer">A pointer to the input buffer that contains the data required to perform the operation. The format of this data depends on the value of the dwIoControlCode parameter.
        /// This parameter can be NULL if dwIoControlCode specifies an operation that does not require input data.</param>
        /// <param name="nInBufferSize">The size of the input buffer, in bytes.</param>
        /// <param name="lpOutBuffer">A pointer to the output buffer that is to receive the data returned by the operation. The format of this data
        /// depends on the value of the dwIoControlCode parameter.
        /// This parameter can be NULL if dwIoControlCode specifies an operation that does not return data.</param>
        /// <param name="nOutBufferSize">The size of the output buffer, in bytes.</param>
        /// <param name="lpBytesReturned">The number of bytes returned in the out buffer.</param>
        /// <param name="lpOverlapped">A pointer to a callback for async commands.</param>
        /// <returns>If the operation completes successfully, the return value is nonzero.
        /// If the operation fails or is pending, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        //http://msdn.microsoft.com/en-us/library/aa363858%28VS.85%29.aspx
        /// <summary>
        /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream,
        /// directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.
        /// The function returns a handle that can be used to access the file or device for various types of I/O depending
        /// on the file or device and the flags and attributes specified.
        /// To perform this operation as a transacted operation, which results in a handle that can be used for transacted
        /// I/O, use the CreateFileTransacted function.
        /// </summary>
        /// <param name="lpFileName">The name of the file or device to be created or opened.</param>
        /// <param name="dwDesiredAccess">The requested access to the file or device, which can be summarized as read, write, both or neither zero).</param>
        /// <param name="dwShareMode">The requested sharing mode of the file or device, which can be read, write, both,
        /// delete, all of these, or none (refer to the following table). Access requests to attributes or extended
        /// attributes are not affected by this flag.</param>
        /// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members:
        /// an optional security descriptor, and a Boolean value that determines whether the returned handle can be
        /// inherited by child processes.</param>
        /// <param name="dwCreationDisposition">An action to take on a file or device that exists or does not exist.</param>
        /// <param name="dwFlagsAndAttributes">The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files.</param>
        /// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created.</param>
        /// <returns>A handle to the device.</returns>
        [DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName, 							// file name
            FileAccess dwDesiredAccess, 					// access mode
            FileShare dwShareMode, 						// share mode
            uint lpSecurityAttributes, 					// SD
            FileMode dwCreationDisposition, 				// how to create
            FileAttributes dwFlagsAndAttributes, 		// file attributes
            IntPtr hTemplateFile						// handle to template file
            );

        //http://msdn.microsoft.com/en-us/library/aa365467%28VS.85%29.aspx
        /// <summary>
        /// Reads data from the specified file or input/output (I/O) device. Reads occur at the position
        /// specified by the file pointer if supported by the device.
        /// </summary>
        /// <param name="handle">A handle to the device (for example, a file, file stream, physical disk, volume, console buffer,
        /// tape drive, socket, communications resource, mailslot, or pipe).</param>
        /// <param name="lpBuffer">A pointer to the buffer that receives the data read from a file or device.</param>
        /// <param name="nNumberOfBytesToRead">The maximum number of bytes to be read.</param>
        /// <param name="lpNumberOfBytesRead">A pointer to the variable that receives the number of bytes read when using
        /// a synchronous hFile parameter. ReadFile sets this value to zero before doing any work or error checking.
        /// Use NULL for this parameter if this is an asynchronous operation to avoid potentially erroneous results.</param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure is required if the hFile parameter was opened
        /// with FILE_FLAG_OVERLAPPED, otherwise it can be NULL.</param>
        /// <returns>If the function succeeds, the return value is nonzero (TRUE).</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool ReadFile(
            IntPtr handle,
            byte[] lpBuffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped);

        //http://msdn.microsoft.com/en-us/library/aa365747%28VS.85%29.aspx
        /// <summary>
        /// Writes data to the specified file or input/output (I/O) device.
        /// This function is designed for both synchronous and asynchronous operation. For a similar function
        /// designed solely for asynchronous operation, see WriteFileEx.
        /// </summary>
        /// <param name="handle">A handle to the file or I/O device (for example, a file, file stream, physical disk, volume,
        /// console buffer, tape drive, socket, communications resource, mailslot, or pipe).</param>
        /// <param name="lpBuffer">A pointer to the buffer containing the data to be written to the file or device.</param>
        /// <param name="nNumberOfBytesToWrite">The number of bytes to be written to the file or device.</param>
        /// <param name="lpNumberOfBytesWritten">A pointer to the variable that receives the number of bytes written when using
        /// a synchronous hFile parameter. WriteFile sets this value to zero before doing any work or error checking.
        /// Use NULL for this parameter if this is an asynchronous operation to avoid potentially erroneous results.</param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure is required if the hFile parameter was opened
        /// with FILE_FLAG_OVERLAPPED, otherwise this parameter can be NULL.</param>
        /// <returns>If the function succeeds, the return value is nonzero (TRUE).</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        public static extern bool WriteFile(
            IntPtr handle,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        #endregion

        #region System Clock

        // http://msdn.microsoft.com/en-us/library/ms724942(VS.85).aspx
        /// <summary>
        /// Sets the current local time and date.
        /// </summary>
        /// <param name="time">A pointer to a SYSTEMTIME structure that contains the new local date and time.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetLocalTime(ref SystemTime time);

        // http://msdn.microsoft.com/en-us/library/ms724942%28VS.85%29.aspx
        /// <summary>
        /// Sets the current system time and date. The system time is expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="time">A pointer to a SYSTEMTIME structure that contains the new system date and time.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetSystemTime(ref SystemTime time);

        #endregion

        #endregion

        // These structures are used on both desktop and mobile device platforms with NO changes

        #region Microsoft Bluetooth device discovery

        /// <summary>
        /// Device Connected
        /// </summary>
        public const int BTHNS_RESULT_DEVICE_CONNECTED = 0x00010000;

        /// <summary>
        /// Device Remembered
        /// </summary>
        public const int BTHNS_RESULT_DEVICE_REMEMBERED = 0x00020000;

        /// <summary>
        /// Device Authenticated
        /// </summary>
        public const int BTHNS_RESULT_DEVICE_AUTHENTICATED = 0x00040000;

        /// <summary>
        /// The namespace of Bluetooth, used by the WSAQUERYSET structure
        /// </summary>
        public const int NS_BTH = 16;

        /// <summary>
        /// The error code for success
        /// </summary>
        public const int ERROR_SUCCESS = 1306;

        // http://msdn.microsoft.com/en-us/library/ms737551(VS.85).aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class Blob
        {
            /// <summary>
            /// Size of the block of data pointed to by pBlobData, in bytes.  cbSize.
            /// </summary>
            public uint Size;

            /// <summary>
            /// Pointer to a block of data.  pInfo.
            /// </summary>
            public IntPtr Info;
        }

        // Represents information about a single Bluetooth device
        // http://msdn.microsoft.com/en-us/library/aa362934(VS.85).aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class BthDeviceInfo
        {
            // This class is created through marshaling.  Even
            // if these members are not used, they need to
            // be left as part of the class.
            /// <summary>
            /// Flags
            /// </summary>
            internal uint Flags;

            /// <summary>
            /// The blue tooth address.
            /// </summary>
            internal ulong AddressCode;

            /// <summary>
            /// The unsigned integer representing the class of the device.
            /// </summary>
            internal uint ClassOfDevice;

            /// <summary>
            /// The binary representation of the name.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 246)]
            internal byte[] NameBinary;

            /// <summary>
            /// Returns the address of the device.
            /// </summary>
            public BluetoothAddress Address
            {
                get { return new BluetoothAddress(AddressCode); }
            }

            /// <summary>
            /// Returns the friendly nameof the device.
            /// </summary>
            public string Name
            {
                get
                {
                    string value = Encoding.UTF8.GetString(NameBinary, 0, NameBinary.Length).Trim('\0');
                    if (String.IsNullOrEmpty(value))
                        return Address.ToString();
                    return value;
                }
            }

            /// <summary>
            /// Converts the object into a BluetoothDevice object.
            /// </summary>
            /// <returns></returns>
            public BluetoothDevice ToDevice()
            {
                return new BluetoothDevice(Address, Name);
            }
        }

        /// <summary>
        /// Bluetooth device status states that can be combined.
        /// </summary>
        [Flags]
        public enum BluetoothDeviceFlags
        {
            /// <summary>
            /// The bluetooth address is present.
            /// </summary>
            AddressIsPresent = 1,

            /// <summary>
            /// The class of the device is present.
            /// </summary>
            ClassOfDeviceIsPresent = 2,

            /// <summary>
            /// The name of the device is present.
            /// </summary>
            NameIsPresent = 4,

            /// <summary>
            /// Whether or not the device is paired.
            /// </summary>
            Paired = 8,

            /// <summary>
            /// The bluetooth device is remembered.
            /// </summary>
            Remembered = 16,

            /// <summary>
            /// The bluetooth device is connected.
            /// </summary>
            Connected = 32
        }

        // http://msdn.microsoft.com/en-us/library/aa362926(VS.85).aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class BluetoothFindRadioParams
        {
            /// <summary>
            /// Size of the BLUETOOTH_FIND_RADIO_PARAMS structure, in bytes. dwSize.
            /// </summary>
            public uint ByteSize = 4;
        }

        // http://msdn.microsoft.com/en-us/library/aa362924(VS.85).aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BluetoothDeviceInfo
        {
            /// <summary>
            /// Size of the BluetoothDeviceInfo structure, in bytes. dwSize.
            /// </summary>
            public int ByteSize;

            /// <summary>
            /// Address of the device.
            /// </summary>
            public long Address;

            /// <summary>
            /// Class of the device. ulClassofDevice.
            /// </summary>
            public uint DeviceClass;

            /// <summary>
            /// Specifies whether the device is connected. fConnected.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool Connected;

            /// <summary>
            /// Specifies whether the device is a remembered device. Not all remembered devices are authenticated. fRemembered.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool Remembered;

            /// <summary>
            /// Specifies whether the device is authenticated, paired, or bonded. All authenticated devices are remembered. fAuthenticated.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool Authenticated;

            /// <summary>
            /// Last time the device was seen, in the form of a SYSTEMTIME structure.  stLastSeen.
            /// </summary>
            public SystemTime LastSeen;

            /// <summary>
            /// Last time the device was used, in the form of a SYSTEMTIME structure. stLastUsed.
            /// </summary>
            public SystemTime LastUsed;

            /// <summary>
            /// Name of the device. szName.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
            public string Name;

            /// <summary>
            /// Initializes a new instance of the <see cref="BluetoothDeviceInfo"/> struct.
            /// </summary>
            /// <param name="address">The address.</param>
            public BluetoothDeviceInfo(long address)
            {
                ByteSize = 560;
                Address = address;
                DeviceClass = 0;
                Connected = false;
                Remembered = false;
                Authenticated = false;
                LastSeen = new SystemTime();
                LastUsed = new SystemTime();
                Name = string.Empty;
            }
        }

        // http://msdn.microsoft.com/en-us/library/aa362929(VS.85).aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BluetoothRadioInfo
        {
            // This class is marshalled, so even if members are not used, they need
            // to be left in the listed order and types (or have MarshalAs to specify marshaling type).

            /// <summary>
            /// Size, in bytes, of the structure. dwSize.
            /// </summary>
            public int ByteSize;

            /// <summary>
            /// Address of the local Bluetooth radio.
            /// </summary>
            public long Address;

            /// <summary>
            /// Name of the local Bluetooth radio.  szName.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
            public string Name;

            /// <summary>
            /// Device class for the local Bluetooth radio.  ClassofDevice.
            /// </summary>
            public uint DeviceClass;

            /// <summary>
            /// This member contains data specific to individual Bluetooth device manufacturers.  lmpSubversion.
            /// </summary>
            public ushort Subversion;

            /// <summary>
            /// Manufacturer of the Bluetooth radio, expressed as a BTH_MFG_Xxx value. For more
            /// information about the Bluetooth assigned numbers document and a current list of values,
            /// see the Bluetooth specification at www.bluetooth.com.
            /// </summary>
            public short Manufacturer;
        }

        // http://msdn.microsoft.com/en-us/library/aa362937%28VS.85%29.aspx
        /// <summary>
        /// The BTH_QUERY_DEVICE structure is used when querying for the presence of a Bluetooth device.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class BthQueryDevice
        {
            /// <summary>
            /// Reserved. Must be set to zero.
            /// </summary>
            public uint LAP;

            /// <summary>
            /// Requested length of the inquiry, in seconds.
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte Length;

            /// <summary>
            /// Initializes a new instance of the BthQueryDevice structure.
            /// </summary>
            public BthQueryDevice()
            {
                // Set the default value
                LAP = 0x9E8B33;
            }
        }

        // http://msdn.microsoft.com/en-us/library/ms724950%28VS.85%29.aspx
        /// <summary>
        /// Specifies a date and time, using individual members for the month, day, year, weekday, hour, minute, second,
        /// and millisecond. The time is either in coordinated universal time (UTC) or local time, depending on the
        /// function that is being called.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            /// <summary>
            /// The year. The valid values for this member are 1601 through 30827. wYear.
            /// </summary>
            public short Year;

            /// <summary>
            /// The month. (January = 1).
            /// </summary>
            public short Month;

            /// <summary>
            /// The day of the week. (Sunday = 0)
            /// </summary>
            public short DayOfWeek;

            /// <summary>
            /// The day of the month. The valid values for this member are 1 through 31.
            /// </summary>
            public short Day;

            /// <summary>
            /// The hour. The valid values for this member are 0 through 23.
            /// </summary>
            public short Hour;

            /// <summary>
            /// The minute. The valid values for this member are 0 through 59.
            /// </summary>
            public short Minute;

            /// <summary>
            /// The second. The valid values for this member are 0 through 59.
            /// </summary>
            public short Second;

            /// <summary>
            /// The millisecond. The valid values for this member are 0 through 999.
            /// </summary>
            public short Milliseconds;

            /// <summary>
            /// Creates a .Net DateTime structure from the SystemTime structure.
            /// </summary>
            /// <returns></returns>
            public DateTime ToDateTime()
            {
                return new DateTime(Year, Month, Day, Hour, Minute, Second, Milliseconds);
            }

            /// <summary>
            /// Creates a SystemTime structure based on the specified DateTime structure.
            /// </summary>
            /// <param name="value">The DateTime value to set.</param>
            /// <returns>The SystemTime equivalent to the specified value.</returns>
            public static SystemTime FromDateTime(DateTime value)
            {
                SystemTime result = new SystemTime
                                        {
                                            Year = (short)value.Year,
                                            Month = (short)value.Month,
                                            DayOfWeek = (short)value.DayOfWeek,
                                            Day = (short)value.Day,
                                            Hour = (short)value.Hour,
                                            Minute = (short)value.Minute,
                                            Second = (short)value.Second,
                                            Milliseconds = (short)value.Millisecond
                                        };
                return result;
            }
        }

        // http://msdn.microsoft.com/en-us/library/ms741563%28VS.85%29.aspx
        /// <summary>
        /// The WSADATA structure contains information about the Windows Sockets implementation.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class WsaData
        {
            /// <summary>
            /// The version of the Windows Sockets specification that the Ws2_32.dll expects the caller to use.
            /// The high-order byte specifies the minor version number; the low-order byte specifies the major version number.
            /// wVersion.
            /// </summary>
            public Int16 Version = 36;        // 2.2

            /// <summary>
            /// The highest version of the Windows Sockets specification that the Ws2_32.dll can support. The high-order byte
            /// specifies the minor version number; the low-order byte specifies the major version number.
            /// This is the same value as the wVersion member when the version requested in the wVersionRequested parameter
            /// passed to the WSAStartup function is the highest version of the Windows Sockets specification that the Ws2_32.dll
            /// can support.  wHighVersion.
            /// </summary>
            public Int16 HighVersion = 36;    // 2.2

            /// <summary>
            /// A NULL-terminated ASCII string into which the Ws2_32.dll copies a description of the Windows Sockets implementation.
            /// The text (up to 256 characters in length) can contain any characters except control and formatting characters.
            /// The most likely use that an application would have for this member is to display it (possibly truncated) in a status
            /// message.  szDescription.
            /// </summary>
            public IntPtr Description;

            /// <summary>
            /// A NULL-terminated ASCII string into which the Ws2_32.dll copies relevant status or configuration information.
            /// The Ws2_32.dll should use this parameter only if the information might be useful to the user or support staff.
            /// This member should not be considered as an extension of the szDescription parameter.  szSystemStatus.
            /// </summary>
            public IntPtr SystemStatus;

            /// <summary>
            /// The maximum number of sockets that may be opened. This member should be ignored for Windows Sockets version 2 and later.
            /// The iMaxSockets member is retained for compatibility with Windows Sockets specification 1.1, but should not be used when
            /// developing new applications. No single value can be appropriate for all underlying service providers. The architecture
            /// of Windows Sockets changed in version 2 to support multiple providers, and the WSADATA structure no longer applies to
            /// a single vendor's stack. iMaxSockets.
            /// </summary>
            public Int16 MaxSockets;

            /// <summary>
            /// The maximum datagram message size. This member is ignored for Windows Sockets version 2 and later.  iMaxUdpDg.
            /// </summary>
            public Int16 MaximumMessageSize;

            /// <summary>
            /// A pointer to vendor-specific information. This member should be ignored for Windows Sockets version 2 and later.
            /// lpVendorInfo
            /// </summary>
            public IntPtr VendorInfo;
        }

        /// <summary>
        /// WAS Lookup Control Options
        /// </summary>
        [Flags]
        internal enum WasLookupControlOptions : uint
        {
            /// <summary>
            /// None of the return options are active.
            /// </summary>
            None = 0,

            /// <summary>
            /// Returns containers only. LUP_CONTAINERS.
            /// </summary>
            Containers = 0x0002,

            /// <summary>
            /// Retrieves the name as lpszServiceInstanceName.  LUP_RETURN_NAME.
            /// </summary>
            ReturnName = 0x0010,

            /// <summary>
            /// Retrieves the type as lpServiceClassId.  LUP_RETURN_TYPE.
            /// </summary>
            ReturnType = 0x0020,

            /// <summary>
            /// Retrieves the version as lpVersion.  LUP_RETURN_VERSION.
            /// </summary>
            ReturnVersion = 0x0040,

            /// <summary>
            /// Retrieves the comment as lpszComment.  LUP_RETURN_COMMENT.
            /// </summary>
            ReturnComment = 0x0080,

            /// <summary>
            /// Retrieves the addresses as lpcsaBuffer.  LUP_RETURN_ADDR.
            /// </summary>
            ReturnAddresses = 0x0100,

            /// <summary>
            /// Retrieves the private data as lpBlob.  LUP_RETURN_BLOB.
            /// </summary>
            ReturnBlob = 0x0200,

            /// <summary>
            /// If the provider has been caching information, ignores the cache, and queries the namespace itself.  LUP_FLUSHCACHE.
            /// </summary>
            FlushCache = 0x1000,

            /// <summary>
            /// This indicates whether prime response is in the remote or local part of CSADDR_INFO structure.
            /// The other part needs to be usable in either case.  LUP_RES_SERVICE.
            /// </summary>
            ResService = 0x8000,
        }

        // http://msdn.microsoft.com/en-us/library/ms898762.aspx
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class WsaQuerySet
        {
            /// <summary>
            /// Must be set to sizeof(WSAQUERYSET) in bytes. This is a versioning mechanism.  dwSize.
            /// </summary>
            public uint ByteSize;

            /// <summary>
            /// (Optional) Referenced string contains service name. The semantics for using wildcards
            /// within the string are not defined, but can be supported by certain name space providers.
            /// lpszServiceInstanceName.
            /// </summary>
            public IntPtr ServiceInstanceName;

            /// <summary>
            /// (Required) The GUID corresponding to the service class.
            /// lpServiceClassId.
            /// </summary>
            public IntPtr ServiceClassId;

            /// <summary>
            /// (Optional) References desired version number and provides version comparison semantics
            /// (that is, version must match exactly, or version must be not less than the value supplied).
            /// lpVersion.
            /// </summary>
            public IntPtr Version;

            /// <summary>
            /// Ignored for queries.  lpszComment.
            /// </summary>
            public IntPtr Comment;

            /// <summary>
            /// Identifier of a single name space in which to constrain the search, or NS_ALL to include
            /// all name spaces.  dwNameSpace.
            /// </summary>
            public uint NameSpace = NS_BTH;

            /// <summary>
            /// (Optional) References the GUID of a specific name-space provider, and limits the query to
            /// this provider only.  lpNSProviderId.
            /// </summary>
            public IntPtr ProviderId;

            /// <summary>
            /// (Optional) Specifies the starting point of the query in a hierarchical name space.
            /// lpszContext.
            /// </summary>
            public IntPtr Context;

            /// <summary>
            /// Size of the protocol constraint array, can be zero.  dwNumberOfProtocols.
            /// </summary>
            public uint NumberOfProtocols;

            /// <summary>
            /// (Optional) References an array of AFPROTOCOLS structure. Only services that utilize
            /// these protocols will be returned.  lpafpProtocols.
            /// </summary>
            public IntPtr Protocols;

            /// <summary>
            /// (Optional)Some name spaces (such as Whois++) support enriched SQL-like queries that are
            /// contained in a simple text string. This parameter is used to specify that string.
            /// lpszQueryString.
            /// </summary>
            public IntPtr QueryString;

            /// <summary>
            /// Ignored for queries.  dwNumberOfCsAddrs
            /// </summary>
            public uint NumberOfCsAddrs;

            /// <summary>
            /// Ignored for queries.  lpcsaBuffer.
            /// </summary>
            public IntPtr Buffer;

            /// <summary>
            /// Ignored for queries. dwOutputFlags.
            /// </summary>
            public uint OutputFlags;

            /// <summary>
            /// (Optional)This is a pointer to a provider-specific entity.  lbBlob.
            /// </summary>
            public IntPtr EntityBlob;

            /// <summary>
            /// Instantiates a new instance of the Wsaqueryset Class.
            /// </summary>
            public WsaQuerySet()
            {
                // The size will vary on 32-bit vs. 64-bit systems!
                ByteSize = (uint)Marshal.SizeOf(this);
            }
        }

        #endregion

        #endregion

        #region USB

        /// <summary>
        ///
        /// </summary>
        public const int DIGCF_DEFAULT = 0x00000001;  // only valid with DIGCF_DEVICEINTERFACE
        /// <summary>
        ///
        /// </summary>
        public const int DIGCF_PRESENT = 0x00000002;
        /// <summary>
        ///
        /// </summary>
        public const int DIGCF_ALLCLASSES = 0x00000004;
        /// <summary>
        ///
        /// </summary>
        public const int DIGCF_PROFILE = 0x00000008;
        /// <summary>
        ///
        /// </summary>
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

        // http://msdn.microsoft.com/en-us/library/ff552344%28VS.85%29.aspx
        /// <summary>
        /// An SP_DEVINFO_DATA structure defines a device instance that is a member of a device information set.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SpDevinfoData
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVINFO_DATA structure. For more information, see the
            /// following Remarks section.  cbSize.
            /// </summary>
            public uint Size;

            /// <summary>
            /// The GUID of the device's setup class.
            /// </summary>
            public Guid ClassGuid;

            /// <summary>
            /// An opaque handle to the device instance (also known as a handle to the devnode).
            /// Some functions, such as SetupDiXxx functions, take the whole SP_DEVINFO_DATA structure
            /// as input to identify a device in a device information set. Other functions, such as
            /// CM_Xxx functions like CM_Get_DevNode_Status, take this DevInst handle as input.
            /// DevInst.
            /// </summary>
            public uint DeviceInstance;

            /// <summary>
            /// Reserved. For internal use only.
            /// </summary>
            public IntPtr Reserved;
        }

        //http://msdn.microsoft.com/en-us/library/ff552342%28VS.85%29.aspx
        /// <summary>
        /// An SP_DEVICE_INTERFACE_DATA structure defines a device interface in a device information set.
        /// </summary>
        /// <remarks>A SetupAPI function that takes an instance of the SP_DEVICE_INTERFACE_DATA structure as a parameter
        /// verifies whether the cbSize member of the supplied structure is equal to the size, in bytes, of the
        /// structure. If the cbSize member is not set correctly, the function will fail and set an error code
        /// of ERROR_INVALID_USER_BUFFER.</remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct SpDeviceInterfaceData
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVICE_INTERFACE_DATA structure. For more information,
            /// see the Remarks section. cbSize.
            /// </summary>
            public uint Size;

            /// <summary>
            /// The GUID for the class to which the device interface belongs.
            /// </summary>
            public Guid InterfaceClassGuid;

            /// <summary>
            /// Can be one or more of the following:
            /// SPINT_ACTIVE - The interface is active (enabled).
            /// SPINT_DEFAULT - The interface is the default interface for the device class.
            /// SPINT_REMOVED - The interface is removed.
            /// </summary>
            public uint Flags;

            /// <summary>
            /// Reserved. Do not use.
            /// </summary>
            public IntPtr Reserved;
        }

        /// <summary>
        /// An SP_DEVICE_INTERFACE_DETAIL_DATA structure contains the path for a device interface.
        /// </summary>
        /// <remarks>An SP_DEVICE_INTERFACE_DETAIL_DATA structure identifies the path for a device interface in a
        /// device information set.  SetupDiXxx functions that take an SP_DEVICE_INTERFACE_DETAIL_DATA
        /// structure as a parameter verify that the cbSize member of the supplied structure is equal
        /// to the size, in bytes, of the structure. If the cbSize member is not set correctly for an
        /// input parameter, the function will fail and set an error code of ERROR_INVALID_PARAMETER.
        /// If the cbSize member is not set correctly for an output parameter, the function will fail
        /// and set an error code of ERROR_INVALID_USER_BUFFER.</remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SpDeviceInterfaceDetailData
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVICE_INTERFACE_DETAIL_DATA structure.
            /// For more information, see the following Remarks section.  cbSize.
            /// </summary>
            public UInt32 ByteSize;

            /// <summary>
            /// A NULL-terminated string that contains the device interface path. This path can be passed to Win32 functions such as CreateFile.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        // http://www.osronline.com/ddkx/install/di-rtns_3z02.htm
        /// <summary>
        /// The SetupDiClassGuidsFromName function retrieves the GUID(s) associated with the specified class name.
        /// This list is built based on the classes currently installed on the system.
        /// </summary>
        /// <param name="className">Supplies the name of the class for which to retrieve the class GUID.</param>
        /// <param name="classGuidList">Supplies a pointer to an array to receive the list of GUIDs associated with the specified class name.</param>
        /// <param name="classGuidListSize">Supplies the number of GUIDs in the ClassGuidList array.</param>
        /// <param name="requiredSize">Supplies a pointer to a variable that receives the number of GUIDs associated with the class name.
        /// If this number is greater than the size of the ClassGuidList buffer, the number indicates how large
        /// the array must be in order to store all the GUIDs.</param>
        /// <returns>The function returns TRUE if it is successful. Otherwise, it returns FALSE and the logged
        /// error can be retrieved with a call to GetLastError.</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiClassGuidsFromName(
            StringBuilder className,
            //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            IntPtr classGuidList,
            int classGuidListSize,
            out int requiredSize);

        // http://msdn.microsoft.com/en-us/library/ff538610%28VS.85%29.aspx
        /// <summary>
        /// The CM_Get_Parent function obtains a device instance handle to the parent node of a
        /// specified device node (devnode) in the local machine's device tree.
        /// </summary>
        /// <param name="pdnDevInst">Caller-supplied pointer to the device instance handle to the
        /// parent node that this function retrieves. The retrieved handle is bound to the local machine.</param>
        /// <param name="dnDevInst">Caller-supplied device instance handle that is bound to the local machine.</param>
        /// <param name="ulFlags">Not used, must be zero.</param>
        /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the
        /// CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
        /// <remarks>For information about using a device instance handle that is bound to the local machine, see CM_Get_Child.</remarks>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int CM_Get_Parent(
           out UInt32 pdnDevInst,
           UInt32 dnDevInst,
           int ulFlags
        );

        // http://msdn.microsoft.com/en-us/library/ff538405%28VS.85%29.aspx
        /// <summary>
        /// The CM_Get_Device_ID function retrieves the device instance ID for a specified device instance on the local machine.
        /// </summary>
        /// <param name="dnDevInst">Caller-supplied device instance handle that is bound to the local machine.</param>
        /// <param name="buffer">Address of a buffer to receive a device instance ID string. The required buffer size
        /// can be obtained by calling CM_Get_Device_ID_Size, then incrementing the received value to allow room for
        /// the string's terminating NULL.</param>
        /// <param name="bufferLen">Caller-supplied length, in characters, of the buffer specified by Buffer.</param>
        /// <param name="ulFlags">Not used, must be zero.</param>
        /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
        /// <remarks>The function appends a NULL terminator to the supplied device instance ID string, unless the buffer is too
        /// small to hold the string. In this case, the function supplies as much of the identifier string as will fit
        /// into the buffer, and then returns CR_BUFFER_SMALL.
        /// For information about device instance IDs, see Device Identification Strings.
        /// For information about using device instance handles that are bound to the local machine, see CM_Get_Child.</remarks>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CM_Get_Device_ID(
           UInt32 dnDevInst,
           IntPtr buffer,
           int bufferLen,
           int ulFlags
        );

        //http://msdn.microsoft.com/en-us/library/ff550996%28VS.85%29.aspx
        /// <summary>
        /// The SetupDiDestroyDeviceInfoList function deletes a device information set and frees all associated memory.
        /// </summary>
        /// <param name="hDevInfo">A handle to the device information set to delete.</param>
        /// <returns>The function returns TRUE if it is successful. Otherwise, it returns FALSE and the
        /// logged error can be retrieved with a call to GetLastError.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        // http://msdn.microsoft.com/en-us/library/ff551069%28VS.85%29.aspx
        /// <summary>
        /// The SetupDiGetClassDevs function returns a handle to a device information set that contains requested
        /// device information elements for a local computer.
        /// </summary>
        /// <param name="classGuid">A pointer to the GUID for a device setup class or a device interface class.
        /// This pointer is optional and can be NULL. For more information about how to set ClassGuid, see the
        /// following Remarks section.</param>
        /// <param name="enumerator">A pointer to a NULL-terminated string that specifies:
        /// * An identifier (ID) of a Plug and Play (PnP) enumerator. This ID can either be the value's globally
        /// unique identifier (GUID) or symbolic name. For example, "PCI" can be used to specify the PCI PnP value.
        /// Other examples of symbolic names for PnP values include "USB," "PCMCIA," and "SCSI".
        /// * A PnP device instance ID. When specifying a PnP device instance ID, DIGCF_DEVICEINTERFACE must be set
        /// in the Flags parameter.
        /// This pointer is optional and can be NULL. If an enumeration value is not used to select devices,
        /// set Enumerator to NULL
        /// For more information about how to set the Enumerator value, see the following Remarks section.</param>
        /// <param name="hwndParent">A handle to the top-level window to be used for a user interface that is
        /// associated with installing a device instance in the device information set. This handle is optional
        /// and can be NULL.</param>
        /// <param name="flags">A variable of type DWORD that specifies control options that filter the device information
        /// elements that are added to the device information set. This parameter can be a bitwise OR of zero
        /// or more of the following flags. For more information about combining these flags, see the following
        /// Remarks section.
        /// DIGCF_ALLCLASSES
        /// Return a list of installed devices for all device setup classes or all device interface classes.
        /// DIGCF_DEVICEINTERFACE
        /// Return devices that support device interfaces for the specified device interface classes.
        /// This flag must be set in the Flags parameter if the Enumerator parameter specifies a device instance ID.
        /// DIGCF_DEFAULT
        /// Return only the device that is associated with the system default device interface, if one is set,
        /// for the specified device interface classes.
        /// DIGCF_PRESENT
        /// Return only devices that are currently present in a system.
        /// DIGCF_PROFILE
        /// Return only devices that are a part of the current hardware profile.</param>
        /// <returns>If the operation succeeds, SetupDiGetClassDevs returns a handle to a device information set that contains
        /// all installed devices that matched the supplied parameters. If the operation fails, the function returns
        /// INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
           ref Guid classGuid,
           IntPtr enumerator,
           IntPtr hwndParent,
           int flags
        );

        // http://msdn.microsoft.com/en-us/library/ff551010%28VS.85%29.aspx
        /// <summary>
        /// The SetupDiEnumDeviceInfo function returns a SP_DEVINFO_DATA structure that specifies a device
        /// information element in a device information set.
        /// </summary>
        /// <param name="hDevInfo">A handle to the device information set for which to return an SP_DEVINFO_DATA
        /// structure that represents a device information element.</param>
        /// <param name="memberIndex">A zero-based index of the device information element to retrieve.</param>
        /// <param name="devInfo">A pointer to an SP_DEVINFO_DATA structure to receive information about an enumerated device information
        /// element. The caller must set DeviceInfoData.cbSize to sizeof(SP_DEVINFO_DATA).</param>
        /// <returns>The function returns TRUE if it is successful. Otherwise, it returns FALSE and the logged error
        /// can be retrieved with a call to GetLastError.</returns>
        /// <remarks>Repeated calls to this function return a device information element for a different device. This function
        /// can be called repeatedly to get information about all devices in the device information set.
        /// To enumerate device information elements, an installer should initially call SetupDiEnumDeviceInfo with
        /// the MemberIndex parameter set to 0. The installer should then increment MemberIndex and call
        /// SetupDiEnumDeviceInfo until there are no more values (the function fails and a call to GetLastError
        /// returns ERROR_NO_MORE_ITEMS).
        /// Call SetupDiEnumDeviceInterfaces to get a context structure for a device interface element (versus a device information element).</remarks>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(
            IntPtr hDevInfo,
            UInt32 memberIndex,
            ref SpDevinfoData devInfo);

        /// <summary>
        /// The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that are contained in a device information set.
        /// </summary>
        /// <param name="hDevInfo">A pointer to a device information set that contains the device interfaces for which to return information.
        /// This handle is typically returned by SetupDiGetClassDevs.</param>
        /// <param name="devInfo">A pointer to an SP_DEVINFO_DATA structure that specifies a device information element in DeviceInfoSet.
        /// This parameter is optional and can be NULL. If this parameter is specified, SetupDiEnumDeviceInterfaces
        /// constrains the enumeration to the interfaces that are supported by the specified device. If this parameter
        /// is NULL, repeated calls to SetupDiEnumDeviceInterfaces return information about the interfaces that are
        /// associated with all the device information elements in DeviceInfoSet. This pointer is typically returned
        /// by SetupDiEnumDeviceInfo.</param>
        /// <param name="interfaceClassGuid">A pointer to a GUID that specifies the device interface class for the requested interface.</param>
        /// <param name="memberIndex">A zero-based index into the list of interfaces in the device information set. The caller should call
        /// this function first with MemberIndex set to zero to obtain the first interface. Then, repeatedly increment
        /// MemberIndex and retrieve an interface until this function fails and GetLastError returns ERROR_NO_MORE_ITEMS.
        /// If DeviceInfoData specifies a particular device, the MemberIndex is relative to only the interfaces exposed
        /// by that device.</param>
        /// <param name="deviceInterfaceData">A pointer to a caller-allocated buffer that contains, on successful return, a completed SP_DEVICE_INTERFACE_DATA
        /// structure that identifies an interface that meets the search parameters. The caller must set DeviceInterfaceData.cbSize
        /// to sizeof(SP_DEVICE_INTERFACE_DATA) before calling this function.</param>
        /// <returns>SetupDiEnumDeviceInterfaces returns TRUE if the function completed without error. If the function completed
        /// with an error, FALSE is returned and the error code for the failure can be retrieved by calling GetLastError.</returns>
        /// <remarks>Repeated calls to this function return an SP_DEVICE_INTERFACE_DATA structure for a different device interface.
        /// This function can be called repeatedly to get information about interfaces in a device information set that
        /// are associated with a particular device information element or that are associated with all device information
        /// elements.
        /// DeviceInterfaceData points to a structure that identifies a requested device interface. To get detailed
        /// information about an interface, call SetupDiGetDeviceInterfaceDetail. The detailed information includes
        /// the name of the device interface that can be passed to a Win32 function such as CreateFile
        /// (described in Microsoft Windows SDK documentation) to get a handle to the interface.</remarks>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           IntPtr devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SpDeviceInterfaceData deviceInterfaceData
        );

        //http://msdn.microsoft.com/en-us/library/ff551120%28VS.85%29.aspx
        /// <summary>
        /// The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.
        /// </summary>
        /// <param name="hDevInfo">A pointer to the device information set that contains the interface
        /// for which to retrieve details. This handle is typically returned by SetupDiGetClassDevs.</param>
        /// <param name="deviceInterfaceData">A pointer to an SP_DEVICE_INTERFACE_DATA structure that specifies
        /// the interface in DeviceInfoSet for which to retrieve details. A pointer of this type is typically
        /// returned by SetupDiEnumDeviceInterfaces.</param>
        /// <param name="deviceInterfaceDetailData">A pointer to an SP_DEVICE_INTERFACE_DETAIL_DATA structure to receive information about the specified
        /// interface. This parameter is optional and can be NULL. This parameter must be NULL if
        /// DeviceInterfaceDetailSize is zero. If this parameter is specified, the caller must set
        /// DeviceInterfaceDetailData.cbSize to sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA) before calling this function.
        /// The cbSize member always contains the size of the fixed part of the data structure, not a size reflecting
        /// the variable-length string at the end.</param>
        /// <param name="deviceInterfaceDetailDataSize">The size of the DeviceInterfaceDetailData buffer. The buffer must be at least
        /// (offsetof(SP_DEVICE_INTERFACE_DETAIL_DATA, DevicePath) + sizeof(TCHAR)) bytes, to contain the fixed part
        /// of the structure and a single NULL to terminate an empty MULTI_SZ string.
        /// This parameter must be zero if DeviceInterfaceDetailData is NULL.</param>
        /// <param name="requiredSize">A pointer to a variable of type DWORD that receives the required size of the DeviceInterfaceDetailData buffer.
        /// This size includes the size of the fixed part of the structure plus the number of bytes required for the
        /// variable-length device path string. This parameter is optional and can be NULL.</param>
        /// <param name="deviceInfoData">A pointer to a buffer that receives information about the device that supports the requested interface.
        /// The caller must set DeviceInfoData.cbSize to sizeof(SP_DEVINFO_DATA). This parameter is optional and can
        /// be NULL.</param>
        /// <returns>SetupDiGetDeviceInterfaceDetail returns TRUE if the function completed without error. If the function
        /// completed with an error, FALSE is returned and the error code for the failure can be retrieved by calling
        /// GetLastError.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref SpDeviceInterfaceData deviceInterfaceData,
           ref SpDeviceInterfaceDetailData deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           ref SpDevinfoData deviceInfoData
        );

        #endregion

        #region Desktop Members

        #region Bluetooth

        // http://msdn.microsoft.com/en-us/library/aa362795.aspx
        /// <summary>
        /// The BluetoothGetDeviceInfo function retrieves information about a remote Bluetooth device.
        /// The Bluetooth device must have been previously identified through a successful device inquiry function call.
        /// </summary>
        /// <param name="hRadio">A handle to a local radio, obtained from a call to the BluetoothFindFirstRadio or similar functions,
        /// or from a call to the SetupDiEnumerateDeviceInterfaces function.</param>
        /// <param name="pbtdi">A pointer to a BLUETOOTH_DEVICE_INFO structure into which data about the first Bluetooth device will be placed.
        /// For more information, see Remarks.</param>
        /// <returns>Returns ERROR_SUCCESS upon success, indicating that data about the remote Bluetooth device was retrieved.
        /// Returns error codes upon failure. The following table lists common error codes associated with the
        /// BluetoothGetDeviceInfo function.</returns>
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetDeviceInfo(IntPtr hRadio, ref BluetoothDeviceInfo pbtdi);

        /// <summary>
        /// The BluetoothGetRadioInfo function obtains information about a Bluetooth radio.
        /// </summary>
        /// <param name="hRadio">A handle to a local Bluetooth radio, obtained by calling the
        /// BluetoothFindFirstRadio or similar functions, or the SetupDiEnumerateDeviceInterfances function.</param>
        /// <param name="pbtdi">A pointer to a BLUETOOTH_RADIO_INFO structure into which information about the radio
        /// will be placed. The dwSize member of the BLUETOOTH_RADIO_INFO structure must match the size of the structure.</param>
        /// <returns>The following table lists common return values.
        /// ERROR_SUCCESS - The radio information was retrieved successfully.
        /// ERROR_INVALID_PARAMETER - The hRadio or pRadioInfo parameter is NULL.
        /// ERROR_REVISION_MISMATCH - The dwSize member of the BLUETOOTH_RADIO_INFO structure pointed to by pRadioInfo is not valid.</returns>
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetRadioInfo(IntPtr hRadio, ref BluetoothRadioInfo pbtdi);

        // http://msdn.microsoft.com/en-us/library/aa362786(VS.85).aspx
        /// <summary>
        /// The BluetoothFindFirstRadio function begins the enumeration of local Bluetooth radios.
        /// </summary>
        /// <param name="pbtfrp">Pointer to a BLUETOOTH_FIND_RADIO_PARAMS structure. The dwSize member of the BLUETOOTH_FIND_RADIO_PARAMS
        /// structure pointed to by pbtfrp must match the size of the structure. See Return Values for additional
        /// information about this parameter.</param>
        /// <param name="phRadio">Pointer to where the first enumerated radio handle will be returned. When no longer needed, this handle
        /// must be closed via CloseHandle.</param>
        /// <returns>In addition to the handle indicated by phRadio, calling this function will also create a
        /// HBLUETOOTH_RADIO_FIND handle for use with the BluetoothFindNextRadio function. When this handle
        /// is no longer needed, it must be closed via the BluetoothFindRadioClose.</returns>
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern SafeBluetoothRadioFindHandle BluetoothFindFirstRadio(BluetoothFindRadioParams pbtfrp, ref IntPtr phRadio);

        // http://msdn.microsoft.com/en-us/library/aa362792(VS.85).aspx
        /// <summary>
        /// The BluetoothFindRadioClose function closes the enumeration handle associated with finding Bluetooth radios.
        /// </summary>
        /// <param name="hFind">Enumeration handle to close, obtained with a previous call to the BluetoothFindFirstRadio function.</param>
        /// <returns>Returns TRUE when the handle is successfully closed. Returns FALSE if the attempt fails to close the
        /// enumeration handle. For additional information on possible errors associated with closing the handle,
        /// call the GetLastError function.</returns>
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindRadioClose(IntPtr hFind);

        //http://msdn.microsoft.com/en-us/library/aa362799%28VS.85%29.aspx
        /// <summary>
        /// The BluetoothIsConnectable function determines whether a Bluetooth radio or radios is connectable.
        /// </summary>
        /// <param name="hRadio">Valid local radio handle, or NULL. If NULL, all local radios are checked for
        /// connectability; if any radio is connectable, the function call succeeds.</param>
        /// <returns>Returns TRUE if at least one Bluetooth radio is accepting incoming connections.
        /// Returns FALSE if no radios are accepting incoming connections.</returns>
        /// <remarks>If multiple Bluetooth radios exist, the first radio to return that it is connectable causes the
        /// BluetoothIsConnectable function to succeed.</remarks>
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsConnectable(IntPtr hRadio);

        // http://msdn.microsoft.com/en-us/library/aa362882%28VS.85%29.aspx
        /// <summary>
        /// The BluetoothIsDiscoverable function determines whether a Bluetooth radio or radios is discoverable.
        /// </summary>
        /// <param name="hRadio">Valid local radio handle, or NULL. If NULL, discovery is determined for all local radios;
        /// if any radio is discoverable, the function call succeeds.</param>
        /// <returns>Returns TRUE if at least one Bluetooth radio is discoverable. Returns FALSE if no Bluetooth radios are discoverable.</returns>
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsDiscoverable(IntPtr hRadio);

        //http://msdn.microsoft.com/en-us/library/aa362776%28VS.85%29.aspx
        /// <summary>
        /// The BluetoothEnableDiscovery function changes the discovery state of a local Bluetooth radio or radios.
        /// </summary>
        /// <param name="hRadio">Valid local radio handle, or NULL. If NULL, discovery is modified on all local radios;
        /// if any radio is modified by the call, the function call succeeds.</param>
        /// <param name="fEnabled">Flag specifying whether discovery is to be enabled or disabled. Set to TRUE to enable discovery,
        /// set to FALSE to disable discovery.</param>
        /// <returns>Returns TRUE if the discovery state was successfully changed. If hRadio is NULL, a return value of
        /// TRUE indicates that at least one local radio state was successfully changed. Returns FALSE if discovery
        /// state was not changed; if hRadio was NULL, no radio accepted the state change.</returns>
        /// <remarks>Use the BluetoothIsDiscoverable function to determine the current state of a Bluetooth radio. Windows ensures that a discoverable system is connectable, and as such, the radio must allow incoming connections prior to making a radio discoverable. Failure to allow incoming connections results in the BluetoothEnableDiscovery function call failing.
        /// When BluetoothEnableDiscovery changes the discovery state, the new state is valid for the lifetime of the
        /// calling application. Additionally, if a Bluetooth radio previously made discoverable with this function
        /// is disabled and re-enabled via the application, discoverability will not persist. Once the calling application
        /// terminates, the discovery state of the specified Bluetooth radio reverts to the state it was in before
        /// BluetoothEnableDiscovery was called.</remarks>
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothEnableDiscovery(IntPtr hRadio,
            [MarshalAs(UnmanagedType.Bool)]
            bool fEnabled);

        // http://msdn.microsoft.com/en-us/library/aa362778%28VS.85%29.aspx
        /// <summary>
        /// The BluetoothEnableIncomingConnections function modifies whether a local Bluetooth radio accepts incoming connections.
        /// </summary>
        /// <param name="hRadio">Valid local radio handle for which to change whether incoming connections are enabled, or NULL. If NULL,
        /// the attempt to modify incoming connection acceptance iterates through all local radios; if any radio is
        /// modified by the call, the function call succeeds.</param>
        /// <param name="fEnabled">Flag specifying whether incoming connection acceptance is to be enabled or disabled. Set to TRUE to
        /// enable incoming connections, set to FALSE to disable incoming connections.</param>
        /// <returns>Returns TRUE if the incoming connection state was successfully changed. If hRadio is NULL, a return
        /// value of TRUE indicates that at least one local radio state was successfully changed. Returns FALSE
        /// if incoming connection state was not changed; if hRadio was NULL, no radio accepted the state change.</returns>
        /// <remarks>A radio that is non-connectable is non-discoverable. The radio must be made non-discoverable prior
        /// to making a radio non-connectable. Failure to make a radio non-discoverable prior to making it
        /// non-connectable will result in failure of the BluetoothEnableIncomingConnections function call.</remarks>
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio,
            [MarshalAs(UnmanagedType.Bool)]
            bool fEnabled);

        //http://msdn.microsoft.com/en-us/library/ms742213%28VS.85%29.aspx
        /// <summary>
        /// The WSAStartup function initiates use of the Winsock DLL by a process.
        /// </summary>
        /// <param name="wVersionRequested">The highest version of Windows Sockets specification that the caller can use.
        /// The high-order byte specifies the minor version number; the low-order byte specifies the major version number.</param>
        /// <param name="wsaData">A pointer to the WSADATA data structure that is to receive details of the Windows Sockets implementation.</param>
        /// <returns>If successful, the WSAStartup function returns zero. Otherwise, it returns one of the error codes listed below.
        /// WSASYSNOTREADY - The underlying network subsystem is not ready for network communication.
        /// WSAVERNOTSUPPORTED - The version of Windows Sockets support requested is not provided by this particular Windows Sockets implementation.
        /// WSAEINPROGRESS - A blocking Windows Sockets 1.1 operation is in progress.
        /// WSAEPROCLIM - A limit on the number of tasks supported by the Windows Sockets implementation has been reached.
        /// WSAEFAULT - The lpWSAData parameter is not a valid pointer.</returns>
        [DllImport("Ws2_32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSAStartup(
            UInt16 wVersionRequested,
            WsaData wsaData);

        //http://msdn.microsoft.com/en-us/library/ms741549%28VS.85%29.aspx
        /// <summary>
        /// The WSACleanup function terminates use of the Winsock 2 DLL (Ws2_32.dll).
        /// </summary>
        /// <returns>The return value is zero if the operation was successful.
        /// Otherwise, the value SOCKET_ERROR is returned, and a specific error number
        /// can be retrieved by calling WSAGetLastError.</returns>
        [DllImport("Ws2_32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSACleanup();

        // http://msdn.microsoft.com/en-us/library/ms898747.aspx
        /// <summary>
        /// The WSALookupServiceBegin function initiates a client query that is constrained by the
        /// information contained within a WSAQUERYSET structure. WSALookupServiceBegin only returns a
        /// handle, which should be used by subsequent calls to WSALookupServiceNext to get the actual results.
        /// </summary>
        /// <param name="qsRestrictions">Pointer to the search criteria. See the following for details.</param>
        /// <param name="dwControlControlOptions">Flag that controls the depth of the search.</param>
        /// <param name="lphLookup">Handle to be used when calling WSALookupServiceNext in order to start retrieving the results set.</param>
        /// <returns>The return value is zero if the operation was successful. Otherwise, the value SOCKET_ERROR is returned,
        /// and a specific error number can be retrieved by calling WSAGetLastError.</returns>
        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceBegin(
            WsaQuerySet qsRestrictions,
            WasLookupControlOptions dwControlControlOptions,
            ref IntPtr lphLookup);

        //http://msdn.microsoft.com/en-us/library/ms741641%28VS.85%29.aspx
        /// <summary>
        /// The WSALookupServiceNext function is called after obtaining a handle from a previous call to
        /// WSALookupServiceBegin in order to retrieve the requested service information.
        /// The provider will pass back a WSAQUERYSET structure in the lpqsResults buffer. The client should
        /// continue to call this function until it returns WSA_E_NO_MORE, indicating that all of WSAQUERYSET
        /// has been returned.
        /// </summary>
        /// <param name="hLookup">Handle returned from the previous call to WSALookupServiceBegin.</param>
        /// <param name="dwControlControlOptions">Flags to control the next operation. Currently, only LUP_FLUSHPREVIOUS is defined as a means to cope
        /// with a result set that is too large. If an application does not (or cannot) supply a large enough buffer,
        /// setting LUP_FLUSHPREVIOUS instructs the provider to discard the last result set—which was too large—and
        /// move on to the next set for this call.</param>
        /// <param name="lpdwBufferLength">On input, the number of bytes contained in the buffer pointed to by lpqsResults.
        /// On output, if the function fails and the error is WSAEFAULT, then it contains the minimum number
        /// of bytes to pass for the lpqsResults to retrieve the record.</param>
        /// <param name="pqsResults">Pointer to a block of memory, which will contain one result set in a WSAQUERYSET structure on return.</param>
        /// <returns>The return value is zero if the operation was successful. Otherwise, the value SOCKET_ERROR is returned,
        /// and a specific error number can be retrieved by calling WSAGetLastError.</returns>
        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceNext(
            IntPtr hLookup,
            WasLookupControlOptions dwControlControlOptions,
            ref int lpdwBufferLength,
            byte[] pqsResults);

        // http://msdn.microsoft.com/en-us/library/ms741637%28VS.85%29.aspx
        /// <summary>
        /// The WSALookupServiceEnd function is called to free the handle after previous calls to WSALookupServiceBegin and WSALookupServiceNext.
        /// If you call WSALookupServiceEnd from another thread while an existing WSALookupServiceNext is blocked, the end call will have the same effect as a cancel and will cause the WSALookupServiceNext call to return immediately.
        /// </summary>
        /// <param name="hLookup">Handle previously obtained by calling WSALookupServiceBegin.</param>
        /// <returns>The return value is zero if the operation was successful. Otherwise, the value SOCKET_ERROR is returned,
        /// and a specific error number can be retrieved by calling WSAGetLastError.</returns>
        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceEnd(IntPtr hLookup);

        #endregion

        #endregion
    }
}