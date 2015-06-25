// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Design.dll
// Description:  A library supporting optional windows forms designer elements for DotSpatial.Positioning
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
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Resources;
using System.Security;

/* These assembly attributes will be the same regardless of the product. */
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: AssemblyCompany("DotSpatial Team")]
[assembly: AssemblyCopyright("This source code has been released to the public domain.")]
[assembly: AssemblyTrademark("")]

/* These attributes will change for each assembly. */
[assembly: AssemblyProduct("DotSpatial.Positioning.Design")]
[assembly: AssemblyTitle("DotSpatial.Positioning.Design")]

[assembly: AssemblyVersion("1.7")]
[assembly: AssemblyDescription("This assembly provides objects used to design geographic applications.")]

/* This assembly contains language-specific resources.  Help the CLR find them. */
[assembly: NeutralResourcesLanguageAttribute("en-US", UltimateResourceFallbackLocation.MainAssembly)]

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

// CAS permission declarations are obsolted in .Net 4.0
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