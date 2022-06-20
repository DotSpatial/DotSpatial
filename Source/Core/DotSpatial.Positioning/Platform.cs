// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
        /// The current platform is a desktop computer.
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
        /// .NET framework version 6.0 (Visual Studio 2022) is being used.
        /// </summary>
        DesktopFramework60 = 8,
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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static Platform()
        {
            HostPlatformID = HostPlatformID.Desktop;
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
        public static HostPlatformID HostPlatformID { get; private set; }

        /// <summary>
        /// Returns the current version of the .NET Framework currently in use.
        /// </summary>
        public static DotNetFrameworkID DotNetFrameworkID => DotNetFrameworkID.DesktopFramework60;
    }
}