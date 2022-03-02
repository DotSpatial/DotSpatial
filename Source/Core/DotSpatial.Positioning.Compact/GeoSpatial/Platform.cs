using System;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Indicates the current host operating system.
    /// </summary>
    public enum HostPlatformID : int
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
        WindowsCE = 2,
        /// <summary>
        /// The current platform is PocketPC (Windows Mobile 2003)
        /// </summary>
        PocketPC = 3,
        /// <summary>
        /// The current platform is Smartphone
        /// </summary>
        Smartphone = 4
    }

    /// <summary>
    /// Indicates the current .NET framework being used.
    /// </summary>
	public enum DotNetFrameworkID : int
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
    public
#if Framework20
 static
#else
    sealed
#endif
 class Platform
    {

        #region Private variables

        private static HostPlatformID _HostPlatformID;

#if !PocketPC
        // Desktop Framework.  Which one?
#if Framework10
            private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.DesktopFramework10;
#elif Framework40
        private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.DesktopFramework40;
#elif Framework30
        private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.DesktopFramework30;
#elif Framework20
        private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.DesktopFramework20;
#else
            private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.DesktopFramework11;
#endif
#else
        // Compact Framework. Which one?
#if Framework20
            private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.CompactFramework20;
#else
            private static DotNetFrameworkID _DotNetFrameworkID = DotNetFrameworkID.CompactFramework10;
#endif
#endif

        #endregion

        #region Constructors

        static Platform()
        {
#if !PocketPC
            _HostPlatformID = HostPlatformID.Desktop;
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

        #endregion

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
                return _HostPlatformID;
            }
        }

        /// <summary>
        /// Returns the current version of the .NET Framework currently in use.
        /// </summary>
        public static DotNetFrameworkID DotNetFrameworkID
        {
            get
            {
                return _DotNetFrameworkID;
            }
        }
    }
}
