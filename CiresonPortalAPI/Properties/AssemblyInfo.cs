using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CiresonPortalAPI")]
[assembly: AssemblyDescription("REST wrapper interfaces for the Cireson Portal")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CiresonPortalAPI")]
[assembly: AssemblyCopyright("Copyright © John Cardenas 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3f353bf2-c024-4020-aeea-85572db0f595")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:

/**
 * AssemblyVersion is in the format "(Major).(Minor)"
 */
[assembly: AssemblyVersion("1.1")]

/**
 * AssemblyFileVersion is in the format "(Major).(Minor).(Build).(Revision)"; automatically generate Build and Revision
 */
[assembly: AssemblyFileVersion("1.1.*")]

/**
 * AssemblyInformationalVersion is what users would understand the version of the assembly to be, and is in the format "(Major).(Minor).(Patch)"
 */
[assembly: AssemblyInformationalVersion("1.1.0")]

/**
 * Milestones:
 *   1.0 - basic components, TypeProjection support, Incident object support
 */