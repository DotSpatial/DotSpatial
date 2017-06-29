#if PocketPC && !DesignTime
using System;

namespace GeoFramework.Licensing
{
	/// <summary>Represents a base class for designing copy protection services.</summary>
	/// <remarks>
	/// <para>This class provides a single method named GetLicense which grants licenses or
	/// throws exceptions for a specified Type.</para>
	/// 	<para>This class exists only for .NET Compact Framework 1.0, which excluded
	///     licensing classes by default.</para>
	/// </remarks>
    public abstract class LicenseProvider 
    {
        protected LicenseProvider()
        {}

		/// <summary>Attempts to authorize use of a protected class.</summary>
		/// <returns>A <strong>License</strong> object if validation is successful.</returns>
		/// <remarks>
		/// This method, when overridden, will perform any necessary validation to ensure
		/// that the developer has sufficient rights to create the specified class. If validation
		/// fails, an exception is thrown if <strong>allowExceptions</strong> is
		/// <strong>True</strong>, otherwise the function returns null.
		/// </remarks>
		/// <param name="context">
		/// A <strong>LicenseContext</strong> object indicating the execution environment for
		/// the protected class.
		/// </param>
		/// <param name="type">The <strong>Type</strong> describing the protected class.</param>
		/// <param name="instance">The instance of the protected class.</param>
		/// <param name="allowExceptions">
		/// A <strong>Boolean</strong>, <strong>True</strong> if an exception should be
		/// thrown if validation fails.
		/// </param>
		public abstract License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions);
    }
}
#endif