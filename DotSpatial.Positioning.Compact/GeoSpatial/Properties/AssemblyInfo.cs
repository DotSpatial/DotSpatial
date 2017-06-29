using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Resources;
using System.Security;
#if !PocketPC && Framework20
using System.Runtime.ConstrainedExecution;
#endif

/* These assembly attributes will be the same regardless of the product. */
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: AssemblyCompany("DotSpatial.Positioning")]
[assembly: AssemblyCopyright("This source code has been released to the public domain.")]
[assembly: AssemblyTrademark("")]

/* These attributes will change for each assembly. */
[assembly: AssemblyProduct("Geographic Framework for Visual Studio.NET")]

#if PocketPC
    [assembly: AssemblyTitle("DotSpatial.Positioning.PocketPC")]
#else
    [assembly: AssemblyTitle("DotSpatial.Positioning")]
#endif

/* The version of the assembly depends on the framework version.  Design-Time assemblies
 * have no revision number because that would require much code change for each release.
 * We only care about the major version number and VS version numbers. */

#if Framework40
		// .NET Framework 4.0
		[assembly: AssemblyVersion("2.0.1.*")]
		[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications using Visual Studio 2010")]
#elif Framework30
		// .NET Framework 3.5
		[assembly: AssemblyVersion("2.0.1.*")]
		[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications using Visual Studio 2008")]
#elif Framework20
    // .NET Framework 2.0
    [assembly: AssemblyVersion("2.0.1.*")]
    [assembly: AssemblyDescription("This assembly provides objects used to design geographic applications using Visual Studio 2005")]
#elif Framework10
		// .NET Framework 1.0
		[assembly: AssemblyVersion("2.0.1.*")]
		[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications using Visual Studio 2002")]
#else
		// .NET Framework 1.1
		[assembly: AssemblyVersion("2.0.1.*")]
		[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications using Visual Studio 2003")]
#endif

/* Compact Framework 1.0 requires that design-time assemblies contain a special assembly attribute to link them
 * up to the assemblies to use on the mobile device.
 */
#if DesignTime && !Framework20
    [assembly: System.CF.Design.RuntimeAssembly("DotSpatial.Positioning.PocketPC, Version=1.4.5000.7, Culture=neutral, PublicKeyToken=d77afaeb30e3236a")]
#endif

/* Thisd assembly contains language-specific resources.  Help the CLR find them. */
#if Framework20 && !PocketPC
    [assembly: NeutralResourcesLanguageAttribute("en-US", UltimateResourceFallbackLocation.MainAssembly)]
#endif

/* The assembly configuration just explains what target platform this assembly is for.  This
 * will be used instead of version numbers from now on to indicate the target platform.
 */
#if PocketPC
    #if Framework30
        [assembly: AssemblyConfiguration("Public Release for .NET Compact Framework 3.5")]
    #elif Framework20
        [assembly: AssemblyConfiguration("Public Release for .NET Compact Framework 2.0")]
    #else
	    [assembly: AssemblyConfiguration("Public Release for .NET Compact Framework 1.0")]
    #endif
#else
    #if Framework40
        [assembly: AssemblyConfiguration("Public Release for .NET Framework 4.0")]
    #elif Framework30
        [assembly: AssemblyConfiguration("Public Release for .NET Framework 3.5")]
    #elif Framework20
        [assembly: AssemblyConfiguration("Public Release for .NET Framework 2.0")]
    #elif Framework10
        [assembly: AssemblyConfiguration("Public Release for .NET Framework 1.0")]
    #else
        [assembly: AssemblyConfiguration("Public Release for .NET Framework 1.1")]
    #endif
#endif

#if !PocketPC 

// On the desktop .NET Framework, we can certify that this code has been debugged well
// enough to be reliable in all production environments.  Some crazy manufacturers will
// throw curve balls at this certification, sure, but we can at least certify this is
// our best, exhaustive effort.
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]

#endif

/* There are security and performance benefits of explicitly declaring permissions required
 * by this assembly.  They are requested now and asserted explicitly to remove the need for
 * time-consuming or redundant security checks.
 */
#if !PocketPC || DesignTime

// CAS permission declarations are obsolted in .Net 4.0
#if !Framework40

    // Grant only the minimum security permissions
    [assembly: SecurityPermission(SecurityAction.RequestMinimum)]
    // Grant only the minimum required reflection permissions
    [assembly: ReflectionPermission(SecurityAction.RequestMinimum)]
    // We won't use the registry, so deny access
    [assembly: RegistryPermission(SecurityAction.RequestRefuse)]
    // We won't use file I/O, so we dfon't need it
    [assembly: FileIOPermission(SecurityAction.RequestRefuse)]
    // We want only the minimum permissions necessary
    [assembly: PermissionSet(SecurityAction.RequestMinimum, Unrestricted = false)]

#endif

    // Allow partially-trusted callers to use this code (such as ASP.NET)
    [assembly: AllowPartiallyTrustedCallers()]

#endif
