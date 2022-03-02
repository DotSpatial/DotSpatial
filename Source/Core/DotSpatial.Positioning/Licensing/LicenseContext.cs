#if PocketPC && !DesignTime
using System;

namespace GeoFramework.Licensing
{
	/// <summary>Indicates the code execution environment for a license.</summary>
	/// <remarks>
	/// 	<para>There are two major license contexts used: design-time and run-time. The
	///     GeoFrameworks licensing system typically uses a more relaxed approach in the
	///     design-time environment. For example, licenses are granted to developers in
	///     design-time environments even if a trial period has expired in order to let them
	///     open their Windows Forms and continue development. Licenses are enforced more
	///     strictly at run-time and functionality is prohibited unless a beta license, trial
	///     license or owner license can be granted.</para>
	/// 	<para>This class exists only for .NET Compact Framework 1.0, which excluded
	///     licensing classes by default.</para>
	/// </remarks>
	public sealed class LicenseContext
	{
		private LicenseUsageMode pUsageMode;

		internal LicenseContext(LicenseUsageMode usageMode)
		{
			pUsageMode = usageMode;
		}

		/// <summary>
		/// Indicates whether a license is used for design-time or run-time
		/// environments.
		/// </summary>
		/// <value>A value from the <strong>LicenseUsageMode</strong> enumaration.</value>
		/// <remarks>
		/// This property returns <strong>Designtime</strong> if the Visual Studio.NET IDE is
		/// detected, otherwise a value of <strong>Runtime</strong> is returned.
		/// </remarks>
		public LicenseUsageMode UsageMode
		{
			get
			{
				return pUsageMode;
			}
		}
	}

    /// <summary>Indicates the operational environment allowed for a license.</summary>
    /// <remarks>
    /// This enumeration is used by the <strong>LicenseContext</strong> class to indicate
    /// the operational environment valid for a license.
    /// </remarks>
    public enum LicenseUsageMode
    {
        /// <summary>
        /// The license is valid for use within the Visual Studio.NET Windows Forms
        /// designer.
        /// </summary>
        Designtime,
        /// <summary>The license is valid for use in a production or release environment.</summary>
        Runtime
    }
}
#endif