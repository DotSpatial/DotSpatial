using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security;
#if DEBUG
using System.Diagnostics;
#endif

namespace DotSpatial.Positioning.Gps.IO
{
#if !PocketPC
    [SuppressUnmanagedCodeSecurity]
#endif
    internal static class NativeMethods
    {
        #region Cross-Platform Members

        /*
         * Many API calls are identical on either framework. The only thing different is
         * the dll name in the DllImport atribute, which can be a string constant. The 
         * API calls that are used by both frameworks can be moved here to lessen the 
         * code footprint for the sake of maintainability. Others, like the GPSID and 
         * Serial Comm can be kept seprarated. 
         */
#if !PocketPC
        private const string Kernel32 = "kernel32.dll";
#else
        private const string Kernel32 = "coredll.dll";
#endif

        #region Kernel32

        public static readonly IntPtr INVALID_HANDLE = new IntPtr(-1);

        [DllImport(Kernel32, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        #region IOControl

        /// <summary>
        /// Interacts with a device driver
        /// </summary>
        /// <param name="hDevice"> A handle to a device opened with the CreateFile function. </param>
        /// <param name="dwIoControlCode"> A control code specific to the device driver. </param>
        /// <param name="lpInBuffer"></param>
        /// <param name="nInBufferSize"></param>
        /// <param name="lpOutBuffer"></param>
        /// <param name="nOutBufferSize"></param>
        /// <param name="lpBytesReturned"> The number of bytes returned in the out buffer. </param>
        /// <param name="lpOverlapped"> A pointer to a callback for async commands. </param>
        /// <returns></returns>
        [DllImport(Kernel32, SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        /// <summary>
        /// Opens an IO channel with a physical device.
        /// </summary>
        /// <param name="lpFileName"> The device path </param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="dwShareMode"></param>
        /// <param name="lpSecurityAttributes"></param>
        /// <param name="dwCreationDisposition"></param>
        /// <param name="dwFlagsAndAttributes"></param>
        /// <param name="hTemplateFile"></param>
        /// <returns> A handle to the device. </returns>
        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName,							// file name
            FileAccess dwDesiredAccess,					// access mode
            FileShare dwShareMode,						// share mode
            uint lpSecurityAttributes,					// SD
            FileMode dwCreationDisposition,				// how to create
            FileAttributes dwFlagsAndAttributes,		// file attributes
            IntPtr hTemplateFile						// handle to template file
            );

        /// <summary>
        /// Performs synchronous reads on an IO channel
        /// </summary>
        /// <param name="hDevice"> A handle to a device opened with the CreateFile function. </param>
        /// <param name="lpBuffer"></param>
        /// <param name="nNumberOfBytesToRead"></param>
        /// <param name="lpNumberOfBytesRead"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport(Kernel32, SetLastError = true)]
        public static extern bool ReadFile(
            IntPtr handle, 
            byte[] lpBuffer, 
            uint nNumberOfBytesToRead, 
            out uint lpNumberOfBytesRead, 
            IntPtr lpOverlapped);

        /// <summary>
        /// Performs synchronous writes on an IO channel
        /// </summary>
        /// <param name="hDevice"> A handle to a device opened with the CreateFile function. </param>
        /// <param name="lpBuffer"></param>
        /// <param name="nNumberOfBytesToWrite"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport(Kernel32, SetLastError = true)]
        public static extern bool WriteFile(
            IntPtr handle, 
            byte[] lpBuffer, 
            uint nNumberOfBytesToWrite, 
            out uint lpNumberOfBytesWritten, 
            IntPtr lpOverlapped);

        #endregion

        #region System Clock

        // http://msdn.microsoft.com/en-us/library/ms724942(VS.85).aspx
        [DllImport(Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);

        #endregion

        #endregion

        // These structures are used on both desktop and mobile device platforms with NO changes

        #region Microsoft Bluetooth device discovery

        public const int BTHNS_RESULT_DEVICE_CONNECTED = 0x00010000;
        public const int BTHNS_RESULT_DEVICE_REMEMBERED = 0x00020000;
        public const int BTHNS_RESULT_DEVICE_AUTHENTICATED = 0x00040000;

        // The namespace of Bluetooth, used by the WSAQUERYSET structure
        public const int NS_BTH = 16;
        public const int ERROR_SUCCESS = 1306;

        // http://msdn.microsoft.com/en-us/library/ms737551(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public class BLOB
        {
            public uint cbSize;
            public IntPtr pInfo;
        }

        // Represents information about a single Bluetooth device
        // http://msdn.microsoft.com/en-us/library/aa362934(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public class BTH_DEVICE_INFO
        {
            public uint flags;
            public ulong address;
            public uint classOfDevice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 246)]
            public byte[] name;

            /// <summary>
            /// Returns the address of the device.
            /// </summary>
            public BluetoothAddress Address
            {
                get { return new BluetoothAddress(address); }
            }

            /// <summary>
            /// Returns the friendly nameof the device.
            /// </summary>
            public string Name
            {
                get
                {
                    string value = UTF8Encoding.UTF8.GetString(name, 0, name.Length).Trim('\0');
                    if (String.IsNullOrEmpty(value))
                        return Address.ToString();
                    else
                        return value;
                }
            }

            /* FxCop says this is unused.
             * 

            /// <summary>
            /// Returns the primary purpose of the device.
            /// </summary>
            public DeviceClass MajorDeviceClass
            {
                get
                {
                    return (DeviceClass)(classOfDevice & 0x1F00);
                }
            }

            /// <summary>
            /// Returns a sub-category describing the purpose of the device.
            /// </summary>
            public DeviceClass MinorDeviceClass
            {
                get
                {
                    return (DeviceClass)(classOfDevice & 0xFC);
                }
            }
             */

            /* FxCop says this is unused
             * 
            /// <summary>
            /// Returns the major type of device.
            /// </summary>
            public ServiceClass ServiceClass
            {
                get
                {
                    return (ServiceClass)(classOfDevice & 0xFFE000); ;
                }
            }
             */

            /* FxCop says this is unused.
             * 

            /// <summary>
            /// Returns information describing what information is present in this record.
            /// </summary>
            public BluetoothDeviceFlags Flags
            {
                get
                {
                    return (BluetoothDeviceFlags)flags;
                }
            }
             */

            /* FxCop says this is unused.
             * 

            /// <summary>
            /// Returns whether the device is paired with the local machine's radio.
            /// </summary>
            public bool IsPaired
            {
                get
                {
                    return (Flags & BluetoothDeviceFlags.Paired) != 0;
                }
            }
             */

            /* FxCop says this is unused.
             * 

            /// <summary>
            /// Returns whether the device has previously been found.
            /// </summary>
            public bool IsRemembered
            {
                get
                {
                    return (Flags & BluetoothDeviceFlags.Remembered) != 0;
                }
            }
             */

            /* FxCop says this is unused.
             * 
            /// <summary>
            /// Returns whether the device is currently connected.
            /// </summary>
            public bool IsConnected
            {
                get
                {
                    return (Flags & BluetoothDeviceFlags.Connected) != 0;
                }
            }
             */

            /// <summary>
            /// Converts the object into a BluetoothDevice object.
            /// </summary>
            /// <returns></returns>
            public BluetoothDevice ToDevice()
            {
                return new BluetoothDevice(Address, Name);
            }
        }

        [Flags]
        public enum BluetoothDeviceFlags
        {
            AddressIsPresent = 1,
            ClassOfDeviceIsPresent = 2,
            NameIsPresent = 4,
            Paired = 8,
            Remembered = 16,
            Connected = 32
        }

        // http://msdn.microsoft.com/en-us/library/aa362926(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public class BLUETOOTH_FIND_RADIO_PARAMS
        {
            public uint dwSize = 4;
        }

        // http://msdn.microsoft.com/en-us/library/aa362924(VS.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_DEVICE_INFO
        {
            public int dwSize;
            public long Address;
            public uint ulClassofDevice;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fConnected;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAuthenticated;
            public SYSTEMTIME stLastSeen;
            public SYSTEMTIME stLastUsed;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
            public string szName;

            public BLUETOOTH_DEVICE_INFO(long address)
            {
                dwSize = 560;
                Address = address;
                ulClassofDevice = 0;
                fConnected = false;
                fRemembered = false;
                fAuthenticated = false;
                stLastSeen = new SYSTEMTIME();
                stLastUsed = new SYSTEMTIME();

#if DEBUG
                // The size is much smaller on CE (no times and string not inline) it
                // appears to ignore the bad dwSize value.  So don't check this on CF.
                System.Diagnostics.Debug.Assert(Marshal.SizeOf(typeof(BLUETOOTH_DEVICE_INFO)) == dwSize, "BLUETOOTH_DEVICE_INFO SizeOf == dwSize");
#endif

                szName = "";
            }
        }

        // http://msdn.microsoft.com/en-us/library/aa362929(VS.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_RADIO_INFO
        {
            public int dwSize;
            public long address;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
            public string szName;
            public uint classOfDevice;
            public ushort lmpSubversion;
            public short manufacturer;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class BTH_QUERY_DEVICE
        {
            public uint LAP;
            [MarshalAs(UnmanagedType.U1)]
            public byte length;

            public BTH_QUERY_DEVICE()
            {
                // Set the default value
                LAP = 0x9E8B33;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;

            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
            }

            public static SYSTEMTIME FromDateTime(DateTime value)
            {
                SYSTEMTIME result = new SYSTEMTIME();
                result.wYear = (short)value.Year;
                result.wMonth = (short)value.Month;
                result.wDayOfWeek = (short)value.DayOfWeek;
                result.wDay = (short)value.Day;
                result.wHour = (short)value.Hour;
                result.wMinute = (short)value.Minute;
                result.wSecond = (short)value.Second;
                result.wMilliseconds = (short)value.Millisecond;
                return result;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class WSAData
        {
            public Int16 wVersion = 36;        // 2.2
            public Int16 wHighVersion = 36;    // 2.2
            public IntPtr szDescription;
            public IntPtr szSystemStatus;
            public Int16 iMaxSockets;
            public Int16 iMaxUdpDg;
            public IntPtr lpVendorInfo;

            /* FxCop says this is unused
             * 
            public string Description
            {
                get { return Marshal.PtrToStringUni(szDescription); }
            }

            public string SystemStatus
            {
                get { return Marshal.PtrToStringUni(szSystemStatus); }
            }
             */
        }

        [Flags()]
        internal enum LookupFlags : uint
        {
            None = 0,
            Containers = 0x0002,
            ReturnName = 0x0010,
            ReturnType = 0x0020,
            ReturnVersion = 0x0040,
            ReturnComment = 0x0080,
            ReturnAddr = 0x0100,
            ReturnBlob = 0x0200,
            FlushCache = 0x1000,
            ResService = 0x8000,
        }

        // http://msdn.microsoft.com/en-us/library/ms898762.aspx
        [StructLayout(LayoutKind.Sequential)]
        public class WSAQUERYSET
        {
            public uint dwSize;
            public IntPtr szServiceInstanceName;
            public IntPtr lpServiceClassId;
            public IntPtr lpVersion;
            public IntPtr lpszComment;
            public uint dwNameSpace = NS_BTH;
            public IntPtr lpNSProviderId;
            public IntPtr lpszContext;
            public uint dwNumberOfProtocols;
            public IntPtr lpafpProtocols;
            public IntPtr lpszQueryString;
            public uint dwNumberOfCsAddrs;
            public IntPtr lpcsaBuffer;
            public uint dwOutputFlags;
            public IntPtr lpBlob;

            /*  The code below was necessary during early development to ensure that parameters
             * were byte-aligned properly.  This is no longer necessary.
             *
#if DEBUG && !PocketPC
            static WSAQUERYSET()
            {
                // All structures passed to unmanaged methods must be precise.  These
                // methods can be used to ensure that fields are properly aligned.

                // The namespace should be at byte 20
                Debug.Assert(Marshal.OffsetOf(typeof(WSAQUERYSET), "dwNameSpace").ToInt32() == 20, "offset dwNameSpace");
                // The address buffer should live at byte 48
                Debug.Assert(Marshal.OffsetOf(typeof(WSAQUERYSET), "lpcsaBuffer").ToInt32() == 48, "offset lpcsaBuffer");
                // The address of output flags should be byte 52
                Debug.Assert(Marshal.OffsetOf(typeof(WSAQUERYSET), "dwOutputFlags").ToInt32() == 52, "offset dwOutputFlags");
                // The address of the BLOB should be byte 56
                Debug.Assert(Marshal.OffsetOf(typeof(WSAQUERYSET), "lpBlob").ToInt32() == 56, "offset lpBlob");
                // The total size should be 60 bytes
                Debug.Assert(Marshal.SizeOf(typeof(WSAQUERYSET)) == 60, "StructLength");
                 
            }
#endif
        */

            public WSAQUERYSET()
            {
                // The size will vary on 32-bit vs. 64-bit systems!
                dwSize = (uint)Marshal.SizeOf(this);
            }

#if PocketPC
            public string FriendlyName
            {
                get
                {
                    return Marshal.PtrToStringUni(szServiceInstanceName);
                }
            }
#endif

            /* FxCop says this is unused
             * 
            //public CSADDR_INFO AddressInfo
            //{
            //    get
            //    {
            //        CSADDR_INFO info = new CSADDR_INFO();
            //        Marshal.PtrToStructure(this.lpcsaBuffer, info);
            //        return info;
            //    }
            //}
             */

            /* FxCop says this is unused
             * 
            public string Comment
            {
                get
                {
                    return Marshal.PtrToStringUni(lpszComment);
                }
            }

            public string Context
            {
                get
                {
                    return Marshal.PtrToStringUni(lpszContext);
                }
            }
             */

            /* FxCop says this is unused
             * 
            //public string QueryString
            //{
            //    get
            //    {
            //        return Marshal.PtrToStringUni(lpszQueryString);
            //    }
            //}

            /* FxCop says this is unused
             * 
            //public Guid ServiceGuid
            //{
            //    get
            //    {
            //        /* The lpServiceClassId parameter is a 16-byte GUID.  Let's convert
            //         * this pointer into a GUID, shall we?
            //         */

            //        byte[] result = new byte[16];
            //        for (int index = 0; index < 16; index++)
            //            result[index] = Marshal.ReadByte(lpServiceClassId, index);
            //        return new Guid(result);
            //    }
            //}

        }

        #endregion

        #endregion

        #region USB

        public const int DIGCF_DEFAULT = 0x00000001;  // only valid with DIGCF_DEVICEINTERFACE
        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_ALLCLASSES = 0x00000004;
        public const int DIGCF_PROFILE = 0x00000008;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiClassGuidsFromName(
            StringBuilder ClassName,
            //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            IntPtr ClassGuidList,
            int ClassGuidListSize,
            out int RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int CM_Get_Parent(
           out UInt32 pdnDevInst,
           UInt32 dnDevInst,
           int ulFlags
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CM_Get_Device_ID(
           UInt32 dnDevInst,
           IntPtr Buffer,
           int BufferLen,
           int ulFlags
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
           ref Guid ClassGuid,
           IntPtr Enumerator,
           IntPtr hwndParent,
           int Flags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(
            IntPtr hDevInfo,
            UInt32 memberIndex,
            ref SP_DEVINFO_DATA devInfo);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           IntPtr devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
           ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           ref SP_DEVINFO_DATA deviceInfoData
        );

        #endregion

#if !PocketPC
        #region Desktop Members

        #region Bluetooth

        // http://msdn.microsoft.com/en-us/library/aa362795.aspx
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetDeviceInfo(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetRadioInfo(IntPtr hRadio, ref BLUETOOTH_RADIO_INFO pbtdi);

        // http://msdn.microsoft.com/en-us/library/aa362786(VS.85).aspx
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern IntPtr BluetoothFindFirstRadio(BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, ref IntPtr phRadio);

        /* FxCops says this is unused
         * 
        [DllImport("Irprops.cpl", SetLastError = true)]
        public static extern int BluetoothFindNextRadio(IntPtr hFind, ref IntPtr phRadio);
         */

        // http://msdn.microsoft.com/en-us/library/aa362792(VS.85).aspx
        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindRadioClose(IntPtr hFind);

        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsConnectable(IntPtr hRadio);

        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsDiscoverable(IntPtr hRadio);

        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothEnableDiscovery(IntPtr hRadio,
            [MarshalAs(UnmanagedType.Bool)]
            bool fEnabled);

        [DllImport("Irprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio,
            [MarshalAs(UnmanagedType.Bool)]
            bool fEnabled);

        [DllImport("Ws2_32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSAStartup(
            UInt16 wVersionRequested,
            WSAData wsaData);

        [DllImport("Ws2_32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSACleanup();

        // http://msdn.microsoft.com/en-us/library/ms898747.aspx
        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceBegin(
            WSAQUERYSET qsRestrictions,
            LookupFlags dwControlFlags,
            ref IntPtr lphLookup);

        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceNext(
            IntPtr hLookup,
            LookupFlags dwControlFlags,
            ref int lpdwBufferLength,
            byte[] pqsResults);

        [DllImport("Ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceEnd(IntPtr hLookup);

        #endregion

        #endregion
#else
        #region Mobile Device Members

        #region GPS Intermediate Driver

        // This method is used to tell the GPSID to reload its settings from the registry.
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            uint lpBytesReturned,
            IntPtr lpOverlapped);

        public const uint IOCTL_SERVICE_REFRESH = 0x4100000C;

        #endregion

        #region Serial Port

        // http://msdn.microsoft.com/en-us/library/aa915369.aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool GetCommState(IntPtr handle, ref DCB dcb);

        // http://msdn.microsoft.com/en-us/library/aa908949.aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool SetCommState(IntPtr handle, [In] ref DCB dcb);

        // http://msdn.microsoft.com/en-us/library/aa363428(VS.85).aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool PurgeComm(IntPtr handle, uint flags);

        // http://msdn.microsoft.com/en-us/library/aa363439(VS.85).aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool SetupComm(IntPtr handle, uint dwInQueue, uint dwOutQueue);

        // http://msdn.microsoft.com/en-us/library/bb202767.aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct DCB
        {

            internal uint DCBLength;
            internal uint BaudRate;
            private BitVector32 Flags;
            private uint wReserved;        // not currently used
            internal uint XonLim;           // transmit XON threshold
            internal uint XoffLim;          // transmit XOFF threshold             
            internal byte ByteSize;
            internal byte Parity;
            internal byte StopBits;
            //...and some more
            internal char XonChar;          // Tx and Rx XON character
            internal char XoffChar;         // Tx and Rx XOFF character
            internal char ErrorChar;        // error replacement character
            internal char EofChar;          // end of input character
            internal char EvtChar;          // received event character
            private uint wReserved1;       // reserved; do not use     

            private static readonly int fBinary;
            private static readonly int fParity;
            private static readonly int fOutxCtsFlow;
            private static readonly int fOutxDsrFlow;
            private static readonly BitVector32.Section fDtrControl;
            private static readonly int fDsrSensitivity;
            private static readonly int fTXContinueOnXoff;
            private static readonly int fOutX;
            private static readonly int fInX;
            private static readonly int fErrorChar;
            private static readonly int fNull;
            private static readonly BitVector32.Section fRtsControl;
            private static readonly int fAbortOnError;

            static DCB()
            {
                // Create Boolean Mask
                int previousMask;
                fBinary = BitVector32.CreateMask();
                fParity = BitVector32.CreateMask(fBinary);
                fOutxCtsFlow = BitVector32.CreateMask(fParity);
                fOutxDsrFlow = BitVector32.CreateMask(fOutxCtsFlow);
                previousMask = BitVector32.CreateMask(fOutxDsrFlow);
                previousMask = BitVector32.CreateMask(previousMask);
                fDsrSensitivity = BitVector32.CreateMask(previousMask);
                fTXContinueOnXoff = BitVector32.CreateMask(fDsrSensitivity);
                fOutX = BitVector32.CreateMask(fTXContinueOnXoff);
                fInX = BitVector32.CreateMask(fOutX);
                fErrorChar = BitVector32.CreateMask(fInX);
                fNull = BitVector32.CreateMask(fErrorChar);
                previousMask = BitVector32.CreateMask(fNull);
                previousMask = BitVector32.CreateMask(previousMask);
                fAbortOnError = BitVector32.CreateMask(previousMask);

                // Create section Mask
                BitVector32.Section previousSection;
                previousSection = BitVector32.CreateSection(1);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                fDtrControl = BitVector32.CreateSection(2, previousSection);
                previousSection = BitVector32.CreateSection(1, fDtrControl);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                previousSection = BitVector32.CreateSection(1, previousSection);
                fRtsControl = BitVector32.CreateSection(3, previousSection);
                previousSection = BitVector32.CreateSection(1, fRtsControl);
            }

            public bool Binary
            {
                get { return Flags[fBinary]; }
                set { Flags[fBinary] = value; }
            }

            public bool CheckParity
            {
                get { return Flags[fParity]; }
                set { Flags[fParity] = value; }
            }

            public bool OutxCtsFlow
            {
                get { return Flags[fOutxCtsFlow]; }
                set { Flags[fOutxCtsFlow] = value; }
            }

            public bool OutxDsrFlow
            {
                get { return Flags[fOutxDsrFlow]; }
                set { Flags[fOutxDsrFlow] = value; }
            }

            public DtrControl DtrControl
            {
                get { return (DtrControl)Flags[fDtrControl]; }
                set { Flags[fDtrControl] = (int)value; }
            }

            public bool DsrSensitivity
            {
                get { return Flags[fDsrSensitivity]; }
                set { Flags[fDsrSensitivity] = value; }
            }

            public bool TxContinueOnXoff
            {
                get { return Flags[fTXContinueOnXoff]; }
                set { Flags[fTXContinueOnXoff] = value; }
            }

            public bool OutX
            {
                get { return Flags[fOutX]; }
                set { Flags[fOutX] = value; }
            }

            public bool InX
            {
                get { return Flags[fInX]; }
                set { Flags[fInX] = value; }
            }

            public bool ReplaceErrorChar
            {
                get { return Flags[fErrorChar]; }
                set { Flags[fErrorChar] = value; }
            }

            public bool Null
            {
                get { return Flags[fNull]; }
                set { Flags[fNull] = value; }
            }

            public RtsControl RtsControl
            {
                get { return (RtsControl)Flags[fRtsControl]; }
                set { Flags[fRtsControl] = (int)value; }
            }

            public bool AbortOnError
            {
                get { return Flags[fAbortOnError]; }
                set { Flags[fAbortOnError] = value; }
            }
        }

        // http://msdn.microsoft.com/en-us/library/aa363180(VS.85).aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool ClearCommError(IntPtr handle, ref uint lpErrors, COMMSTAT lpStat);

        // http://msdn.microsoft.com/en-us/library/aa363200(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct COMMSTAT
        {
            private BitVector32 Flags;
            internal uint dbInQue;
            internal uint dbOutQueue;

            private static readonly int fCtsHold;
            private static readonly int fDsrHold;
            private static readonly int fRlsdHold;
            private static readonly int fXoffHold;
            private static readonly int fXoffSent;
            private static readonly int fEof;
            private static readonly int fTrim;

            static COMMSTAT()
            {
                // Create Boolean Mask
                fCtsHold = BitVector32.CreateMask();
                fDsrHold = BitVector32.CreateMask(fCtsHold);
                fRlsdHold = BitVector32.CreateMask(fDsrHold);
                fXoffHold = BitVector32.CreateMask(fRlsdHold);
                fXoffSent = BitVector32.CreateMask(fXoffHold);
                fEof = BitVector32.CreateMask(fXoffSent);
                fTrim = BitVector32.CreateMask(fEof);
            }

            public bool CtsHold
            {
                get { return Flags[fCtsHold]; }
                set { Flags[fCtsHold] = value; }
            }

            public bool DsrHold
            {
                get { return Flags[fDsrHold]; }
                set { Flags[fDsrHold] = value; }
            }

            public bool RlsdHold
            {
                get { return Flags[fRlsdHold]; }
                set { Flags[fRlsdHold] = value; }
            }

            public bool XoffHold
            {
                get { return Flags[fXoffHold]; }
                set { Flags[fXoffHold] = value; }
            }

            public bool Eof
            {
                get { return Flags[fEof]; }
                set { Flags[fEof] = value; }
            }

            public bool Trim
            {
                get { return Flags[fTrim]; }
                set { Flags[fTrim] = value; }
            }
        }


        public enum DtrControl : int
        {
            Disable = 0,
            Enable = 1,
            Handshake = 2
        };

        public enum RtsControl : int
        {
            Disable = 0,
            Enable = 1,
            Handshake = 2,
            Toggle = 3
        };

        // http://msdn.microsoft.com/en-us/library/aa363190(VS.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct COMMTIMEOUTS
        {
            public uint ReadIntervalTimeout;
            public uint ReadTotalTimeoutMultiplier;
            public uint ReadTotalTimeoutConstant;
            public uint WriteTotalTimeoutMultiplier;
            public uint WriteTotalTimeoutConstant;
        }

        // http://msdn.microsoft.com/en-us/library/aa363261(VS.85).aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool GetCommTimeouts(IntPtr handle, ref COMMTIMEOUTS timeouts);

        // http://msdn.microsoft.com/en-us/library/aa363437(VS.85).aspx
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool SetCommTimeouts(IntPtr handle, [In] ref COMMTIMEOUTS timeouts);

        #endregion

        #region Bluetooth

        // http://msdn.microsoft.com/en-us/library/aa916756.aspx
        [StructLayout(LayoutKind.Sequential)]
        public class BthInquiryResult
        {
            UInt64 ba;              // Love ULONGLONG time
            uint classOfDevice;     // Class of device
            ushort clock_offset;
            byte page_scan_mode;
            byte page_scan_period_mode;
            byte page_scan_repetition_mode;

            public uint ClassOfDevice
            {
                get { return classOfDevice; }
            }

            public BluetoothAddress Address
            {
                get
                {
                    return new BluetoothAddress(ba);
                }
            }

            public BluetoothDevice ToDevice()
            {
                BluetoothDevice result = new BluetoothDevice(Address);
                result.SetClassOfDevice(classOfDevice);
                return result;
            }


            /// <summary>
            /// Returns the primary purpose of the device.
            /// </summary>
            public DeviceClass Class
            {
                get
                {
                    return (DeviceClass)(classOfDevice & 0x1F00);
                }
            }

            /// <summary>
            /// Returns a sub-category describing the purpose of the device.
            /// </summary>
            public DeviceClass MinorDeviceClass
            {
                get
                {
                    return (DeviceClass)(classOfDevice & 0xFC);
                }
            }

            /// <summary>
            /// Returns the major type of device.
            /// </summary>
            public ServiceClass ServiceClass
            {
                get
                {
                    return (ServiceClass)(classOfDevice & 0xFFE000); ;
                }
            }
        }

        [DllImport("Ws2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSAStartup(
            UInt16 wVersionRequested,
            WSAData wsaData);

        [DllImport("Ws2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSACleanup();

        [DllImport("Ws2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceBegin(
            WSAQUERYSET qsRestrictions,
            LookupFlags dwControlFlags,
            ref IntPtr lphLookup);

        [DllImport("Ws2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceNext(
            IntPtr hLookup,
            LookupFlags dwControlFlags,
            ref int lpdwBufferLength,
            byte[] pqsResults);

        [DllImport("Ws2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int WSALookupServiceEnd(IntPtr hLookup);

        // http://msdn.microsoft.com/en-us/library/ms837414.aspx
        [DllImport("BthUtil.dll", SetLastError = true)]
        public static extern int BthSetMode(BluetoothRadioMode dwMode);

        // http://msdn.microsoft.com/en-us/library/ms837409.aspx
        [DllImport("BthUtil.dll", SetLastError = true)]
        public static extern int BthGetMode(out BluetoothRadioMode dwMode);

        /// <summary>
        /// Indicates the state of a Bluetooth wireless receiver.
        /// </summary>
        public enum BluetoothRadioMode
        {
            /// <summary>
            /// Bluetooth is disabled on the device.
            /// </summary>
            PowerOff,
            /// <summary>
            /// Bluetooth is connectable but your device cannot be discovered by other devices.
            /// </summary>
            Connectable,
            /// <summary>
            /// Bluetooth is activated and fully discoverable.
            /// </summary>
            Discoverable
        }

        #endregion

        #endregion

#endif


        #region Unused Code (Commented Out)


        /* FxCop says this is unused
         * 
    // http://msdn.microsoft.com/en-us/library/aa362897(VS.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    public class BLUETOOTH_ADDRESS
    {
        UInt64 ullLong;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        byte[] rgBytes;
    }
         */


        /* FxCop says this is unused
             * 
        [StructLayout(LayoutKind.Sequential)]
        public class CSADDR_INFO
        {
            // We could use SOCKET_ADDRESS here, but the Compact Framework
            // cannot deserialize complex structures.  (PtrToStructure).

            public IntPtr lpLocalSockaddr;
            public int iLocalSockaddrLength;
            public IntPtr lpRemoteSockaddr;
            public int iRemoteSockaddrLength;
            public int iSocketType;
            public int iProtocol;

            public SOCKET_ADDRESS LocalAddr
            {
                get
                {
                    SOCKET_ADDRESS item = new SOCKET_ADDRESS();
                    item.lpSockaddr = lpLocalSockaddr;
                    item.iSockaddrLength = iLocalSockaddrLength;
                    return item;
                }
            }

            public SOCKET_ADDRESS RemoteAddr
            {
                get
                {
                    SOCKET_ADDRESS item = new SOCKET_ADDRESS();
                    item.lpSockaddr = lpRemoteSockaddr;
                    item.iSockaddrLength = iRemoteSockaddrLength;
                    return item;
                }
            }
        }
             */

        /* FxCop says this is unused
         * 
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class SOCKET_ADDRESS
            {
                public IntPtr lpSockaddr;
                public int iSockaddrLength;

                /// <summary>
                /// Returns a well-formed Bluetooth address from data in the structure.
                /// </summary>
                public BluetoothEndPoint EndPoint
                {
                    get
                    {
                        return new BluetoothEndPoint(Bytes);
                    }
                }

                /// <summary>
                /// Returns a 30-byte array for the socket address.
                /// </summary>
                public byte[] Bytes
                {
                    get
                    {
                        byte[] result = new byte[iSockaddrLength];
                        for (int index = 0; index < iSockaddrLength; index++)
                            result[index] = Marshal.ReadByte(lpSockaddr, index);
                        return result;
                    }
                }
            }
         */

        /* FxCop says this is unused
         * 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class SOCKADDR_BTH
        {
            public UInt16 addressFamily;
            public UInt64 address;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] serviceClassId;
            public uint port;
        }
             */

        #endregion

    }
}
