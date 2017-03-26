using System;
using System.Reflection;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

/* These assembly attributes will be the same regardless of the product. */
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

[assembly: AssemblyCopyright("Copyright � DotSpatial Team 2012-2016")]
[assembly: AssemblyTrademark("")]

/* These attributes will change for each assembly. */
[assembly: AssemblyProduct("DotSpatial.Positioning.Design")]
[assembly: AssemblyTitle("DotSpatial.Positioning.Design")]
[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications.")]

/* This assembly contains language-specific resources.  Help the CLR find them. */
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

/* The assembly configuration just explains what target platform this assembly is for.  This
 * will be used instead of version numbers from now on to indicate the target platform.
 */

#if Framework40
    [assembly: AssemblyConfiguration("Public Release for .NET Framework 4.0")]
#elif Framework30
[assembly: AssemblyConfiguration("Public Release for .NET Framework 3.5")]
#elif Framework20
    [assembly: AssemblyConfiguration("Public Release for .NET Framework 2.0")]
#endif

// On the desktop .NET Framework, we can certify that this code has been debugged well
// enough to be reliable in all production environments.  Some crazy manufacturers will
// throw curve balls at this certification, sure, but we can at least certify this is
// our best, exhaustive effort.
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]

/* There are security and performance benefits of explicitly declaring permissions required
 * by this assembly.  They are requested now and asserted explicitly to remove the need for
 * time-consuming or redundant security checks.
 */

// CAS permission declarations are obsolete in .Net 4.0
//#if !Framework40

//    // Grant only the minimum security permissions
//    [assembly: SecurityPermission(SecurityAction.RequestMinimum)]
//    // Grant only the minimum required reflection permissions
//    [assembly: ReflectionPermission(SecurityAction.RequestMinimum)]
//    // We won't use the registry, so deny access
//    [assembly: RegistryPermission(SecurityAction.RequestRefuse)]
//    // We won't use file I/O, so we dfon't need it
//    [assembly: FileIOPermission(SecurityAction.RequestRefuse)]
//    // We want only the minimum permissions necessary
//    [assembly: PermissionSet(SecurityAction.RequestMinimum, Unrestricted = false)]

//#endif

// Allow partially-trusted callers to use this code (such as ASP.NET)
[assembly: AllowPartiallyTrustedCallers]