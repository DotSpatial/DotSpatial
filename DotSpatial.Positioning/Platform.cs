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
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Indicates the current host operating system.
    /// </summary>
    public enum HostPlatformID
    {
        /// <summary>
        /// The current platform has not yet been determined.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The current plarform is a desktop computer.
        /// </summary>
        Desktop = 1,
        /// <summary>
        /// The current platform is Windows CE 4.2
        /// </summary>
        WindowsCe = 2,
        /// <summary>
        /// The current platform is PocketPC (Windows Mobile 2003)
        /// </summary>
        PocketPc = 3,
        /// <summary>
        /// The current platform is Smartphone
        /// </summary>
        Smartphone = 4
    }

    /// <summary>
    /// Indicates the current .NET framework being used.
    /// </summary>
    public enum DotNetFrameworkID
    {
        /// <summary>
        /// The .NET framework version has not been determined.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// .NET framework version 1.0 (Visual Studio 2002) is being used.
        /// </summary>
        DesktopFramework10 = 1,
        /// <summary>
        /// .NET framework version 1.1 (Visual Studio 2003) is being used.
        /// </summary>
        DesktopFramework11 = 2,
        /// <summary>
        /// .NET framework version 2.0 (Visual Studio 2005) is being used.
        /// </summary>
        DesktopFramework20 = 3,
        /// <summary>
        /// .NET framework version 3.0 (Visual Studio 2008) is being used.
        /// </summary>
        DesktopFramework30 = 6,
        /// <summary>
        /// .NET framework version 4.0 (Visual Studio 2010) is being used.
        /// </summary>
        DesktopFramework40 = 7,
        /// <summary>
        /// .NET Compact Framework version 1.0 (Visual Studio 2003) is being used.
        /// </summary>
        CompactFramework10 = 4,
        /// <summary>
        /// .NET Compact Framework version 2.0 (Visual Studio 2005) is being used.
        /// </summary>
        CompactFramework20 = 5
    }

    /// <summary>
    /// Provides features for determining the current host platform.
    /// </summary>
    public static class Platform
    {
        #region Private variables

        /// <summary>
        ///
        /// </summary>
        private static readonly HostPlatformID _hostPlatformID;
#if Framework40
        /// <summary>
        ///
        /// </summary>
        private const DotNetFrameworkID DOT_NET_FRAMEWORK_ID = DotNetFrameworkID.DesktopFramework40;
#elif Framework30
        private const DotNetFrameworkID DOT_NET_FRAMEWORK_ID = DotNetFrameworkID.DesktopFramework30;
#endif

        #endregion Private variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static Platform()
        {
#if !PocketPC
            _hostPlatformID = HostPlatformID.Desktop;
#else
            // Are we running on "Deploy to My Computer" ?
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                // Determine the platform
                StringBuilder sb = new System.Text.StringBuilder(128);

                // SPI_GETPLATFORMTYPE
                bool success = NativeMethods.GetSystemParameterString(257, (UInt32)(sb.Capacity * System.Runtime.InteropServices.Marshal.SizeOf(typeof(char))), sb, false);

                // compare strings as lowercase
                string str = sb.ToString().ToLower();
                switch (str)
                {
                    case "smartphone":
                        _HostPlatformID = HostPlatformID.Smartphone;
                        break;
                    case "pocketpc":
                        _HostPlatformID = HostPlatformID.PocketPC;
                        break;
                    default:
                        _HostPlatformID = HostPlatformID.WindowsCE;
                        break;
                }
            }
            else
            {
                // Deploy to My Computer runs on the desktop
                _HostPlatformID = HostPlatformID.Desktop;
            }
#endif
        }

        #endregion Constructors

        /// <summary>
        /// Returns the current host platform.
        /// </summary>
        /// <remarks>This property is used to determine the current host platform: Windows CE 4.2,
        /// PocketPC / Windows Mobile 2003, Smartphone, or Desktop.  This property is typically
        /// used to adjust the performance and behavior of an application to work on a specific platform.
        /// For example, thread priorities are more sensitive on the Smartphone platform than the
        /// PocketPC platform.  This can also be used to determine correct locations of system folders
        /// and installed system software such as Bluetooth stacks.</remarks>
        public static HostPlatformID HostPlatformID
        {
            get
            {
                return _hostPlatformID;
            }
        }

        /// <summary>
        /// Returns the current version of the .NET Framework currently in use.
        /// </summary>
        public static DotNetFrameworkID DotNetFrameworkID
        {
            get
            {
                return DOT_NET_FRAMEWORK_ID;
            }
        }
    }
}