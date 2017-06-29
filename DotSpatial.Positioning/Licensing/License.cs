#if PocketPC && !DesignTime
using System;
using System.Text;

namespace GeoFramework.Licensing
{
	/// <summary>Represents an abstract class used for software licensing.</summary>
	/// <remarks>
	/// This class is used for GeoFrameworks licensing, though it could theoretically be
	/// used as a base class for custom licensing. This class exists only for .NET Compact
	/// Framework 1.0, which excluded licensing classes by default.
	/// </remarks>
    public abstract class License
    {
		/// <summary>Creates a new instance of the license.</summary>
		/// <remarks>
		/// Since <strong>License</strong> is an abstract class, this constructor has no
		/// functionality.
		/// </remarks>
        protected License()
        {            
        }

		/// <summary>Indicates the license key associated with the license.</summary>
		/// <remarks>This property must be overridden in classes inheriting from this class.</remarks>
		public abstract string LicenseKey { get; }
    }
}
#endif