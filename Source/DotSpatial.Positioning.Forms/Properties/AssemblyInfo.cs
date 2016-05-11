// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
// Description:  The user interface layer libraries related to the GPS positioning library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
#if !PocketPC && Framework20

using System.Runtime.ConstrainedExecution;

#endif

/* These assembly attributes will be the same regardless of the product. */
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: AssemblyCompany("DotSpatial Team")]
[assembly: AssemblyCopyright("Copyright © DotSpatial Team 2012-2016")]

/* These attributes will change for each assembly. */
[assembly: AssemblyDescription("Global Positioning System Framework.")]
[assembly: AssemblyProduct("DotSpatial.Positioning.Forms")]

/* The version will crank up depending on how much the code has changed. */
[assembly: AssemblyVersion("1.9")]

[assembly: AssemblyFileVersion("1.9")]

/* The title of the assembly varies depending on whether the assembly is for design-time use,
 * and whether the assembly targets a desktop or mobile device.
 */
#if PocketPC
    [assembly: AssemblyTitle("DotSpatial.Positioning.Gps.PocketPC")]
#else
[assembly: AssemblyTitle("DotsSpatial.Positioning.Forms")]
#endif

/* Thisd assembly contains language-specific resources.  Help the CLR find them. */
#if Framework20 && !PocketPC
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
#else
[assembly: NeutralResourcesLanguageAttribute("en-US")]
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

#if !PocketPC && (Framework20 || Framework30)
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

    // Grant only the minimum security permissions.  We use unmanaged code in some classes.  Serial/Bluetooth, etc.
    [assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true, Execution = false)]
    // We don't use reflection
    [assembly: ReflectionPermission(SecurityAction.RequestRefuse)]
    // We won't use the registry, so deny access
    [assembly: RegistryPermission(SecurityAction.RequestMinimum)]
    // We don't use the environment, so deny
    [assembly: EnvironmentPermission(SecurityAction.RequestRefuse)]
    // We DO use file I/O, so request it now
    [assembly: FileIOPermission(SecurityAction.RequestMinimum)]

#endif

#endif