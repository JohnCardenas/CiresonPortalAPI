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

// Make internals visible to the test project
[assembly: InternalsVisibleTo("CiresonPortalAPI.Tests")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3f353bf2-c024-4020-aeea-85572db0f595")]

[assembly: AssemblyVersion("5.0.0.0")]
[assembly: AssemblyFileVersion("5.0.7.0")]
[assembly: AssemblyInformationalVersion("5.0.7.0")]
